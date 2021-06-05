using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface ISelector : IDbOperator
    {
        public ISelector Union(ISelector selector);

        public ISelector UnionAll(ISelector selector);

        public NestedTable As(string alias);
    }
}
