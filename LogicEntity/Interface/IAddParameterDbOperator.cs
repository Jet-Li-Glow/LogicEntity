using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    /// <summary>
    /// 可添加参数的数据库操作器
    /// </summary>
    public interface IAddParameterDbOperator : ITimerDbOperator
    {
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public IAddParameterDbOperator AddParameter(string key, object value);
    }
}
