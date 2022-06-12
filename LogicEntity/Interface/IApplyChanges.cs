using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface IApplyChanges<T> where T : Table, new()
    {
        public IChangerOn ApplyChanges(T table);
    }
}
