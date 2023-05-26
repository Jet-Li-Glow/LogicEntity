using LogicEntity.Collections;
using LogicEntity.Collections.Generic;
using LogicEntity.Default.MySql.Linq.Expressions;
using LogicEntity.Linq;
using LogicEntity.Linq.Expressions;
using LogicEntity.Method;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    public partial class LinqConvertProvider
    {
        void InitEnumerableMethodFormat()
        {
            MethodInfo[] methods = typeof(LogicEntity.Linq.Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Select)))
            {
                MemberFormat[m] = (object)FormatSelect;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Join)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, nameof(LogicEntity.Linq.Enumerable.Join));
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Where) || m.Name == nameof(LogicEntity.Linq.Enumerable.TakeWhile)))
            {
                MemberFormat[m] = (object)FormatWhere;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.GroupBy)))
            {
                MemberFormat[m] = (object)FormatGroupBy;
            }

            foreach (var m in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAssignableTo(typeof(IGroupingDataTable)))
                .Select(t => t.GetMethod(nameof(IGroupingDataTable<int, int, int>.Element))).Where(m => m is not null))
            {
                MemberFormat[m] = (object)FormatIGroupingDataTableElement;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Concat)))
            {
                MemberFormat[m] = (object)FormatConcat;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Union)))
            {
                MemberFormat[m] = (object)FormatUnion;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.OrderBy)))
            {
                MemberFormat[m] = (object)FormatOrderBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.OrderByDescending)))
            {
                MemberFormat[m] = (object)FormatOrderByDescending;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.ThenBy)))
            {
                MemberFormat[m] = (object)FormatThenBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.ThenByDescending)))
            {
                MemberFormat[m] = (object)FormatThenByDescending;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Skip)))
            {
                MemberFormat[m] = (object)FormatSkip;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Take)))
            {
                MemberFormat[m] = (object)FormatTake;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.All)))
            {
                MemberFormat[m] = (object)FormatAll;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Any)))
            {
                MemberFormat[m] = (object)FormatAny;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Average)))
            {
                MemberFormat[m] = (object)FormatAverage;
            }

            MemberFormat[typeof(System.Linq.Enumerable).GetMethods().Single(m => m.Name == nameof(System.Linq.Enumerable.Contains) && m.GetParameters().Length == 2)] = (object)FormatContains;

            MemberFormat[typeof(List<>).GetMethod(nameof(List<int>.Contains))] = (object)FormatListContains;

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Count) || m.Name == nameof(LogicEntity.Linq.Enumerable.LongCount)))
            {
                MemberFormat[m] = (object)FormatCount;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Distinct)))
            {
                MemberFormat[m] = (object)FormatDistinct;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.DistinctBy)))
            {
                MemberFormat[m] = (object)FormatDistinctBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.ElementAt)))
            {
                MemberFormat[m] = (object)FormatElementAt;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.First)))
            {
                MemberFormat[m] = (object)FormatFirst;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Max)))
            {
                MemberFormat[m] = (object)FormatMax;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.MaxBy)))
            {
                MemberFormat[m] = (object)FormatMaxBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Min)))
            {
                MemberFormat[m] = (object)FormatMin;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.MinBy)))
            {
                MemberFormat[m] = (object)FormatMinBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(LogicEntity.Linq.Enumerable.Sum)))
            {
                MemberFormat[m] = (object)FormatSum;
            }
        }

        void InitMySqlEnumerableMethodFormat()
        {
            MethodInfo[] methods = typeof(MySqlEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.Value)))
            {
                MemberFormat[m] = (object)FormatValue;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.InnerJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, "Inner Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.CrossJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, "Cross Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.LeftJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, "Left Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.RightJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, "Right Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.NaturalJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, "Natural Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.NaturalInnerJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, "Natural Inner Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.NaturalLeftJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, "Natural Left Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.NaturalRightJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context) => FormatJoin(methodCallExpression, context, "Natural Right Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.RecursiveConcat)))
            {
                MemberFormat[m] = (object)FormatRecursiveConcat;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.RecursiveUnion)))
            {
                MemberFormat[m] = (object)FormatRecursiveUnion;
            }
        }

        SqlExpressions.ISqlExpression FormatSelect(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return SelectToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                ((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                methodCallExpression.Type,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatJoin(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context, string join)
        {
            System.Linq.Expressions.Expression predicate = methodCallExpression.Arguments.Count > 2 ? ((UnaryExpression)methodCallExpression.Arguments[2]).Operand : null;

            return JoinToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                join,
                (SqlExpressions.ITableExpression)GetSqlExpression(methodCallExpression.Arguments[1], context),
                predicate as LambdaExpression,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatWhere(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return WhereToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                methodCallExpression.Method.IsDefined(typeof(HasIndexAttribute)),
                context
                );
        }

        SqlExpressions.ISqlExpression FormatGroupBy(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return GroupToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatIGroupingDataTableElement(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.GroupingDataTableExpression groupingDataTableExpression = (SqlExpressions.GroupingDataTableExpression)GetSqlExpression(methodCallExpression.Object, context);

            LambdaExpression expression = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[0]).Operand;

            context = context.ConcatParameters(
                expression.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.Table(groupingDataTableExpression.From.GetTable(i))))
                );

            return GetSqlExpression(expression.Body, context);
        }

        SqlExpressions.ISqlExpression FormatConcat(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return BinaryTableToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                SqlExpressions.BinaryTableExpression.BinaryOperate.Union,
                false,
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[1], context)
                );
        }

        SqlExpressions.ISqlExpression FormatUnion(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return BinaryTableToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                SqlExpressions.BinaryTableExpression.BinaryOperate.Union,
                true,
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[1], context)
                );
        }

        SqlExpressions.ISqlExpression FormatOrderBy(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return OrderByToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                false,
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                false,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatOrderByDescending(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return OrderByToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                false,
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                true,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatThenBy(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return OrderByToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                true,
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                false,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatThenByDescending(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return OrderByToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                true,
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                true,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatSkip(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            var countExpression = GetSqlExpression(methodCallExpression.Arguments[1], context);

            if (countExpression is not SqlExpressions.ConstantExpression constantExpression)
                throw new UnsupportedExpressionException(methodCallExpression.Arguments[1]);

            return SkipToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                (int)constantExpression.Value
                );
        }

        SqlExpressions.ISqlExpression FormatTake(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            var countExpression = GetSqlExpression(methodCallExpression.Arguments[1], context);

            if (countExpression is not SqlExpressions.ConstantExpression constantExpression)
                throw new UnsupportedExpressionException(methodCallExpression.Arguments[1]);

            return TakeToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                (int)constantExpression.Value
                );
        }

        SqlExpressions.ISqlExpression FormatAll(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return AllToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatAny(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.ISelectSql selectSql = (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context);

            if (methodCallExpression.Arguments.Count > 1)
            {
                selectSql = WhereToSql(
                    selectSql,
                    (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                    false,
                    context
                    );
            }

            return AnyToSql(
                selectSql
                );
        }

        SqlExpressions.ISqlExpression FormatAverage(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.ISqlExpression source = GetSqlExpression(methodCallExpression.Arguments[0], context);

            LambdaExpression selector = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            if (source is SqlExpressions.GroupingDataTableExpression groupingDataTableExpression)
            {
                SqlExpressions.SqlContext averageContext = context.ConcatParameters(
                    selector.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.Table(groupingDataTableExpression.From.GetTable(i))))
                    );

                return new SqlExpressions.MethodCallExpression("Avg", (SqlExpressions.IValueExpression)GetSqlExpression(selector.Body, averageContext));
            }

            return AverageToSql(
                (SqlExpressions.ISelectSql)source,
                selector,
                methodCallExpression.Type,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatListContains(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.ISqlExpression valueArray = GetSqlExpression(methodCallExpression.Object, context);

            SqlExpressions.IValueExpression value = (SqlExpressions.IValueExpression)GetSqlExpression(methodCallExpression.Arguments[0], context);

            return GetInExpression(value, valueArray);
        }

        SqlExpressions.ISqlExpression FormatContains(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.ISqlExpression valueArray = GetSqlExpression(methodCallExpression.Arguments[0], context);

            SqlExpressions.IValueExpression value = (SqlExpressions.IValueExpression)GetSqlExpression(methodCallExpression.Arguments[1], context);

            return GetInExpression(value, valueArray);
        }

        SqlExpressions.IValueExpression GetInExpression(SqlExpressions.IValueExpression value, SqlExpressions.ISqlExpression valueArray)
        {
            if (valueArray is SqlExpressions.IObjectExpression objectExpression)
            {
                if (objectExpression.Value is null)
                    throw new ArgumentNullException(nameof(System.Linq.Enumerable.Contains));

                List<SqlExpressions.ParameterExpression> parameters = new();

                foreach (object obj in (IEnumerable)objectExpression.Value)
                {
                    parameters.Add(new(obj));
                }

                if (parameters.Count == 0)
                    return new SqlExpressions.ConstantExpression(false);

                return new SqlExpressions.InExpression(value, new SqlExpressions.ValuesExpression(parameters));
            }

            if (valueArray is SqlExpressions.ISelectSql selectSql)
            {
                return new SqlExpressions.InExpression(value, selectSql);
            }

            return new SqlExpressions.MemberOfExpression((SqlExpressions.IValueExpression)valueArray, value);
        }

        SqlExpressions.ISqlExpression FormatCount(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            var source = GetSqlExpression(methodCallExpression.Arguments[0], context);

            if (source is SqlExpressions.GroupingDataTableExpression)
            {
                return SqlExpressions.SqlExpression.CountExpression();
            }

            return CountToSql(
                (SqlExpressions.ISelectSql)source,
                methodCallExpression.Type
                );
        }

        SqlExpressions.ISqlExpression FormatDistinct(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return DistinctToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context)
                );
        }

        SqlExpressions.ISqlExpression FormatDistinctBy(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            Type[] genericArguments = methodCallExpression.Method.GetGenericArguments();

            ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(typeof(IGroupingDataTable<,>).MakeGenericType(genericArguments[1], genericArguments[0]));

            var selector = System.Linq.Expressions.Expression.Lambda(
                System.Linq.Expressions.Expression.Property(parameterExpression, nameof(IGroupingDataTable<int, int>.Element)),
                parameterExpression);

            return SelectToSql(
                GroupToSql(
                    (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                    (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                    context
                ),
                selector,
                methodCallExpression.Type,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatElementAt(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            var countExpression = GetSqlExpression(methodCallExpression.Arguments[1], context);

            if (countExpression is not SqlExpressions.ConstantExpression constantExpression)
                throw new UnsupportedExpressionException(methodCallExpression.Arguments[1]);

            return TakeToSql(
                SkipToSql(
                    (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                    (int)constantExpression.Value
                    ),
                1
                );
        }

        SqlExpressions.ISqlExpression FormatFirst(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            var source = GetSqlExpression(methodCallExpression.Arguments[0], context);

            if (methodCallExpression.Arguments.Count > 1)
            {
                source = WhereToSql(
                    (SqlExpressions.ISelectSql)source,
                    (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                    false,
                    context
                    );
            }

            return TakeToSql(
                (SqlExpressions.ISelectSql)source,
                1
                );
        }

        SqlExpressions.ISqlExpression FormatMax(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            var source = GetSqlExpression(methodCallExpression.Arguments[0], context);

            LambdaExpression selector = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            if (source is SqlExpressions.GroupingDataTableExpression groupingDataTableExpression)
            {
                SqlExpressions.SqlContext maxContext = context.ConcatParameters(
                    selector.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.Table(groupingDataTableExpression.From.GetTable(i))))
                    );

                return new SqlExpressions.MethodCallExpression("Max", (SqlExpressions.IValueExpression)GetSqlExpression(selector.Body, maxContext));
            }

            return MaxToSql(
                (SqlExpressions.ISelectSql)source,
                selector,
                methodCallExpression.Type,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatMaxBy(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return TakeToSql(
                OrderByToSql(
                    (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                    false,
                    (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                    true,
                    context
                    ),
                1
                );
        }

        SqlExpressions.ISqlExpression FormatMin(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            var source = GetSqlExpression(methodCallExpression.Arguments[0], context);

            LambdaExpression selector = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            if (source is SqlExpressions.GroupingDataTableExpression groupingDataTableExpression)
            {
                SqlExpressions.SqlContext maxContext = context.ConcatParameters(
                    selector.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.Table(groupingDataTableExpression.From.GetTable(i))))
                    );

                return new SqlExpressions.MethodCallExpression("Min", (SqlExpressions.IValueExpression)GetSqlExpression(selector.Body, maxContext));
            }

            return MinToSql(
                (SqlExpressions.ISelectSql)source,
                selector,
                methodCallExpression.Type,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatMinBy(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return TakeToSql(
                OrderByToSql(
                    (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                    false,
                    (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                    false,
                    context
                    ),
                1
                );
        }

        SqlExpressions.ISqlExpression FormatSum(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            var source = GetSqlExpression(methodCallExpression.Arguments[0], context);

            LambdaExpression selector = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            if (source is SqlExpressions.GroupingDataTableExpression groupingDataTableExpression)
            {
                SqlExpressions.SqlContext maxContext = context.ConcatParameters(
                    selector.Parameters.Select((p, i) => KeyValuePair.Create(p, SqlExpressions.LambdaParameterInfo.Table(groupingDataTableExpression.From.GetTable(i))))
                    );

                return new SqlExpressions.MethodCallExpression("Sum", (SqlExpressions.IValueExpression)GetSqlExpression(selector.Body, maxContext));
            }

            return SumToSql(
                (SqlExpressions.ISelectSql)source,
                selector,
                methodCallExpression.Type,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatValue(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return SelectToSql(
                new SqlExpressions.SelectExpression(),
                ((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                methodCallExpression.Method.GetGenericArguments()[0],
                context
                );
        }

        SqlExpressions.ISqlExpression FormatRecursiveConcat(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return RecursiveUnionToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                false,
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                context
                );
        }

        SqlExpressions.ISqlExpression FormatRecursiveUnion(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            return RecursiveUnionToSql(
                (SqlExpressions.ISelectSql)GetSqlExpression(methodCallExpression.Arguments[0], context),
                true,
                (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand,
                context
                );
        }
    }
}
