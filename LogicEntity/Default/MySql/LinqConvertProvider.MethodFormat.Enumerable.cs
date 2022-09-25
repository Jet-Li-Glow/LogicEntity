using LogicEntity.Collections;
using LogicEntity.Collections.Generic;
using LogicEntity.Linq.Expressions;
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
            MethodInfo[] methods = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Select)))
            {
                MemberFormat[m] = (object)FormatSelect;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Join)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, nameof(Enumerable.Join));
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Where) || m.Name == nameof(Enumerable.TakeWhile)))
            {
                MemberFormat[m] = (object)FormatWhere;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.GroupBy)))
            {
                MemberFormat[m] = (object)FormatGroupBy;
            }

            foreach (var m in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAssignableTo(typeof(IGroupingDataTable)))
                .Select(t => t.GetMethod(nameof(IGroupingDataTable<int, int, int>.Element))).Where(m => m is not null))
            {
                MemberFormat[m] = (object)FormatIGroupingDataTableElement;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Concat)))
            {
                MemberFormat[m] = (object)FormatConcat;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Union)))
            {
                MemberFormat[m] = (object)FormatUnion;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.OrderBy)))
            {
                MemberFormat[m] = (object)FormatOrderBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.OrderByDescending)))
            {
                MemberFormat[m] = (object)FormatOrderByDescending;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.ThenBy)))
            {
                MemberFormat[m] = (object)FormatThenBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.ThenByDescending)))
            {
                MemberFormat[m] = (object)FormatThenByDescending;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Skip)))
            {
                MemberFormat[m] = (object)FormatSkip;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Take)))
            {
                MemberFormat[m] = (object)FormatTake;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.All)))
            {
                MemberFormat[m] = (object)FormatAll;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Any)))
            {
                MemberFormat[m] = (object)FormatAny;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Average)))
            {
                MemberFormat[m] = (object)FormatAverage;
            }

            MemberFormat[typeof(System.Linq.Enumerable).GetMethods().Single(m => m.Name == nameof(System.Linq.Enumerable.Contains) && m.GetParameters().Length == 2)] = (object)FormatContains;

            MemberFormat[typeof(List<>).GetMethod(nameof(List<int>.Contains))] = (object)FormatListContains;

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Count) || m.Name == nameof(Enumerable.LongCount)))
            {
                MemberFormat[m] = (object)FormatCount;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Distinct)))
            {
                MemberFormat[m] = (object)FormatDistinct;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.DistinctBy)))
            {
                MemberFormat[m] = (object)FormatDistinctBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.ElementAt)))
            {
                MemberFormat[m] = (object)FormatElementAt;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.First)))
            {
                MemberFormat[m] = (object)FormatFirst;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Max)))
            {
                MemberFormat[m] = (object)FormatMax;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.MaxBy)))
            {
                MemberFormat[m] = (object)FormatMaxBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Min)))
            {
                MemberFormat[m] = (object)FormatMin;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.MinBy)))
            {
                MemberFormat[m] = (object)FormatMinBy;
            }

            foreach (var m in methods.Where(m => m.Name == nameof(Enumerable.Sum)))
            {
                MemberFormat[m] = (object)FormatSum;
            }
        }

        void InitMySqlEnumerableMethodFormat()
        {
            MethodInfo[] methods = typeof(MySqlEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.InnerJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, "Inner Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.CrossJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, "Cross Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.LeftJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, "Left Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.RightJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, "Right Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.NaturalJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, "Natural Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.NaturalInnerJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, "Natural Inner Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.NaturalLeftJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, "Natural Left Join");
            }

            foreach (var m in methods.Where(m => m.Name == nameof(MySqlEnumerable.NaturalRightJoin)))
            {
                MemberFormat[m] = (MethodCallExpression methodCallExpression, SqlContext context) => FormatJoin(methodCallExpression, context, "Natural Right Join");
            }
        }

        SqlValue FormatSelect(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new SelectedTableExpression((TableExpression)source, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand, methodCallExpression.Type));
        }

        SqlValue FormatJoin(MethodCallExpression methodCallExpression, SqlContext context, string join)
        {
            object left = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (left is IDataTable leftDataTable)
                left = leftDataTable.Expression;

            object right = GetValueExpression(methodCallExpression.Arguments[1], context with { GetTableExpression = true }).ConstantValue;

            if (right is IDataTable rightDataTable)
                right = rightDataTable.Expression;

            System.Linq.Expressions.Expression predicate = methodCallExpression.Arguments.Count > 2 ? ((UnaryExpression)methodCallExpression.Arguments[2]).Operand : null;

            return SqlValue.TableExpression(new JoinedTableExpression((TableExpression)left, join, (TableExpression)right, predicate));
        }

        SqlValue FormatWhere(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new RowFilteredTableExpression((TableExpression)source, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand));
        }

        SqlValue FormatGroupBy(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new GroupedTableExpression((TableExpression)source, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand));
        }

        SqlValue FormatIGroupingDataTableElement(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var objectCmd = GetValueExpression(methodCallExpression.Object, context);

            if (objectCmd.LambdaParameterInfo?.ParameterType != LambdaParameterType.GroupingDataTable)
                throw new UnsupportedExpressionException(methodCallExpression);

            LambdaExpression expression = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[0]).Operand;

            return GetValueExpression(expression.Body, new(context.Level)
            {
                Parameters = expression.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(objectCmd.LambdaParameterInfo.FromTables[i])))
                        .ToDictionary(s => s.Key, s => s.Value)
            });
        }

        SqlValue FormatConcat(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var sqlContext = context with { GetTableExpression = true };

            object first = GetValueExpression(methodCallExpression.Arguments[0], sqlContext).ConstantValue;

            if (first is IDataTable firstDataTable)
                first = firstDataTable.Expression;

            object second = GetValueExpression(methodCallExpression.Arguments[1], sqlContext).ConstantValue;

            if (second is IDataTable secondDataTable)
                second = secondDataTable.Expression;

            return SqlValue.TableExpression(new UnionedTableExpression((TableExpression)first, (TableExpression)second, false));
        }

        SqlValue FormatUnion(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object first = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (first is IDataTable firstDataTable)
                first = firstDataTable.Expression;

            object second = GetValueExpression(methodCallExpression.Arguments[1], context with { GetTableExpression = true }).ConstantValue;

            if (second is IDataTable secondDataTable)
                second = secondDataTable.Expression;

            return SqlValue.TableExpression(new UnionedTableExpression((TableExpression)first, (TableExpression)second, true));
        }

        SqlValue FormatOrderBy(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new OrderedTableExpression((TableExpression)source, false, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand, false));
        }

        SqlValue FormatOrderByDescending(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new OrderedTableExpression((TableExpression)source, false, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand, true));
        }

        SqlValue FormatThenBy(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new OrderedTableExpression((TableExpression)source, true, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand, false));
        }

        SqlValue FormatThenByDescending(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new OrderedTableExpression((TableExpression)source, true, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand, true));
        }

        SqlValue FormatSkip(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            if (methodCallExpression.Arguments[1] is not ConstantExpression constantExpression)
                throw new UnsupportedExpressionException(methodCallExpression.Arguments[1]);

            return SqlValue.TableExpression(new SkippedTableExpression((TableExpression)source, (int)constantExpression.Value));
        }

        SqlValue FormatTake(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            if (methodCallExpression.Arguments[1] is not ConstantExpression constantExpression)
                throw new UnsupportedExpressionException(methodCallExpression.Arguments[1]);

            return SqlValue.TableExpression(new TakedTableExpression((TableExpression)source, (int)constantExpression.Value));
        }

        SqlValue FormatAll(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new AllTableExpression((TableExpression)source, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand));
        }

        SqlValue FormatAny(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            object predicate = null;

            if (methodCallExpression.Arguments.Count > 1)
                predicate = ((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            return SqlValue.TableExpression(new AnyTableExpression((TableExpression)source, predicate));
        }

        SqlValue FormatAverage(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var sourceCmd = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true });

            LambdaExpression selector = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            if (sourceCmd.LambdaParameterInfo?.ParameterType == LambdaParameterType.GroupingDataTable)
            {
                var cmd = GetValueExpression(selector.Body, new(context.Level)
                {
                    Parameters = selector.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(sourceCmd.LambdaParameterInfo.FromTables[i])))
                        .ToDictionary(s => s.Key, s => s.Value)
                });

                return new()
                {
                    CommantText = SqlNode.Call("Avg", cmd.CommantText?.ToString()),
                    Parameters = cmd.Parameters
                };
            }

            object sourceExpression = sourceCmd.ConstantValue;

            if (sourceExpression is IDataTable dataTable)
                sourceExpression = dataTable.Expression;

            return SqlValue.TableExpression(new AverageTableExpression((TableExpression)sourceExpression, selector, methodCallExpression.Type));
        }

        SqlValue FormatListContains(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var source = GetValueExpression(methodCallExpression.Object, context);

            var value = GetValueExpression(methodCallExpression.Arguments[0], context);

            return GetInSqlValue(source, value);
        }

        SqlValue FormatContains(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var source = GetValueExpression(methodCallExpression.Arguments[0], context);

            var value = GetValueExpression(methodCallExpression.Arguments[1], context);

            return GetInSqlValue(source, value);
        }

        SqlValue GetInSqlValue(SqlValue source, SqlValue value)
        {
            string text = null;

            List<KeyValuePair<string, object>> ps = new();

            if (source.IsConstant)
            {
                List<string> keys = new();

                foreach (object obj in (IEnumerable)source.ConstantValue)
                {
                    var p = SqlNode.Parameter(obj);

                    keys.Add(p.Key);

                    ps.Add(p);
                }

                text = SqlNode.Bracket(string.Join(", ", keys));
            }
            else
            {
                text = source.CommantText?.ToString();

                if (source.Parameters is not null)
                    ps.AddRange(source.Parameters);
            }

            text = SqlNode.In(value.CommantText?.ToString(), text);

            if (value.Parameters is not null)
                ps.AddRange(value.Parameters);

            return new()
            {
                CommantText = text,
                Parameters = ps
            };
        }

        SqlValue FormatCount(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var sourceCmd = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true });

            if (sourceCmd.LambdaParameterInfo?.ParameterType == LambdaParameterType.GroupingDataTable)
            {
                return new()
                {
                    CommantText = SqlNode.Call("Count", "*")
                };
            }

            object sourceExpression = sourceCmd.ConstantValue;

            if (sourceExpression is IDataTable dataTable)
                sourceExpression = dataTable.Expression;

            return SqlValue.TableExpression(new CountTableExpression((TableExpression)sourceExpression, methodCallExpression.Type));
        }

        SqlValue FormatDistinct(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new DistinctTableExpression((TableExpression)source));
        }

        SqlValue FormatDistinctBy(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            Type[] genericArguments = methodCallExpression.Method.GetGenericArguments();

            ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(typeof(IGroupingDataTable<,>).MakeGenericType(genericArguments[1], genericArguments[0]));

            var selector = System.Linq.Expressions.Expression.Lambda(
                System.Linq.Expressions.Expression.Property(parameterExpression, nameof(IGroupingDataTable<int, int>.Element)),
                parameterExpression);

            return SqlValue.TableExpression(new SelectedTableExpression(
                new GroupedTableExpression((TableExpression)source, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand),
                selector,
                methodCallExpression.Type));
        }

        SqlValue FormatElementAt(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            if (methodCallExpression.Arguments[1] is not ConstantExpression constantExpression)
                throw new UnsupportedExpressionException(methodCallExpression.Arguments[1]);

            return SqlValue.TableExpression(new TakedTableExpression(new SkippedTableExpression((TableExpression)source, (int)constantExpression.Value), 1));
        }

        SqlValue FormatFirst(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            if (methodCallExpression.Arguments.Count > 1)
                source = new RowFilteredTableExpression((TableExpression)source, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand);

            return SqlValue.TableExpression(new TakedTableExpression((TableExpression)source, 1));
        }

        SqlValue FormatMax(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var sourceCmd = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true });

            LambdaExpression selector = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            if (sourceCmd.LambdaParameterInfo?.ParameterType == LambdaParameterType.GroupingDataTable)
            {
                var cmd = GetValueExpression(selector.Body, new(context.Level)
                {
                    Parameters = selector.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(sourceCmd.LambdaParameterInfo.FromTables[i])))
                        .ToDictionary(s => s.Key, s => s.Value)
                });

                return new()
                {
                    CommantText = SqlNode.Call("Max", cmd.CommantText?.ToString()),
                    Parameters = cmd.Parameters
                };
            }

            object sourceExpression = sourceCmd.ConstantValue;

            if (sourceExpression is IDataTable dataTable)
                sourceExpression = dataTable.Expression;

            Type[] genericTypes = methodCallExpression.Method.GetGenericArguments();

            return SqlValue.TableExpression(new MaxTableExpression((TableExpression)sourceExpression, selector, genericTypes[genericTypes.Length - 1]));
        }

        SqlValue FormatMaxBy(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new TakedTableExpression(new OrderedTableExpression((TableExpression)source, false, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand, true), 1));
        }

        SqlValue FormatMin(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var sourceCmd = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true });

            LambdaExpression selector = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            if (sourceCmd.LambdaParameterInfo?.ParameterType == LambdaParameterType.GroupingDataTable)
            {
                var cmd = GetValueExpression(selector.Body, new(context.Level)
                {
                    Parameters = selector.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(sourceCmd.LambdaParameterInfo.FromTables[i])))
                        .ToDictionary(s => s.Key, s => s.Value)
                });

                return new()
                {
                    CommantText = SqlNode.Call("Min", cmd.CommantText?.ToString()),
                    Parameters = cmd.Parameters
                };
            }

            object sourceExpression = sourceCmd.ConstantValue;

            if (sourceExpression is IDataTable dataTable)
                sourceExpression = dataTable.Expression;

            Type[] genericTypes = methodCallExpression.Method.GetGenericArguments();

            return SqlValue.TableExpression(new MinTableExpression((TableExpression)sourceExpression, selector, genericTypes[genericTypes.Length - 1]));
        }

        SqlValue FormatMinBy(MethodCallExpression methodCallExpression, SqlContext context)
        {
            object source = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true }).ConstantValue;

            if (source is IDataTable dataTable)
                source = dataTable.Expression;

            return SqlValue.TableExpression(new TakedTableExpression(new OrderedTableExpression((TableExpression)source, false, ((UnaryExpression)methodCallExpression.Arguments[1]).Operand, false), 1));
        }

        SqlValue FormatSum(MethodCallExpression methodCallExpression, SqlContext context)
        {
            var sourceCmd = GetValueExpression(methodCallExpression.Arguments[0], context with { GetTableExpression = true });

            LambdaExpression selector = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;

            if (sourceCmd.LambdaParameterInfo?.ParameterType == LambdaParameterType.GroupingDataTable)
            {
                var cmd = GetValueExpression(selector.Body, new(context.Level)
                {
                    Parameters = selector.Parameters.Select((p, i) => KeyValuePair.Create(p, LambdaParameterInfo.Entity(sourceCmd.LambdaParameterInfo.FromTables[i])))
                        .ToDictionary(s => s.Key, s => s.Value)
                });

                return new()
                {
                    CommantText = SqlNode.Call("Sum", cmd.CommantText?.ToString()),
                    Parameters = cmd.Parameters
                };
            }

            object sourceExpression = sourceCmd.ConstantValue;

            if (sourceExpression is IDataTable dataTable)
                sourceExpression = dataTable.Expression;

            Type[] genericTypes = methodCallExpression.Method.GetGenericArguments();

            return SqlValue.TableExpression(new SumTableExpression((TableExpression)sourceExpression, selector, genericTypes[genericTypes.Length - 1]));
        }
    }
}
