using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IDeleterWhere : IDeleterOrderBy
    {
        public IDeleterOrderBy Where(ConditionDescription condition);
    }
}
