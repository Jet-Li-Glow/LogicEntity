using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IFrom : IGroupBy
    {
        public IWhere From(params TableDescription[] tables);
    }
}
