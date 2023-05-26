using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class OrderKeys
    {
        public OrderKeys(OrderExpression expression)
        {
            Expressions.Add(expression);
        }

        public List<OrderExpression> Expressions { get; } = new();

        public string Build(BuildContext context)
        {
            return string.Join(", ", Expressions.Select(s => s.ValueExpression.BuildValue(context).Text + " " + (s.Descending ? SqlNode.Desc : SqlNode.Asc)));
        }
    }
}
