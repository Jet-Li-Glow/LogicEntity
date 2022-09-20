using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class DistinctTableExpression : TableExpression
    {
        public DistinctTableExpression(TableExpression source)
        {
            Source = source;
        }

        public override Type Type => Source.Type;

        public TableExpression Source { get; private set; }
    }
}
