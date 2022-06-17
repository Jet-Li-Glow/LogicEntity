using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IThenBy : ILimit
    {
        public IThenBy ThenBy(IValueExpression valueExpression);

        public IThenBy ThenByDescending(IValueExpression valueExpression);
    }
}
