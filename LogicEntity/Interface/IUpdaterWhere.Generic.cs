using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IUpdaterWhere<T> : IUpdaterOrderBy where T : Table, new()
    {
        public IUpdaterOrderBy Where(Description condition);
    }
}
