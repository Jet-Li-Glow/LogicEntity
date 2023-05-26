using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class NotExpression : SqlExpression, IValueExpression
    {
        public NotExpression(IValueExpression operand)
        {
            Operand = operand;
        }

        public IValueExpression Operand { get; private set; }

        SqlOperator? IValueExpression.Operator => SqlOperator.Not;

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = "Not (" + Operand.BuildValue(context).Text + ")"
            };
        }
    }
}
