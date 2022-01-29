using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    public interface IUnion : IOrderBy
    {
        /// <summary>
        /// 联合
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IOrderBy Union(ISelector selector);

        /// <summary>
        /// 联合所有
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IOrderBy UnionAll(ISelector selector);
    }
}
