using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Data.SqlTypes;
using LogicEntity.Default.MySql.Linq.Expressions;
using LogicEntity.Default.MySql.ExpressionVisit;

namespace LogicEntity.Default.MySql
{
    public partial class LinqConvertProvider : ILinqConvertProvider
    {
        readonly PropertyInfo _GroupingDataTableKey = typeof(IGroupingDataTable<>).GetProperty(nameof(IGroupingDataTable<int>.Key));
        readonly LambdaExpression _AllColumnsLambdaExpression = System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression.Call(typeof(DbFunction).GetMethod(nameof(DbFunction.AllColumns), BindingFlags.Static | BindingFlags.NonPublic)));
        readonly MethodInfo _Any = typeof(DbFunction).GetMethod(nameof(DbFunction.Any)).MakeGenericMethod(typeof(bool));
        readonly MethodInfo _All = typeof(DbFunction).GetMethod(nameof(DbFunction.All)).MakeGenericMethod(typeof(bool));
        readonly MethodInfo _Average = typeof(DbFunction).GetMethod(nameof(DbFunction.Average), BindingFlags.Static | BindingFlags.NonPublic);
        readonly MethodInfo _Count = typeof(DbFunction).GetMethod(nameof(DbFunction.Count), BindingFlags.Static | BindingFlags.NonPublic);
        readonly MethodInfo _Max = typeof(DbFunction).GetMethod(nameof(DbFunction.Max), BindingFlags.Static | BindingFlags.NonPublic);
        readonly MethodInfo _Min = typeof(DbFunction).GetMethod(nameof(DbFunction.Min), BindingFlags.Static | BindingFlags.NonPublic);
        readonly MethodInfo _Sum = typeof(DbFunction).GetMethod(nameof(DbFunction.Sum), BindingFlags.Static | BindingFlags.NonPublic);
        readonly MethodInfo _Read = typeof(DbFunction).GetMethod(nameof(DbFunction.Read));
        readonly MethodInfo _ReadBytes = typeof(DbFunction).GetMethod(nameof(DbFunction.ReadBytes));
        readonly MethodInfo _ReadChars = typeof(DbFunction).GetMethod(nameof(DbFunction.ReadChars));
        readonly static MethodInfo _Assign1 = typeof(OperatorFunction).GetMethods().Single(m =>
        {
            ParameterInfo[] parameterInfos = m.GetParameters();

            return m.Name == nameof(OperatorFunction.Assign)
                && parameterInfos.Length == 2
                && parameterInfos[0].ParameterType.IsGenericType == false
                && parameterInfos[1].ParameterType.IsGenericType == false;
        });
        readonly static MethodInfo _Assign2 = typeof(OperatorFunction).GetMethods().Single(m =>
        {
            ParameterInfo[] parameterInfos = m.GetParameters();

            return m.Name == nameof(OperatorFunction.Assign)
                && parameterInfos.Length == 2
                && parameterInfos[0].ParameterType.IsGenericType == true
                && parameterInfos[1].ParameterType.IsGenericType == false;
        });
        readonly static MethodInfo _Assign3 = typeof(OperatorFunction).GetMethods().Single(m =>
        {
            ParameterInfo[] parameterInfos = m.GetParameters();

            return m.Name == nameof(OperatorFunction.Assign)
                && parameterInfos.Length == 2
                && parameterInfos[0].ParameterType.IsGenericType == false
                && parameterInfos[1].ParameterType.IsGenericType == true;
        });

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
            Command command = null;

            if (expression is TableExpression tableExpression && expression is not AddNextTableExpression)
                command = GetSelectCommand(tableExpression);
            else
                command = GetOperateCommand(expression);

            List<KeyValuePair<string, object>> parameters = new();

            for (int i = 0; i < command.Parameters.Count; i++)
            {
                string key = "@param" + i.ToString();

                command.CommandText = command.CommandText.Replace(command.Parameters[i].Key, key);

                parameters.Add(KeyValuePair.Create(key, command.Parameters[i].Value));
            }

            command.Parameters.Clear();

            command.Parameters.AddRange(parameters);

#if DEBUG
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                string key = ("@param" + i);

                if (command.Parameters[i].Key != key)
                    throw new Exception("参数名称错误");

                var collection = new System.Text.RegularExpressions.Regex("(^|[^0-9a-zA-Z])" + key + "($|[^0-9a-zA-Z])").Matches(command.CommandText);

                if (collection.Count == 0 || collection.Count > command.Parameters.Count)
                    throw new Exception("参数名称错误");
            }

            if (command.CommandText.Contains(" @param" + command.Parameters.Count + " "))
                throw new Exception("参数名称错误");

            if (command.CommandText.Contains(" @Guid_"))
                throw new Exception("参数名称错误");
