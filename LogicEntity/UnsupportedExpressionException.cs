using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public class UnsupportedExpressionException : Exception
    {
        public UnsupportedExpressionException(System.Linq.Expressions.Expression expression) : base(expression?.GetType().Name + " is an unsupported expression")
        {

        }

        public UnsupportedExpressionException(Linq.Expressions.Expression expression) : base(expression?.GetType().Name + " is an unsupported expression")
        {

        }
    }
}
