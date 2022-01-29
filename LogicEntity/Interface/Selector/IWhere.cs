using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IWhere : IGroupBy
    {
        public IGroupBy Where(Condition condition);

        public IGroupBy Conditions(ConditionCollection condition);
    }
}
