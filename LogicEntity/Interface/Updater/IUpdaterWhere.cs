using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IUpdaterWhere : IUpdater
    {
        public IUpdater Where(Condition condition);

        public IUpdater Conditions(ConditionCollection condition);
    }
}
