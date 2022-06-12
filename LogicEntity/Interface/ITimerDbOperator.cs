using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    /// <summary>
    /// 可设置超时时间的数据库操作器
    /// </summary>
    public interface ITimerDbOperator : IDbOperator
    {
        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="seconds">超时时间（秒）</param>
        /// <returns></returns>
        public IDbOperator SetCommandTimeout(int seconds);
    }
}
