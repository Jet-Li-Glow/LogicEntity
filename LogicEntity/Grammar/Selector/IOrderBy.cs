using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Grammar
{
    public interface IOrderBy : ILimit
    {
        public IThenBy OrderBy(IValueExpression valueExpression);

        public IThenBy OrderByDescending(IValueExpression valueExpression);
    }
}
