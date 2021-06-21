using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Extension;

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

        /// <summary>
        /// 直接获取字符串（没有参数）
        /// </summary>
        internal string ConditionStr
        {
            get
            {
                string result = ToString();

                foreach (KeyValuePair<string, object> p in GetParameters())
                {
                    result = result.Replace(p.Key, p.Value.ToSqlParam());
                }

                return result;
            }
        }
    }
}
