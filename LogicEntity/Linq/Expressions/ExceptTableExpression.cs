using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class ExceptTableExpression : TableExpression
    {
        public ExceptTableExpression(TableExpression left, TableExpression right)
        {
            Left = left;

            Right = right;
        }

        public override Type Type => Left.Type;

        public TableExpression Left { get; init; }

        public TableExpression Right { get; init; }
    }
}
