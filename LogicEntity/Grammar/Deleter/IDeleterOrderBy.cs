using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Grammar
{
    public interface IDeleterOrderBy : IDeleterLimit
    {
        public IDeleterThenBy OrderBy(IValueExpression valueExpression);

        public IDeleterThenBy OrderByDescending(IValueExpression valueExpression);
    }
}
