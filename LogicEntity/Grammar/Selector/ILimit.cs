using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Grammar
{
    public interface ILimit : IForUpdate
    {
        public IForUpdate Limit(ulong limit);

        public IForUpdate Limit(ulong offset, ulong limit);
    }
}
