using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class AddIgnoreOperateExpression : AddOperateExpression
    {
        public AddIgnoreOperateExpression(TableExpression source, IEnumerable<object> elements) : base(source, elements, false)
        {

        }
    }
}
