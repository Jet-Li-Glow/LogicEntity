using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class MemberOfExpression : SqlExpression, IValueExpression
    {
        public MemberOfExpression(IValueExpression jsonArray, IValueExpression value)
        {
            JsonArray = jsonArray;

            Value = value;
        }

        public IValueExpression JsonArray { get; private set; }

        public IValueExpression Value { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = Value.BuildValue(context).Text + " Member Of " + JsonArray.BuildValue(context).Text
            };
        }
    }
}
