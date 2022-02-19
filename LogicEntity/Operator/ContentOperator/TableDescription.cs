using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 表描述
    /// </summary>
    public abstract class TableDescription
    {
        /// <summary>
        /// 最后的表名
        /// </summary>
        internal abstract string FinalTableName { get; }

        /// <summary>
        /// 列
        /// </summary>
        internal abstract IEnumerable<Description> Columns { get; }

        /// <summary>
        /// 所有的列
        /// </summary>
        /// <returns></returns>
        public Description All()
        {
            return new AllColumnDescription(this);
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        internal abstract Command GetCommand();

        /// <summary>
        /// 命令
        /// </summary>
        internal class Command
        {
            public string CommandText { get; set; }

            public IEnumerable<KeyValuePair<string, object>> Parameters { get; set; }
        }
    }
}
