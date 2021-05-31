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
        /// 描述
        /// </summary>
        /// <returns></returns>
        internal abstract string Description();

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        internal abstract IEnumerable<KeyValuePair<string, object>> GetParameters();
    }
}
