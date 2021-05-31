using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IGroupBy : IOrderBy
    {
        public IHaving GroupBy(params Description[] columns);
    }
}
