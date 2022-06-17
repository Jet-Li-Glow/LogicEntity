using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IGroupBy : IUnion
    {
        public IHaving GroupBy(params IValueExpression[] columns);
    }
}
