using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class ModuloExpression : BinaryExpression
    {
        public ModuloExpression(IValueExpression left, IValueExpression right) : base(left, right)
        {

        }

        public override SqlOperator Operator { get; } = SqlOperator.Modulo;

        public override string OperatorText { get; } = SqlNode.Modulo;
    }
}
