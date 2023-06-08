using LogicEntity.Linq.Expressions;
using LogicEntity.Method;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    public partial class LinqConvertProvider
    {
        readonly MethodInfo _StringConcat = typeof(string).GetMethod(nameof(string.Concat), new Type[] { typeof(string), typeof(string) });

        readonly Dictionary<MemberInfo, object> MemberFormat = new()
        {
            //Cast
            { typeof(object).GetMethod(nameof(object.ToString)),  "Cast({0} As Char)" },

            //string
            { typeof(string).GetMethod(nameof(string.Concat), new Type[]{ typeof(object[])}), (string[] args) => $"Concat({string.Join(", ", args.Skip(1))})" },
            { typeof(string).GetMethod(nameof(string.Concat), new Type[]{ typeof(string[])}), (string[] args) => $"Concat({string.Join(", ", args.Skip(1))})" },
            { typeof(string).GetMethod(nameof(string.Concat), new Type[]{ typeof(string), typeof(string), typeof(string) }), "Concat({1}, {2}, {3})" },
            { typeof(string).GetMethod(nameof(string.Concat), new Type[]{ typeof(string), typeof(string), typeof(string), typeof(string) }), "Concat({1}, {2}, {3}, {4})" },
            { typeof(string).GetMethod(nameof(string.Concat), new Type[]{ typeof(object) }), "Concat({1})" },
            { typeof(string).GetMethod(nameof(string.Concat), new Type[]{ typeof(object), typeof(object) }), "Concat({1}, {2})" },
            { typeof(string).GetMethod(nameof(string.Concat), new Type[]{ typeof(object), typeof(object), typeof(object) }), "Concat({1}, {2}, {3})" },
            { typeof(string).GetMethod(nameof(string.Contains), new Type[]{ typeof(char) }), "(({1} Like '') Or (Locate({1}, {0}) > 0))" },
            { typeof(string).GetMethod(nameof(string.Contains), new Type[]{ typeof(string) }), "(({1} Like '') Or (Locate({1}, {0}) > 0))" },
            { typeof(string).GetMethod(nameof(string.IsNullOrEmpty), new Type[]{ typeof(string) }), "({1} is Null Or {1} = '')" },
            { typeof(string).GetMethod(nameof(string.IsNullOrWhiteSpace), new Type[]{ typeof(string) }), "({1} is Null Or Trim({1}) = '')" },
            { typeof(string).GetMethod(nameof(string.Join), new Type[]{ typeof(char), typeof(string[]) }), (string[] args) => $"Concat_Ws({string.Join(", ", args.Skip(1))})" },
            { typeof(string).GetMethod(nameof(string.Join), new Type[]{ typeof(char), typeof(object[]) }), (string[] args) => $"Concat_Ws({string.Join(", ", args.Skip(1))})" },
            { typeof(string).GetMethod(nameof(string.Join), new Type[]{ typeof(string), typeof(string[]) }), (string[] args) => $"Concat_Ws({string.Join(", ", args.Skip(1))})" },
            { typeof(string).GetMethod(nameof(string.Join), new Type[]{ typeof(string), typeof(object[]) }), (string[] args) => $"Concat_Ws({string.Join(", ", args.Skip(1))})" },
            { typeof(string).GetMethod(nameof(string.CompareTo), new Type[]{ typeof(object) }), "StrCmp({0}, {1})" },
            { typeof(string).GetMethod(nameof(string.CompareTo), new Type[]{ typeof(string) }), "StrCmp({0}, {1})" },
            { typeof(string).GetMethod(nameof(string.IndexOf), new Type[]{ typeof(string), typeof(int) }), "(Locate({1}, {0}, {2} + 1) - 1)" },
            { typeof(string).GetMethod(nameof(string.IndexOf), new Type[]{ typeof(string) }), "(Locate({1}, {0}) - 1)" },
            { typeof(string).GetMethod(nameof(string.IndexOf), new Type[]{ typeof(char), typeof(int) }), "(Locate({1}, {0}, {2} + 1) - 1)" },
            { typeof(string).GetMethod(nameof(string.IndexOf), new Type[]{ typeof(char) }), "(Locate({1}, {0}) - 1)" },
            { typeof(string).GetMethod(nameof(string.PadLeft), new Type[]{ typeof(int), typeof(char) }), "LPad({0}, {1}, {2})" },
            { typeof(string).GetMethod(nameof(string.PadLeft), new Type[]{ typeof(int) }), "LPad({0}, {1}, ' ')" },
            { typeof(string).GetMethod(nameof(string.PadRight), new Type[]{ typeof(int), typeof(char) }), "RPad({0}, {1}, {2})" },
            { typeof(string).GetMethod(nameof(string.PadRight), new Type[]{ typeof(int) }), "RPad({0}, {1}, ' ')" },
            { typeof(string).GetMethod(nameof(string.Replace), new Type[]{ typeof(char), typeof(char) }), "Replace({0}, {1}, {2})" },
            { typeof(string).GetMethod(nameof(string.Replace), new Type[]{ typeof(string), typeof(string) }), "Replace({0}, {1}, {2})" },
            { typeof(string).GetMethod(nameof(string.EndsWith), new Type[]{ typeof(char) }), "({0} Like Concat('%', {1}))" },
            { typeof(string).GetMethod(nameof(string.EndsWith), new Type[]{ typeof(string) }), "({0} Like Concat('%', {1}))" },
            { typeof(string).GetMethod(nameof(string.StartsWith), new Type[]{ typeof(char) }), "({0} Like Concat({1}, '%'))" },
            { typeof(string).GetMethod(nameof(string.StartsWith), new Type[]{ typeof(string) }), "({0} Like Concat({1}, '%'))" },
            { typeof(string).GetMethod(nameof(string.Substring), new Type[]{ typeof(int), typeof(int) }), "SubString({0}, {1} + 1, {2})" },
            { typeof(string).GetMethod(nameof(string.Substring), new Type[]{ typeof(int) }), "SubString({0}, {1} + 1)" },
            { typeof(string).GetMethod(nameof(string.ToLower), new Type[]{ }), "Lower({0})" },
            { typeof(string).GetMethod(nameof(string.ToUpper), new Type[]{ }), "Upper({0})" },
            { typeof(string).GetMethod(nameof(string.Trim), new Type[]{ }), "Trim({0})" },
            { typeof(string).GetMethod(nameof(string.Trim), new Type[]{ typeof(char) }), "Trim(Both {1} From {0})" },
            { typeof(string).GetMethod(nameof(string.TrimEnd), new Type[]{ }), "RTrim({0})" },
            { typeof(string).GetMethod(nameof(string.TrimEnd), new Type[]{ typeof(char) }), "RTrim({0})" },
            { typeof(string).GetMethod(nameof(string.TrimStart), new Type[]{ }), "LTrim({0})" },
            { typeof(string).GetMethod(nameof(string.TrimStart), new Type[]{ typeof(char) }), "LTrim({0})" },
            { typeof(string).GetProperties().Single(p => p.GetIndexParameters().Length > 0).GetMethod, "SubString({0}, {1} + 1, 1)" },
            { typeof(string).GetProperty(nameof(string.Length)), "Char_Length({0})" },

            //DateTime
            { typeof(DateTime).GetMethod(nameof(DateTime.Add), new Type[]{ typeof(TimeSpan) }), "AddTime({0}, {1})" },
            { typeof(DateTime).GetMethod(nameof(DateTime.AddYears), new Type[]{ typeof(int) }), "AddDate({0}, Interval {1} Year)" },
            { typeof(DateTime).GetMethod(nameof(DateTime.AddMonths), new Type[]{ typeof(int) }), "AddDate({0}, Interval {1} Month)" },
            { typeof(DateTime).GetMethod(nameof(DateTime.AddDays), new Type[]{ typeof(double) }), "AddDate({0}, Interval {1} * 24 * 60 * 60 * 1E6 MicroSecond)" },
            { typeof(DateTime).GetMethod(nameof(DateTime.AddHours), new Type[]{ typeof(double) }), "AddDate({0}, Interval {1} * 60 * 60 * 1E6 MicroSecond)" },
            { typeof(DateTime).GetMethod(nameof(DateTime.AddMinutes), new Type[]{ typeof(double) }), "AddDate({0}, Interval {1} * 60 * 1E6 MicroSecond)" },
            { typeof(DateTime).GetMethod(nameof(DateTime.AddSeconds), new Type[]{ typeof(double) }), "AddDate({0}, Interval {1} * 1E6 MicroSecond)" },
            { typeof(DateTime).GetMethod(nameof(DateTime.AddMilliseconds), new Type[]{ typeof(double) }), "AddDate({0}, Interval {1} * 1E3 MicroSecond)" },
            { typeof(DateTime).GetMethod(nameof(DateTime.AddTicks), new Type[]{ typeof(long) }), "AddDate({0}, Interval {1} / 10 MicroSecond)" },
            { typeof(DateTime).GetMethod(nameof(DateTime.Subtract), new Type[]{ typeof(TimeSpan) }), "SubTime({0}, {1})" },
            { typeof(DateTime).GetMethods().Single(m => m.Name == "op_Subtraction" && m.GetParameters()[1].ParameterType == typeof(TimeSpan)), "SubTime({1}, {2})" },
            { typeof(DateTime).GetMethod(nameof(DateTime.Subtract), new Type[]{ typeof(DateTime) }), "Sec_To_Time(TimeStampDiff(MicroSecond, {1}, {0}) / 1E6)" },
            { typeof(DateTime).GetMethods().Single(m => m.Name == "op_Subtraction" && m.GetParameters()[1].ParameterType == typeof(DateTime)), "Sec_To_Time(TimeStampDiff(MicroSecond, {2}, {1}) / 1E6)" },

            //Math
            { typeof(Math).GetMethod(nameof(Math.Abs), new Type[]{ typeof(decimal) }), "Abs({1})" },
            { typeof(Math).GetMethod(nameof(Math.Abs), new Type[]{ typeof(double) }), "Abs({1})" },
            { typeof(Math).GetMethod(nameof(Math.Abs), new Type[]{ typeof(short) }), "Abs({1})" },
            { typeof(Math).GetMethod(nameof(Math.Abs), new Type[]{ typeof(int) }), "Abs({1})" },
            { typeof(Math).GetMethod(nameof(Math.Abs), new Type[]{ typeof(long) }), "Abs({1})" },
            { typeof(Math).GetMethod(nameof(Math.Abs), new Type[]{ typeof(nint) }), "Abs({1})" },
            { typeof(Math).GetMethod(nameof(Math.Abs), new Type[]{ typeof(sbyte) }), "Abs({1})" },
            { typeof(Math).GetMethod(nameof(Math.Abs), new Type[]{ typeof(float) }), "Abs({1})" },
            { typeof(Math).GetMethod(nameof(Math.Acos), new Type[]{ typeof(double) }), "Acos({1})" },
            { typeof(Math).GetMethod(nameof(Math.Asin), new Type[]{ typeof(double) }), "Asin({1})" },
            { typeof(Math).GetMethod(nameof(Math.Atan), new Type[]{ typeof(double) }), "Atan({1})" },
            { typeof(Math).GetMethod(nameof(Math.Atan2), new Type[]{ typeof(double), typeof(double) }), "Atan2({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Ceiling), new Type[]{ typeof(decimal) }), "Ceiling({1})" },
            { typeof(Math).GetMethod(nameof(Math.Ceiling), new Type[]{ typeof(double) }), "Ceiling({1})" },
            { typeof(Math).GetMethod(nameof(Math.Cos), new Type[]{ typeof(double) }), "Cos({1})" },
            { typeof(Math).GetMethod(nameof(Math.Exp), new Type[]{ typeof(double) }), "Exp({1})" },
            { typeof(Math).GetMethod(nameof(Math.Floor), new Type[]{ typeof(decimal) }), "Floor({1})" },
            { typeof(Math).GetMethod(nameof(Math.Floor), new Type[]{ typeof(double) }), "Floor({1})" },
            { typeof(Math).GetMethod(nameof(Math.Log), new Type[]{ typeof(double) }), "Log({1})" },
            { typeof(Math).GetMethod(nameof(Math.Log), new Type[]{ typeof(double), typeof(double) }), "Log({2}, {1})" },
            { typeof(Math).GetMethod(nameof(Math.Log10), new Type[]{ typeof(double) }), "Log10({1})" },
            { typeof(Math).GetMethod(nameof(Math.Log2), new Type[]{ typeof(double) }), "Log2({1})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(nuint), typeof(nuint) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(ulong), typeof(ulong) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(uint), typeof(uint) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(ushort), typeof(ushort) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(float), typeof(float) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(nint), typeof(nint) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(sbyte), typeof(sbyte) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(int), typeof(int) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(short), typeof(short) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(double), typeof(double) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(decimal), typeof(decimal) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(byte), typeof(byte) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Max), new Type[]{ typeof(long), typeof(long) }), "Greatest({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(sbyte), typeof(sbyte) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(nuint), typeof(nuint) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(uint), typeof(uint) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(ushort), typeof(ushort) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(float), typeof(float) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(nint), typeof(nint) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(ulong), typeof(ulong) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(int), typeof(int) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(short), typeof(short) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(double), typeof(double) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(decimal), typeof(decimal) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(long), typeof(long) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Min), new Type[]{ typeof(byte), typeof(byte) }), "Least({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Pow), new Type[]{ typeof(double), typeof(double) }), "Pow({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Round), new Type[]{ typeof(double), typeof(int) }), "Round({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Round), new Type[]{ typeof(double) }), "Round({1})" },
            { typeof(Math).GetMethod(nameof(Math.Round), new Type[]{ typeof(decimal), typeof(int) }), "Round({1}, {2})" },
            { typeof(Math).GetMethod(nameof(Math.Round), new Type[]{ typeof(decimal) }), "Round({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sign), new Type[]{ typeof(float) }), "Sign({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sign), new Type[]{ typeof(sbyte) }), "Sign({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sign), new Type[]{ typeof(long) }), "Sign({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sign), new Type[]{ typeof(nint) }), "Sign({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sign), new Type[]{ typeof(short) }), "Sign({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sign), new Type[]{ typeof(double) }), "Sign({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sign), new Type[]{ typeof(decimal) }), "Sign({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sign), new Type[]{ typeof(int) }), "Sign({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sin), new Type[]{ typeof(double) }), "Sin({1})" },
            { typeof(Math).GetMethod(nameof(Math.Sqrt), new Type[]{ typeof(double) }), "Sqrt({1})" },
            { typeof(Math).GetMethod(nameof(Math.Tan), new Type[]{ typeof(double) }), "Tan({1})" },
            { typeof(Math).GetMethod(nameof(Math.Truncate), new Type[]{ typeof(decimal) }), "Truncate({1}, 0)" },
            { typeof(Math).GetMethod(nameof(Math.Truncate), new Type[]{ typeof(double) }), "Truncate({1}, 0)" },

            //Tuple
            { typeof(Tuple).GetMethods().Single(m => m.Name == nameof(Tuple.Create) && m.GetGenericArguments().Length == 1), "({1})"},
            { typeof(Tuple).GetMethods().Single(m => m.Name == nameof(Tuple.Create) && m.GetGenericArguments().Length == 2), "({1}, {2})"},
            { typeof(Tuple).GetMethods().Single(m => m.Name == nameof(Tuple.Create) && m.GetGenericArguments().Length == 3), "({1}, {2}, {3})"},
            { typeof(Tuple).GetMethods().Single(m => m.Name == nameof(Tuple.Create) && m.GetGenericArguments().Length == 4), "({1}, {2}, {3}, {4})"},
            { typeof(Tuple).GetMethods().Single(m => m.Name == nameof(Tuple.Create) && m.GetGenericArguments().Length == 5), "({1}, {2}, {3}, {4}, {5})"},
            { typeof(Tuple).GetMethods().Single(m => m.Name == nameof(Tuple.Create) && m.GetGenericArguments().Length == 6), "({1}, {2}, {3}, {4}, {5}, {6})"},
            { typeof(Tuple).GetMethods().Single(m => m.Name == nameof(Tuple.Create) && m.GetGenericArguments().Length == 7), "({1}, {2}, {3}, {4}, {5}, {6}, {7})"},
        };

        void InitDbFunctionMethodFormat()
        {
            MemberFormat[_StringConcat] = (object)FormatStringConcat;
        }

        bool TryGetMemberFormat(MemberInfo member, out object format)
        {
            format = member.GetCustomAttribute<MethodFormatAttribute>()?.Format;

            if (format is not null)
                return true;

            return MemberFormat.TryGetValue(GetGenericDefinition(member), out format);
        }

        MemberInfo GetGenericDefinition(MemberInfo member)
        {
            if (member.DeclaringType.IsGenericType)
            {
                member = member.DeclaringType.GetGenericTypeDefinition().GetMemberWithSameMetadataDefinitionAs(member);
            }
            else if (member is MethodInfo method && method.IsGenericMethod)
            {
                member = method.GetGenericMethodDefinition();
            }

            return member;
        }

        SqlExpressions.ISqlExpression FormatStringConcat(MethodCallExpression methodCallExpression, SqlExpressions.SqlContext context)
        {
            SqlExpressions.IValueExpression left = (SqlExpressions.IValueExpression)GetSqlExpression(methodCallExpression.Arguments[0], context);

            SqlExpressions.IValueExpression right = (SqlExpressions.IValueExpression)GetSqlExpression(methodCallExpression.Arguments[1], context);

            List<SqlExpressions.IValueExpression> strExpressions = new();

            if (left is SqlExpressions.ConcatExpression concatExpression)
                strExpressions.AddRange(concatExpression.StrExpressions);
            else
                strExpressions.Add(left);

            strExpressions.Add(right);

            return new SqlExpressions.ConcatExpression(strExpressions.ToArray());
        }
    }
}
