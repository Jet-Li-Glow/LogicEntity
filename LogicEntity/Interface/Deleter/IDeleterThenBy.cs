using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IDeleterThenBy : IDeleterLimit
    {
        public IDeleterThenBy ThenBy(Description description);

        public IDeleterThenBy ThenByDescending(Description description);
    }
}
