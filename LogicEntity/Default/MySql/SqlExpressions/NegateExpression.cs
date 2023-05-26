using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class NegateExpression : SqlExpression, IValueExpression
    {
        public NegateExpression(IValueExpression operand)
        {
            Operand = operand;
        }

        public IValueExpression Operand { get; private set; }

        SqlOperator? IValueExpression.Operator => SqlOperator.Negate;

        public SqlCommand BuildValue(BuildContext context)
        {
            string text = Operand.BuildValue(context).Text;

            if (NeedRightBracket(SqlOperator.Negate, Operand.Operator))
                text = SqlNode.Bracket(text);

            return new()
            {
                Text = text
            };
        }
    }
}
