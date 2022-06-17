using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IUpdaterThenBy : IUpdaterLimit
    {
        public IUpdaterThenBy ThenBy(IValueExpression valueExpression);

        public IUpdaterThenBy ThenByDescending(IValueExpression valueExpression);
    }
}
