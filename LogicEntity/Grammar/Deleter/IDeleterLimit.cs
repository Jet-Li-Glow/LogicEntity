using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Grammar
{
    public interface IDeleterLimit : IDeleter
    {
        public IDeleter Limit(ulong limit);
    }
}
