using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class ExistsExpression : SqlExpression, IValueExpression
    {
        public ExistsExpression(ISelectSql selectSql)
        {
            SelectSql = selectSql;
        }

        public ISelectSql SelectSql { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = "Exists " + SelectSql.BuildValue(context).Text
            };
        }
    }
}
