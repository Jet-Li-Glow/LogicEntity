using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IThenBy : ILimit
    {
        public IThenBy ThenBy(Description column);

        public IThenBy ThenByDescending(Description column);
    }
}
