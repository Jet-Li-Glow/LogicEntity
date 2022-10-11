using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal interface ITimeoutExpression
    {
        public Expression Source { get; }

        public int Timeout { get; }
    }

    internal class TimeoutTableExpression : TableExpression, ITimeoutExpression
    {
        public TimeoutTableExpression(TableExpression source, int timeout)
        {
            Source = source;

            Timeout = timeout;
        }

        public override Type Type => Source.Type;

        public TableExpression Source { get; set; }

        Expression ITimeoutExpression.Source => Source;

        public int Timeout { get; set; }
    }

    internal class TimeoutOperateExpression : OperateExpression, ITimeoutExpression
    {
        public TimeoutOperateExpression(OperateExpression source, int timeout)
        {
            Source = source;

            Timeout = timeout;
        }

        public override Type Type => Source.Type;

        public OperateExpression Source { get; set; }

        Expression ITimeoutExpression.Source => Source;

        public int Timeout { get; set; }
    }
}
