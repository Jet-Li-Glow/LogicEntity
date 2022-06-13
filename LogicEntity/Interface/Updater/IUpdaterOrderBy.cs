using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IUpdaterOrderBy : IUpdaterLimit
    {
        public IUpdaterThenBy OrderBy(Description description);

        public IUpdaterThenBy OrderByDescending(Description description);
    }
}
