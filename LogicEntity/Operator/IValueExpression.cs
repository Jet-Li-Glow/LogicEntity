using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    public interface IValueExpression
    {

        public static IValueExpression operator +(IValueExpression left, IValueExpression right)
        {
            return null;
        }
    }
}
