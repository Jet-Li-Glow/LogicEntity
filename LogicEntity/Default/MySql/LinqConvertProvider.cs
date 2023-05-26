using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using System.Collections;
using LogicEntity.Linq.Expressions;
using LogicEntity.Json;
using LogicEntity.Default.MySql.ValueConversion;
using LogicEntity.Collections.Generic;
using LogicEntity.Collections;
using LogicEntity.Method;
using LogicEntity.Default.MySql.Linq.Expressions;
using LogicEntity.Default.MySql.ExpressionVisit;
using LogicEntity.Linq;

namespace LogicEntity.Default.MySql
{
    public partial class LinqConvertProvider : ILinqConvertProvider
    {
        readonly PropertyInfo _GroupingDataTableKey = typeof(IGroupingDataTable<>).GetProperty(nameof(IGroupingDataTable<int>.Key));
        readonly MethodInfo _Read = typeof(DbFunction).GetMethod(nameof(DbFunction.Read));
        readonly MethodInfo _ReadBytes = typeof(DbFunction).GetMethod(nameof(DbFunction.ReadBytes));
        readonly MethodInfo _ReadChars = typeof(DbFunction).GetMethod(nameof(DbFunction.ReadChars));

        readonly Dictionary<PropertyInfo, ValueConverter> PropertyConvert = new();

        readonly UpdateFactoryVersion _updateFactoryVersion = UpdateFactoryVersion.V5_7;

        public LinqConvertProvider()
        {
            InitEnumerableMethodFormat();

            InitMySqlEnumerableMethodFormat();

            InitDbFunctionMethodFormat();
        }

        public LinqConvertProvider(LinqConvertOptions linqConvertOptions) : this()
        {
            if (linqConvertOptions is null)
                return;

            if (linqConvertOptions.MemberFormat is not null)
            {
                foreach (KeyValuePair<MemberInfo, object> format in linqConvertOptions.MemberFormat)
                {
                    if (format.Value is null)
                        throw new ArgumentNullException($"The format of the {format.Key} is empty");

                    MemberFormat[GetGenericDefinition(format.Key)] = format.Value;
                }
            }

            if (linqConvertOptions.PropertyConverters is not null)
            {
                foreach (KeyValuePair<PropertyInfo, ValueConverter> converter in linqConvertOptions.PropertyConverters.PropertyConvert)
                {
                    PropertyConvert[converter.Key] = converter.Value;
                }
            }

            _updateFactoryVersion = linqConvertOptions.UpdateFactoryVersion;
        }

        public Command Convert(LogicEntity.Linq.Expressions.Expression expression)
        {
            Command command;

            if (expression is TableExpression tableExpression && expression is not AddNextTableExpression)
                command = GetSelectCommand(tableExpression);
            else
                command = GetOperateCommand(expression);

            return command;
        }

        Command GetSelectCommand(TableExpression tableExpression)
        {
            return GetDataManipulationSql(tableExpression).Build(this);
        }

        Command GetOperateCommand(LogicEntity.Linq.Expressions.Expression operateExpression)
        {
            if (operateExpression is AddOperateExpression addOperateExpression)
            {
                return GetOperateCommand(new AddOrUpdateWithFactoryOperateExpression(
                    SqlExpressions.InsertExpression.AddOperateType.Insert,
                    addOperateExpression.Source,
                    addOperateExpression.Elements
                    ));
            }

            if (operateExpression is AddOrUpdateOperateExpression addOrUpdateOperateExpression)
            {
                return GetOperateCommand(new AddOrUpdateWithFactoryOperateExpression(
                    SqlExpressions.InsertExpression.AddOperateType.Insert,
                    addOrUpdateOperateExpression.Source,
                    addOrUpdateOperateExpression.Elements,
                    true,
                    null
                    ));
            }

            if (operateExpression is AddOrUpdateWithFactoryOperateExpression addOrUpdateWithFactoryOperateExpression)
            {
                SqlExpressions.InsertExpression insertExpression = new();

                //AddOperate
                insertExpression.AddOperate = addOrUpdateWithFactoryOperateExpression.AddOperate;

                //Table
                insertExpression.Table = (SqlExpressions.OriginalTableExpression)GetDataManipulationSql(addOrUpdateWithFactoryOperateExpression.Source);

                //Values
                string valuesExpression = string.Empty;

                if (addOrUpdateWithFactoryOperateExpression.DataSource == AddDataSource.Entity)
                {
                    PropertyInfo[] properties = insertExpression.Table.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                        .Where(p => p.PropertyType.IsAssignableTo(typeof(IValue))).ToArray();

                    Dictionary<PropertyInfo, EntityPropertyInfo> validProperties = new();

                    List<Dictionary<PropertyInfo, IValue>> rows = new();

                    foreach (object element in addOrUpdateWithFactoryOperateExpression.Elements)
                    {
                        Dictionary<PropertyInfo, IValue> row = new();

                        foreach (PropertyInfo property in properties)
                        {
                            IValue propertyValue = (IValue)property.GetValue(element);

                            if (propertyValue.ValueSetted is false)
                                continue;

                            if (validProperties.ContainsKey(property) == false)
                            {
                                Expression<Func<object, object>> writer = null;

                                if (PropertyConvert.TryGetValue(property, out ValueConverter converter) && converter.Writer is not null)
                                {
                                    ParameterExpression obj = System.Linq.Expressions.Expression.Parameter(typeof(object));

                                    System.Linq.Expressions.Expression val = obj;

                                    Type parameterType = converter.Writer.Method.GetParameters()[0].ParameterType;

                                    if (val.Type != parameterType)
                                        val = System.Linq.Expressions.Expression.Convert(val, parameterType);

                                    val = System.Linq.Expressions.Expression.Invoke(System.Linq.Expressions.Expression.Constant(converter.Writer), val);

                                    writer = System.Linq.Expressions.Expression.Lambda<Func<object, object>>(val, obj);
                                }

                                validProperties.Add(property, new()
                                {
                                    Writer = writer?.Compile()
                                });
                            }

                            row[property] = propertyValue;
                        }

                        rows.Add(row);
                    }

                    insertExpression.Columns = validProperties.Select(s => s.Key).ToList();

                    List<SqlExpressions.ValuesExpression> rowExpressions = new();

                    foreach (Dictionary<PropertyInfo, IValue> row in rows)
                    {
                        List<SqlExpressions.IValueExpression> valueExpressions = new();

                        foreach (KeyValuePair<PropertyInfo, EntityPropertyInfo> entityProperty in validProperties)
                        {
                            if (row.TryGetValue(entityProperty.Key, out IValue value))
                            {
                                if (value.ValueType == ValueType.Value)
                                {
                                    valueExpressions.Add(new SqlExpressions.ParameterExpression(
                                        entityProperty.Value.Writer is null ? value.Object : entityProperty.Value.Writer(value.Object)
                                        ));
                                }
                                else
                                {
                                    LambdaExpression lambdaExpression = (LambdaExpression)value.Object;

                                    valueExpressions.Add((SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, new()));
                                }

                                continue;
                            }

                            valueExpressions.Add(SqlExpressions.SqlExpression.DefaultValue);
                        }

                        rowExpressions.Add(new(valueExpressions));
                    }

                    insertExpression.Rows = new SqlExpressions.InsertRowsExpression(rowExpressions);
                }
                else if (addOrUpdateWithFactoryOperateExpression.DataSource == AddDataSource.DataTable)
                {
                    insertExpression.Rows = (SqlExpressions.ISelectSql)GetDataManipulationSql(addOrUpdateWithFactoryOperateExpression.DataTable.Expression);
                }

                //Update
                string onDuplicateKeyUpdate = string.Empty;

                if (addOrUpdateWithFactoryOperateExpression.Update)
                {
                    insertExpression.OnDuplicateKeyUpdate = true;

                    insertExpression.UpdateFactoryVersion = _updateFactoryVersion;

                    if (addOrUpdateWithFactoryOperateExpression.UpdateFactory is not null)
                    {
                        MemberInitExpression memberInitExpression = addOrUpdateWithFactoryOperateExpression.UpdateFactory.Body as MemberInitExpression;

                        if (memberInitExpression is null)
                            throw new UnsupportedExpressionException(addOrUpdateWithFactoryOperateExpression.UpdateFactory.Body);

                        BlockExpression blockExpression = (BlockExpression)memberInitExpression.Reduce();

                        var sqlContext = new SqlExpressions.SqlContext()
                        {
                            LambdaParameters = new()
                            {
                                {
                                    addOrUpdateWithFactoryOperateExpression .UpdateFactory.Parameters[0],
                                    SqlExpressions.LambdaParameterInfo.Table(insertExpression.Table)
                                },
                                {
                                    addOrUpdateWithFactoryOperateExpression .UpdateFactory.Parameters[1],
                                    SqlExpressions.LambdaParameterInfo.Table(
                                        _updateFactoryVersion == UpdateFactoryVersion.V8_0  ?
                                            new SqlExpressions.OriginalTableExpression(insertExpression.Table.Schema, insertExpression.Table.Name) { Alias = SqlNode.NewRowAlias }
                                            : insertExpression.Table
                                            )
                                }
                            }
                        };

                        ColumnVisitor columnVisitor = new ColumnVisitor(addOrUpdateWithFactoryOperateExpression.UpdateFactory.Parameters[1]);

                        insertExpression.Assignments = blockExpression.Expressions.Skip(1).Take(blockExpression.Expressions.Count - 2).Select(memberInit =>
                        {
                            BinaryExpression assign = (BinaryExpression)memberInit;

                            var left = new SqlExpressions.MemberExpression(insertExpression.Table, ((MemberExpression)assign.Left).Member);

                            System.Linq.Expressions.Expression rightExpression = assign.Right;

                            if (_updateFactoryVersion == UpdateFactoryVersion.V5_7)
                                rightExpression = columnVisitor.Visit(rightExpression);

                            return new SqlExpressions.AssignmentExpression(left, (SqlExpressions.IValueExpression)GetSqlExpression(rightExpression, sqlContext));
                        }).ToList();
                    }
                    else
                    {
                        insertExpression.Assignments = insertExpression.Columns.Select(p =>
                        {
                            var valueExpression = new SqlExpressions.MemberExpression(insertExpression.Table, p);

                            return new SqlExpressions.AssignmentExpression(
                                valueExpression,
                                new SqlExpressions.MethodCallExpression(nameof(DbFunction.Values), valueExpression)
                                );
                        }).ToList();
                    }
                }

                return insertExpression.Build();
            }

            if (operateExpression is AddNextTableExpression addNextTableExpression)
            {
                Command command = GetOperateCommand(new AddOperateExpression(addNextTableExpression.Source, new object[] { addNextTableExpression.Element }));

                command.CommandText += ";\nSelect Last_Insert_Id();";

                return command;
            }

            if (operateExpression is RemoveOperateExpression removeOperateExpression)
            {
                return GetDataManipulationSql(removeOperateExpression).Build(this);
            }

            if (operateExpression is SetOperateExpression setOperateExpression)
            {
                return GetDataManipulationSql(setOperateExpression).Build(this);
            }

            throw new UnsupportedExpressionException(operateExpression);
        }

