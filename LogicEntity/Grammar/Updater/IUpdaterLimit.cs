using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Grammar
{
    public interface IUpdaterLimit : IUpdater
    {
        public IUpdater Limit(ulong limit);
    }
}
