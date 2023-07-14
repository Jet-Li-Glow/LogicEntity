using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal enum SelectNodeType
    {
        From,
        Where,
        /// <summary>
        /// 聚合函数
        /// </summary>
        AggregateFunction,
        GroupBy,
        Select,
        Having,
        OrderBy,
        Limit
    }
}