        CommandExtend Build(IDataManipulationSql sql, Dictionary<ParameterExpression, LambdaParameterInfo> parameters, int level, string cteAlias = null)
        {
            if (parameters is null)
                parameters = new();

            //if (sql is SelectSql dataManipulationSql)
            //    return BuildSql(dataManipulationSql, parameters, level);

            //if (sql is BinaryTableSql unionedSql)
            //    return BuildUnionSql(unionedSql, parameters, level, cteAlias);

            throw new Exception();
        }

        CommandExtend BuildSql(SqlExpressions.SelectExpression sql, Dictionary<ParameterExpression, LambdaParameterInfo> parameters, int level)
        {
            throw new NotImplementedException();

            //CommandExtend command = new();

            //command.CommandTimeout = sql.Timeout;

            //CommandResult result = new();

            //if (sql.SqlType == DataManipulationSqlType.Select)
            //    result.Type = GetResultType(sql.Type);

            //command.Results.Add(result);

            ////With
            //List<CTEInfo> ctes = new();

            ////From
            //string from = string.Empty;

            //List<EntityInfo> entityInfos = new();

            //if (sql.From is not null)
            //{
            //    List<string> tableTexts = new();

            //    for (int i = 0; i < sql.From.Count; i++)
            //    {
            //        var table = sql.From[i];

            //        string alias = string.Empty;

            //        string text = string.Empty;

            //        if (table.Table is OriginalTableExpression originalTable)
            //        {
            //            alias = SqlNode.GetTableAlias(i, level);

            //            text = FullName(originalTable).As(alias);
            //        }
            //        else if (table.Table is CTETableExpression cteTableExpression)
            //        {
            //            alias = cteTableExpression.Alias;

            //            text = alias;
            //        }
            //        else if (table.Table is IDataManipulationSql subQuery)
            //        {
            //            if (subQuery.IsCTE)
            //            {
            //                alias = SqlNode.GetCTEAlias(ctes.Count, level);

            //                ctes.Add(new()
            //                {
            //                    Alias = alias,
            //                    Sql = subQuery
            //                });

            //                text = alias;
            //            }
            //            else
            //            {
            //                var subQueryCommand = Build(subQuery, new(), 0);

            //                command.Parameters.AddRange(subQueryCommand.Parameters);

            //                alias = SqlNode.GetTableAlias(i, level);

            //                text = SqlNode.SubQuery(subQueryCommand.CommandText).As(alias);
            //            }
            //        }
            //        else
            //        {
            //            throw new Exception();
            //        }

            //        entityInfos.Add(new()
            //        {
            //            CommandText = alias,
            //            EntitySource = table.Table is OriginalTableExpression ? EntitySource.OriginalTable : EntitySource.SubQuery
            //        });

            //        if (table.LambdaExpression is not null)
            //        {
            //            var predicateValue = GetValueExpression(table.LambdaExpression.Body, new SqlContext(level)
            //            {
            //                Parameters = parameters.Concat(
            //                    table.LambdaExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i])))
            //                    ).ToDictionary(s => s.Key, s => s.Value)
            //            });

            //            text += " On " + predicateValue.CommantText;

            //            if (predicateValue.Parameters is not null)
            //                command.Parameters.AddRange(predicateValue.Parameters);
            //        }

            //        tableTexts.Add(table.Join + text);
            //    }

            //    from = (sql.SqlType == DataManipulationSqlType.Update ? string.Empty : "\nFrom") + "\n" + string.Join("\n", tableTexts).Indent(2);
            //            }

            ////Group By
            //            string groupBy = string.Empty;

            //            Dictionary<MemberInfo, string> groupKeys = new();

            //            if (sql.GroupBy is not null)
            //            {
            //                List<GroupKeyExpression> groupKeyExpressions = new();

            //                if (sql.GroupBy.Body is NewExpression keysEntityExpression)
            //                {
            //                    if (keysEntityExpression.Arguments.Count != keysEntityExpression.Members?.Count)
            //                        throw new UnsupportedExpressionException(keysEntityExpression);

            //                    groupKeyExpressions.AddRange(keysEntityExpression.Members.Zip(keysEntityExpression.Arguments, (a, b) => new GroupKeyExpression()
            //                    {
            //                        Member = a,
            //                        Expression = b
            //                    }));
            //                }
            //                else
            //                {
            //                    groupKeyExpressions.Add(new()
            //                    {
            //                        Member = _GroupingDataTableKey,
            //                        Expression = sql.GroupBy.Body
            //                    });
            //                }

            //                List<string> groupColumns = new();

            //                foreach (var groupKey in groupKeyExpressions)
            //                {
            //                    var keyCmd = GetValueExpression(groupKey.Expression, new SqlContext(level)
            //                    {
            //                        Parameters = parameters.Concat(
            //                            sql.GroupBy.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i])))
            //                            ).ToDictionary(s => s.Key, s => s.Value)
            //                    });

            //                    string keyText = keyCmd.CommantText?.ToString();

            //                    groupColumns.Add(keyText);

            //                    if (keyCmd.Parameters is not null)
            //                        command.Parameters.AddRange(keyCmd.Parameters);

            //                    groupKeys[groupKey.Member] = keyText;
            //                }

            //                groupBy = "\nGroup By\n  " + string.Join(", ", groupColumns);
            //            }

            //            //Manipulation
            //            string manipulation = string.Empty;

            //            //Select
            //            EntityInfo resultEntityInfo = entityInfos.FirstOrDefault();

            //            if (sql.SqlType == DataManipulationSqlType.Select)
            //            {
            //                List<string> columns = new();

            //                if (sql.HasIndex)
            //                    columns.Add(SqlNode.ColumnIndexValue.AsColumn(SqlNode.IndexColumnName));

            //                List<SqlColumnInfo> columnExpressions = new();

            //                if (sql.Select is null)
            //                {
            //                    ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(sql.Type);

            //                    sql.Select = System.Linq.Expressions.Expression.Lambda(parameter, parameter);
            //                }

            //                if (sql.Select is LambdaExpression[] lambdaExpressions)
            //                {
            //                    columnExpressions.AddRange(lambdaExpressions.Select(s =>
            //                    {
            //                        System.Linq.Expressions.Expression expression = s.Body;

            //                        if (expression.NodeType == ExpressionType.Convert && expression.Type == typeof(object))
            //                            expression = ((UnaryExpression)expression).Operand;

            //                        string alias = null;

            //                        if (expression is MemberExpression memberExpression)
            //                            alias = memberExpression.Member.Name;

            //                        return new SqlColumnInfo()
            //                        {
            //                            Alias = alias,
            //                            SqlContext = new SqlContext(level)
            //                            {
            //                                Parameters = parameters.Concat(
            //                                    s.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i])))
            //                                ).ToDictionary(s => s.Key, s => s.Value)
            //                            },
            //                            Expression = expression
            //                        };
            //                    }));
            //                }
            //                else if (sql.Select is LambdaExpression columnSelector)
            //                {
            //                    if (columnSelector.Body is MemberExpression memberExpression
            //                        && memberExpression.Expression is ParameterExpression
            //                        && memberExpression.Expression.Type.IsAssignableTo(typeof(IGroupingDataTable))
            //                        && memberExpression.Member.Name == nameof(IGroupingDataTable<int, int>.Element))
            //                    {
            //                        ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(memberExpression.Type);

            //                        columnSelector = System.Linq.Expressions.Expression.Lambda(parameter, parameter);
            //                    }

            //                    Dictionary<string, ConstructorInfo> constructors = new();

            //                    SqlContext sqlContext = new SqlContext(level)
            //                    {
            //                        Parameters = parameters.Concat(columnSelector.Parameters.Select((p, i) =>
            //                        {
            //                            LambdaParameterInfo lambdaParameterInfo = null;

            //                            if (i >= entityInfos.Count)
            //                            {
            //                                lambdaParameterInfo = LambdaParameterInfo.ColumnIndexValue;
            //                            }
            //                            else if (p.Type.IsAssignableTo(typeof(IGroupingDataTable)))
            //                            {
            //                                lambdaParameterInfo = LambdaParameterInfo.GroupingDataTable(groupKeys, entityInfos);
            //                            }
            //                            else
            //                            {
            //                                lambdaParameterInfo = LambdaParameterInfo.Entity(entityInfos[i]);
            //                            }

            //                            return KeyValuePair.Create(p, lambdaParameterInfo);
            //                        })).ToDictionary(s => s.Key, s => s.Value)
            //                    };

            //                    columnExpressions.AddRange(ExpandColumns(
            //                        new List<SqlColumnInfo>()
            //                        {
            //                            new SqlColumnInfo()
            //                            {
            //                                Alias = null,
            //                                Expression = columnSelector.Body
            //                            }
            //                        },
            //                        groupKeys,
            //                        ref constructors,
            //                        sqlContext.Parameters.Where(p => p.Value.ParameterType == LambdaParameterType.ColumnIndexValue).ToDictionary(p => p.Key, p => p.Value.CommandText)
            //                        ).Select(s =>
            //                        {
            //                            s.SqlContext = sqlContext;

            //                            return s;
            //                        })
            //                        );

            //                    foreach (var keyValue in constructors)
            //                    {
            //                        result.Constructors[keyValue.Key] = keyValue.Value;
            //                    }
            //                }
            //                else
            //                {
            //                    throw new Exception("Unsupported Selector");
            //                }

            //                SetClientReader(columnExpressions);

            //                command.ColumnProperties = columnExpressions.Select(s => s.PropertyInfo).ToList();

            //                for (int i = 0; i < columnExpressions.Count; i++)
            //                {
            //                    var column = columnExpressions[i];

            //                    if (column.Reader is not null)
            //                        result.Readers[i] = column.Reader;

            //                    var columnCmd = column.Expression is System.Linq.Expressions.Expression expression ?
            //                          GetValueExpression(expression, column.SqlContext) : new() { CommantText = column.Expression.ToString() };

            //                    string columnText = columnCmd.CommantText?.ToString();

            //                    if (column.Alias is not null && SqlNode.NameEqual(column.Alias, columnText) == false)
            //                        columnText = columnText.AsColumn(column.Alias);

            //                    columns.Add(columnText);

            //                    if (columnCmd.Parameters is not null)
            //                        command.Parameters.AddRange(columnCmd.Parameters);
            //                }

            //                manipulation = "Select " + (sql.Distinct ? "Distinct" : string.Empty) + "\n" + string.Join(",\n", columns).Indent(2);

            //                resultEntityInfo = new()
            //                {
            //                    CommandText = columns.Count == 1 ? columns[0] : null,
            //                    EntitySource = EntitySource.SubQuery
            //                };
            //            }

            //            //Delete
            //            if (sql.SqlType == DataManipulationSqlType.Delete)
            //            {
            //                manipulation = "Delete";

            //                if (sql.Delete is not null)
            //                {
            //                    manipulation += "\n" + string.Join(",\n", sql.Delete).Indent(2);
            //                }
            //            }

            //            //Update
            //            string set = string.Empty;

            //            if (sql.SqlType == DataManipulationSqlType.Update)
            //            {
            //                manipulation = "Update";

            //                List<string> assignments = sql.Set.Select(assignment =>
            //                {
            //                    System.Linq.Expressions.Expression left = null;

            //                    System.Linq.Expressions.Expression right = null;

            //                    if (assignment.Body.NodeType == ExpressionType.Assign)
            //                    {
            //                        BinaryExpression binaryExpression = (BinaryExpression)assignment.Body;

            //                        left = binaryExpression.Left;

            //                        right = binaryExpression.Right;
            //                    }
            //                    else if (assignment.Body is MethodCallExpression methodCallExpression
            //                        && methodCallExpression.Method.IsGenericMethod)
            //                    {
            //                        MethodInfo method = methodCallExpression.Method.GetGenericMethodDefinition();

            //                        if (method != _Assign1 && method != _Assign2 && method != _Assign3)
            //                            throw new UnsupportedExpressionException(assignment.Body);

            //                        left = methodCallExpression.Arguments[0];

            //                        right = methodCallExpression.Arguments[1];
            //                    }
            //                    else
            //                    {
            //                        throw new UnsupportedExpressionException(assignment.Body);
            //                    }

            //                    SqlContext sqlContext = new(level)
            //                    {
            //                        Parameters = assignment.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i])))
            //                            .ToDictionary(s => s.Key, s => s.Value)
            //                    };

            //                    var leftCmd = GetValueExpression(left, sqlContext);

            //                    var rightCmd = GetValueExpression(right, sqlContext);

            //                    if (leftCmd.Parameters is not null)
            //                        command.Parameters.AddRange(leftCmd.Parameters);

            //                    if (rightCmd.Parameters is not null)
            //                        command.Parameters.AddRange(rightCmd.Parameters);

            //                    if (leftCmd.CommantText is JsonAccess jsonAccess && jsonAccess.Valid)
            //                    {
            //                        return SqlNode.Assign(
            //                            jsonAccess.JsonDocument,
            //                            SqlNode.Call("Json_Set", jsonAccess.JsonDocument, jsonAccess.JsonPath, rightCmd.CommantText?.ToString())
            //                            );
            //                    }

            //                    return SqlNode.Assign(leftCmd.CommantText?.ToString(), rightCmd.CommantText?.ToString());
            //                }).ToList();

            //                set = "\nSet\n" + string.Join(",\n", assignments).Indent(2);
            //            }

            //            //Where
            //            string where = string.Empty;

            //            if (sql.Where is not null)
            //            {
            //                List<string> whereConditions = new();

            //                bool isMultiple = sql.Where.Count > 1;

            //                for (int i = 0; i < sql.Where.Count; i++)
            //                {
            //                    LambdaExpression predicateExpression = sql.Where[i];

            //                    var valueCmd = GetValueExpression(predicateExpression.Body, new SqlContext(level)
            //                    {
            //                        Parameters = parameters.Concat(predicateExpression.Parameters.Select((p, i) =>
            //                        {
            //                            LambdaParameterInfo lambdaParameterInfo = null;

            //                            if (i >= entityInfos.Count)
            //                            {
            //                                lambdaParameterInfo = LambdaParameterInfo.IndexColumnName;
            //                            }
            //                            else
            //                            {
            //                                lambdaParameterInfo = LambdaParameterInfo.Entity(entityInfos[i]);
            //                            }

            //                            return KeyValuePair.Create(p, lambdaParameterInfo);
            //                        })).ToDictionary(s => s.Key, s => s.Value)
            //                    }, i == 0);

            //                    if (isMultiple)
            //                    {
            //                        if (i == 0)
            //                        {
            //                            if (NeedLeftBracket(valueCmd.SqlOperator, SqlOperator.AndAlso))
            //                                valueCmd.CommantText = SqlNode.Bracket(valueCmd.CommantText?.ToString());
            //                        }
            //                        else
            //                        {
            //                            if (NeedRightBracket(SqlOperator.AndAlso, valueCmd.SqlOperator))
            //                                valueCmd.CommantText = SqlNode.Bracket(valueCmd.CommantText?.ToString());
            //                        }
            //                    }

            //                    whereConditions.Add(valueCmd.CommantText?.ToString());

            //                    if (valueCmd.Parameters is not null)
            //                        command.Parameters.AddRange(valueCmd.Parameters);
            //                }

            //                where = "\nWhere\n" + string.Join("\n" + SqlNode.AndAlso + " ", whereConditions).Indent(2);
            //            }

            //            //Having
            //            string having = string.Empty;

            //            if (sql.Having is not null)
            //            {
            //                List<string> havingConditions = new();

            //                bool isMultiple = sql.Having.Count > 1;

            //                for (int i = 0; i < sql.Having.Count; i++)
            //                {
            //                    LambdaExpression predicateExpression = sql.Having[i];

            //#if DEBUG
            //                    if (predicateExpression.Parameters.Count != 1)
            //                        throw new Exception();
            //#endif

            //                    var valueCmd = GetValueExpression(predicateExpression.Body, new SqlContext(level)
            //                    {
            //                        Parameters = parameters.Concat(
            //                            predicateExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(resultEntityInfo)))
            //                            ).ToDictionary(s => s.Key, s => s.Value)
            //                    }, i == 0);

            //                    if (isMultiple)
            //                    {
            //                        if (i == 0)
            //                        {
            //                            if (NeedLeftBracket(valueCmd.SqlOperator, SqlOperator.AndAlso))
            //                                valueCmd.CommantText = SqlNode.Bracket(valueCmd.CommantText?.ToString());
            //                        }
            //                        else
            //                        {
            //                            if (NeedRightBracket(SqlOperator.AndAlso, valueCmd.SqlOperator))
            //                                valueCmd.CommantText = SqlNode.Bracket(valueCmd.CommantText?.ToString());
            //                        }
            //                    }

            //                    havingConditions.Add(valueCmd.CommantText?.ToString());

            //                    if (valueCmd.Parameters is not null)
            //                        command.Parameters.AddRange(valueCmd.Parameters);
            //                }

            //                having = "\nHaving\n" + string.Join("\n" + SqlNode.AndAlso + " ", havingConditions).Indent(2);
            //            }

            //            //Order By
            //            string orderBy = string.Empty;

            //            if (sql.OrderBy is not null)
            //            {
            //                var orderByText = GetSqlOrderBy(sql.OrderBy, parameters, level, resultEntityInfo);

            //                orderBy = orderByText.CommandText;

            //                if (orderByText.Parameters is not null)
            //                    command.Parameters.AddRange(orderByText.Parameters);
            //            }

            //            //Limit
            //            string limit = string.Empty;

            //            if (sql.Limit is not null)
            //            {
            //                limit = GetSqlLimit(sql.Limit);
            //            }

            //            //with
            //            string with = string.Empty;

            //            if (ctes.Any())
            //            {
            //                var withText = GetSqlWith(ctes, parameters, level);

            //                with = withText.CommandText;

            //                if (withText.Parameters is not null)
            //                    command.Parameters.AddRange(withText.Parameters);
            //            }

            //            command.CommandText = with + manipulation + from + set + where + groupBy + having + orderBy + limit;

            //            return command;
        }

