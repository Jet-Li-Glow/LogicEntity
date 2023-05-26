using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class EmptyExpression : SqlExpression, IValueExpression
    {
        public SqlCommand BuildValue(BuildContext context)
        {
            return new();
        }
    }
}
