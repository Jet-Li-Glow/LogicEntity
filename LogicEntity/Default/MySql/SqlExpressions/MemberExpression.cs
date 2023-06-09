using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class MemberExpression : SqlExpression, IValueExpression
    {
        public MemberExpression(ISqlExpression sqlExpression, MemberInfo member)
        {
            SqlExpression = sqlExpression;

            Member = member;
        }

        public ISqlExpression SqlExpression { get; private set; }

        public MemberInfo Member { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            if (SqlExpression is null)
            {
                return new()
                {
                    Text = SqlNode.Member(Member.DeclaringType.Name, SqlNode.SqlName(Member.Name))
                };
            }
            else if (SqlExpression is ITableExpression tableExpression)
            {
                string columnName = tableExpression is OriginalTableExpression ? SqlNode.ColumnName(Member) : Member.Name;

                return new()
                {
                    Text = SqlNode.Member(tableExpression.ShortName, SqlNode.SqlName(columnName))
                };
            }
            else
            {
                return new()
                {
                    Text = SqlNode.Member(((IValueExpression)SqlExpression).BuildValue(context).Text, SqlNode.SqlName(Member.Name))
                };
            }
        }
    }
}
