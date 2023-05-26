using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal abstract class BinaryExpression : SqlExpression, IValueExpression
    {
        public BinaryExpression(IValueExpression left, IValueExpression right)
        {
            Left = left;

            Right = right;
        }

        public IValueExpression Left { get; private set; }

        public abstract SqlOperator Operator { get; }

        SqlOperator? IValueExpression.Operator => Operator;

        public abstract string OperatorText { get; }

        public IValueExpression Right { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            string left = Left.BuildValue(context).Text;

            string right = Right.BuildValue(context).Text;

            if (NeedLeftBracket(Left.Operator, Operator))
                left = SqlNode.Bracket(left);

            if (NeedRightBracket(Operator, Right.Operator))
                right = SqlNode.Bracket(right);

            return new()
            {
                Text = left + " " + OperatorText + " " + right
            };
        }
    }
}
