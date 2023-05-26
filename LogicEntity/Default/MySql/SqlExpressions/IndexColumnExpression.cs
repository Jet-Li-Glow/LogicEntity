using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class IndexColumnExpression : SqlExpression, IValueExpression
    {
        public IndexColumnExpression(ITableExpression tableExpression)
        {
            TableExpression = tableExpression;
        }

        public ITableExpression TableExpression { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.Member(TableExpression.ShortName, SqlNode.IndexColumnName)
            };
        }
    }
}