        CommandExtend BuildUnionSql(SqlExpressions.BinaryTableExpression sql, Dictionary<ParameterExpression, LambdaParameterInfo> parameters, int level, string cteAlias = null)
        {
            throw new NotImplementedException();

            //CommandExtend command = new();

            //command.CommandTimeout = sql.Timeout;

            ////With
            //List<CTEInfo> ctes = new();

            ////left
            //string left = string.Empty;

            //var leftCommand = Build(sql.Left, parameters, level);

            //left = SqlNode.Bracket("\n" + leftCommand.CommandText.Indent(2) + "\n");

            //command.Parameters.AddRange(leftCommand.Parameters);

            //command.Results.AddRange(leftCommand.Results);

            //command.ColumnProperties = leftCommand.ColumnProperties;

            ////right
            //string right = string.Empty;

            //if (sql.Right is IDataManipulationSql rightSql)
            //{
            //    if (rightSql.IsCTE)
            //    {
            //        string alias = SqlNode.GetCTEAlias(ctes.Count, level);

            //        ctes.Add(new()
            //        {
            //            Alias = alias,
            //            Sql = rightSql
            //        });

            //        right = BuildSql(new()
            //        {
            //            Select = _AllColumnsLambdaExpression,
            //            From = new()
            //                {
            //                    new()
            //                    {
            //                        Table = new CTETableExpression(alias, rightSql.Type)
            //                    }
            //                },
            //            Type = rightSql.Type
            //        }, parameters, level).CommandText;
            //    }
            //    else
            //    {
            //        var rightCommand = Build(rightSql, new(), 0);

            //        right = rightCommand.CommandText;

            //        command.Parameters.AddRange(rightCommand.Parameters);
            //    }
            //}
            //else if (sql.Right is LambdaExpression lambdaExpression) //cte
            //{
            //    var sqlValue = GetValueExpression(lambdaExpression.Body, new(level)
            //    {
            //        Parameters = parameters.Concat(
            //            lambdaExpression.Parameters.Select((p) => KeyValuePair.Create(p, LambdaParameterInfo.DataTable(new CTETableExpression(cteAlias, sql.Type))))
            //            ).ToDictionary(s => s.Key, s => s.Value)
            //    });

            //    right = sqlValue.CommantText?.ToString();

            //    if (sqlValue.Parameters is not null)
            //        command.Parameters.AddRange(sqlValue.Parameters);
            //}

            //right = SqlNode.Bracket("\n" + right.Indent(2) + "\n");

            ////Order By
            //string orderBy = string.Empty;

            //if (sql.OrderBy is not null)
            //{
            //    var orderByText = GetSqlOrderBy(sql.OrderBy, parameters, level, new EntityInfo() { EntitySource = EntitySource.SubQuery });

            //    orderBy = orderByText.CommandText;

            //    if (orderByText.Parameters is not null)
            //        command.Parameters.AddRange(orderByText.Parameters);
            //}

            ////Limit
            //string limit = string.Empty;

            //if (sql.Limit is not null)
            //{
            //    limit = GetSqlLimit(sql.Limit);
            //}

            ////with
            //string with = string.Empty;

            //if (ctes.Any())
            //{
            //    var withText = GetSqlWith(ctes, parameters, level);

            //    with = withText.CommandText;

            //    if (withText.Parameters is not null)
            //        command.Parameters.AddRange(withText.Parameters);
            //}

            //command.CommandText = with + left + "\n\n" + sql.BinaryTableOperate + "\n\n" + right + "\n" + orderBy + limit;

            //return command;
        }

