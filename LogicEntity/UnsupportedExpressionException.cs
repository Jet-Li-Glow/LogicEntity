using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public class UnsupportedExpressionException : Exception
    {
        public UnsupportedExpressionException() : base("There is an unsupported expression")
        {

        }

        public UnsupportedExpressionException(System.Linq.Expressions.Expression expression, string message = null) : base(expression?.GetType().Name + $" is an unsupported expression{(string.IsNullOrEmpty(message) ? null : (", " + message))}")
        {

        }

        public UnsupportedExpressionException(Linq.Expressions.Expression expression) : base(expression?.GetType().Name + " is an unsupported expression")
        {

        }
    }
}
