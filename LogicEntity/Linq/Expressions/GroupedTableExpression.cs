using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class GroupedTableExpression : TableExpression
    {
        public GroupedTableExpression(TableExpression source, object keySelector)
        {
            Source = source;

            KeySelector = keySelector;
        }

        public override Type Type => Source.Type;

        public TableExpression Source { get; private set; }

        public object KeySelector { get; private set; }
    }
}
