using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class ValuesExpression : SqlExpression, IValuesExpression
    {
        public ValuesExpression(IEnumerable<IValueExpression> values)
        {
            Values = values;
        }

        public IEnumerable<IValueExpression> Values { get; private set; }

        public SqlCommand BuildValues(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.Bracket(string.Join(", ", Values.Select(s => s.BuildValue(context).Text)))
            };
        }
    }
}
