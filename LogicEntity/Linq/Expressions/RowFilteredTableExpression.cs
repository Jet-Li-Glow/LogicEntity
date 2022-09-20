using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class RowFilteredTableExpression : TableExpression
    {
        public RowFilteredTableExpression(TableExpression source, object filter)
        {
            Source = source;

            Filter = filter;
        }

        public override Type Type => Source.Type;

        public TableExpression Source { get; private set; }

        public object Filter { get; private set; }
    }
}
