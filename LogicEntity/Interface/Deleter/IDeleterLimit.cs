using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    public interface IDeleterLimit : IDeleter
    {
        public IDeleter Limit(int limit);

        public IDeleter Limit(int offset, int limit);
    }
}
