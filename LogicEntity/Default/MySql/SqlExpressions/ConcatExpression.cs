using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class ConcatExpression : SqlExpression, IValueExpression
    {
        IValueExpression[] _strExpressions;

        public ConcatExpression(params IValueExpression[] strExpressions)
        {
            _strExpressions = strExpressions;
        }

        public IEnumerable<IValueExpression> StrExpressions => _strExpressions;

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.Call("Concat", _strExpressions.Select(s => s.BuildValue(context).Text).ToArray())
            };
        }
    }
}
