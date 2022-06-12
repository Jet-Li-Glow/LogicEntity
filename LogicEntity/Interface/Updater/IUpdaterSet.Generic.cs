using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface IUpdaterSet<T> where T : Table, new()
    {
        public IUpdaterWhere<T> Set(Action<T> setValue);
    }
}
