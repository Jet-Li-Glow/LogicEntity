using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    public interface ILimit : ISelector
    {
        public ISelector Limit(int limit);

        public ISelector Limit(int offset, int limit);
    }
}
