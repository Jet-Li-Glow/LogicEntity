using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class OrderExpression : SqlExpression
    {
        public OrderExpression(IValueExpression valueExpression, bool descending)
        {
            ValueExpression = valueExpression;

            Descending = descending;
        }

        public IValueExpression ValueExpression { get; private set; }

        public bool Descending { get; private set; }
    }
}
