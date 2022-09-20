using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal static class SqlNode
    {
        public const string Null = "Null";
        public const string True = "True";
        public const string False = "False";
        public const string And = "&";
        public const string AndAlso = "And";
        public const string Or = "|";
        public const string OrElse = "Or";
        public const string Asc = "Asc";
        public const string Desc = "Desc";
        public const string Equal = "=";
        public const string NotEqual = "!=";
        public const string Is = "is";
        public const string IsNot = "is Not";
        public const string GreaterThan = ">";
        public const string GreaterThanOrEqual = ">=";
        public const string LessThan = "<";
        public const string LessThanOrEqual = "<=";
        public const string Add = "+";
        public const string Subtract = "-";
        public const string Multiply = "*";
        public const string Divide = "/";
        public const string Modulo = "%";
        public const string ExclusiveOr = "^";
        public const string LeftShift = "<<";
        public const string RightShift = ">>";
        public const string JsonPathRoot = "$";
        public const string Point = ".";
        public const string LeftBracket = "(";
        public const string RightBracket = ")";
        public const string LeftIndexBracket = "[";
        public const string RightIndexBracket = "]";
        public const string ColumnIndexValue = "(Row_Number() Over() - 1)";
        public const string IndexColumnName = "__Index";
        public const string Default = "Default";

        /// <summary>
        /// 唯一名称
        /// </summary>
        /// <returns></returns>
        public static string UniqueName()
        {
            return $" @Guid_{Guid.NewGuid().ToString("N")} ";
        }

        /// <summary>
        /// 参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static KeyValuePair<string, object> Parameter(object value)
        {
            if (value is IValue val)
            {
                if (val.ValueSetted == false || val.ValueType == ValueType.Expression)
                    throw new Exception();

                value = val.Object;
            }

            if (value is Enum)
                value = value.ToString();

            return KeyValuePair.Create(UniqueName(), value);
        }

        /// <summary>
        /// 缩进
        /// </summary>
        /// <param name="src"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string Indent(this string src, int indent)
        {
            string space = new string(System.Linq.Enumerable.Repeat(' ', indent).ToArray());

            string result = src.Replace("\n", "\n" + space);

            if (result.StartsWith("\n") == false)
                result = space + result;

            return result;
        }

        public static string AsTable(this string src, string alias)
        {
            return src + " As " + alias;
        }

        public static string AsColumn(this string src, string alias)
        {
            return src + " As " + SqlString(alias);
        }

        public static string GetTableAlias(int index, int level = 0)
        {
            string alias = "t" + index.ToString();

            if (level > 0)
                return "s" + level.ToString() + "_" + alias;

            return alias;
        }

        public static string SqlString(string src)
        {
            return Wrap(src, '\'', '\\');
        }

        public static string SqlName(string src)
        {
            return Wrap(src, '`');
        }

        public static string Wrap(string src, char border, char? escape = null)
        {
            if (escape.HasValue)
            {
                src = src.Replace(new string(new char[] { escape.Value }), new string(new char[] { escape.Value, escape.Value }))
                    .Replace(new string(new char[] { border }), new string(new char[] { escape.Value, border }));
            }

            return border + src + border;
        }

        public static bool NameEqual(string alias, string expression)
        {
            return alias == expression || alias == expression.Trim('`') || expression.EndsWith("." + alias) || expression.EndsWith("." + SqlName(alias));
        }

        public static string Member(string instance, string member)
        {
            if (instance is null)
                return member;

            if (member is null)
                return instance;

            return instance + Point + member;
        }

        public static string JsonMemberName(string memberName)
        {
            return Wrap(memberName, '"', '\\');
        }

        public static JsonAccess JsonMember(JsonAccess path, string memberExpression)
        {
            path.Add(SqlString(Point), memberExpression);

            return path;
        }

        public static string Index(string instance, string index)
        {
            return instance + LeftIndexBracket + index + RightIndexBracket;
        }

        public static JsonAccess JsonIndex(JsonAccess path, string indexExpression)
        {
            path.Add(SqlString(LeftIndexBracket), indexExpression, SqlString(RightIndexBracket));

            return path;
        }

        public static DataType? DbType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Value<>))
                return type.GetGenericArguments()[0].DbType();

            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(int) || type == typeof(long) || type == typeof(short))
                return DataType.Signed;

            if (type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort))
                return DataType.UnSigned;

            if (type == typeof(double))
                return DataType.Double;

            if (type == typeof(float))
                return DataType.Float;

            if (type == typeof(decimal))
                return DataType.Decimal;

            if (type == typeof(string))
                return DataType.Char;

            if (type == typeof(DateOnly))
                return DataType.Date;

            if (type == typeof(TimeSpan) || type == typeof(TimeOnly))
                return DataType.Time;

            if (type == typeof(DateTime))
                return DataType.Datetime;

            if (type.IsSubclassOf(typeof(IEnumerable<byte>)))
                return DataType.Binary;

            return null;
        }

        public static string Cast(string expression, DataType dataType)
        {
            return $"Cast({expression} As {dataType})";
        }

        public static string Call(string method, params string[] args)
        {
            return $"{method}({string.Join(", ", args)})";
        }

        public static string Bracket(string sql)
        {
            return LeftBracket + sql + RightBracket;
        }

        public static string SubQuery(string sql)
        {
            return LeftBracket + "\n" + sql.Indent(2) + "\n" + RightBracket;
        }

        public static string In(string left, string right)
        {
            return left + " In " + right;
        }

        public static string Assign(string left, string right)
        {
            return left + " " + Equal + " " + right;
        }
    }
}
