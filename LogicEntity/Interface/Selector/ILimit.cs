using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    public interface ILimit : IForUpdate
    {
        public IForUpdate Limit(int limit);

        public IForUpdate Limit(int offset, int limit);

        public IForUpdate Top(int limit);
    }
}
