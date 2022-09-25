using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal class SqlValue
    {
        public static SqlValue Constant(object value)
        {
            SqlValue result = new();

            result.IsConstant = true;

            result.ConstantValue = value;

            if (value is null)
            {
                result.CommantText = SqlNode.Null;
            }
            else if (value.Equals(true))
            {
                result.CommantText = SqlNode.True;
            }
            else if (value.Equals(false))
            {
                result.CommantText = SqlNode.False;
            }
            else
            {
                KeyValuePair<string, object> p = SqlNode.Parameter(value);

                result.CommantText = p.Key;

                result.Parameters = new List<KeyValuePair<string, object>>() { p };
            }

            return result;
        }

        public static SqlValue TableExpression(TableExpression tableExpression)
        {
            return new()
            {
                IsConstant = true,
                ConstantValue = tableExpression
            };
        }

        public object CommantText { get; set; }

        public IEnumerable<KeyValuePair<string, object>> Parameters { get; set; }

        public bool IsConstant { get; private set; } = false;

        public object ConstantValue { get; private set; }

        public LambdaParameterInfo LambdaParameterInfo { get; set; }

        public SqlValueValueType? ValueType { get; set; }

        public Dictionary<MemberInfo, string> GroupKeys { get; set; }

        public List<EntityInfo> FromTables { get; set; }
    }
}
