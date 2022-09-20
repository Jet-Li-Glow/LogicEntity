using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public abstract class OperateExpression : Expression
    {
        public override Type Type => typeof(int);
    }
}
