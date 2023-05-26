using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class EqualExpression : BinaryExpression
    {
        public EqualExpression(IValueExpression left, IValueExpression right) : base(left, right)
        {

        }

        public override SqlOperator Operator { get; } = SqlOperator.Equal;

        public override string OperatorText => (Right is ConstantExpression constantExpression && constantExpression.Value is null) ? SqlNode.Is : SqlNode.Equal;
    }
}
