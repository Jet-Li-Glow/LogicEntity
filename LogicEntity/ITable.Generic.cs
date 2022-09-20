using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public interface ITable<T> : ITable, IDataTable<T>
    {

    }
}
