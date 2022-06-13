using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IUpdaterThenBy : IUpdaterLimit
    {
        public IUpdaterThenBy ThenBy(Description description);

        public IUpdaterThenBy ThenByDescending(Description description);
    }
}