        List<SqlColumnInfo> ExpandColumns(IEnumerable<SqlColumnInfo> nodes, Dictionary<MemberInfo, SqlExpressions.IValueExpression> groupKeys, ref Dictionary<string, ConstructorInfo> constructors, Dictionary<ParameterExpression, SqlExpressions.ISqlExpression> indexParemeters)
        {
            List<SqlColumnInfo> columns = new();

            foreach (var node in nodes)
            {
                if (node.Expression is NewExpression newExpression)
                {
                    constructors[node.Alias ?? string.Empty] = newExpression.Constructor;

                    List<SqlColumnInfo> ns = new();

                    if (newExpression.Members is not null)
                    {
                        ns.AddRange(newExpression.Members.Zip(newExpression.Arguments, (a, b) => new SqlColumnInfo()
                        {
                            Alias = SqlNode.Member(node.Alias, a.Name),
                            Expression = b,
                            Member = a
                        }));
                    }
                    else
                    {
                        ns.AddRange(newExpression.Constructor.GetParameters().Zip(newExpression.Arguments, (a, b) => new SqlColumnInfo()
                        {
                            Alias = SqlNode.Member(node.Alias, a.Name),
                            Expression = b
                        }));
                    }

                    columns.AddRange(ExpandColumns(ns, groupKeys, ref constructors, indexParemeters));

                    continue;
                }

                if (node.Expression is NewArrayExpression newArrayExpression)
                {
                    if (newArrayExpression.NodeType == ExpressionType.NewArrayBounds)
                        throw new UnsupportedExpressionException(newArrayExpression);

                    constructors[node.Alias ?? string.Empty] = newArrayExpression.Type.GetConstructors().First();

                    columns.AddRange(ExpandColumns(newArrayExpression.Expressions.Select((e, i) => new SqlColumnInfo()
                    {
                        Alias = SqlNode.Index(node.Alias, i.ToString()),
                        Expression = e
                    }), groupKeys, ref constructors, indexParemeters));

                    continue;
                }

                if (node.Expression is MemberInitExpression memberInitExpression)
                {
                    columns.AddRange(ExpandColumns(new List<SqlColumnInfo>()
                    {
                        new SqlColumnInfo()
                        {
                            Alias = node.Alias,
                            Expression = memberInitExpression.NewExpression
                        }
                    }, groupKeys, ref constructors, indexParemeters));

                    BlockExpression blockExpression = (BlockExpression)memberInitExpression.Reduce();

                    columns.AddRange(ExpandColumns(blockExpression.Expressions.Skip(1).Take(blockExpression.Expressions.Count - 2).Select(e =>
                    {
                        BinaryExpression assign = (BinaryExpression)e;

                        MemberInfo member = ((MemberExpression)assign.Left).Member;

                        return new SqlColumnInfo()
                        {
                            Alias = SqlNode.Member(node.Alias, member.Name),
                            Expression = assign.Right,
                            Member = member
                        };
                    }), groupKeys, ref constructors, indexParemeters));

                    continue;
                }

                if (node.Expression is ParameterExpression parameterExpression)
                {
                    if (indexParemeters.TryGetValue(parameterExpression, out SqlExpressions.ISqlExpression sqlExpression))
                    {
                        columns.Add(new()
                        {
                            Alias = node.Alias,
                            Expression = (SqlExpressions.IValueExpression)sqlExpression
                        });

                        continue;
                    }

                    foreach (PropertyInfo property in parameterExpression.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        columns.Add(new()
                        {
                            Alias = SqlNode.Member(node.Alias, property.Name),
                            Expression = System.Linq.Expressions.Expression.Property(parameterExpression, property),
                            Member = property
                        });
                    }

                    continue;
                }

                if (node.Expression is MemberExpression memberExpression)
                {
                    if (groupKeys is not null
                        && memberExpression.Expression is ParameterExpression
                        && memberExpression.Expression.Type.IsAssignableTo(typeof(IGroupingDataTable))
                        && memberExpression.Member.Name == nameof(IGroupingDataTable<int>.Key))
                    {
                        columns.AddRange(groupKeys.Select(k => new SqlColumnInfo()
                        {
                            Alias = SqlNode.Member(node.Alias, k.Key == _GroupingDataTableKey ? null : k.Key.Name),
                            Expression = k.Value,
                            Member = k.Key
                        }));

                        continue;
                    }

                    node.Member = memberExpression.Member;

                    if (node.Alias is null)
                        node.Alias = memberExpression.Member.Name;

                    columns.Add(node);

                    continue;
                }

                columns.Add(node);
            }

            return columns;
        }

        void SetClientReader(List<SqlColumnInfo> columnInfos)
        {
            foreach (var node in columnInfos)
            {
                if (node.Expression is InvocationExpression invocationExpression)
                {
                    if (invocationExpression.Arguments.Count != 1)
                        throw new UnsupportedExpressionException(invocationExpression);

                    var sqlExpression = GetSqlExpression(invocationExpression.Expression, null);

                    if (sqlExpression is not SqlExpressions.IObjectExpression objectExpression)
                        throw new UnsupportedExpressionException(invocationExpression);

                    node.Reader = (Delegate)objectExpression.Value;

                    node.Expression = invocationExpression.Arguments[0];
                }
                else if (node.Expression is MethodCallExpression methodCallExpression)
                {
                    MethodInfo method = methodCallExpression.Method;

                    if (method.IsGenericMethod)
                        method = method.GetGenericMethodDefinition();

                    if (method == _Read)
                    {
                        System.Linq.Expressions.Expression readerExpression = methodCallExpression.Arguments[1];

                        if (readerExpression is LambdaExpression lambdaExpression)
                        {
                            node.Reader = lambdaExpression.Compile();
                        }
                        else
                        {
                            var sqlExpression = GetSqlExpression(readerExpression, new());

                            if (sqlExpression is not SqlExpressions.IObjectExpression objectExpression)
                                throw new UnsupportedExpressionException(readerExpression);

                            node.Reader = (Delegate)objectExpression.Value;
                        }

                        node.Expression = methodCallExpression.Arguments[0];
                    }
                    else if (method == _ReadBytes)
                    {
                        System.Linq.Expressions.Expression readerExpression = methodCallExpression.Arguments[1];

                        if (readerExpression is LambdaExpression lambdaExpression)
                        {
                            node.Reader = lambdaExpression.Compile();
                        }
                        else
                        {
                            var sqlExpression = GetSqlExpression(readerExpression, new());

                            if (sqlExpression is not SqlExpressions.IObjectExpression objectExpression)
                                throw new UnsupportedExpressionException(readerExpression);

                            node.Reader = (Delegate)objectExpression.Value;
                        }

                        node.Expression = methodCallExpression.Arguments[0];
                    }
                    else if (method == _ReadChars)
                    {
                        System.Linq.Expressions.Expression readerExpression = methodCallExpression.Arguments[1];

                        if (readerExpression is LambdaExpression lambdaExpression)
                        {
                            node.Reader = lambdaExpression.Compile();
                        }
                        else
                        {
                            var sqlExpression = GetSqlExpression(readerExpression, new());

                            if (sqlExpression is not SqlExpressions.IObjectExpression objectExpression)
                                throw new UnsupportedExpressionException(readerExpression);

                            node.Reader = (Delegate)objectExpression.Value;
                        }

                        node.Expression = methodCallExpression.Arguments[0];
                    }
                    else if (method.IsStatic && method.GetParameters().Length == 1 && TryGetMemberFormat(method, out _) == false)
                    {
                        node.Reader = method.CreateDelegate(typeof(Func<,>).MakeGenericType(methodCallExpression.Type, methodCallExpression.Arguments[0].Type));

                        node.Expression = methodCallExpression.Arguments[0];
                    }
                }
                else if (node.Expression is MemberExpression entityMemberExpression
                    && entityMemberExpression.Member.MemberType == MemberTypes.Property
                    && PropertyConvert.TryGetValue((PropertyInfo)entityMemberExpression.Member, out ValueConverter converter))
                {
                    node.Reader = converter.Reader;
                }
            }
        }

