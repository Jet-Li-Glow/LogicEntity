using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class AddNextTableExpression : TableExpression
    {
        public AddNextTableExpression(TableExpression source, object element)
        {
            Source = source;

            Element = element;
        }

        public override Type Type => typeof(ulong);

        public TableExpression Source { get; private set; }

        public object Element { get; private set; }
    }
}
