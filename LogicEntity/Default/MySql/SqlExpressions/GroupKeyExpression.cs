using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class GroupKeyExpression : SqlExpression, IValueExpression
    {
        public GroupKeyExpression(Dictionary<MemberInfo, IValueExpression> members)
        {
            Members = members;
        }

        public Dictionary<MemberInfo, IValueExpression> Members { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return Members.Single().Value.BuildValue(context);
        }
    }
}
