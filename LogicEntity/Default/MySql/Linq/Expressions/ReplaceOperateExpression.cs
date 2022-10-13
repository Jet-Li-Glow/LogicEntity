using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class ReplaceOperateExpression : AddOperateExpression
    {
        public ReplaceOperateExpression(TableExpression source, IEnumerable<object> elements) : base(source, elements, false)
        {

        }
    }
}
