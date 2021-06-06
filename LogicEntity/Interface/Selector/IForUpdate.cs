using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    public interface IForUpdate : ISelector
    {
        public ISelector ForUpdate();
    }
}
