using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 所有列描述（延迟加载）
    /// </summary>
    public class AllColumnDescription : Description
    {
        /// <summary>
        /// 表
        /// </summary>
        internal TableDescription Table { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        public AllColumnDescription(TableDescription table)
        {
            Table = table;
        }

        /// <summary>
        /// 主体内容
        /// </summary>
        protected override string Content => Table.FinalTableName + ".*";
    }
}
