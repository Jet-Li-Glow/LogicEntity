using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Grammar
{
    public interface IUpdaterOrderBy : IUpdaterLimit
    {
        public IUpdaterThenBy OrderBy(IValueExpression valueExpression);

        public IUpdaterThenBy OrderByDescending(IValueExpression valueExpression);
    }
}