#endif

            return command;
        }

        Command GetSelectCommand(TableExpression tableExpression)
        {
            var sql = GetDataManipulationSql(tableExpression);

            if (sql.IsCTE)
            {
                sql = new()
                {
                    Select = _AllColumnsLambdaExpression,
                    From = sql,
                    Type = sql.Type
                };
            }

            return Build(sql, new(), 0);
        }

        Command GetOperateCommand(LogicEntity.Linq.Expressions.Expression operateExpression)
        {
            if (operateExpression is AddOperateExpression addOperateExpression)
            {
                OriginalTableExpression table = (OriginalTableExpression)addOperateExpression.Source;

                PropertyInfo[] properties = table.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    .Where(p => p.PropertyType.IsAssignableTo(typeof(IValue))).ToArray();

                Dictionary<PropertyInfo, EntityPropertyInfo> validProperties = new();

                List<Dictionary<PropertyInfo, IValue>> rows = new();

                foreach (object element in addOperateExpression.Elements)
                {
                    Dictionary<PropertyInfo, IValue> row = new();

                    foreach (PropertyInfo property in properties)
                    {
                        IValue propertyValue = (IValue)property.GetValue(element);

                        if (propertyValue.ValueSetted is false)
                            continue;

                        if (validProperties.ContainsKey(property) == false)
                        {
                            System.Linq.Expressions.Expression<Func<object, object>> writer = null;

                            ParameterExpression obj = System.Linq.Expressions.Expression.Parameter(typeof(object));

                            System.Linq.Expressions.Expression val = obj;

                            if (PropertyConvert.TryGetValue(property, out ValueConverter converter) && converter.Writer is not null)
                            {
                                Type parameterType = converter.Writer.Method.GetParameters()[0].ParameterType;

                                if (val.Type != parameterType)
                                    val = System.Linq.Expressions.Expression.Convert(val, parameterType);

                                val = System.Linq.Expressions.Expression.Invoke(System.Linq.Expressions.Expression.Constant(converter.Writer), val);
                            }

                            writer = System.Linq.Expressions.Expression.Lambda<Func<object, object>>(val, obj);

                            validProperties.Add(property, new()
                            {
                                Writer = writer.Compile()
                            });
                        }

                        row[property] = propertyValue;
                    }

                    rows.Add(row);
                }

                List<string> rowCmds = new();

                List<KeyValuePair<string, object>> parameters = new();

                foreach (Dictionary<PropertyInfo, IValue> row in rows)
                {
                    List<string> columnCmds = new();

                    foreach (KeyValuePair<PropertyInfo, EntityPropertyInfo> entityProperty in validProperties)
                    {
                        if (row.TryGetValue(entityProperty.Key, out IValue value))
                        {
                            if (value.ValueType == ValueType.Value)
                            {
                                var p = SqlNode.Parameter(entityProperty.Value.Writer(value.Object));

                                columnCmds.Add(p.Key);

                                parameters.Add(p);
                            }
                            else
                            {
                                LambdaExpression lambdaExpression = (LambdaExpression)value.Object;

                                SqlValue sqlValue = GetValueExpression(lambdaExpression.Body, new(0), true);

                                columnCmds.Add(sqlValue.CommantText?.ToString());

                                if (sqlValue.Parameters is not null)
                                    parameters.AddRange(sqlValue.Parameters);
                            }

                            continue;
                        }

                        columnCmds.Add(SqlNode.Default);
                    }

                    rowCmds.Add(SqlNode.Bracket(string.Join(", ", columnCmds)));
                }

                string onDuplicateKeyUpdate = string.Empty;

                if (addOperateExpression.Update)
                {
                    onDuplicateKeyUpdate = "\nOn Duplicate Key Update\n";

                    if (_updateFactoryVersion == UpdateFactoryVersion.V8_0)
                        onDuplicateKeyUpdate = $"\nAs {SqlNode.NewRowAlias}" + onDuplicateKeyUpdate;

                    List<KeyValuePair<string, string>> columnAndValues;

                    if (addOperateExpression is AddOrUpdateOperateExpression addOrUpdateOperateExpression)
                    {
                        MemberInitExpression memberInitExpression = addOrUpdateOperateExpression.UpdateFactory.Body as MemberInitExpression;

                        if (memberInitExpression is null)
                            throw new UnsupportedExpressionException(addOrUpdateOperateExpression.UpdateFactory.Body);

                        BlockExpression blockExpression = (BlockExpression)memberInitExpression.Reduce();

                        SqlContext sqlContext = new(0)
                        {
                            Parameters = new()
                            {
                                {
                                    addOrUpdateOperateExpression.UpdateFactory.Parameters[0],
                                    LambdaParameterInfo.Entity(new EntityInfo()
                                    {
                                        CommandText = _updateFactoryVersion == UpdateFactoryVersion.V8_0 ? FullName(table) : null,
                                        EntitySource = EntitySource.OriginalTable
                                    } )
                                },
                                {
                                    addOrUpdateOperateExpression.UpdateFactory.Parameters[1],
                                    LambdaParameterInfo.Entity(new EntityInfo()
                                    {
                                        CommandText = _updateFactoryVersion == UpdateFactoryVersion.V8_0 ? SqlNode.NewRowAlias : null,
                                        EntitySource = EntitySource.OriginalTable
                                    })
                                }
                            }
                        };

                        ColumnVisitor columnVisitor = new ColumnVisitor(addOrUpdateOperateExpression.UpdateFactory.Parameters[1]);

                        columnAndValues = blockExpression.Expressions.Skip(1).Take(blockExpression.Expressions.Count - 2).Select(memberInit =>
                        {
                            BinaryExpression assign = (BinaryExpression)memberInit;

                            string columnName = SqlNode.SqlName(ColumnName((PropertyInfo)((MemberExpression)assign.Left).Member));

                            System.Linq.Expressions.Expression right = assign.Right;

                            if (_updateFactoryVersion == UpdateFactoryVersion.V5_7)
                                right = columnVisitor.Visit(right);

                            var updateValue = GetValueExpression(right, sqlContext);

                            if (updateValue.Parameters is not null)
                                parameters.AddRange(updateValue.Parameters);

                            return KeyValuePair.Create(columnName, updateValue.CommantText?.ToString());
                        }).ToList();
                    }
                    else
                    {
                        columnAndValues = validProperties.Select(p =>
                        {
                            string columnName = SqlNode.SqlName(ColumnName(p.Key));

                            return KeyValuePair.Create(columnName, SqlNode.Call(nameof(DbFunction.Values), columnName));
                        }).ToList();
                    }

                    onDuplicateKeyUpdate += string.Join(",\n", columnAndValues.Select(s => SqlNode.Assign(s.Key, s.Value))).Indent(2);
                }

                Command command = new();

                string addOperate = "Insert Into";

                if (addOperateExpression is AddIgnoreOperateExpression)
                    addOperate = "Insert Ignore";
                else if (addOperateExpression is ReplaceOperateExpression)
                    addOperate = "Replace Into";

                command.CommandText = $"{addOperate} {FullName(table)}"
                    + $"\n(\n{string.Join(",\n", validProperties.Select(p => SqlNode.SqlName(ColumnName(p.Key)))).Indent(2)}\n)"
                    + $"\nValues\n{string.Join(",\n", rowCmds).Indent(2)}"
                    + $"{onDuplicateKeyUpdate}";

                command.Parameters.AddRange(parameters);

                return command;
            }

            if (operateExpression is AddNextTableExpression addNextTableExpression)
            {
                Command command = GetOperateCommand(new AddOperateExpression(addNextTableExpression.Source, new object[] { addNextTableExpression.Element }, false));

                command.CommandText += ";\nSelect Last_Insert_Id();";

                return command;
            }

            if (operateExpression is RemoveOperateExpression removeOperateExpression)
            {
                return Build(GetDataManipulationSql(removeOperateExpression), null, 0);
            }

            if (operateExpression is SetOperateExpression setOperateExpression)
            {
                return Build(GetDataManipulationSql(setOperateExpression), null, 0);
            }

            if (operateExpression is TimeoutOperateExpression timeoutOperateExpression)
            {
                var command = GetOperateCommand(timeoutOperateExpression.Source);

                command.CommandTimeout = timeoutOperateExpression.Timeout;

                return command;
            }

            throw new UnsupportedExpressionException(operateExpression);
        }

        DataManipulationSql GetDataManipulationSql(LogicEntity.Linq.Expressions.Expression expression)
        {
            if (expression is OriginalTableExpression)
            {
                return new()
                {
                    From = new()
                    {
                        new()
                        {
                            Table = expression
                        }
                    },
                    Type = expression.Type
                };
            }
            else if (expression is JoinedTableExpression joinedTableExpression)
            {
                List<DataManipulationSqlJoinedInfo> tables = new();

                TableExpression current = joinedTableExpression;

                while (current is JoinedTableExpression s)
                {
                    tables.Add(new()
                    {
                        Join = s.Join + " ",
                        Table = s.Right,
                        LambdaExpression = (LambdaExpression)s.Predicate
                    });

                    current = s.Left;
                }

                tables.Add(new()
                {
                    Table = current
                });

                tables.Reverse();

                foreach (var table in tables)
                {
                    if (table.Table is not OriginalTableExpression && table.Table is not CTETableExpression)
                        table.Table = GetDataManipulationSql((TableExpression)table.Table);
                }

                return new()
                {
                    From = tables
                };
            }
            else if (expression is RowFilteredTableExpression rowFilteredTableExpression)
            {
                List<LambdaExpression> filters = new();

                filters.Add((LambdaExpression)rowFilteredTableExpression.Filter);

                if (HasIndex(rowFilteredTableExpression))
                {
                    var sourceSql = GetDataManipulationSql(rowFilteredTableExpression.Source);

                    sourceSql.HasIndex = true;

                    return new()
                    {
                        From = sourceSql,
                        Where = filters,
                        Type = expression.Type
                    };
                }

                TableExpression current = rowFilteredTableExpression.Source;

                while (current is RowFilteredTableExpression rowFiltered && HasIndex(rowFiltered) == false)
                {
                    filters.Add((LambdaExpression)rowFiltered.Filter);

                    current = rowFiltered.Source;
                }

                filters.Reverse();

                var sql = GetDataManipulationSql(current);

                if (sql.CanAdd(SelectNodeType.Where))
                {
                    sql.Where = filters;
                }
                else if (sql.CanAdd(SelectNodeType.Having))
                {
                    sql.Having = filters;
                }
                else
                {
                    sql = new() { From = sql };

                    sql.Where = filters;
                }

                sql.Type = expression.Type;

                return sql;
            }
            else if (expression is GroupedTableExpression groupedTableExpression)
            {
                var sql = GetDataManipulationSql(groupedTableExpression.Source);

                if (sql.CanAdd(SelectNodeType.GroupBy) == false)
                {
                    sql = new() { From = sql };
                }

                sql.GroupBy = (LambdaExpression)groupedTableExpression.KeySelector;

                sql.Type = expression.Type;

                return sql;
            }
            else if (expression is UnionedTableExpression unionedTableExpression)
            {
                var sql = GetDataManipulationSql(unionedTableExpression.Left);

                if (sql.CanAdd(SelectNodeType.Union) == false)
                {
                    sql = new() { From = sql };
                }

                if (unionedTableExpression.Right is RecursiveUnionedTableExpression)
                {
                    sql.Union = unionedTableExpression.Right;
                }
                else
                {
                    sql.Union = GetDataManipulationSql(unionedTableExpression.Right);
                }

                sql.UnionDistinct = unionedTableExpression.Distinct;

                sql.Type = expression.Type;

                return sql;
            }
            else if (expression is OrderedTableExpression orderedTableExpression)
            {
                List<OrderedTableExpression> es = new();

                bool hasStart = false;

                TableExpression current = orderedTableExpression;

                while (current is OrderedTableExpression orderedTable)
                {
                    if (hasStart == false)
                    {
                        es.Add(orderedTable);

                        hasStart = orderedTable.Ordered == false;
                    }

                    current = orderedTable.Source;
                }

                es.Reverse();

                var sql = GetDataManipulationSql(current);

                if (sql.CanAdd(SelectNodeType.OrderBy) == false)
                {
                    sql = new() { From = sql };
                }

                sql.OrderBy = es;

                sql.Type = expression.Type;

                return sql;
            }
            else if (expression is SkippedTableExpression || expression is TakedTableExpression)
            {
                List<(bool, int)> skipTakes = new();

                TableExpression current = (TableExpression)expression;

                while (current is SkippedTableExpression || current is TakedTableExpression)
                {
                    if (current is SkippedTableExpression skipExp)
                    {
                        skipTakes.Add((true, skipExp.Count));

                        current = skipExp.Source;
                    }
                    else
                    {
                        TakedTableExpression tableExp = (TakedTableExpression)current;

                        skipTakes.Add((false, tableExp.Count));

                        current = tableExp.Source;
                    }
                }

                skipTakes.Reverse();

                int skip = 0;

                int taked = int.MaxValue;

                foreach (var e in skipTakes)
                {
                    if (e.Item1)
                    {
                        skip += e.Item2;

                        taked -= e.Item2;
                    }
                    else
                    {
                        if (e.Item2 < taked)
                            taked = e.Item2;
                    }
                }

                if (taked < 0)
                    taked = 0;

                var sql = GetDataManipulationSql(current);

                if (sql.CanAdd(SelectNodeType.Limit) == false)
                {
                    sql = new() { From = sql };
                }

                sql.Limit = new()
                {
                    Skip = skip,
                    Taked = taked
                };

                sql.Type = expression.Type;

                return sql;
            }
            else if (expression is SelectedTableExpression columnFilteredTableExpression)
            {
                var sql = columnFilteredTableExpression.Source is null ? new() : GetDataManipulationSql(columnFilteredTableExpression.Source);

                if (sql.CanAdd(SelectNodeType.Select) == false)
                {
                    sql = new() { From = sql };
                }

                sql.Select = columnFilteredTableExpression.Selector;

                sql.Type = expression.Type;

                return sql;
            }
            else if (expression is DistinctTableExpression distinctTableExpression)
            {
                var sql = GetDataManipulationSql(distinctTableExpression.Source);

                sql.Distinct = true;

                return sql;
            }
            else if (expression is AverageTableExpression averageTableExpression)
            {
                var sql = GetDataManipulationSql(averageTableExpression.Source);

                if (sql.CanAdd(SelectNodeType.Select) == false)
                {
                    sql = new() { From = sql };
                }

                LambdaExpression selector = (LambdaExpression)averageTableExpression.Selector;

                sql.Select = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Call(_Average.MakeGenericMethod(selector.Body.Type), selector.Body),
                    selector.Parameters
                    );

                sql.Type = averageTableExpression.Type;

                return sql;
            }
            else if (expression is CountTableExpression countTableExpression)
            {
                var sql = GetDataManipulationSql(countTableExpression.Source);

                if (sql.CanAdd(SelectNodeType.Select) == false)
                {
                    sql = new() { From = sql };
                }

                sql.Select = System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression.Call(_Count));

                sql.Type = countTableExpression.Type;

                return sql;
            }
            else if (expression is MaxTableExpression maxTableExpression)
            {
                var sql = GetDataManipulationSql(maxTableExpression.Source);

                if (sql.CanAdd(SelectNodeType.Select) == false)
                {
                    sql = new() { From = sql };
                }

                LambdaExpression selector = (LambdaExpression)maxTableExpression.Selector;

                sql.Select = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Call(_Max.MakeGenericMethod(selector.Body.Type), selector.Body),
                    selector.Parameters
                    );

                sql.Type = maxTableExpression.Type;

                return sql;
            }
            else if (expression is MinTableExpression minTableExpression)
            {
                var sql = GetDataManipulationSql(minTableExpression.Source);

                if (sql.CanAdd(SelectNodeType.Select) == false)
                {
                    sql = new() { From = sql };
                }

                LambdaExpression selector = (LambdaExpression)minTableExpression.Selector;

                sql.Select = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Call(_Min.MakeGenericMethod(selector.Body.Type), selector.Body),
                    selector.Parameters
                    );

                sql.Type = minTableExpression.Type;

                return sql;
            }
            else if (expression is SumTableExpression sumTableExpression)
            {
                var sql = GetDataManipulationSql(sumTableExpression.Source);

                if (sql.CanAdd(SelectNodeType.Select) == false)
                {
                    sql = new() { From = sql };
                }

                LambdaExpression selector = (LambdaExpression)sumTableExpression.Selector;

                sql.Select = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Call(_Sum.MakeGenericMethod(selector.Body.Type), selector.Body),
                    selector.Parameters
                    );

                sql.Type = sumTableExpression.Type;

                return sql;
            }
            else if (expression is AnyTableExpression anyTableExpression)
            {
                TableExpression source = anyTableExpression.Source;

                if (anyTableExpression.Predicate is not null)
                    source = new RowFilteredTableExpression(source, anyTableExpression.Predicate);

                source = new SelectedTableExpression(
                    source,
                    System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression.Constant(true)),
                    typeof(bool)
                    );

                LambdaExpression lambdaExpression = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Equal(
                        System.Linq.Expressions.Expression.Constant(true),
                        System.Linq.Expressions.Expression.Call(_Any, System.Linq.Expressions.Expression.Constant(new DataTableImpl<bool>(null, source)))
                        )
                    );

                return new DataManipulationSql()
                {
                    Select = lambdaExpression,
                    Type = typeof(bool)
                };
            }
            else if (expression is AllTableExpression allTableExpression)
            {
                TableExpression source = new SelectedTableExpression(
                    allTableExpression.Source,
                    allTableExpression.Predicate,
                    typeof(bool)
                    );

                LambdaExpression lambdaExpression = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Equal(
                        System.Linq.Expressions.Expression.Constant(true),
                        System.Linq.Expressions.Expression.Call(_All, System.Linq.Expressions.Expression.Constant(new DataTableImpl<bool>(null, source)))
                        )
                    );

                return new DataManipulationSql()
                {
                    Select = lambdaExpression,
                    Type = typeof(bool)
                };
            }
            else if (expression is RecursiveUnionedTableExpression recursiveUnionedTableExpression)
            {
                var sql = GetDataManipulationSql(recursiveUnionedTableExpression.Left);

                if (sql.CanAdd(SelectNodeType.Union) == false)
                {
                    sql = new() { From = sql };
                }

                sql.Union = recursiveUnionedTableExpression.RightFactory;

                sql.UnionDistinct = recursiveUnionedTableExpression.Distinct;

                sql.Type = expression.Type;

                sql.IsCTE = true;

                return sql;
            }
            else if (expression is CTETableExpression cteTableExpression)
            {
                return new()
                {
                    From = new()
                    {
                        new()
                        {
                            Table = cteTableExpression
                        }
                    },
                    Type = expression.Type
                };
            }
            else if (expression is RemoveOperateExpression removeOperateExpression)
            {
                var sql = GetDataManipulationSql(removeOperateExpression.Source);

                sql.DataManipulationType = DataManipulationSqlType.Delete;

                if (removeOperateExpression.Selectors is not null)
                {
                    LambdaExpression[] lambdaExpressions = (LambdaExpression[])removeOperateExpression.Selectors;

                    sql.Delete = lambdaExpressions.Select(lambdaExpression =>
                    {
                        if (lambdaExpression.Body is not ParameterExpression parameterExpression)
                            throw new UnsupportedExpressionException(lambdaExpression.Body);

                        return SqlNode.GetTableAlias(lambdaExpression.Parameters.IndexOf(parameterExpression), 0);
                    }).ToList();
                }

                return sql;
            }
            else if (expression is SetOperateExpression setOperateExpression)
            {
                var sql = GetDataManipulationSql(setOperateExpression.Source);

                sql.DataManipulationType = DataManipulationSqlType.Update;

                sql.Set = (LambdaExpression[])setOperateExpression.Assignments;

                return sql;
            }
            else if (expression is TimeoutTableExpression timeoutTableExpression)
            {
                var sql = GetDataManipulationSql(timeoutTableExpression.Source);

                sql.Timeout = timeoutTableExpression.Timeout;

                return sql;
            }

            throw new UnsupportedExpressionException(expression);
        }

        Command Build(DataManipulationSql sql, Dictionary<ParameterExpression, LambdaParameterInfo> parameters, int level, string cteAlias = null)
        {
            if (parameters is null)
                parameters = new();

            Command command = new();

            command.CommandTimeout = sql.Timeout;

            Command.Result result = new();

            if (sql.DataManipulationType == DataManipulationSqlType.Select)
                result.Type = GetResultType(sql.Type);

            command.Results.Add(result);

            //With
            List<CTEInfo> ctes = new();

            //From
            string from = string.Empty;

            List<EntityInfo> entityInfos = new();

            if (sql.From is not null)
            {
                List<string> tableTexts = new();

                for (int i = 0; i < sql.From.Count; i++)
                {
                    var table = sql.From[i];

                    string alias = string.Empty;

                    string text = string.Empty;

                    if (table.Table is OriginalTableExpression originalTable)
                    {
                        alias = SqlNode.GetTableAlias(i, level);

                        text = FullName(originalTable).As(alias);
                    }
                    else if (table.Table is CTETableExpression cteTableExpression)
                    {
                        alias = cteTableExpression.Alias;

                        text = alias;
                    }
                    else if (table.Table is DataManipulationSql subQuery)
                    {
                        if (subQuery.IsCTE)
                        {
                            alias = SqlNode.GetCTEAlias(ctes.Count, level);

                            ctes.Add(new()
                            {
                                Alias = alias,
                                Sql = subQuery
                            });

                            text = alias;
                        }
                        else
                        {
                            var subQueryCommand = Build(subQuery, new(), 0);

                            command.Parameters.AddRange(subQueryCommand.Parameters);

                            alias = SqlNode.GetTableAlias(i, level);

                            text = SqlNode.SubQuery(subQueryCommand.CommandText).As(alias);
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }

                    entityInfos.Add(new()
                    {
                        CommandText = alias,
                        EntitySource = table.Table is OriginalTableExpression ? EntitySource.OriginalTable : EntitySource.SubQuery
                    });

                    if (table.LambdaExpression is not null)
                    {
                        var predicateValue = GetValueExpression(table.LambdaExpression.Body, new SqlContext(level)
                        {
                            Parameters = parameters.Concat(
                                table.LambdaExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i])))
                                ).ToDictionary(s => s.Key, s => s.Value)
                        });

                        text += " On " + predicateValue.CommantText;

                        if (predicateValue.Parameters is not null)
                            command.Parameters.AddRange(predicateValue.Parameters);
                    }

                    tableTexts.Add(table.Join + text);
                }

                from = (sql.DataManipulationType == DataManipulationSqlType.Update ? string.Empty : "\nFrom") + "\n" + string.Join("\n", tableTexts).Indent(2);
            }

            //Group By
            string groupBy = string.Empty;

            Dictionary<MemberInfo, string> groupKeys = new();

            if (sql.GroupBy is not null)
            {
                List<GroupKeyExpression> groupKeyExpressions = new();

                if (sql.GroupBy.Body is NewExpression keysEntityExpression)
                {
                    if (keysEntityExpression.Arguments.Count != keysEntityExpression.Members?.Count)
                        throw new UnsupportedExpressionException(keysEntityExpression);

                    groupKeyExpressions.AddRange(keysEntityExpression.Members.Zip(keysEntityExpression.Arguments, (a, b) => new GroupKeyExpression()
                    {
                        Member = a,
                        Expression = b
                    }));
                }
                else
                {
                    groupKeyExpressions.Add(new()
                    {
                        Member = _GroupingDataTableKey,
                        Expression = sql.GroupBy.Body
                    });
                }

                List<string> groupColumns = new();

                foreach (var groupKey in groupKeyExpressions)
                {
                    var keyCmd = GetValueExpression(groupKey.Expression, new SqlContext(level)
                    {
                        Parameters = parameters.Concat(
                            sql.GroupBy.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i])))
                            ).ToDictionary(s => s.Key, s => s.Value)
                    });

                    groupColumns.Add(keyCmd.CommantText?.ToString());

                    if (keyCmd.Parameters is not null)
                        command.Parameters.AddRange(keyCmd.Parameters);

                    groupKeys[groupKey.Member] = keyCmd.CommantText?.ToString();
                }

                groupBy = "\nGroup By\n  " + string.Join(", ", groupColumns);
            }

            //Manipulation
            string manipulation = string.Empty;

            //Select
            if (sql.DataManipulationType == DataManipulationSqlType.Select)
            {
                List<string> columns = new();

                if (sql.HasIndex)
                    columns.Add(SqlNode.ColumnIndexValue.AsColumn(SqlNode.IndexColumnName));

                List<SqlColumnInfo> columnExpressions = new();

                if (sql.Select is null)
                {
                    ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(sql.Type);

                    sql.Select = System.Linq.Expressions.Expression.Lambda(parameter, parameter);
                }

                if (sql.Select is LambdaExpression[] lambdaExpressions)
                {
                    columnExpressions.AddRange(lambdaExpressions.Select(s =>
                    {
                        System.Linq.Expressions.Expression expression = s.Body;

                        if (expression.NodeType == ExpressionType.Convert && expression.Type == typeof(object))
                            expression = ((UnaryExpression)expression).Operand;

                        string alias = null;

                        if (expression is MemberExpression memberExpression)
                            alias = memberExpression.Member.Name;

                        return new SqlColumnInfo()
                        {
                            Alias = alias,
                            SqlContext = new SqlContext(level)
                            {
                                Parameters = parameters.Concat(
                                    s.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i])))
                                ).ToDictionary(s => s.Key, s => s.Value)
                            },
                            Expression = expression
                        };
                    }));
                }
                else if (sql.Select is LambdaExpression columnSelector)
                {
                    if (columnSelector.Body is MemberExpression memberExpression
                        && memberExpression.Expression is ParameterExpression
                        && memberExpression.Expression.Type.IsAssignableTo(typeof(IGroupingDataTable))
                        && memberExpression.Member.Name == nameof(IGroupingDataTable<int, int>.Element))
                    {
                        ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(memberExpression.Type);

                        columnSelector = System.Linq.Expressions.Expression.Lambda(parameter, parameter);
                    }

                    Dictionary<string, ConstructorInfo> constructors = new();

                    SqlContext sqlContext = new SqlContext(level)
                    {
                        Parameters = parameters.Concat(columnSelector.Parameters.Select((p, i) =>
                        {
                            LambdaParameterInfo lambdaParameterInfo = null;

                            if (i >= entityInfos.Count)
                            {
                                lambdaParameterInfo = LambdaParameterInfo.ColumnIndexValue;
                            }
                            else if (p.Type.IsAssignableTo(typeof(IGroupingDataTable)))
                            {
                                lambdaParameterInfo = LambdaParameterInfo.GroupingDataTable(groupKeys, entityInfos);
                            }
                            else
                            {
                                lambdaParameterInfo = LambdaParameterInfo.Entity(entityInfos[i]);
                            }

                            return KeyValuePair.Create(p, lambdaParameterInfo);
                        })).ToDictionary(s => s.Key, s => s.Value)
                    };

                    columnExpressions.AddRange(ExpandColumns(
                        new List<SqlColumnInfo>()
                        {
                            new SqlColumnInfo()
                            {
                                Alias = null,
                                Expression = columnSelector.Body
                            }
                        },
                        groupKeys,
                        ref constructors,
                        sqlContext.Parameters.Where(p => p.Value.ParameterType == LambdaParameterType.ColumnIndexValue).ToDictionary(p => p.Key, p => p.Value.CommandText)
                        ).Select(s =>
                        {
                            s.SqlContext = sqlContext;

                            return s;
                        })
                        );

                    foreach (var keyValue in constructors)
                    {
                        result.Constructors[keyValue.Key] = keyValue.Value;
                    }
                }
                else
                {
                    throw new Exception("Unsupported Selector");
                }

                SetClientReader(columnExpressions);

                for (int i = 0; i < columnExpressions.Count; i++)
                {
                    var column = columnExpressions[i];

                    if (column.Reader is not null)
                        result.Readers[i] = column.Reader;

                    var columnCmd = column.Expression is System.Linq.Expressions.Expression expression ?
                          GetValueExpression(expression, column.SqlContext) : new() { CommantText = column.Expression.ToString() };

                    string columnText = columnCmd.CommantText?.ToString();

                    if (column.Alias is not null && SqlNode.NameEqual(column.Alias, columnText) == false)
                        columnText = columnText.AsColumn(column.Alias);

                    columns.Add(columnText);

                    if (columnCmd.Parameters is not null)
                        command.Parameters.AddRange(columnCmd.Parameters);
                }

                manipulation = "Select " + (sql.Distinct ? "Distinct" : string.Empty) + "\n" + string.Join(",\n", columns).Indent(2);
            }

            //Delete
            if (sql.DataManipulationType == DataManipulationSqlType.Delete)
            {
                manipulation = "Delete";

                if (sql.Delete is not null)
                {
                    manipulation += "\n" + string.Join(",\n", sql.Delete).Indent(2);
                }
            }

            //Update
            string set = string.Empty;

            if (sql.DataManipulationType == DataManipulationSqlType.Update)
            {
                manipulation = "Update";

                List<string> assignments = sql.Set.Select(assignment =>
                {
                    System.Linq.Expressions.Expression left = null;

                    System.Linq.Expressions.Expression right = null;

                    if (assignment.Body.NodeType == ExpressionType.Assign)
                    {
                        BinaryExpression binaryExpression = (BinaryExpression)assignment.Body;

                        left = binaryExpression.Left;

                        right = binaryExpression.Right;
                    }
                    else if (assignment.Body is MethodCallExpression methodCallExpression
                        && methodCallExpression.Method.IsGenericMethod)
                    {
                        MethodInfo method = methodCallExpression.Method.GetGenericMethodDefinition();

                        if (method != _Assign1 && method != _Assign2 && method != _Assign3)
                            throw new UnsupportedExpressionException(assignment.Body);

                        left = methodCallExpression.Arguments[0];

                        right = methodCallExpression.Arguments[1];
                    }
                    else
                    {
                        throw new UnsupportedExpressionException(assignment.Body);
                    }

                    SqlContext sqlContext = new(level)
                    {
                        Parameters = assignment.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i])))
                            .ToDictionary(s => s.Key, s => s.Value)
                    };

                    var leftCmd = GetValueExpression(left, sqlContext);

                    var rightCmd = GetValueExpression(right, sqlContext);

                    if (leftCmd.Parameters is not null)
                        command.Parameters.AddRange(leftCmd.Parameters);

                    if (rightCmd.Parameters is not null)
                        command.Parameters.AddRange(rightCmd.Parameters);

                    if (leftCmd.CommantText is JsonAccess jsonAccess && jsonAccess.Valid)
                    {
                        return SqlNode.Assign(
                            jsonAccess.JsonDocument,
                            SqlNode.Call("Json_Set", jsonAccess.JsonDocument, jsonAccess.JsonPath, rightCmd.CommantText?.ToString())
                            );
                    }

                    return SqlNode.Assign(leftCmd.CommantText?.ToString(), rightCmd.CommantText?.ToString());
                }).ToList();

                set = "\nSet\n" + string.Join(",\n", assignments).Indent(2);
            }

            //Where
            string where = string.Empty;

            if (sql.Where is not null)
            {
                List<string> whereConditions = new();

                for (int i = 0; i < sql.Where.Count; i++)
                {
                    LambdaExpression predicateExpression = sql.Where[i];

                    var valueCmd = GetValueExpression(predicateExpression.Body, new SqlContext(level)
                    {
                        Parameters = parameters.Concat(predicateExpression.Parameters.Select((p, i) =>
                        {
                            LambdaParameterInfo lambdaParameterInfo = null;

                            if (i >= entityInfos.Count)
                            {
                                lambdaParameterInfo = LambdaParameterInfo.IndexColumnName;
                            }
                            else
                            {
                                lambdaParameterInfo = LambdaParameterInfo.Entity(entityInfos[i]);
                            }

                            return KeyValuePair.Create(p, lambdaParameterInfo);
                        })).ToDictionary(s => s.Key, s => s.Value)
                    }, i == 0);

                    if (i > 0 && valueCmd.ValueType == SqlValueValueType.Logic)
                        valueCmd.CommantText = SqlNode.Bracket(valueCmd.CommantText?.ToString());

                    whereConditions.Add(valueCmd.CommantText?.ToString());

                    if (valueCmd.Parameters is not null)
                        command.Parameters.AddRange(valueCmd.Parameters);
                }

                where = "\nWhere\n" + string.Join("\n" + SqlNode.AndAlso + " ", whereConditions).Indent(2);
            }

            //Having
            string having = string.Empty;

            if (sql.Having is not null)
            {
                List<string> havingConditions = new();

                for (int i = 0; i < sql.Having.Count; i++)
                {
                    LambdaExpression predicateExpression = sql.Having[i];

                    var valueCmd = GetValueExpression(predicateExpression.Body, new SqlContext(level)
                    {
                        Parameters = parameters.Concat(
                            predicateExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i].EntitySource)))
                            ).ToDictionary(s => s.Key, s => s.Value)
                    }, i == 0);

                    if (i > 0 && valueCmd.ValueType == SqlValueValueType.Logic)
                        valueCmd.CommantText = SqlNode.Bracket(valueCmd.CommantText?.ToString());

                    havingConditions.Add(valueCmd.CommantText?.ToString());

                    if (valueCmd.Parameters is not null)
                        command.Parameters.AddRange(valueCmd.Parameters);
                }

                having = "\nHaving\n" + string.Join("\n" + SqlNode.And + " ", havingConditions).Indent(2);
            }

            //Union
            string union = string.Empty;

            if (sql.Union is not null)
            {
                string unionedTable = string.Empty;

                if (sql.Union is DataManipulationSql unionSql)
                {
                    if (unionSql.IsCTE)
                    {
                        string alias = SqlNode.GetCTEAlias(ctes.Count, level);

                        ctes.Add(new()
                        {
                            Alias = alias,
                            Sql = unionSql
                        });

                        unionedTable = Build(new()
                        {
                            Select = _AllColumnsLambdaExpression,
                            From = new()
                            {
                                new()
                                {
                                    Table = new CTETableExpression(alias, unionSql.Type)
                                }
                            },
                            Type = unionSql.Type
                        }, parameters, level).CommandText;
                    }
                    else
                    {
                        var unionCommand = Build(unionSql, new(), 0);

                        unionedTable = unionCommand.CommandText;

                        if (unionSql.Max >= (int)SelectNodeType.Union)
                            unionedTable = SqlNode.SubQuery(unionedTable);

                        command.Parameters.AddRange(unionCommand.Parameters);
                    }
                }
                else if (sql.Union is LambdaExpression lambdaExpression) //cte
                {
                    var sqlValue = GetValueExpression(lambdaExpression.Body, new(level)
                    {
                        Parameters = parameters.Concat(
                            lambdaExpression.Parameters.Select((p) => KeyValuePair.Create(p, LambdaParameterInfo.DataTable(new CTETableExpression(cteAlias, sql.Type))))
                            ).ToDictionary(s => s.Key, s => s.Value)
                    });

                    unionedTable = sqlValue.CommantText?.ToString();

                    if (sqlValue.Parameters is not null)
                        command.Parameters.AddRange(sqlValue.Parameters);
                }

                union = "\nUnion" + (sql.UnionDistinct ? string.Empty : " All") + "\n" + unionedTable.Indent(2);
            }

            //Order By
            string orderBy = string.Empty;

            if (sql.OrderBy is not null)
            {
                List<string> keys = new();

                foreach (OrderedTableExpression expression in sql.OrderBy)
                {
                    LambdaExpression lambdaExpression = (LambdaExpression)expression.KeySelector;

                    var keyCmd = GetValueExpression(lambdaExpression.Body, new SqlContext(level)
                    {
                        Parameters = parameters.Concat(
                            lambdaExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(entityInfos[i].EntitySource)))
                            ).ToDictionary(s => s.Key, s => s.Value)
                    });

                    keys.Add(keyCmd.CommantText + " " + (expression.Descending ? SqlNode.Desc : SqlNode.Asc));

                    if (keyCmd.Parameters is not null)
                        command.Parameters.AddRange(keyCmd.Parameters);
                }

                orderBy = "\nOrder By\n  " + string.Join(", ", keys);
            }

            //Limit
            string limit = string.Empty;

            if (sql.Limit is not null)
            {
                string count = sql.Limit.Taked.ToString();

                if (sql.Limit.Skip > 0)
                    count = sql.Limit.Skip.ToString() + ", " + count;

                limit = "\nLimit\n  " + count;
            }

            //with
            string with = string.Empty;

            if (ctes.Any())
            {
                foreach (var cte in ctes)
                {
                    var cteCommand = BuildCTE(cte.Sql, parameters, level + 1, cte.Alias);

                    cte.CommandText = cteCommand.CommandText;

                    command.Parameters.AddRange(cteCommand.Parameters);
                }

                with = "With Recursive\n" + string.Join(",\n", ctes.Select(s => SqlNode.As(s.Alias, "\n" + SqlNode.SubQuery(s.CommandText)))).Indent(2) + "\n";
            }

            command.CommandText = with + manipulation + from + set + where + groupBy + having + union + orderBy + limit;

            return command;
        }

        Command BuildCTE(DataManipulationSql sql, Dictionary<ParameterExpression, LambdaParameterInfo> parameters, int level, string cteAlias)
        {
            return Build(sql, parameters, level, cteAlias);
        }

        List<SqlColumnInfo> ExpandColumns(IEnumerable<SqlColumnInfo> nodes, Dictionary<MemberInfo, string> groupKeys, ref Dictionary<string, ConstructorInfo> constructors, Dictionary<ParameterExpression, string> indexParemeters)
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
                            Expression = b
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

                        return new SqlColumnInfo()
                        {
                            Alias = SqlNode.Member(node.Alias, ((MemberExpression)assign.Left).Member.Name),
                            Expression = assign.Right
                        };
                    }), groupKeys, ref constructors, indexParemeters));

                    continue;
                }

                if (node.Expression is ParameterExpression parameterExpression)
                {
                    if (indexParemeters.TryGetValue(parameterExpression, out string expression))
                    {
                        columns.Add(new SqlColumnInfo()
                        {
                            Alias = node.Alias,
                            Expression = expression
                        });

                        continue;
                    }

                    foreach (PropertyInfo property in parameterExpression.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        columns.Add(new SqlColumnInfo()
                        {
                            Alias = SqlNode.Member(node.Alias, property.Name),
                            Expression = System.Linq.Expressions.Expression.Property(parameterExpression, property)
                        });
                    }

                    continue;
                }

                if (groupKeys is not null
                    && node.Expression is MemberExpression memberExpression
                    && memberExpression.Expression is ParameterExpression
                    && memberExpression.Expression.Type.IsAssignableTo(typeof(IGroupingDataTable))
                    && memberExpression.Member.Name == nameof(IGroupingDataTable<int>.Key))
                {
                    columns.AddRange(groupKeys.Select(k => new SqlColumnInfo()
                    {
                        Alias = SqlNode.Member(node.Alias, k.Key == _GroupingDataTableKey ? null : k.Key.Name),
                        Expression = k.Value
                    }));

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
                    var funcCmd = GetValueExpression(invocationExpression.Expression, new SqlContext(0));

                    if (funcCmd.IsConstant == false)
                        throw new UnsupportedExpressionException(invocationExpression);

                    if (invocationExpression.Arguments.Count != 1)
                        throw new UnsupportedExpressionException(invocationExpression);

                    node.Reader = (Delegate)funcCmd.ConstantValue;

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
                            var readerCmd = GetValueExpression(readerExpression, new(0));

                            if (readerCmd.IsConstant == false)
                                throw new UnsupportedExpressionException(readerExpression);

                            node.Reader = (Delegate)readerCmd.ConstantValue;
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
                            var bytesReaderCmd = GetValueExpression(readerExpression, new(0));

                            if (bytesReaderCmd.IsConstant == false)
                                throw new UnsupportedExpressionException(readerExpression);

                            node.Reader = (Delegate)bytesReaderCmd.ConstantValue;
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
                            var CharsReaderCmd = GetValueExpression(readerExpression, new(0));

                            if (CharsReaderCmd.IsConstant == false)
                                throw new UnsupportedExpressionException(readerExpression);

                            node.Reader = (Delegate)CharsReaderCmd.ConstantValue;
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
                        var text = new JsonAccess(SqlNode.Member(instanceCmd.CommantText?.ToString(), SqlNode.SqlName(ColumnName(member))));

                        var jsonpath = member.GetCustomAttribute<JsonPathAttribute>();

                        if (jsonpath is not null)
                            text.SetPathRoot(SqlNode.SqlString(jsonpath.Path));

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

                if (unaryExpression.NodeType == ExpressionType.Convert ||
                    unaryExpression.NodeType == ExpressionType.ConvertChecked ||
                    unaryExpression.NodeType == ExpressionType.TypeAs)
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

                    if (operandCommand.ValueType == SqlValueValueType.Calculation)
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

                var left = GetValueExpression(binaryExpression.Left, context, isRoot);

                var right = GetValueExpression(binaryExpression.Right, context);

                List<KeyValuePair<string, object>> ps = new();

                if (left.Parameters != null)
                {
                    ps.AddRange(left.Parameters);
                }

                if (right.Parameters != null)
                {
                    ps.AddRange(right.Parameters);
                }

                if (left.ValueType == SqlValueValueType.Calculation)
                    left.CommantText = SqlNode.Bracket(left.CommantText?.ToString());

                if (right.ValueType == SqlValueValueType.Logic || right.ValueType == SqlValueValueType.Calculation)
                    right.CommantText = SqlNode.Bracket(right.CommantText?.ToString());

                object cmdText = null;

                SqlValueValueType? valueType = null;

                if (binaryExpression.NodeType == ExpressionType.GreaterThan)
                    cmdText = $"{left.CommantText} {SqlNode.GreaterThan} {right.CommantText}";
                else if (binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual)
                    cmdText = $"{left.CommantText} {SqlNode.GreaterThanOrEqual} {right.CommantText}";
                else if (binaryExpression.NodeType == ExpressionType.LessThan)
                    cmdText = $"{left.CommantText} {SqlNode.LessThan} {right.CommantText}";
                else if (binaryExpression.NodeType == ExpressionType.LessThanOrEqual)
                    cmdText = $"{left.CommantText} {SqlNode.LessThanOrEqual} {right.CommantText}";
                else if (binaryExpression.NodeType == ExpressionType.Equal)
                    cmdText = $"{left.CommantText} {((right.IsConstant && right.ConstantValue is null) ? SqlNode.Is : SqlNode.Equal)} {right.CommantText}";
                else if (binaryExpression.NodeType == ExpressionType.NotEqual)
                    cmdText = $"{left.CommantText} {((right.IsConstant && right.ConstantValue is null) ? SqlNode.IsNot : SqlNode.NotEqual)} {right.CommantText}";
                else if (binaryExpression.NodeType == ExpressionType.And)
                {
                    cmdText = $"{left.CommantText} {SqlNode.And} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.AndAlso)
                {
                    cmdText = $"{left.CommantText} {(isRoot ? "\n" : string.Empty) + SqlNode.AndAlso} {right.CommantText}";

                    valueType = SqlValueValueType.Logic;
                }
                else if (binaryExpression.NodeType == ExpressionType.Or)
                {
                    cmdText = $"{left.CommantText} {SqlNode.Or} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.OrElse)
                {
                    cmdText = $"{left.CommantText} {(isRoot ? "\n " : string.Empty) + SqlNode.OrElse} {right.CommantText}";

                    valueType = SqlValueValueType.Logic;
                }
                else if (binaryExpression.NodeType == ExpressionType.Add || binaryExpression.NodeType == ExpressionType.AddChecked)
                {
                    cmdText = $"{left.CommantText} {SqlNode.Add} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.Subtract || binaryExpression.NodeType == ExpressionType.SubtractChecked)
                {
                    cmdText = $"{left.CommantText} {SqlNode.Subtract} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.Multiply || binaryExpression.NodeType == ExpressionType.MultiplyChecked)
                {
                    cmdText = $"{left.CommantText} {SqlNode.Multiply} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.Divide)
                {
                    cmdText = $"{left.CommantText} {SqlNode.Divide} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.Modulo)
                {
                    cmdText = $"{left.CommantText} {SqlNode.Modulo} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.ArrayIndex)
                    cmdText = SqlNode.JsonIndex(
                        left.CommantText as JsonAccess ?? new JsonAccess(left.CommantText?.ToString()),
                        right.CommantText?.ToString()
                        );
                else if (binaryExpression.NodeType == ExpressionType.Coalesce)
                    cmdText = SqlNode.Call("ifNull", left.CommantText?.ToString(), right.CommantText?.ToString());
                else if (binaryExpression.NodeType == ExpressionType.ExclusiveOr)
                {
                    cmdText = $"{left.CommantText} {SqlNode.ExclusiveOr} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.LeftShift)
                {
                    cmdText = $"{left.CommantText} {SqlNode.LeftShift} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else if (binaryExpression.NodeType == ExpressionType.RightShift)
                {
                    cmdText = $"{left.CommantText} {SqlNode.RightShift} {right.CommantText}";

                    valueType = SqlValueValueType.Calculation;
                }
                else
                    throw new UnsupportedExpressionException(expression);

                return new()
                {
                    CommantText = cmdText,
                    Parameters = ps,
                    ValueType = valueType
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

                        if (parameterInfos[i].IsDefined(typeof(ConstantParameterAttribute), true))
                        {
                            if (argCmd.IsConstant is false)
                                throw new UnsupportedExpressionException(arg, $"The parameter {parameterInfos[i].Name} of method {method.Name} must be a constant");

                            string str = argCmd.ConstantValue?.ToString();

                            args.Add(new()
                            {
                                CommantText = str is null ? SqlNode.Null : SqlNode.SqlString(str)
                            });

                            continue;
                        }

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

        Type GetResultType(Type expressionType)
        {
            if (expressionType.IsGenericType && expressionType.GetGenericTypeDefinition() == typeof(IDataTable<>))
                return expressionType.GetGenericArguments()[0];

            return expressionType;
        }

        bool IsIndexMethod(MethodInfo method, out bool indexIsNumber)
        {
            indexIsNumber = method.GetParameters().FirstOrDefault()?.ParameterType == typeof(int);

            return method.DeclaringType.GetProperties().Where(p => p.GetIndexParameters().Length > 0).Select(p => p.GetMethod).Contains(method);
        }

        bool HasIndex(RowFilteredTableExpression rowFilteredTableExpression)
        {
            return rowFilteredTableExpression.Type is not null && ((LambdaExpression)rowFilteredTableExpression.Filter).Parameters.Count == 2;
        }

        string FullName(OriginalTableExpression table)
        {
            string tableName = SqlNode.SqlName(table.Name);

            if (string.IsNullOrEmpty(table.Schema) is false)
                tableName = SqlNode.Member(SqlNode.SqlName(table.Schema), tableName);

            return tableName;
        }

        string ColumnName(MemberInfo member)
        {
            return member.GetCustomAttribute<ColumnAttribute>()?.Name ?? member.Name;
        }

        class CTEInfo
        {
            public string Alias { get; set; }

            public DataManipulationSql Sql { get; set; }

            public string CommandText { get; set; }
        }

        class SqlColumnInfo
        {
            public string Alias { get; set; }

            public SqlContext SqlContext { get; set; }

            public object Expression { get; set; }

            public Delegate Reader { get; set; }
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
    }
}
