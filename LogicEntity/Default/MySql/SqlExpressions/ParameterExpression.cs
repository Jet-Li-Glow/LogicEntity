using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class ParameterExpression : SqlExpression, IValueExpression, IObjectExpression
    {
        public ParameterExpression(object value)
        {
            Value = value;
        }

        public object Value { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            string name = context.GetParameterName();

            object obj = Value;

            if (obj is IValue val)
            {
                if (val.ValueSetted == false)
                {
                    obj = null;
                }
                else
                {

                    obj = val.Object;
                }
            }

            if (obj is Enum)
                obj = obj.ToString();

            context.SqlParameters.Add(KeyValuePair.Create(name, obj));

            return new()
            {
                Text = name
            };
        }
    }
}
