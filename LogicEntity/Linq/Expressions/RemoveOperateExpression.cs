using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class RemoveOperateExpression : OperateExpression
    {
        public RemoveOperateExpression(TableExpression source, object selectors)
        {
            Source = source;

            Selectors = selectors;
        }

        public TableExpression Source { get; private set; }

        public object Selectors { get; private set; }
    }
}
