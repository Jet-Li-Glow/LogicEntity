using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Grammar
{
    public interface IWhere : IGroupBy
    {
        public IGroupBy Where(IValueExpression condition);
    }
}
