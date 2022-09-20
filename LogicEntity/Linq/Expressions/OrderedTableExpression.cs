using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class OrderedTableExpression : TableExpression
    {
        public OrderedTableExpression(TableExpression source, bool ordered, object keySelector, bool descending)
        {
            Source = source;

            Ordered = ordered;

            KeySelector = keySelector;

            Descending = descending;
        }

        public override Type Type => Source.Type;

        public TableExpression Source { get; private set; }

        public bool Ordered { get; private set; }

        public object KeySelector { get; private set; }

        public bool Descending { get; private set; }
    }
}
