using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Default.MySql.Linq.Expressions;
using LogicEntity.Linq.Expressions;
using static LogicEntity.Default.MySql.SqlExpressions.JoinedTableExpression;

namespace LogicEntity.Default.MySql
{
    public partial class LinqConvertProvider
    {
        internal SqlExpressions.ITableExpression GetTableExpression(LogicEntity.Linq.Expressions.Expression expression)
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
                return JoinToSql(
                    GetTableExpression(joinedTableExpression.Left),
                    joinedTableExpression.Join,
                    GetTableExpression(joinedTableExpression.Right),
                    joinedTableExpression.Predicate as LambdaExpression,
                    new()
                    );
            }
            else if (expression is RowFilteredTableExpression rowFilteredTableExpression)
            {
                return WhereToSql(
                    GetTableExpression(rowFilteredTableExpression.Source),
                    (LambdaExpression)rowFilteredTableExpression.Filter,
                    rowFilteredTableExpression.HasIndex,
                    new()
                    );
            }
            else if (expression is GroupedTableExpression groupedTableExpression)
            {
                return GroupToSql(
                    GetTableExpression(groupedTableExpression.Source),
                    (LambdaExpression)groupedTableExpression.KeySelector,
                    new()
                    );
            }
            else if (expression is UnionedTableExpression unionedTableExpression)
            {
                return BinaryTableToSql(
                    GetTableExpression(unionedTableExpression.Left),
                    SqlExpressions.BinaryTableExpression.BinaryOperate.Union,
                    unionedTableExpression.Distinct,
                    GetTableExpression(unionedTableExpression.Right)
                    );
            }
            else if (expression is IntersectTableExpression intersectTableExpression)
            {
                return BinaryTableToSql(
                    GetTableExpression(intersectTableExpression.Left),
                    SqlExpressions.BinaryTableExpression.BinaryOperate.Intersect,
                    intersectTableExpression is not IntersectAllTableExpression,
                    GetTableExpression(intersectTableExpression.Right)
                    );
            }
            else if (expression is ExceptTableExpression exceptTableExpression)
            {
                return BinaryTableToSql(
                    GetTableExpression(exceptTableExpression.Left),
                    SqlExpressions.BinaryTableExpression.BinaryOperate.Except,
                    exceptTableExpression is not ExceptAllTableExpression,
                    GetTableExpression(exceptTableExpression.Right)
                    );
            }
            else if (expression is OrderedTableExpression orderedTableExpression)
            {
                return OrderByToSql(
                    GetTableExpression(orderedTableExpression.Source),
                    orderedTableExpression.Ordered,
                    (LambdaExpression)orderedTableExpression.KeySelector,
                    orderedTableExpression.Descending,
                    new()
                    );
            }
            else if (expression is SkippedTableExpression skippedTableExpression)
            {
                return SkipToSql(
                    GetTableExpression(skippedTableExpression.Source),
                    skippedTableExpression.Count
                    );
            }
            else if (expression is TakedTableExpression takedTableExpression)
            {
                return TakeToSql(
                    GetTableExpression(takedTableExpression.Source),
                    takedTableExpression.Count
                    );
            }
            else if (expression is SelectedTableExpression selectedTableExpression)
            {
                SqlExpressions.ITableExpression tableExpression = selectedTableExpression.Source is null ? new SqlExpressions.SelectExpression() : GetTableExpression(selectedTableExpression.Source);

                return SelectToSql(
                    tableExpression,
                    selectedTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is DistinctTableExpression distinctTableExpression)
            {
                return DistinctToSql(
                    GetTableExpression(distinctTableExpression.Source)
                    );
            }
            else if (expression is AverageTableExpression averageTableExpression)
            {
                return AverageToSql(
                    GetTableExpression(averageTableExpression.Source),
                    (LambdaExpression)averageTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is CountTableExpression countTableExpression)
            {
                return CountToSql(
                    GetTableExpression(countTableExpression.Source),
                    expression.Type
                    );
            }
            else if (expression is MaxTableExpression maxTableExpression)
            {
                return MaxToSql(
                    GetTableExpression(maxTableExpression.Source),
                    (LambdaExpression)maxTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is MinTableExpression minTableExpression)
            {
                return MinToSql(
                    GetTableExpression(minTableExpression.Source),
                    (LambdaExpression)minTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is SumTableExpression sumTableExpression)
            {
                return SumToSql(
                    GetTableExpression(sumTableExpression.Source),
                    (LambdaExpression)sumTableExpression.Selector,
                    expression.Type,
                    new()
                    );
            }
            else if (expression is AnyTableExpression anyTableExpression)
            {
                return AnyToSql(
                    GetTableExpression(anyTableExpression.Source)
                    );
            }
            else if (expression is AllTableExpression allTableExpression)
            {
                return AllToSql(
                    GetTableExpression(allTableExpression.Source),
                    (LambdaExpression)allTableExpression.Predicate,
                    new()
                    );
            }
            else if (expression is RecursiveUnionedTableExpression recursiveUnionedTableExpression)
            {
                return RecursiveUnionToSql(
                    GetTableExpression(recursiveUnionedTableExpression.Left),
                    recursiveUnionedTableExpression.Distinct,
                    recursiveUnionedTableExpression.RightFactory,
                    new()
                    );
            }
            else if (expression is TimeoutTableExpression timeoutTableExpression)
            {
                return TimeoutToSql(GetTableExpression(timeoutTableExpression.Source), timeoutTableExpression.Timeout);
            }

            throw new UnsupportedExpressionException(expression);
        }

        SqlExpressions.JoinedTableExpression JoinToSql(SqlExpressions.ITableExpression left, string join, SqlExpressions.ITableExpression right, LambdaExpression lambdaExpression, SqlExpressions.SqlContext context)
        {
            var joinedTableExpression = left.AddJoin();

            SqlExpressions.JoinedTableExpression.JoinedTable joinedTable = new()
            {
                Join = join,
                TableExpression = right
            };

            joinedTableExpression.JoinedTables.Add(joinedTable);

            if (lambdaExpression is not null)
            {
                joinedTable.Predicate = (SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body,
                    context.ConcatParameters(
                        lambdaExpression.Parameters
                            .Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.LambdaParameter(joinedTableExpression.GetTable(i))))
                        )
                    );
            }

            return joinedTableExpression;
        }

        SqlExpressions.SelectExpression WhereToSql(SqlExpressions.ITableExpression tableExpression, LambdaExpression lambdaExpression, bool hasIndex, SqlExpressions.SqlContext context)
        {
            Type type = tableExpression.Type;

            if (hasIndex)
            {
                tableExpression = tableExpression.AddIndex();

                tableExpression = new SqlExpressions.SelectExpression(tableExpression);
            }

            SqlExpressions.SelectExpression selectExpression;

            if (tableExpression.CanAddNode(SelectNodeType.Where))
            {
                selectExpression = tableExpression.AddWhere();

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
            else if (tableExpression.CanAddNode(SelectNodeType.Having))
            {
                selectExpression = tableExpression.AddHaving();

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
                selectExpression = new SqlExpressions.SelectExpression(tableExpression).AddWhere();

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

        SqlExpressions.SelectExpression GroupToSql(SqlExpressions.ITableExpression tableExpression, LambdaExpression lambdaExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.SelectExpression selectExpression = tableExpression.AddGroupBy();

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

        SqlExpressions.BinaryTableExpression BinaryTableToSql(SqlExpressions.ITableExpression left, SqlExpressions.BinaryTableExpression.BinaryOperate binaryOperate, bool isDistinct, SqlExpressions.ITableExpression right)
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

        SqlExpressions.ITableExpression OrderByToSql(SqlExpressions.ITableExpression tableExpression, bool ordered, LambdaExpression lambdaExpression, bool descending, SqlExpressions.SqlContext context)
        {
            Type type = tableExpression.Type;

            if (ordered)
            {
                tableExpression = tableExpression.AddThenBy();

                tableExpression.OrderBy.Expressions.Add(GetOrderExpression());
            }
            else
            {
                tableExpression = tableExpression.AddOrderBy();

                tableExpression.OrderBy = new SqlExpressions.OrderKeys(GetOrderExpression());
            }

            tableExpression.Type = type;

            return tableExpression;

            SqlExpressions.OrderExpression GetOrderExpression()
            {
                var expressions = tableExpression.GetOrderByParameters();

                context = context.ConcatParameters(lambdaExpression.Parameters.Select((p, i) =>
                {
                    var expression = expressions[i];

                    return KeyValuePair.Create(p, expression is SqlExpressions.IValueExpression valueExpression ? SqlExpressions.LambdaParameterInfo.Value(valueExpression) : SqlExpressions.LambdaParameterInfo.LambdaParameter((SqlExpressions.ITableExpression)expression));
                }));

                return new SqlExpressions.OrderExpression((SqlExpressions.IValueExpression)GetSqlExpression(lambdaExpression.Body, context), descending);
            }
        }

        SqlExpressions.ITableExpression SkipToSql(SqlExpressions.ITableExpression tableExpression, int count)
        {
            Type type = tableExpression.Type;

            tableExpression = tableExpression.AddLimit();

            if (tableExpression.Limit is null)
                tableExpression.Limit = new();

            tableExpression.Limit.Offset += count;
            tableExpression.Limit.Limit -= count;

            tableExpression.Type = type;

            return tableExpression;
        }

        SqlExpressions.ITableExpression TakeToSql(SqlExpressions.ITableExpression tableExpression, int count)
        {
            Type type = tableExpression.Type;

            tableExpression = tableExpression.AddLimit();

            if (tableExpression.Limit is null)
                tableExpression.Limit = new();

            if (count < tableExpression.Limit.Limit)
                tableExpression.Limit.Limit = count;

            if (tableExpression.Limit.Limit < 0)
                tableExpression.Limit.Limit = 0;

            tableExpression.Type = type;

            return tableExpression;
        }

        SqlExpressions.SelectExpression SelectToSql(SqlExpressions.ITableExpression tableExpression, object selector, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = tableExpression.AddSelect();

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

        SqlExpressions.SelectExpression DistinctToSql(SqlExpressions.ITableExpression tableExpression)
        {
            Type type = tableExpression.Type;

            var selectExpression = tableExpression.Distinct();

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression AverageToSql(SqlExpressions.ITableExpression tableExpression, LambdaExpression lambdaExpression, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = tableExpression.AddSelect();

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

        SqlExpressions.SelectExpression CountToSql(SqlExpressions.ITableExpression tableExpression, Type type)
        {
            var selectExpression = tableExpression.AddSelect();

            selectExpression.Columns.Clear();

            selectExpression.Columns.Add(new SqlExpressions.ColumnInfo()
            {
                ValueExpression = SqlExpressions.SqlExpression.CountExpression()
            });

            selectExpression.Type = type;

            return selectExpression;
        }

        SqlExpressions.SelectExpression MaxToSql(SqlExpressions.ITableExpression tableExpression, LambdaExpression lambdaExpression, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = tableExpression.AddSelect();

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

        SqlExpressions.SelectExpression MinToSql(SqlExpressions.ITableExpression tableExpression, LambdaExpression lambdaExpression, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = tableExpression.AddSelect();

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

        SqlExpressions.SelectExpression SumToSql(SqlExpressions.ITableExpression tableExpression, LambdaExpression lambdaExpression, Type type, SqlExpressions.SqlContext context)
        {
            var selectExpression = tableExpression.AddSelect();

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

        SqlExpressions.SelectExpression AnyToSql(SqlExpressions.ITableExpression tableExpression)
        {
            var sourceExpression = tableExpression.AddSelect();

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

        SqlExpressions.SelectExpression AllToSql(SqlExpressions.ITableExpression tableExpression, LambdaExpression lambdaExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.SelectExpression sourceExpression = WhereToSql(
                tableExpression,
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

        SqlExpressions.CommonTableExpression RecursiveUnionToSql(SqlExpressions.ITableExpression left, bool distinct, LambdaExpression rightFactory, SqlExpressions.SqlContext context)
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

            binaryTableExpression.Right = (SqlExpressions.ITableExpression)GetSqlExpression(rightFactory.Body, context.ConcatParameters(
                new KeyValuePair<ParameterExpression, SqlExpressions.LambdaParameterInfo>[]
                {
                    KeyValuePair.Create(rightFactory.Parameters[0], SqlExpressions.LambdaParameterInfo.Table(commonTableExpression))
                }));

            commonTableExpression.Type = type;

            return commonTableExpression;
        }

        SqlExpressions.ITableExpression TimeoutToSql(SqlExpressions.ITableExpression tableExpression, int timeout)
        {
            tableExpression.Timeout = timeout;

            return tableExpression;
        }
    }
}
