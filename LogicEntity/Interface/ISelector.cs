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

        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="seconds">超时时间（秒）</param>
        /// <returns></returns>
        public ISelector SetCommandTimeout(int seconds);
    }
}
