using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class GreaterThanOrEqualExpression : BinaryExpression
    {
        public GreaterThanOrEqualExpression(IValueExpression left, IValueExpression right) : base(left, right)
        {

        }

        public override SqlOperator Operator { get; } = SqlOperator.GreaterThanOrEqual;

        public override string OperatorText { get; } = SqlNode.GreaterThanOrEqual;
    }
}
