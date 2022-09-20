using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class TakedTableExpression : TableExpression
    {
        public TakedTableExpression(TableExpression source, int count)
        {
            Source = source;

            Count = count;
        }

        public override Type Type => Source.Type;

        public TableExpression Source { get; private set; }

        public int Count { get; private set; }
    }
}
