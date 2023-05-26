using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class InExpression : SqlExpression, IValueExpression
    {
        public InExpression(IValueExpression left, IValuesExpression right)
        {
            Left = left;

            Right = right;
        }

        public IValueExpression Left { get; private set; }

        public IValuesExpression Right { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = Left.BuildValue(context).Text + " In " + Right.BuildValues(context).Text
            };
        }
    }
}
