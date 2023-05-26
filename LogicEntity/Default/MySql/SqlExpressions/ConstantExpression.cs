using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class ConstantExpression : SqlExpression, IValueExpression, IObjectExpression
    {
        public ConstantExpression(object value)
        {
            Value = value;
        }

        public object Value { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            string text;

            if (Value is null)
            {
                text = SqlNode.Null;
            }
            else if (Value.Equals(true))
            {
                text = SqlNode.True;
            }
            else if (Value.Equals(false))
            {
                text = SqlNode.False;
            }
            else if (Value is string || Value is Enum || Value is char)
            {
                text = SqlNode.SqlString(Value.ToString());
            }
            else
            {
                text = Value.ToString();
            }

            return new()
            {
                Text = text
            };
        }
    }
}
