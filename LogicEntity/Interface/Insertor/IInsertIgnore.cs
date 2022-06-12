using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface IInsertIgnore : IInsertInto
    {
        IInsertInto Ignore();
    }
}
