using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Default.MySql.Linq.Expressions;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Default.MySql
{
    public partial class LinqConvertProvider
    {
        internal IDataManipulationSql GetDataManipulationSql(LogicEntity.Linq.Expressions.Expression expression)
        {
            if (expression is OriginalTableExpression originalTableExpression)
            {
                return new SqlExpressions.OriginalTableExpression(originalTableExpression.Schema, originalTableExpression.Name)
                {
                    Type = expression.Type
                };
            }
            else if (expression is JoinedTableExpression joinedTableExpression)
            {
                SqlExpressions.ITableExpression right;

                if (joinedTableExpression.Right is OriginalTableExpression originalTable)
                {
                    right = new SqlExpressions.OriginalTableExpression(originalTable.Schema, originalTable.Name);
                }
                else
                {
                    right = GetDataManipulationSql(joinedTableExpression.Right) as SqlExpressions.ITableExpression;
                }

                if (right is null)
                    throw new UnsupportedExpressionException(joinedTableExpression.Right);

                return JoinToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(joinedTableExpression.Left),
                    joinedTableExpression.Join,
                    right,
                    joinedTableExpression.Predicate as LambdaExpression,
                    new()
                    );
            }
            else if (expression is RowFilteredTableExpression rowFilteredTableExpression)
            {
                return WhereToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(rowFilteredTableExpression.Source),
                    (LambdaExpression)rowFilteredTableExpression.Filter,
                    rowFilteredTableExpression.HasIndex,
                    new()
                    );
            }
            else if (expression is GroupedTableExpression groupedTableExpression)
            {
                return GroupToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(groupedTableExpression.Source),
                    (LambdaExpression)groupedTableExpression.KeySelector,
                    new()
                    );
            }
            else if (expression is UnionedTableExpression unionedTableExpression)
            {
                return BinaryTableToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(unionedTableExpression.Left),
                    SqlExpressions.BinaryTableExpression.BinaryOperate.Union,
                    unionedTableExpression.Distinct,
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(unionedTableExpression.Right)
                    );
            }
            else if (expression is IntersectTableExpression intersectTableExpression)
            {
                return BinaryTableToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(intersectTableExpression.Left),
                    SqlExpressions.BinaryTableExpression.BinaryOperate.Intersect,
                    intersectTableExpression is not IntersectAllTableExpression,
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(intersectTableExpression.Right)
                    );
            }
            else if (expression is ExceptTableExpression exceptTableExpression)
            {
                return BinaryTableToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(exceptTableExpression.Left),
                    SqlExpressions.BinaryTableExpression.BinaryOperate.Except,
                    exceptTableExpression is not ExceptAllTableExpression,
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(exceptTableExpression.Right)
                    );
            }
            else if (expression is OrderedTableExpression orderedTableExpression)
            {
                return OrderByToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(orderedTableExpression.Source),
                    orderedTableExpression.Ordered,
                    (LambdaExpression)orderedTableExpression.KeySelector,
                    orderedTableExpression.Descending,
                    new()
                    );
            }
            else if (expression is SkippedTableExpression skippedTableExpression)
            {
                return SkipToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(skippedTableExpression.Source),
                    skippedTableExpression.Count
                    );
            }
            else if (expression is TakedTableExpression takedTableExpression)
            {
                return TakeToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(takedTableExpression.Source),
                    takedTableExpression.Count
                    );
            }
            else if (expression is SelectedTableExpression selectedTableExpression)
            {
                SqlExpressions.ISelectSql sql = selectedTableExpression.Source is null ? new SqlExpressions.SelectExpression() : (SqlExpressions.ISelectSql)GetDataManipulationSql(selectedTableExpression.Source);

                return SelectToSql(
                    sql,
                    selectedTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is DistinctTableExpression distinctTableExpression)
            {
                return DistinctToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(distinctTableExpression.Source)
                    );
            }
            else if (expression is AverageTableExpression averageTableExpression)
            {
                return AverageToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(averageTableExpression.Source),
                    (LambdaExpression)averageTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is CountTableExpression countTableExpression)
            {
                return CountToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(countTableExpression.Source),
                    expression.Type
                    );
            }
            else if (expression is MaxTableExpression maxTableExpression)
            {
                return MaxToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(maxTableExpression.Source),
                    (LambdaExpression)maxTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is MinTableExpression minTableExpression)
            {
                return MinToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(minTableExpression.Source),
                    (LambdaExpression)minTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is SumTableExpression sumTableExpression)
            {
                return SumToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(sumTableExpression.Source),
                    (LambdaExpression)sumTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is AnyTableExpression anyTableExpression)
            {
                return AnyToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(anyTableExpression.Source)
                    );
            }
            else if (expression is AllTableExpression allTableExpression)
            {
                return AllToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(allTableExpression.Source),
                    (LambdaExpression)allTableExpression.Predicate,
                    new()
                    );
            }
            else if (expression is RecursiveUnionedTableExpression recursiveUnionedTableExpression)
            {
                return RecursiveUnionToSql(
                    (SqlExpressions.ISelectSql)GetDataManipulationSql(recursiveUnionedTableExpression.Left),
                    recursiveUnionedTableExpression.Distinct,
                    recursiveUnionedTableExpression.RightFactory,
                    new()
                    );
            }
            else if (expression is RemoveOperateExpression removeOperateExpression)
            {
                var deleteExpression = GetDataManipulationSql(removeOperateExpression.Source).AddDelete();

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

                deleteExpression.Type = expression.Type;

                return deleteExpression;
            }
            else if (expression is SetOperateExpression setOperateExpression)
            {
                var updateExpression = GetDataManipulationSql(setOperateExpression.Source).AddUpdateSet();

                LambdaExpression[] lambdaExpressions = (LambdaExpression[])setOperateExpression.Assignments;

                updateExpression.Assignments.AddRange(lambdaExpressions.Select(lambdaExpression =>
                {
                    MethodCallExpression methodCallExpression = lambdaExpression.Body as MethodCallExpression;

                    if (methodCallExpression is null || methodCallExpression.Method.DeclaringType != typeof(SetOperatorFunction))
                        throw new UnsupportedExpressionException(lambdaExpression.Body);

                    SqlExpressions.SqlContext sqlContext = new()
                    {
                        LambdaParameters = new()
                    };

                    for (int i = 0; i < lambdaExpression.Parameters.Count; i++)
                    {
                        sqlContext.LambdaParameters[lambdaExpression.Parameters[i]] = SqlExpressions.LambdaParameterInfo.Table(updateExpression.From.GetTable(i));
                    }

                    return new SqlExpressions.AssignmentExpression(
                        (SqlExpressions.IValueExpression)GetSqlExpression(methodCallExpression.Arguments[0], sqlContext),
                        (SqlExpressions.IValueExpression)GetSqlExpression(methodCallExpression.Arguments[1], sqlContext)
                        );
                }));

                updateExpression.Type = expression.Type;

                return updateExpression;
            }
            else if (expression is TimeoutTableExpression timeoutTableExpression)
            {
                return TimeoutToSql(GetDataManipulationSql(timeoutTableExpression.Source), timeoutTableExpression.Timeout);
            }

            throw new UnsupportedExpressionException(expression);
        }

        SqlExpressions.SelectExpression JoinToSql(SqlExpressions.ISelectSql left, string join, SqlExpressions.ITableExpression right, LambdaExpression lambdaExpression, SqlExpressions.SqlContext context)
        {
            var selectExpression = left.AddJoin();

            SqlExpressions.JoinedTableExpression.JoinedTable joinedTable = new()
            {
                Join = join,
                TableExpression = right
            };

            selectExpression.AddJoinedTable(joinedTable);

            if (lambdaExpression is not null)
            {
                joinedTable.Predicate = (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body,
                    context.ConcatParameters(
                        lambdaExpression.Parameters
                            .Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i))))
                        )
                    );
            }

            return selectExpression;
        }

        SqlExpressions.SelectExpression WhereToSql(SqlExpressions.ISelectSql selectSql, LambdaExpression lambdaExpression, bool hasIndex, SqlExpressions.SqlContext context)
        {
            Type type = selectSql.Type;

            if (hasIndex)
            {
                selectSql = selectSql.AddIndex();

                selectSql = new SqlExpressions.SelectExpression(selectSql);
            }

            SqlExpressions.SelectExpression selectExpression;

            if (selectSql.CanAddNode(SelectNodeType.Where))
            {
                selectExpression = selectSql.AddWhere();

                int count = selectExpression.From.Count;

                SqlExpressions.IValueExpression value = (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context.ConcatParameters(
                    lambdaExpression.Parameters.Select((p, i) =>
                    {
                        if (i < count)
                            return KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i)));
                        else
                            return KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.IndexColumn(new(selectExpression.From)));
                    })
                    ));

                if (selectExpression.Where is null)
                    selectExpression.Where = value;
                else
                    selectExpression.Where = new SqlExpressions.AndAlsoExpression(selectExpression.Where, value);
            }
            else if (selectSql.CanAddNode(SelectNodeType.Having))
            {
                selectExpression = selectSql.AddHaving();

                SqlExpressions.IValueExpression parameterExpression = selectExpression.IsVector ?
                    SqlExpressions.SqlExpression.Empty : new SqlExpressions.ColumnExpression(null, selectExpression.Columns[0].Alias);

                SqlExpressions.IValueExpression value = (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context.ConcatParameters(new KeyValuePair<ParameterExpression, SqlExpressions.LambdaParameterInfo>[]
                {
                    KeyValuePair.Create(lambdaExpression.Parameters[0], SqlExpressions.LambdaParameterInfo.Value(parameterExpression))
                }));

                if (selectExpression.Having is null)
                    selectExpression.Having = value;
                else
                    selectExpression.Having = new SqlExpressions.AndExpression(selectExpression.Having, value);
            }
            else
            {
                selectExpression = new SqlExpressions.SelectExpression(selectSql).AddWhere();

                int count = selectExpression.From.Count;

                SqlExpressions.IValueExpression value = (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context.ConcatParameters(
                    lambdaExpression.Parameters.Select((p, i) =>
                    {
                        if (i < count)
                            return KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i)));
                        else
                            return KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.IndexColumn(new(selectExpression.From)));
                    })
                    ));

                if (selectExpression.Where is null)
                    selectExpression.Where = value;
                else
                    selectExpression.Where = new SqlExpressions.AndExpression(selectExpression.Where, value);
            }

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression GroupToSql(SqlExpressions.ISelectSql selectSql, LambdaExpression lambdaExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.SelectExpression selectExpression = selectSql.AddGroupBy();

            Dictionary<MemberInfo, SqlExpressions.IValueExpression> groupKeys = new();

            context = context.ConcatParameters(
                lambdaExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i))))
                );

            if (lambdaExpression.Body is NewExpression newExpression)
            {
                if (newExpression.Arguments.Count != newExpression.Members?.Count)
                    throw new UnsupportedExpressionException(newExpression);

                foreach (var member in newExpression.Members.Zip(newExpression.Arguments, (a, b) => new { MemberInfo = a, Expression = b }))
                {
                    groupKeys[member.MemberInfo] = (SqlExpressions.IValueExpression)GetSqlExpression(member.Expression, context);
                }
            }
            else
            {
                groupKeys[_GroupingDataTableKey] = (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context);
            }

            selectExpression.GroupBy = groupKeys;

            return selectExpression;
        }

        SqlExpressions.BinaryTableExpression BinaryTableToSql(SqlExpressions.ISelectSql left, SqlExpressions.BinaryTableExpression.BinaryOperate binaryOperate, bool isDistinct, SqlExpressions.ISelectSql right)
        {
            SqlExpressions.BinaryTableExpression binaryTableExpression = new SqlExpressions.BinaryTableExpression()
            {
                Left = left,
                Operate = binaryOperate,
                IsDistinct = isDistinct,
                Right = right
            };

            binaryTableExpression.Type = left.Type;

            return binaryTableExpression;
        }

        SqlExpressions.ISelectSql OrderByToSql(SqlExpressions.ISelectSql selectSql, bool ordered, LambdaExpression lambdaExpression, bool descending, SqlExpressions.SqlContext context)
        {
            Type type = selectSql.Type;

            if (ordered)
            {
                selectSql = selectSql.AddThenBy();

                selectSql.OrderBy.Expressions.Add(GetOrderExpression());
            }
            else
            {
                selectSql = selectSql.AddOrderBy();

                selectSql.OrderBy = new SqlExpressions.OrderKeys(GetOrderExpression());
            }

            selectSql.Type = type;

            return selectSql;

            SqlExpressions.OrderExpression GetOrderExpression()
            {
                var expressions = selectSql.GetOrderByParameters();

                context = context.ConcatParameters(lambdaExpression.Parameters.Select((p, i) =>
                {
                    var expression = expressions[i];

                    return KeyValuePair.Create(p, expression is SqlExpressions.IValueExpression valueExpression ? SqlExpressions.LambdaParameterInfo.Value(valueExpression) : SqlExpressions.LambdaParameterInfo.LambdaParameter((SqlExpressions.ITableExpression)expression));
                }));

                return new SqlExpressions.OrderExpression((SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context), descending);
            }
        }

        SqlExpressions.ISelectSql SkipToSql(SqlExpressions.ISelectSql selectSql, int count)
        {
            Type type = selectSql.Type;

            selectSql = selectSql.AddLimit();

            if (selectSql.Limit is null)
                selectSql.Limit = new();

            selectSql.Limit.Offset += count;
            selectSql.Limit.Limit -= count;

            selectSql.Type = type;

            return selectSql;
        }

        SqlExpressions.ISelectSql TakeToSql(SqlExpressions.ISelectSql selectSql, int count)
        {
            Type type = selectSql.Type;

            selectSql = selectSql.AddLimit();

            if (selectSql.Limit is null)
                selectSql.Limit = new();

            if (count < selectSql.Limit.Limit)
                selectSql.Limit.Limit = count;

            if (selectSql.Limit.Limit < 0)
                selectSql.Limit.Limit = 0;

            selectSql.Type = type;

            return selectSql;
        }

        SqlExpressions.SelectExpression SelectToSql(SqlExpressions.ISelectSql selectSql, object selector, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = selectSql.AddSelect();

            selectExpression.Columns.Clear();

            Dictionary<string, ConstructorInfo> constructors = new();

            selectExpression.Columns.AddRange(GetSelectColumns(selectExpression, selector, ref constructors, context));

            selectExpression.Constructors = constructors;

            selectExpression.IsVector = selectExpression.Columns.Count > 1 || (selector is LambdaExpression lambdaExpression && lambdaExpression.Body is NewExpression);

            if (selectExpression.IsVector == false && selectExpression.Columns[0].Alias is null)
                selectExpression.Columns[0].Alias = SqlNode.ScalarAlias;

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression DistinctToSql(SqlExpressions.ISelectSql selectSql)
        {
            Type type = selectSql.Type;

            var selectExpression = selectSql.Distinct();

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression AverageToSql(SqlExpressions.ISelectSql selectSql, LambdaExpression lambdaExpression, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = selectSql.AddSelect();

            selectExpression.Columns.Clear();

            context = context.ConcatParameters(
                lambdaExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i))))
                );

            selectExpression.Columns.Add(new SqlExpressions.ColumnInfo()
            {
                ValueExpression = new SqlExpressions.MethodCallExpression("Avg", (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context))
            });

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression CountToSql(SqlExpressions.ISelectSql selectSql, Type type)
        {
            var selectExpression = selectSql.AddSelect();

            selectExpression.Columns.Clear();

            selectExpression.Columns.Add(new SqlExpressions.ColumnInfo()
            {
                ValueExpression = SqlExpressions.SqlExpression.CountExpression()
            });

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression MaxToSql(SqlExpressions.ISelectSql selectSql, LambdaExpression lambdaExpression, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = selectSql.AddSelect();

            selectExpression.Columns.Clear();

            context = context.ConcatParameters(
                lambdaExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i))))
                );

            selectExpression.Columns.Add(new SqlExpressions.ColumnInfo()
            {
                ValueExpression = new SqlExpressions.MethodCallExpression("Max", (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context))
            });

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression MinToSql(SqlExpressions.ISelectSql selectSql, LambdaExpression lambdaExpression, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = selectSql.AddSelect();

            selectExpression.Columns.Clear();

            context = context.ConcatParameters(
                lambdaExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i))))
                );

            selectExpression.Columns.Add(new SqlExpressions.ColumnInfo()
            {
                ValueExpression = new SqlExpressions.MethodCallExpression("Min", (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context))
            });

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression SumToSql(SqlExpressions.ISelectSql selectSql, LambdaExpression lambdaExpression, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = selectSql.AddSelect();

            selectExpression.Columns.Clear();

            context = context.ConcatParameters(
                lambdaExpression.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(selectExpression.From.GetTable(i))))
                );

            selectExpression.Columns.Add(new SqlExpressions.ColumnInfo()
            {
                ValueExpression = new SqlExpressions.MethodCallExpression("Sum", (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context))
            });

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression AnyToSql(SqlExpressions.ISelectSql selectSql)
        {
            var sourceExpression = selectSql.AddSelect();

            sourceExpression.Columns.Clear();

            sourceExpression.Columns.Add(new()
            {
                ValueExpression = SqlExpressions.SqlExpression.AllColumns()
            });

            SqlExpressions.SelectExpression selectExpression = new SqlExpressions.SelectExpression().AddSelect();

            selectExpression.Columns.Clear();

            selectExpression.Columns.Add(new SqlExpressions.ColumnInfo()
            {
                ValueExpression = new SqlExpressions.ExistsExpression(sourceExpression)
            });

            selectExpression.Type = typeof(bool);

            return selectExpression;
        }

        SqlExpressions.SelectExpression AllToSql(SqlExpressions.ISelectSql selectSql, LambdaExpression lambdaExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.SelectExpression sourceExpression = WhereToSql(
                selectSql,
                System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Not(lambdaExpression.Body),
                    lambdaExpression.Parameters
                ),
                false,
                context
                ).AddSelect();

            sourceExpression.Columns.Clear();

            sourceExpression.Columns.Add(new()
            {
                ValueExpression = SqlExpressions.SqlExpression.AllColumns()
            });

            SqlExpressions.SelectExpression selectExpression = new SqlExpressions.SelectExpression().AddSelect();

            selectExpression.Columns.Clear();

            selectExpression.Columns.Add(new SqlExpressions.ColumnInfo()
            {
                ValueExpression = new SqlExpressions.NotExpression(new SqlExpressions.ExistsExpression(sourceExpression))
            });

            selectExpression.Type = typeof(bool);

            return selectExpression;
        }

        SqlExpressions.CommonTableExpression RecursiveUnionToSql(SqlExpressions.ISelectSql left, bool distinct, LambdaExpression rightFactory, SqlExpressions.SqlContext context)
        {
            Type type = left.Type;

            var binaryTableExpression = new SqlExpressions.BinaryTableExpression()
            {
                Left = left,
                Operate = SqlExpressions.BinaryTableExpression.BinaryOperate.Union,
                IsDistinct = distinct
            };

            var commonTableExpression = new SqlExpressions.CommonTableExpression(binaryTableExpression, true);

            commonTableExpression.CanModify = true;

            binaryTableExpression.Right = (SqlExpressions.ISelectSql)GetSqlExpression(rightFactory.Body, context.ConcatParameters(
                new KeyValuePair<ParameterExpression, SqlExpressions.LambdaParameterInfo>[]
                {
                    KeyValuePair.Create(rightFactory.Parameters[0], SqlExpressions.LambdaParameterInfo.Table(commonTableExpression))
                }));

            commonTableExpression.Type = type;

            return commonTableExpression;
        }

        IDataManipulationSql TimeoutToSql(IDataManipulationSql sql, int timeout)
        {
            sql.Timeout = timeout;

            return sql;
        }
    }
}
