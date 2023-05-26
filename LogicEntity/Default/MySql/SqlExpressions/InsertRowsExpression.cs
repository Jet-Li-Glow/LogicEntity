using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class InsertRowsExpression : IInsertRowsExpression
    {
        public InsertRowsExpression(IEnumerable<ValuesExpression> rows)
        {
            Rows = rows;
        }

        public IEnumerable<ValuesExpression> Rows { get; private set; }

        public InsertRowsSqlCommand BuildRows(BuildContext context)
        {
            return new()
            {
                Text = "Values\n" + string.Join(",\n", Rows.Select(s => s.BuildValues(context).Text)).Indent(2)
            };
        }
    }
}
