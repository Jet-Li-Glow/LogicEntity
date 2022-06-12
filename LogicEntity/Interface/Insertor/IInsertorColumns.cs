using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface IInsertorColumns<T> : IInsertorSet<T> where T : Table, new()
    {
        public IInsertorValues<T> Columns(params Column[] columns);
    }
}