        internal List<SqlExpressions.ColumnInfo> GetSelectColumns(SqlExpressions.SelectExpression selectExpression, object selector, ref Dictionary<string, ConstructorInfo> constructors, SqlExpressions.SqlContext context)
        {
            List<SqlExpressions.ColumnInfo> columns = new();

            List<SqlColumnInfo> columnInfos = new();

            if (selector is null)
            {
                ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(selectExpression.Type);

                selector = System.Linq.Expressions.Expression.Lambda(parameter, parameter);
            }

            if (selector is LambdaExpression[] lambdaExpressions)
            {
                columnInfos.AddRange(lambdaExpressions.Select(s =>
                {
                    System.Linq.Expressions.Expression expression = s.Body;

                    if (expression.NodeType == ExpressionType.Convert && expression.Type == typeof(object))
                        expression = ((UnaryExpression)expression).Operand;

                    return new SqlColumnInfo()
                    {
                        Alias = expression is MemberExpression memberExpression ? memberExpression.Member.Name : null,
                        Expression = expression,
                        SqlContext = context.ConcatParameters(
                            s.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.Table(selectExpression.From.GetTable(i))))
                        )
                    };
                }));
            }
            else if (selector is LambdaExpression columnSelector)
            {
                if (columnSelector.Body is MemberExpression memberExpression
                    && memberExpression.Expression is ParameterExpression
                    && memberExpression.Expression.Type.IsAssignableTo(typeof(IGroupingDataTable))
                    && memberExpression.Member.Name == nameof(IGroupingDataTable<int, int>.Element))
                {
                    ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(memberExpression.Type);

                    columnSelector = System.Linq.Expressions.Expression.Lambda(parameter, parameter);
                }

                SqlExpressions.SqlContext sqlContext = context.ConcatParameters(
                    columnSelector.Parameters.Select((p, i) =>
                    {
                        SqlExpressions.LambdaParameterInfo lambdaParameterInfo = null;

                        int tableCount = selectExpression.From.Count;

                        if (i >= tableCount)
                        {
                            lambdaParameterInfo = SqlExpressions.LambdaParameterInfo.ColumnIndexValue;
                        }
                        else if (p.Type.IsAssignableTo(typeof(IGroupingDataTable)))
                        {
                            lambdaParameterInfo = SqlExpressions.LambdaParameterInfo.GroupingDataTable(selectExpression.From, selectExpression.GroupBy);
                        }
                        else
                        {
                            lambdaParameterInfo = SqlExpressions.LambdaParameterInfo.Table(selectExpression.From.GetTable(i));
                        }

                        return KeyValuePair.Create(p, lambdaParameterInfo);
                    })
                    );

                columnInfos.AddRange(ExpandColumns(
                    new List<SqlColumnInfo>()
                    {
                         new SqlColumnInfo()
                         {
                             Alias = null,
                             Expression = columnSelector.Body
                         }
                    },
                    selectExpression.GroupBy,
                    ref constructors,
                    new(sqlContext.LambdaParameters.Where(p => p.Value.ParameterType == SqlExpressions.LambdaParameterType.ColumnIndexValue).Select(p => KeyValuePair.Create(p.Key, p.Value.SqlExpression)))
                    ));

                columnInfos.ForEach(s => s.SqlContext = sqlContext);
            }

            SetClientReader(columnInfos);

            columns.AddRange(columnInfos.Select(s =>
            {
                SqlExpressions.IValueExpression valueExpression = s.Expression as SqlExpressions.IValueExpression;

                if (valueExpression is null)
                {
                    valueExpression = (SqlExpressions.IValueExpression)GetSqlExpression((System.Linq.Expressions.Expression)s.Expression, s.SqlContext);
                }

                SqlExpressions.ColumnInfo columnInfo = new()
                {
                    ValueExpression = valueExpression,
                    Member = s.Member,
                    Reader = s.Reader
                };

                if (s.Alias is not null)
                    columnInfo.Alias = s.Alias;

                return columnInfo;
            }));

