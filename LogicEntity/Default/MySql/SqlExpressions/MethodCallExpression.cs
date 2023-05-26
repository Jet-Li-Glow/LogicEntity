using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class MethodCallExpression : SqlExpression, IValueExpression
    {
        IValueExpression[] _arguments;

        public MethodCallExpression(string methodName, params IValueExpression[] arguments)
        {
            MethodName = methodName;

            _arguments = arguments ?? Array.Empty<IValueExpression>();
        }

        public string MethodName { get; private set; }

        public ReadOnlyCollection<IValueExpression> Arguments => _arguments.AsReadOnly();

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.Call(MethodName, _arguments.Select(s => s.BuildValue(context).Text).ToArray())
            };
        }
    }
}
