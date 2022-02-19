using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IDeleterOrderBy : IDeleterLimit
    {
        public IDeleterThenBy OrderBy(Description description);

        public IDeleterThenBy OrderByDescending(Description description);
    }
}