            return columns;
        }

        SqlExpressions.ISqlExpression GetSqlExpression(System.Linq.Expressions.Expression expression, SqlExpressions.SqlContext context)
        {
            if (expression is null)
                return null;

            if (expression.NodeType == ExpressionType.Quote)
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression;

                return GetSqlExpression(unaryExpression.Operand, context);
            }

            if (expression.NodeType == ExpressionType.Constant)
            {
                ConstantExpression constantExpression = (ConstantExpression)expression;

                return TryUnboxSubquery(constantExpression.Value, out var sqlExpression) ?
                    sqlExpression : new SqlExpressions.ConstantExpression(constantExpression.Value);
            }

            if (expression.NodeType == ExpressionType.Parameter)
            {
                return context.LambdaParameters[(ParameterExpression)expression].SqlExpression;
            }

            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)expression;

                var sqlExpression = GetSqlExpression(memberExpression.Expression, context);

                if (memberExpression.Expression is null || sqlExpression is SqlExpressions.IObjectExpression)
                {
                    object instance = memberExpression.Expression is null ? null : ((SqlExpressions.IObjectExpression)sqlExpression).Value;

                    object obj;

                    if (memberExpression.Member is PropertyInfo property)
                    {
                        obj = property.GetValue(instance);
                    }
                    else if (memberExpression.Member is FieldInfo field)
                    {
                        obj = field.GetValue(instance);
                    }
                    else
                    {
                        throw new UnsupportedExpressionException(expression);
                    }

                    return TryUnboxSubquery(obj, out var memberSqlExpression) ? memberSqlExpression : new SqlExpressions.ParameterExpression(obj);
                }

                if (TryGetMemberFormat(memberExpression.Member, out object format) && format is string formatStr)
                {
                    return new SqlExpressions.RawSqlExpression(formatStr, (SqlExpressions.IValueExpression)sqlExpression);
                }

                if (memberExpression.Expression.Type.IsAssignableTo(typeof(IGroupingDataTable)))
                {
                    if (memberExpression.Member.Name == nameof(IGroupingDataTable<int>.Key))
                    {
                        return new SqlExpressions.GroupKeyExpression(((SqlExpressions.GroupingDataTableExpression)sqlExpression).Members);
                    }
                    else if (memberExpression.Member.Name == nameof(IGroupingDataTable<int, int>.Element))
                    {
                        return ((SqlExpressions.GroupingDataTableExpression)sqlExpression).From.GetTable(0);
                    }
                }

                if (sqlExpression is SqlExpressions.JsonExtractExpression jsonExtractExpression)
                {
                    jsonExtractExpression.Member(memberExpression.Member);

                    return jsonExtractExpression;
                }

                if (sqlExpression is SqlExpressions.MemberExpression sqlMemberExpression)
                {
                    SqlExpressions.JsonExtractExpression sqlJsonExtractExpression = new SqlExpressions.JsonExtractExpression(sqlMemberExpression);

                    sqlJsonExtractExpression.Member(memberExpression.Member);

                    return sqlJsonExtractExpression;
                }

                return new SqlExpressions.MemberExpression(sqlExpression, memberExpression.Member);
            }

            if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked || expression.NodeType == ExpressionType.TypeAs)
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression;

                var sqlExpression = GetSqlExpression(unaryExpression.Operand, context);

                if (unaryExpression.Operand.Type == typeof(char) && unaryExpression.Type == typeof(int))
                {
                    return new SqlExpressions.MethodCallExpression("ASCII", (SqlExpressions.IValueExpression)sqlExpression);
                }

                if (unaryExpression.Operand.Type == typeof(int) && unaryExpression.Type == typeof(char))
                {
                    return new SqlExpressions.MethodCallExpression("Char", (SqlExpressions.IValueExpression)sqlExpression);
                }

                DataType? targetType = unaryExpression.Type.DbType();
                DataType? sourceType = unaryExpression.Operand.Type.DbType();

                if (targetType is null || targetType == sourceType)
                    return sqlExpression;

                return new SqlExpressions.CastExpression((SqlExpressions.IValueExpression)sqlExpression, targetType.Value);
            }

            if (expression.NodeType == ExpressionType.Negate || expression.NodeType == ExpressionType.NegateChecked)
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression;

                return new SqlExpressions.NegateExpression((SqlExpressions.IValueExpression)GetSqlExpression(unaryExpression.Operand, context));
            }

            if (expression.NodeType == ExpressionType.Not)
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression;

                return new SqlExpressions.NotExpression((SqlExpressions.IValueExpression)GetSqlExpression(unaryExpression.Operand, context));
            }

            if (expression.NodeType == ExpressionType.ArrayLength)
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression;

                return new SqlExpressions.MethodCallExpression("Json_Length", (SqlExpressions.IValueExpression)GetSqlExpression(unaryExpression.Operand, context));
            }

            if (expression is BinaryExpression binaryCallExpression && binaryCallExpression.Method is not null && MemberFormat.ContainsKey(binaryCallExpression.Method))
            {
                ParameterInfo[] parameterInfos = binaryCallExpression.Method.GetParameters();

                System.Linq.Expressions.Expression methodArg1 = binaryCallExpression.Left;

                Type parameterType1 = parameterInfos[0].ParameterType;

                if (methodArg1.Type != parameterType1)
                    methodArg1 = System.Linq.Expressions.Expression.Convert(methodArg1, parameterType1);

                System.Linq.Expressions.Expression methodArg2 = binaryCallExpression.Right;

                Type parameterType2 = parameterInfos[1].ParameterType;

                if (methodArg2.Type != parameterType2)
                    methodArg2 = System.Linq.Expressions.Expression.Convert(methodArg2, parameterType2);

                return GetSqlExpression(System.Linq.Expressions.Expression.Call(null, binaryCallExpression.Method, methodArg1, methodArg2), context);
            }

            if (expression.NodeType == ExpressionType.GreaterThan)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.GreaterThanExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.GreaterThanOrEqualExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.LessThan)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.LessThanExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.LessThanOrEqual)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.LessThanOrEqualExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.Equal)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                System.Linq.Expressions.Expression left = binaryExpression.Left;

                System.Linq.Expressions.Expression right = binaryExpression.Right;

                if (IsEnumCompare(binaryExpression))
                {
                    left = ((UnaryExpression)left).Operand;
                    right = ((UnaryExpression)right).Operand;
                }

                return new SqlExpressions.EqualExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.NotEqual)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                System.Linq.Expressions.Expression left = binaryExpression.Left;

                System.Linq.Expressions.Expression right = binaryExpression.Right;

                if (IsEnumCompare(binaryExpression))
                {
                    left = ((UnaryExpression)left).Operand;
                    right = ((UnaryExpression)right).Operand;
                }

                return new SqlExpressions.NotEqualExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.And)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.AndExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.AndAlso)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.AndAlsoExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.Or)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.OrExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.OrElse)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.OrElseExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.Add || expression.NodeType == ExpressionType.AddChecked)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.AddExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.Subtract || expression.NodeType == ExpressionType.SubtractChecked)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.SubtractExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.Multiply || expression.NodeType == ExpressionType.MultiplyChecked)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.MultiplyExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.Divide)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.DivideExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.Modulo)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.ModuloExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.ArrayIndex)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                var left = GetSqlExpression(binaryExpression.Left, context);

                SqlExpressions.JsonExtractExpression jsonExtractExpression = left as SqlExpressions.JsonExtractExpression ?? new((SqlExpressions.IValueExpression)left);

                jsonExtractExpression.Index((SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context));

                return jsonExtractExpression;
            }

            if (expression.NodeType == ExpressionType.Coalesce)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.MethodCallExpression(
                    "IfNull",
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.ExclusiveOr)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.ExclusiveOrExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.LeftShift)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.LeftShiftExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.RightShift)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;

                return new SqlExpressions.RightShiftExpression(
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Left, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(binaryExpression.Right, context)
                    );
            }

            if (expression.NodeType == ExpressionType.Call)
            {
                MethodCallExpression methodCallExpression = (MethodCallExpression)expression;

                MethodInfo method = methodCallExpression.Method;

                //获取format
                bool hasFormat = TryGetMemberFormat(method, out object format);

                //SubQuery
                if (hasFormat && format is Func<MethodCallExpression, SqlExpressions.SqlContext, SqlExpressions.ISqlExpression> formatFunc)
                {
                    return formatFunc(methodCallExpression, context);
                }

                //Object 和 参数
                List<ParameterValueExpressionInfo> valueExpressions = new();

                //获取 Object
                SqlExpressions.IValueExpression objectValueExpression = methodCallExpression.Object is null ? new SqlExpressions.ConstantExpression(null) : (SqlExpressions.IValueExpression)GetSqlExpression(methodCallExpression.Object, context);

                if (objectValueExpression is not null)
                {
                    valueExpressions.Add(new()
                    {
                        Value = objectValueExpression
                    });
                }

                //获取 参数
                ParameterInfo[] parameterInfos = methodCallExpression.Method.GetParameters();

                for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
                {
                    var arg = methodCallExpression.Arguments[i];

                    ParameterInfo parameterInfo = parameterInfos[i];

                    if (parameterInfos[i].IsDefined(typeof(ParamArrayAttribute), true))
                    {
                        List<SqlExpressions.IValueExpression> paramValues = new();

                        NewArrayExpression newArrayExpression = (NewArrayExpression)arg;

                        foreach (var element in newArrayExpression.Expressions)
                        {
                            paramValues.Add((SqlExpressions.IValueExpression)GetSqlExpression(element, context));
                        }

                        valueExpressions.Add(new()
                        {
                            Values = paramValues,
                            IsParamArray = true
                        });

                        continue;
                    }

                    var argExpression = GetSqlExpression(arg, context);

                    valueExpressions.Add(new()
                    {
                        Value = (SqlExpressions.IValueExpression)argExpression
                    });
                }

                //format
                if (hasFormat)
                {
                    if (format is string formatStr)
                    {
                        return new SqlExpressions.RawSqlExpression(formatStr, valueExpressions.Select(s =>
                        {
                            if (s.IsParamArray)
                                return new SqlExpressions.ParameterArrayExpression(s.Values.ToArray());

                            return s.Value;
                        }).ToArray());
                    }

                    if (format is Func<string[], string> convert)
                    {
                        return new SqlExpressions.RawSqlFactoryExpression(convert, valueExpressions.SelectMany(s => s.IsParamArray ? s.Values : new SqlExpressions.IValueExpression[] { s.Value }).ToArray());
                    }

                    throw new UnsupportedExpressionException(methodCallExpression);
                }

                //常量值
                if (valueExpressions.All(s => s.IsConstant))
                {
                    object obj = method.Invoke(
                        ((SqlExpressions.ConstantExpression)valueExpressions[0].Value).Value,
                        valueExpressions.Skip(1).Select(s => s.ConstantValue).ToArray()
                    );

                    return TryUnboxSubquery(obj, out var sqlExpression) ? sqlExpression : new SqlExpressions.ParameterExpression(obj);
                }

                //索引器
                if (IsIndexMethod(method, out bool indexIsNumber))
                {
                    SqlExpressions.JsonExtractExpression jsonExtractExpression = objectValueExpression as SqlExpressions.JsonExtractExpression ?? new(objectValueExpression);

                    if (indexIsNumber)
                        jsonExtractExpression.Index(valueExpressions[1].Value);
                    else
                        jsonExtractExpression.Member(valueExpressions[1].Value);

                    return jsonExtractExpression;
                }

                return new SqlExpressions.MethodCallExpression(
                    methodCallExpression.Method.Name,
                    (methodCallExpression.Object is null ? valueExpressions.Skip(1) : valueExpressions).SelectMany(s => s.IsParamArray ? s.Values : new SqlExpressions.IValueExpression[] { s.Value }).ToArray()
                    );
            }

            if (expression.NodeType == ExpressionType.Conditional)
            {
                ConditionalExpression conditionalExpression = (ConditionalExpression)expression;

                return new SqlExpressions.MethodCallExpression(
                    "If",
                    (SqlExpressions.IValueExpression)GetSqlExpression(conditionalExpression.Test, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(conditionalExpression.IfTrue, context),
                    (SqlExpressions.IValueExpression)GetSqlExpression(conditionalExpression.IfFalse, context)
                    );
            }

            throw new UnsupportedExpressionException(expression);
        }

        bool TryUnboxSubquery(object obj, out SqlExpressions.ISqlExpression sqlExpression)
        {
            sqlExpression = null;

            if (obj is not null && obj.GetType().IsAssignableTo(typeof(IDataTable)))
            {
                TableExpression tableExpression = ((IDataTable)obj).Expression;

                var sql = GetDataManipulationSql(tableExpression);

                if (sql is SqlExpressions.OriginalTableExpression originalTableExpression)
                {
                    sqlExpression = originalTableExpression;

                    return true;
                }

                if (sql is SqlExpressions.CommonTableExpression commonTableExpression)
                {
                    commonTableExpression.CanModify = false;

                    sqlExpression = commonTableExpression;
                }
                else
                {
                    sqlExpression = new SqlExpressions.CommonTableExpression((SqlExpressions.ISelectSql)sql, false);
                }

                return true;
            }

            return false;
        }

        SqlValue GetValueExpression(System.Linq.Expressions.Expression expression, SqlContext context, bool isRoot = false)
        {
            if (expression is null)
                return SqlValue.Constant(null);

            if (expression is ConstantExpression constantExpression)
            {
                if (constantExpression.Type.IsAssignableTo(typeof(IDataTable)) && context.GetTableExpression == false)
                {
                    var command = Build(GetDataManipulationSql(((IDataTable)constantExpression.Value).Expression), context.Parameters, context.Level + 1);

                    return new()
                    {
                        CommantText = SqlNode.SubQuery(command.CommandText),
                        Parameters = command.Parameters
                    };
                }

                return SqlValue.Constant(constantExpression.Value);
            }

            if (expression is ParameterExpression parameterExpression)
            {
                var lambdaParameterInfo = context.Parameters[parameterExpression];

                if (lambdaParameterInfo.ParameterType == LambdaParameterType.DataTable)
                {
                    var sqlValue = SqlValue.TableExpression(lambdaParameterInfo.TableExpression);

                    sqlValue.CommantText = Build(GetDataManipulationSql(lambdaParameterInfo.TableExpression), new(), 0).CommandText;

                    return sqlValue;
                }

                return new()
                {
                    CommantText = lambdaParameterInfo.CommandText,
                    LambdaParameterInfo = lambdaParameterInfo
                };
            }

            if (expression is MemberExpression memberExpression)
            {
                MemberInfo member = memberExpression.Member;

                var instanceCmd = GetValueExpression(memberExpression.Expression, context);

                if (TryGetMemberFormat(member, out object format))
                {
                    if (format is string formatStr)
                    {
                        return new()
                        {
                            CommantText = string.Format(formatStr, instanceCmd.CommantText),
                            Parameters = instanceCmd.Parameters
                        };
                    }
                }

                if (instanceCmd.IsConstant)
                {
                    if (member is PropertyInfo property)
                    {
                        return SqlValue.Constant(property.GetValue(instanceCmd.ConstantValue));
                    }
                    else if (member is FieldInfo field)
                    {
                        return SqlValue.Constant(field.GetValue(instanceCmd.ConstantValue));
                    }
                }

                if (instanceCmd.ValueType == SqlValueValueType.GroupKey)
                {
                    return new()
                    {
                        CommantText = instanceCmd.GroupKeys[member]
                    };
                }

                LambdaParameterType? lambdaParameterType = instanceCmd.LambdaParameterInfo?.ParameterType;

                if (lambdaParameterType == LambdaParameterType.GroupingDataTable)
                {
                    if (member.Name == nameof(IGroupingDataTable<int, int>.Element))
                    {
                        return new()
                        {
                            CommantText = instanceCmd.LambdaParameterInfo.FromTables[0].CommandText,
                            LambdaParameterInfo = LambdaParameterInfo.Entity(instanceCmd.LambdaParameterInfo.FromTables[0]),
                            ValueType = SqlValueValueType.GroupElement
                        };
                    }
                    else if (member.Name == nameof(IGroupingDataTable<int>.Key))
                    {
                        return new()
                        {
                            CommantText = instanceCmd.LambdaParameterInfo.GroupKeys.TryGetValue(_GroupingDataTableKey, out string keyCmd) ? keyCmd : null,
                            ValueType = SqlValueValueType.GroupKey,
                            GroupKeys = instanceCmd.LambdaParameterInfo.GroupKeys
                        };
                    }
                }
                else if (lambdaParameterType == LambdaParameterType.Entity)
                {
                    if (instanceCmd.LambdaParameterInfo.EntitySource == EntitySource.OriginalTable)
                    {
                        var text = new JsonAccess(SqlNode.Member(instanceCmd.CommantText?.ToString(), SqlNode.SqlName(SqlNode.ColumnName(member))));

                        return new()
                        {
                            CommantText = text,
                            Parameters = instanceCmd.Parameters
                        };
                    }
                    else if (instanceCmd.LambdaParameterInfo.EntitySource == EntitySource.SubQuery)
                    {
                        return new()
                        {
                            CommantText = new JsonAccess(SqlNode.Member(instanceCmd.CommantText?.ToString(), SqlNode.SqlName(member.Name))),
                            Parameters = instanceCmd.Parameters
                        };
                    }
                }
                else
                {
                    if (memberExpression.Expression is null)
                    {
                        return new()
                        {
                            CommantText = SqlNode.Member(member.DeclaringType.Name, member.Name),
                            Parameters = instanceCmd.Parameters
                        };
                    }
                    else
                    {
                        if (instanceCmd.CommantText is JsonAccess jsonAccess)
                        {
                            return new()
                            {
                                CommantText = SqlNode.JsonMember(jsonAccess, SqlNode.SqlString(member.Name)),
                                Parameters = instanceCmd.Parameters
                            };
                        }
                        else
                        {
                            return new()
                            {
                                CommantText = SqlNode.Member(instanceCmd.CommantText?.ToString(), SqlNode.SqlName(member.Name)),
                                Parameters = instanceCmd.Parameters
                            };
                        }
                    }
                }

                throw new UnsupportedExpressionException(memberExpression);
            }

            if (expression is UnaryExpression unaryExpression)
            {
                var operandCommand = GetValueExpression(unaryExpression.Operand, context);

                if (unaryExpression.NodeType == ExpressionType.Quote)
                    return operandCommand;

                if (IsConvertExpression(unaryExpression))
                {
                    if (unaryExpression.Operand.Type == typeof(char) && unaryExpression.Type == typeof(int))
                    {
                        return new()
                        {
                            CommantText = SqlNode.Call("ASCII", operandCommand.CommantText?.ToString()),
                            Parameters = operandCommand.Parameters
                        };
                    }

                    if (unaryExpression.Operand.Type == typeof(int) && unaryExpression.Type == typeof(char))
                    {
                        return new()
                        {
                            CommantText = SqlNode.Call("Char", operandCommand.CommantText?.ToString()),
                            Parameters = operandCommand.Parameters
                        };
                    }

                    DataType? targetType = unaryExpression.Type.DbType();
                    DataType? sourceType = unaryExpression.Operand.Type.DbType();

                    if (targetType is null || targetType == sourceType)
                        return operandCommand;

                    return new()
                    {
                        CommantText = SqlNode.Cast(operandCommand.CommantText?.ToString(), targetType.Value),
                        Parameters = operandCommand.Parameters
                    };
                }

                if (unaryExpression.NodeType == ExpressionType.Negate || unaryExpression.NodeType == ExpressionType.NegateChecked)
                {
                    string text = operandCommand.CommantText?.ToString();

                    if (NeedRightBracket(SqlOperator.Negate, operandCommand.SqlOperator))
                        text = SqlNode.Bracket(text);

                    return new()
                    {
                        CommantText = $"-{text}",
                        Parameters = operandCommand.Parameters
                    };
                }

                if (unaryExpression.NodeType == ExpressionType.Not)
                {
                    return new()
                    {
                        CommantText = $"Not ({operandCommand.CommantText})",
                        Parameters = operandCommand.Parameters
                    };
                }

                if (unaryExpression.NodeType == ExpressionType.ArrayLength)
                {
                    return new()
                    {
                        CommantText = SqlNode.Call("Json_Length", operandCommand.CommantText?.ToString()),
                        Parameters = operandCommand.Parameters
                    };
                }

                throw new UnsupportedExpressionException(expression);
            }

            if (expression is BinaryExpression binaryExpression)
            {
                if (binaryExpression.Method is not null && MemberFormat.ContainsKey(binaryExpression.Method))
                {
                    ParameterInfo[] parameterInfos = binaryExpression.Method.GetParameters();

                    System.Linq.Expressions.Expression methodArg1 = binaryExpression.Left;

                    Type parameterType1 = parameterInfos[0].ParameterType;

                    if (methodArg1.Type != parameterType1)
                        methodArg1 = System.Linq.Expressions.Expression.Convert(methodArg1, parameterType1);

                    System.Linq.Expressions.Expression methodArg2 = binaryExpression.Right;

                    Type parameterType2 = parameterInfos[1].ParameterType;

                    if (methodArg2.Type != parameterType2)
                        methodArg2 = System.Linq.Expressions.Expression.Convert(methodArg2, parameterType2);

                    return GetValueExpression(System.Linq.Expressions.Expression.Call(null, binaryExpression.Method, methodArg1, methodArg2), context);
                }

                System.Linq.Expressions.Expression leftExpression = binaryExpression.Left;
                System.Linq.Expressions.Expression rightExpression = binaryExpression.Right;

                //Enum Equal
                if (IsEnumCompare(binaryExpression))
                {
                    leftExpression = ((UnaryExpression)binaryExpression.Left).Operand;
                    rightExpression = ((UnaryExpression)binaryExpression.Right).Operand;
                }

                var left = GetValueExpression(leftExpression, context, isRoot);

                var right = GetValueExpression(rightExpression, context);

                List<KeyValuePair<string, object>> ps = new();

                if (left.Parameters != null)
                    ps.AddRange(left.Parameters);

                if (right.Parameters != null)
                    ps.AddRange(right.Parameters);

                object cmdText = null;

                SqlOperator? sqlOperator = null;

                if (binaryExpression.NodeType == ExpressionType.GreaterThan)
                {
                    sqlOperator = SqlOperator.GreaterThan;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.GreaterThan} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual)
                {
                    sqlOperator = SqlOperator.GreaterThanOrEqual;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.GreaterThanOrEqual} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.LessThan)
                {
                    sqlOperator = SqlOperator.LessThan;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.LessThan} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.LessThanOrEqual)
                {
                    sqlOperator = SqlOperator.LessThanOrEqual;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.LessThanOrEqual} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.Equal)
                {
                    sqlOperator = SqlOperator.Equal;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {((right.IsConstant && right.ConstantValue is null) ? SqlNode.Is : SqlNode.Equal)} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.NotEqual)
                {
                    sqlOperator = SqlOperator.NotEqual;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {((right.IsConstant && right.ConstantValue is null) ? SqlNode.IsNot : SqlNode.NotEqual)} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.And)
                {
                    sqlOperator = SqlOperator.And;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.And} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.AndAlso)
                {
                    sqlOperator = SqlOperator.AndAlso;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {(isRoot ? "\n" : string.Empty) + SqlNode.AndAlso} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.Or)
                {
                    sqlOperator = SqlOperator.Or;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.Or} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.OrElse)
                {
                    sqlOperator = SqlOperator.OrElse;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {(isRoot ? "\n " : string.Empty) + SqlNode.OrElse} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.Add || binaryExpression.NodeType == ExpressionType.AddChecked)
                {
                    sqlOperator = SqlOperator.Add;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.Add} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.Subtract || binaryExpression.NodeType == ExpressionType.SubtractChecked)
                {
                    sqlOperator = SqlOperator.Subtract;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.Subtract} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.Multiply || binaryExpression.NodeType == ExpressionType.MultiplyChecked)
                {
                    sqlOperator = SqlOperator.Multiply;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.Multiply} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.Divide)
                {
                    sqlOperator = SqlOperator.Divide;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.Divide} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.Modulo)
                {
                    sqlOperator = SqlOperator.Modulo;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.Modulo} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.ArrayIndex)
                {
                    cmdText = SqlNode.JsonIndex(
                        left.CommantText as JsonAccess ?? new JsonAccess(left.CommantText?.ToString()),
                        right.CommantText?.ToString()
                        );
                }
                else if (binaryExpression.NodeType == ExpressionType.Coalesce)
                {
                    cmdText = SqlNode.Call("ifNull", left.CommantText?.ToString(), right.CommantText?.ToString());
                }
                else if (binaryExpression.NodeType == ExpressionType.ExclusiveOr)
                {
                    sqlOperator = SqlOperator.ExclusiveOr;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.ExclusiveOr} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.LeftShift)
                {
                    sqlOperator = SqlOperator.LeftShift;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.LeftShift} {right.CommantText}";
                }
                else if (binaryExpression.NodeType == ExpressionType.RightShift)
                {
                    sqlOperator = SqlOperator.RightShift;

                    if (NeedLeftBracket(left.SqlOperator, sqlOperator.Value))
                        left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                    if (NeedRightBracket(sqlOperator.Value, right.SqlOperator))
                        right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                    cmdText = $"{left.CommantText} {SqlNode.RightShift} {right.CommantText}";
                }
                else
                {
                    throw new UnsupportedExpressionException(expression);
                }

                return new()
                {
                    CommantText = cmdText,
                    Parameters = ps,
                    SqlOperator = sqlOperator
                };
            }

            if (expression is MethodCallExpression methodCallExpression)
            {
                MethodInfo method = methodCallExpression.Method;

                bool hasFormat = TryGetMemberFormat(method, out object format);

                if (hasFormat && format is Func<MethodCallExpression, SqlContext, SqlValue> formatFunc)
                {
                    var cmd = formatFunc(methodCallExpression, context);

                    if (cmd.IsConstant && cmd.ConstantValue is TableExpression tableExpression && context.GetTableExpression == false)
                    {
                        var command = Build(GetDataManipulationSql(tableExpression), context.Parameters, context.Level + 1);

                        return new()
                        {
                            CommantText = SqlNode.SubQuery(command.CommandText),
                            Parameters = command.Parameters
                        };
                    }

                    return cmd;
                }

                List<ParameterValue> args = new();

                var objectCmd = GetValueExpression(methodCallExpression.Object, context);

                args.Add(new()
                {
                    CommantText = objectCmd.CommantText,
                    Parameters = objectCmd.Parameters,
                    IsConstant = objectCmd.IsConstant,
                    ConstantValue = objectCmd.ConstantValue
                });

                ParameterInfo[] parameterInfos = methodCallExpression.Method.GetParameters();

                for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
                {
                    var arg = methodCallExpression.Arguments[i];

                    if (parameterInfos[i].IsDefined(typeof(ParamArrayAttribute), true))
                    {
                        List<SqlValue> paramValues = new();

                        NewArrayExpression newArrayExpression = (NewArrayExpression)arg;

                        foreach (var element in newArrayExpression.Expressions)
                        {
                            paramValues.Add(GetValueExpression(element, context));
                        }

                        bool isAllParamsConstant = paramValues.All(p => p.IsConstant);

                        args.Add(new()
                        {
                            CommantText = string.Join(", ", paramValues.Select(p => p.CommantText?.ToString())),
                            Parameters = paramValues.SelectMany(p => p.Parameters ?? System.Linq.Enumerable.Empty<KeyValuePair<string, object>>()),
                            IsConstant = isAllParamsConstant,
                            ConstantValue = isAllParamsConstant ? paramValues.Select(p => p.ConstantValue).ToArray() : null,
                            IsParamArray = true,
                            ParamValues = paramValues.Select(p => p.CommantText?.ToString()).ToList()
                        });
                    }
                    else
                    {
                        var argCmd = GetValueExpression(arg, context);

                        //if (parameterInfos[i].IsDefined(typeof(ConstantParameterAttribute), true))
                        //{
                        //    if (argCmd.IsConstant is false)
                        //        throw new UnsupportedExpressionException(arg, $"The parameter {parameterInfos[i].Name} of method {method.Name} must be a constant");

                        //    string commandText = null;

                        //    if (argCmd.ConstantValue is null)
                        //        commandText = SqlNode.Null;
                        //    else if (argCmd.ConstantValue is string or Enum or char)
                        //        commandText = SqlNode.SqlString(argCmd.ConstantValue.ToString());
                        //    else
                        //        commandText = argCmd.ConstantValue.ToString();

                        //    args.Add(new()
                        //    {
                        //        CommantText = commandText
                        //    });

                        //    continue;
                        //}

                        args.Add(new()
                        {
                            CommantText = argCmd.CommantText?.ToString(),
                            Parameters = argCmd.Parameters,
                            IsConstant = argCmd.IsConstant,
                            ConstantValue = argCmd.ConstantValue
                        });
                    }
                }

                if (hasFormat == false)
                {
                    if (args.All(a => a.IsConstant))
                    {
                        object constantValue = methodCallExpression.Method.Invoke(args[0].ConstantValue, args.Skip(1).Select(a => a.ConstantValue).ToArray());

                        return SqlValue.Constant(constantValue);
                    }

                    if (IsIndexMethod(method, out bool indexIsNumber))
                    {
                        var jsonAccess = objectCmd.CommantText as JsonAccess ?? new JsonAccess(objectCmd.CommantText?.ToString());

                        var indexCmd = args[1];

                        var index = indexCmd.CommantText?.ToString();

                        JsonAccess cmd = null;

                        var indexParameters = indexCmd.Parameters?.ToList();

                        if (indexIsNumber)
                        {
                            cmd = SqlNode.JsonIndex(jsonAccess, index);
                        }
                        else
                        {
                            cmd = SqlNode.JsonMember(jsonAccess, index);

                            if (indexCmd.IsConstant && indexCmd.ConstantValue is string memberName)
                            {
                                var keyValue = indexParameters.First(p => p.Value == indexCmd.ConstantValue);

                                indexParameters.Remove(keyValue);

                                indexParameters.Add(KeyValuePair.Create<string, object>(keyValue.Key, SqlNode.JsonMemberName(memberName)));
                            }
                        }

                        List<KeyValuePair<string, object>> cmdParameters = new();

                        if (objectCmd.Parameters is not null)
                            cmdParameters.AddRange(objectCmd.Parameters);

                        if (indexParameters is not null)
                            cmdParameters.AddRange(indexParameters);

                        return new()
                        {
                            CommantText = cmd,
                            Parameters = indexParameters
                        };
                    }

                    List<string> keys = new();

                    if (methodCallExpression.Object is not null)
                        keys.Add("{0}");

                    keys.AddRange(methodCallExpression.Arguments.Select((s, i) => "{" + (i + 1) + "}"));

                    format = methodCallExpression.Method.Name + "(" + string.Join(", ", keys) + ")";
                }

                object cmdText = null;

                if (format is string formatStr)
                {
                    cmdText = string.Format(formatStr, args.Select(a => a.CommantText).ToArray());
                }
                else if (format is Func<string[], string> convert)
                {
                    cmdText = convert(args.SelectMany(a => a.IsParamArray ? a.ParamValues : new string[] { a.CommantText?.ToString() }).ToArray());
                }
                else
                {
                    throw new UnsupportedExpressionException(methodCallExpression);
                }

                return new()
                {
                    CommantText = cmdText,
                    Parameters = args.SelectMany(a => a.Parameters ?? System.Linq.Enumerable.Empty<KeyValuePair<string, object>>()).ToList()
                };
            }

            if (expression is ConditionalExpression conditionalExpression)
            {
                var test = GetValueExpression(conditionalExpression.Test, context);

                var ifTrue = GetValueExpression(conditionalExpression.IfTrue, context);

                var ifFalse = GetValueExpression(conditionalExpression.IfFalse, context);

                List<KeyValuePair<string, object>> ps = new();

                if (test.Parameters is not null)
                    ps.AddRange(test.Parameters);

                if (ifTrue.Parameters is not null)
                    ps.AddRange(ifTrue.Parameters);

                if (ifFalse.Parameters is not null)
                    ps.AddRange(ifFalse.Parameters);

                return new()
                {
                    CommantText = SqlNode.Call("if", test.CommantText?.ToString(), ifTrue.CommantText?.ToString(), ifFalse.CommantText?.ToString()),
                    Parameters = ps
                };
            }

            throw new UnsupportedExpressionException(expression);
        }

        bool NeedLeftBracket(SqlOperator? left, SqlOperator right)
        {
            if (left is null)
                return false;

            return (int)left.Value > (int)right;
        }

        bool NeedRightBracket(SqlOperator left, SqlOperator? right)
        {
            if (right is null)
                return false;

            return (int)right.Value >= (int)left;
        }

        bool IsIndexMethod(MethodInfo method, out bool indexIsNumber)
        {
            Type parameterType = method.GetParameters().FirstOrDefault()?.ParameterType;

            indexIsNumber = parameterType == typeof(int)
                || parameterType == typeof(uint)
                || parameterType == typeof(short)
                || parameterType == typeof(ushort)
                || parameterType == typeof(long)
                || parameterType == typeof(ulong);

            return method.DeclaringType.GetProperties().Where(p => p.GetIndexParameters().Length > 0).Select(p => p.GetMethod).Contains(method);
        }

        bool IsEnumCompare(BinaryExpression binaryExpression)
        {
            return IsConvertExpression(binaryExpression.Left)
                && GetUnderlyingValueGenericType(((UnaryExpression)binaryExpression.Left).Operand.Type).IsSubclassOf(typeof(Enum))
                && IsConvertExpression(binaryExpression.Right)
                && GetUnderlyingValueGenericType(((UnaryExpression)binaryExpression.Right).Operand.Type).IsSubclassOf(typeof(Enum));
        }

        bool IsConvertExpression(System.Linq.Expressions.Expression expression)
        {
            return expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked || expression.NodeType == ExpressionType.TypeAs;
        }

        string FullName(OriginalTableExpression table)
        {
            string tableName = SqlNode.SqlName(table.Name);

            if (string.IsNullOrEmpty(table.Schema) is false)
                tableName = SqlNode.Member(SqlNode.SqlName(table.Schema), tableName);

            return tableName;
        }

        Type GetUnderlyingValueGenericType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Value<>))
                return GetUnderlyingValueGenericType(type.GetGenericArguments()[0]);

            return Nullable.GetUnderlyingType(type) ?? type;
        }

        class CTEInfo
        {
            public string Alias { get; set; }

            public IDataManipulationSql Sql { get; set; }

            public string CommandText { get; set; }
        }

        class SqlColumnInfo
        {
            public string Alias { get; set; }

            /// <summary>
            /// System.Linq.Expressions.Expression Or SqlExpressions.IValueExpression
            /// </summary>
            public object Expression { get; set; }

            public SqlExpressions.SqlContext SqlContext { get; set; }

            public Delegate Reader { get; set; }

            public MemberInfo Member { get; set; }
        }

        class GroupKeyExpression
        {
            public MemberInfo Member { get; set; }

            public System.Linq.Expressions.Expression Expression { get; set; }
        }

        class EntityPropertyInfo
        {
            public Func<object, object> Writer { get; set; }
        }

        class SqlText
        {
            public string CommandText { get; set; }

            public IEnumerable<KeyValuePair<string, object>> Parameters { get; set; }
        }

        class ParameterValueExpressionInfo
        {
            public SqlExpressions.IValueExpression Value { get; set; }

            public IEnumerable<SqlExpressions.IValueExpression> Values { get; set; }

            public bool IsParamArray { get; set; } = false;

            public bool IsConstant => IsParamArray ? Values.All(v => v is SqlExpressions.ConstantExpression) : Value is SqlExpressions.ConstantExpression;

            public object ConstantValue => IsParamArray ? Values.Select(s => ((SqlExpressions.ConstantExpression)s).Value).ToArray() : ((SqlExpressions.ConstantExpression)Value).Value;
        }
    }
}
