using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class AddOrUpdateOperateExpression : AddOperateExpression
    {
        public AddOrUpdateOperateExpression(TableExpression source, IEnumerable<object> elements) : base(source, elements)
        {

        }
    }
}
