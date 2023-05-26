using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class RowFilteredTableExpression : TableExpression
    {
        public RowFilteredTableExpression(TableExpression source, object filter) : this(source, filter, false)
        {

        }

        public RowFilteredTableExpression(TableExpression source, object filter, bool hasIndex)
        {
            Source = source;

            Filter = filter;

            HasIndex = hasIndex;
        }

        public override Type Type => Source.Type;

        public TableExpression Source { get; private set; }

        public object Filter { get; private set; }

        public bool HasIndex { get; private set; }
    }
}
