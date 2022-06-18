using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Grammar
{
    public interface IDeleterThenBy : IDeleterLimit
    {
        public IDeleterThenBy ThenBy(IValueExpression valueExpression);

        public IDeleterThenBy ThenByDescending(IValueExpression valueExpression);
    }
}
