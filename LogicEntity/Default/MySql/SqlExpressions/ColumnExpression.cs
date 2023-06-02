using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
#nullable enable
    internal class ColumnExpression : SqlExpression, IValueExpression
    {
        public ColumnExpression(ITableExpression? table, string columnName)
        {
            Table = table;

            ColumnName = columnName;
        }

        public ITableExpression? Table { get; private set; }

        public string ColumnName { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            string text = SqlNode.SqlName(ColumnName);

            if (Table is not null)
                text = SqlNode.Member(Table.ShortName, text);

            return new()
            {
                Text = text
            };
        }
    }
}
