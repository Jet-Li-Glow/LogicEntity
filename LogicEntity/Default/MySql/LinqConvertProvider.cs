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
            return GetTableExpression(tableExpression).Build(this);
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
                insertExpression.Table = (SqlExpressions.OriginalTableExpression)GetTableExpression(addOrUpdateWithFactoryOperateExpression.Source);

                //Values
                string valuesExpression = string.Empty;

                if (addOrUpdateWithFactoryOperateExpression.DataSource == AddDataSource.Entity)
                {
                    PropertyInfo[] properties = insertExpression.Table.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).ToArray();

                    Dictionary<PropertyInfo, EntityPropertyInfo> validProperties = new();

                    //Rows
                    List<Dictionary<PropertyInfo, PropertyValue>> rows = new();

                    foreach (object element in addOrUpdateWithFactoryOperateExpression.Elements)
                    {
                        Dictionary<PropertyInfo, PropertyValue> row = new();

                        if (element is LambdaExpression lambdaExpression)
                        {
                            MemberInitExpression memberInitExpression = lambdaExpression.Body as MemberInitExpression;

                            if (memberInitExpression is null)
                                throw new UnsupportedExpressionException(lambdaExpression.Body);

                            foreach (MemberBinding memberBinding in memberInitExpression.Bindings)
                            {
                                if (memberBinding.BindingType != MemberBindingType.Assignment)
                                    throw new UnsupportedExpressionException(memberInitExpression);

                                MemberAssignment memberAssignment = (MemberAssignment)memberBinding;

                                PropertyInfo property = memberAssignment.Member as PropertyInfo;

                                if (property is null)
                                    throw new UnsupportedExpressionException(memberInitExpression);

                                if (validProperties.ContainsKey(property) == false)
                                {
                                    validProperties.Add(property, new());
                                }

                                row[property] = new()
                                {
                                    Expression = memberAssignment.Expression,
                                    Type = PropertyValue.ValueType.Expression
                                };
                            }
                        }
                        else
                        {
                            foreach (PropertyInfo property in properties)
                            {
                                if (validProperties.ContainsKey(property) == false)
                                {
                                    validProperties.Add(property, new());
                                }

                                row[property] = new()
                                {
                                    Object = property.GetValue(element),
                                    Type = PropertyValue.ValueType.Object
                                };
                            }
                        }

                        rows.Add(row);
                    }

                    //Property Writer
                    foreach (KeyValuePair<PropertyInfo, EntityPropertyInfo> propertyInfo in validProperties)
                    {
                        Expression<Func<object, object>> writer = null;

                        if (PropertyConvert.TryGetValue(propertyInfo.Key, out ValueConverter converter) && converter.Writer is not null)
                        {
                            ParameterExpression obj = System.Linq.Expressions.Expression.Parameter(typeof(object));

                            System.Linq.Expressions.Expression val = obj;

                            Type parameterType = converter.Writer.Method.GetParameters()[0].ParameterType;

                            if (val.Type != parameterType)
                                val = System.Linq.Expressions.Expression.Convert(val, parameterType);

                            val = System.Linq.Expressions.Expression.Invoke(System.Linq.Expressions.Expression.Constant(converter.Writer), val);

                            writer = System.Linq.Expressions.Expression.Lambda<Func<object, object>>(val, obj);
                        }

                        propertyInfo.Value.Writer = writer?.Compile();
                    }

                    //Expression
                    insertExpression.Columns = validProperties.Select(s => s.Key).ToList();

                    List<SqlExpressions.ValuesExpression> rowExpressions = new();

                    foreach (Dictionary<PropertyInfo, PropertyValue> row in rows)
                    {
                        List<SqlExpressions.IValueExpression> valueExpressions = new();

                        foreach (KeyValuePair<PropertyInfo, EntityPropertyInfo> entityProperty in validProperties)
                        {
                            if (row.TryGetValue(entityProperty.Key, out PropertyValue value))
                            {
                                if (value.Type == PropertyValue.ValueType.Object)
                                {
                                    valueExpressions.Add(new SqlExpressions.ParameterExpression(
                                        entityProperty.Value.Writer is null ? value.Object : entityProperty.Value.Writer(value.Object)
                                        ));
                                }
                                else
                                {
                                    valueExpressions.Add((SqlExpressions.IValueExpression)GetSqlExpression(value.Expression, new()));
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
                    insertExpression.Rows = GetTableExpression(addOrUpdateWithFactoryOperateExpression.DataTable.Expression);
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

                        insertExpression.Assignments = memberInitExpression.Bindings.Select(memberBinding =>
                        {
                            if (memberBinding.BindingType != MemberBindingType.Assignment)
                                throw new UnsupportedExpressionException(memberInitExpression);

                            MemberAssignment memberAssignment = (MemberAssignment)memberBinding;

                            var left = new SqlExpressions.MemberExpression(insertExpression.Table, memberAssignment.Member);

                            System.Linq.Expressions.Expression right = memberAssignment.Expression;

                            if (_updateFactoryVersion == UpdateFactoryVersion.V5_7)
                                right = columnVisitor.Visit(right);

                            return new SqlExpressions.AssignmentExpression(left, (SqlExpressions.IValueExpression)GetSqlExpression(right, sqlContext));
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
                var deleteExpression = GetTableExpression(removeOperateExpression.Source).AddDelete();

                if (removeOperateExpression.Selectors is not null)
                {
                    LambdaExpression[] lambdaExpressions = (LambdaExpression[])removeOperateExpression.Selectors;

                    deleteExpression.DeletedTables.AddRange(lambdaExpressions.Select(lambdaExpression =>
                    {
                        if (lambdaExpression.Body is not ParameterExpression parameterExpression)
                            throw new UnsupportedExpressionException(lambdaExpression.Body);

                        return deleteExpression.From.GetTable(lambdaExpression.Parameters.IndexOf(parameterExpression));
                    }));
                }

                deleteExpression.Type = removeOperateExpression.Type;

                return deleteExpression.Build(this);
            }

            if (operateExpression is SetOperateExpression setOperateExpression)
            {
                var updateExpression = GetTableExpression(setOperateExpression.Source).AddUpdateSet();

                LambdaExpression[] lambdaExpressions = (LambdaExpression[])setOperateExpression.Assignments;

                updateExpression.Assignments.AddRange(lambdaExpressions.Select(lambdaExpression =>
                {
                    SqlExpressions.SqlContext context = new()
                    {
                        LambdaParameters = new()
                    };

                    for (int i = 0; i < lambdaExpression.Parameters.Count; i++)
                    {
                        context.LambdaParameters[lambdaExpression.Parameters[i]] = SqlExpressions.LambdaParameterInfo.Table(updateExpression.From.GetTable(i));
                    }

                    var assignmentExpression = GetSqlExpression(lambdaExpression.Body, context) as SqlExpressions.AssignmentExpression;

                    if (assignmentExpression is null)
                        throw new UnsupportedExpressionException(lambdaExpression.Body);

                    return assignmentExpression;
                }));

                updateExpression.Type = setOperateExpression.Type;

                return updateExpression.Build(this);
            }

            throw new UnsupportedExpressionException(operateExpression);
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

                    columns.AddRange(ExpandColumns(memberInitExpression.Bindings.Select(memberBinding =>
                    {
                        if (memberBinding.BindingType != MemberBindingType.Assignment)
                            throw new UnsupportedExpressionException(memberInitExpression);

                        MemberAssignment memberAssignment = (MemberAssignment)memberBinding;

                        return new SqlColumnInfo()
                        {
                            Alias = SqlNode.Member(node.Alias, memberAssignment.Member.Name),
                            Expression = memberAssignment.Expression,
                            Member = memberAssignment.Member
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

                    if (node.Member is null)
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

                    ParameterInfo[] parameterInfos = method.GetParameters();

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
                    else if (method.IsStatic && parameterInfos.Length == 1 && TryGetMemberFormat(method, out _) == false)
                    {
                        node.Reader = method.CreateDelegate(typeof(Func<,>).MakeGenericType(method.ReturnType, parameterInfos[0].ParameterType));

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
                if (selectExpression.IsVector)
                {
                    ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(selectExpression.Type);

                    selector = System.Linq.Expressions.Expression.Lambda(parameter, parameter);
                }
                else
                {
                    columnInfos.Add(new()
                    {
                        Expression = new SqlExpressions.ColumnExpression(null, selectExpression.From.Columns[0].Alias)
                    });
                }
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
                            s.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i))))
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
                            lambdaParameterInfo = SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i));
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

                bool hasFormat = TryGetMemberFormat(memberExpression.Member, out object format);

                if (hasFormat && format is Func<MemberExpression, SqlExpressions.SqlContext, SqlExpressions.ISqlExpression> formatFunc)
                {
                    return formatFunc(memberExpression, context);
                }

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

                if (hasFormat && format is string formatStr)
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

                if (sqlExpression is SqlExpressions.GroupKeyExpression groupKeyExpression)
                {
                    return groupKeyExpression.Members[memberExpression.Member];
                }

                if (memberExpression.Expression is not ParameterExpression && sqlExpression is SqlExpressions.ISubQuerySql subQuerySql)
                {
                    SqlExpressions.SelectExpression selectExpression = subQuerySql.ChangeColumns();

                    if (selectExpression.IsVector)
                    {
                        if (selectExpression.Columns.Count > 0)
                        {
                            var column = selectExpression.Columns.Single(s => s.Member == memberExpression.Member);

                            column.Alias = memberExpression.Member.Name;

                            selectExpression.Columns.Clear();

                            selectExpression.Columns.Add(column);
                        }
                        else
                        {
                            selectExpression.Columns.Add(new()
                            {
                                Alias = memberExpression.Member.Name,
                                ValueExpression = new SqlExpressions.MemberExpression(selectExpression.From.GetTable(0), memberExpression.Member),
                                Member = memberExpression.Member
                            });
                        }

                        selectExpression.IsVector = false;
                    }
                    else
                    {
                        var columnInfo = selectExpression.Columns[0];

                        columnInfo.Alias = memberExpression.Member.Name;

                        columnInfo.ValueExpression = (SqlExpressions.IValueExpression)MemberAccess(columnInfo.ValueExpression);
                    }

                    return selectExpression;
                }

                return MemberAccess(sqlExpression);

                SqlExpressions.ISqlExpression MemberAccess(SqlExpressions.ISqlExpression instanceExpression)
                {
                    if (instanceExpression is SqlExpressions.JsonExtractExpression jsonExtractExpression)
                    {
                        jsonExtractExpression.Member(memberExpression.Member);

                        return jsonExtractExpression;
                    }

                    if (instanceExpression is SqlExpressions.MemberExpression sqlMemberExpression)
                    {
                        SqlExpressions.JsonExtractExpression sqlJsonExtractExpression = new SqlExpressions.JsonExtractExpression(sqlMemberExpression);

                        sqlJsonExtractExpression.Member(memberExpression.Member);

                        return sqlJsonExtractExpression;
                    }

                    return new SqlExpressions.MemberExpression(instanceExpression, memberExpression.Member);
                }
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

            if (expression.NodeType == ExpressionType.New)
            {
                NewExpression newExpression = (NewExpression)expression;

                List<SqlExpressions.IValueExpression> parameters = new();

                if (newExpression.Members is not null)
                {
                    foreach (var memberArgument in newExpression.Members.Zip(newExpression.Arguments, (member, argument) => new { Member = member, Argument = argument }))
                    {
                        parameters.Add(new SqlExpressions.ConstantExpression(memberArgument.Member.Name));

                        parameters.Add((SqlExpressions.IValueExpression)GetSqlExpression(memberArgument.Argument, context));
                    }

                    return new SqlExpressions.MethodCallExpression("Json_Object", parameters.ToArray());
                }

                if (newExpression.Type.IsArray || (newExpression.Type.IsGenericType && newExpression.Type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    return new SqlExpressions.MethodCallExpression("Json_Array");
                }

                return new SqlExpressions.MethodCallExpression("Json_Object");
            }

            if (expression.NodeType == ExpressionType.MemberInit)
            {
                MemberInitExpression memberInitExpression = (MemberInitExpression)expression;

                List<SqlExpressions.IValueExpression> parameters = new();

                foreach (MemberBinding memberBinding in memberInitExpression.Bindings)
                {
                    if (memberBinding.BindingType != MemberBindingType.Assignment)
                        throw new UnsupportedExpressionException(memberInitExpression);

                    MemberAssignment memberAssignment = (MemberAssignment)memberBinding;

                    parameters.Add(new SqlExpressions.ConstantExpression(memberAssignment.Member.Name));

                    parameters.Add((SqlExpressions.IValueExpression)GetSqlExpression(memberAssignment.Expression, context));
                }

                return new SqlExpressions.MethodCallExpression("Json_Object", parameters.ToArray());
            }

            if (expression.NodeType == ExpressionType.ListInit)
            {
                ListInitExpression listInitExpression = (ListInitExpression)expression;

                if (listInitExpression.Type.IsGenericType && listInitExpression.Type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return new SqlExpressions.MethodCallExpression("Json_Array", listInitExpression.Initializers.Select(s => (SqlExpressions.IValueExpression)GetSqlExpression(s.Arguments[0], context)).ToArray());
                }

                if (listInitExpression.Type.IsGenericType && listInitExpression.Type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    List<SqlExpressions.IValueExpression> parameters = new();

                    foreach (ElementInit elementInit in listInitExpression.Initializers)
                    {
                        parameters.Add((SqlExpressions.IValueExpression)GetSqlExpression(elementInit.Arguments[0], context));

                        parameters.Add((SqlExpressions.IValueExpression)GetSqlExpression(elementInit.Arguments[1], context));
                    }

                    return new SqlExpressions.MethodCallExpression("Json_Object", parameters.ToArray());
                }

                throw new UnsupportedExpressionException(listInitExpression);
            }

            if (expression.NodeType == ExpressionType.NewArrayInit)
            {
                NewArrayExpression newArrayExpression = (NewArrayExpression)expression;

                return new SqlExpressions.MethodCallExpression("Json_Array", newArrayExpression.Expressions.Select(s => (SqlExpressions.IValueExpression)GetSqlExpression(s, context)).ToArray());
            }

            if (expression.NodeType == ExpressionType.NewArrayBounds)
            {
                return new SqlExpressions.MethodCallExpression("Json_Array");
            }

            throw new UnsupportedExpressionException(expression);
        }

        bool TryUnboxSubquery(object obj, out SqlExpressions.ISqlExpression sqlExpression)
        {
            sqlExpression = null;

            if (obj is not null && obj.GetType().IsAssignableTo(typeof(IDataTable)))
            {
                var tableExpression = GetTableExpression(((IDataTable)obj).Expression);

                if (tableExpression is SqlExpressions.OriginalTableExpression originalTableExpression)
                {
                    sqlExpression = originalTableExpression;

                    return true;
                }

                if (tableExpression is SqlExpressions.CommonTableExpression commonTableExpression)
                {
                    commonTableExpression.CanModify = false;

                    sqlExpression = commonTableExpression;
                }
                else
                {
                    sqlExpression = new SqlExpressions.CommonTableExpression((SqlExpressions.ITableExpression)tableExpression, false);
                }

                return true;
            }

            return false;
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
                && GetUnderlyingType(((UnaryExpression)binaryExpression.Left).Operand.Type).IsSubclassOf(typeof(Enum))
                && IsConvertExpression(binaryExpression.Right)
                && GetUnderlyingType(((UnaryExpression)binaryExpression.Right).Operand.Type).IsSubclassOf(typeof(Enum));
        }

        bool IsConvertExpression(System.Linq.Expressions.Expression expression)
        {
            return expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked || expression.NodeType == ExpressionType.TypeAs;
        }

        Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
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

        class EntityPropertyInfo
        {
            public Func<object, object> Writer { get; set; }
        }

        class ParameterValueExpressionInfo
        {
            public SqlExpressions.IValueExpression Value { get; set; }

            public IEnumerable<SqlExpressions.IValueExpression> Values { get; set; }

            public bool IsParamArray { get; set; } = false;

            public bool IsConstant => IsParamArray ? Values.All(v => v is SqlExpressions.ConstantExpression) : Value is SqlExpressions.ConstantExpression;

            public object ConstantValue => IsParamArray ? Values.Select(s => ((SqlExpressions.ConstantExpression)s).Value).ToArray() : ((SqlExpressions.ConstantExpression)Value).Value;
        }

        class PropertyValue
        {
            public object Object { get; set; }

            public System.Linq.Expressions.Expression Expression { get; set; }

            public ValueType Type { get; set; }

            public enum ValueType
            {
                Object,
                Expression
            }
        }
    }
}
