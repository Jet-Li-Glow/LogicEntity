using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class GroupingDataTableExpression : SqlExpression, IValueExpression
    {
        public GroupingDataTableExpression(ITableExpression from, Dictionary<MemberInfo, IValueExpression> members)
        {
            From = from;

            Members = members;
        }

        public ITableExpression From { get; private set; }

        public Dictionary<MemberInfo, IValueExpression> Members { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            throw new NotImplementedException();
        }
    }
}
