using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Grammar
{
    public interface IUnion : IOrderBy
    {
        /// <summary>
        /// 联合
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IUnion Union(ISelector selector);

        /// <summary>
        /// 联合所有
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IUnion UnionAll(ISelector selector);
    }
}
