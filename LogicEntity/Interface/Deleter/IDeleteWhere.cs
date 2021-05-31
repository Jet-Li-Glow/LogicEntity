using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IDeleteWhere : IDeleter
    {
        public IDeleter Where(Condition condition);

        public IDeleter With(ConditionCollection conditions);
    }
}
