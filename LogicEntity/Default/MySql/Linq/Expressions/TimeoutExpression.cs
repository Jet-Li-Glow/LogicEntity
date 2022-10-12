using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class TimeoutTableExpression : TableExpression
    {
        public TimeoutTableExpression(TableExpression source, int timeout)
        {
            Source = source;

            Timeout = timeout;
        }

        public override Type Type => Source.Type;

        public TableExpression Source { get; private set; }

        public int Timeout { get; private set; }
    }

    internal class TimeoutOperateExpression : OperateExpression
    {
        public TimeoutOperateExpression(OperateExpression source, int timeout)
        {
            Source = source;

            Timeout = timeout;
        }

        public override Type Type => Source.Type;

        public OperateExpression Source { get; private set; }

        public int Timeout { get; private set; }
    }
}
