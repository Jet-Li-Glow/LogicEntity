using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class JoinedTableExpression : TableExpression
    {
        public JoinedTableExpression(TableExpression left, TableExpression right, object predicate) : this(left, nameof(TableEnumerable.Join), right, predicate)
        {

        }

        public JoinedTableExpression(TableExpression left, string join, TableExpression right, object predicate)
        {
            Left = left;

            Join = join;

            Right = right;

            Predicate = predicate;
        }

        public TableExpression Left { get; private set; }

        public string Join { get; private set; }

        public TableExpression Right { get; private set; }

        public object Predicate { get; private set; }
    }
}
