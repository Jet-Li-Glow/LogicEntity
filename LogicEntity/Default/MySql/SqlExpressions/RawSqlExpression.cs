using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    /// <summary>
    /// 原始sql
    /// </summary>
    internal class RawSqlExpression : SqlExpression, IValueExpression
    {
        public RawSqlExpression(string rawSql, params IValueExpression[] valueExpressions)
        {
            RawSql = rawSql;

            ValueExpressions = valueExpressions;
        }

        public string RawSql { get; private set; }

        public IReadOnlyCollection<IValueExpression> ValueExpressions { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = ValueExpressions is not null && ValueExpressions.Count > 0 ? string.Format(RawSql, ValueExpressions.Select(s => s.BuildValue(context).Text).ToArray()) : RawSql
            };
        }
    }
}
