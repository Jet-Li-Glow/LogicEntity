using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class SelectedTableExpression : TableExpression
    {
        Type _type;

        public SelectedTableExpression(TableExpression source, object selector, Type type)
        {
            Source = source;

            Selector = selector;

            _type = type;
        }

        public override Type Type => _type;

        public TableExpression Source { get; private set; }

        public object Selector { get; private set; }
    }
}
