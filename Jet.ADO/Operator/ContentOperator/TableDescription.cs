using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 表描述
    /// </summary>
    public class TableDescription
    {
        /// <summary>
        /// 最后的表名
        /// </summary>
        internal virtual string FinalTableName { get; }

        /// <summary>
        /// 所有的列
        /// </summary>
        /// <returns></returns>
        public virtual Description All()
        {
            return new AllColumnDescription(this);
        }

        /// <summary>
        /// 描述
        /// </summary>
        internal virtual string Description()
        {
            return default;
        }

        /// <summary>
        /// 参数
        /// </summary>
        internal virtual IEnumerable<KeyValuePair<string, object>> GetParameters()
        {
            return default;
        }
    }
}
