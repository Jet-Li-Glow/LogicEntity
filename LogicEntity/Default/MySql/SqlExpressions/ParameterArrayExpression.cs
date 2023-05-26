using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class ParameterArrayExpression : SqlExpression, IValueExpression
    {
        public ParameterArrayExpression(params IValueExpression[] values)
        {
            Values = values;
        }

        public IEnumerable<IValueExpression> Values { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = string.Join(", ", Values.Select(s => s.BuildValue(context).Text))
            };
        }
    }
}
