using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class UnionedTableExpression : TableExpression
    {
        public UnionedTableExpression(TableExpression left, TableExpression right, bool distinct)
        {
            Left = left;

            Right = right;

            Distinct = distinct;
        }

        public override Type Type => Left.Type;

        public TableExpression Left { get; private set; }

        public TableExpression Right { get; private set; }

        public bool Distinct { get; private set; }
    }
}
