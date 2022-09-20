using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class SetOperateExpression : OperateExpression
    {
        public SetOperateExpression(TableExpression source, object assignments)
        {
            Source = source;

            Assignments = assignments;
        }

        public TableExpression Source { get; private set; }

        public object Assignments { get; private set; }
    }
}
