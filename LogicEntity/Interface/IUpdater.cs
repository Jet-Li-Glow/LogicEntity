using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    public interface IUpdater : IDbOperator
    {
        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="seconds">超时时间（秒）</param>
        /// <returns></returns>
        public IUpdater SetCommandTimeout(int seconds);
    }
}
