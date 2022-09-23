using LogicEntity.Collections.Generic;
using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class RecursiveUnionedTableExpression : TableExpression
    {
        public RecursiveUnionedTableExpression(TableExpression left, LambdaExpression rightFactory, bool distinct)
        {
            Left = left;

            RightFactory = rightFactory;

            Distinct = distinct;
        }

        public override Type Type => Left.Type;

        public TableExpression Left { get; private set; }

        public LambdaExpression RightFactory { get; private set; }

        public bool Distinct { get; private set; }
    }
}
