using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class AnyTableExpression : TableExpression
    {
        public AnyTableExpression(TableExpression source)
        {
            Source = source;
        }

        public override Type Type => typeof(bool);

        public TableExpression Source { get; private set; }
    }
}
