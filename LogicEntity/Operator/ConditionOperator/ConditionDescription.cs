using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 条件描述
    /// </summary>
    public abstract class ConditionDescription
    {
        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return default;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        internal abstract IEnumerable<KeyValuePair<string, object>> GetParameters();
    }
}
