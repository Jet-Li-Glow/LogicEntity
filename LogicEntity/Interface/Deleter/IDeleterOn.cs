using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IDeleterOn : IDeleterWhere
    {
        public IDeleterJoin On(Condition condition);
    }
}
