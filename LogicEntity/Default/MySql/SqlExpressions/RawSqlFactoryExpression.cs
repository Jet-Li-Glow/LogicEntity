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
    internal class RawSqlFactoryExpression : SqlExpression, IValueExpression
    {
        public RawSqlFactoryExpression(Func<string[], string> factory, params IValueExpression[] valueExpressions)
        {
            Factory = factory;

            ValueExpressions = valueExpressions;
        }

        public Func<string[], string> Factory { get; private set; }

        public IReadOnlyCollection<IValueExpression> ValueExpressions { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = Factory(ValueExpressions.Select(s => s.BuildValue(context).Text).ToArray())
            };
        }
    }
}
