using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.EnumCollection
{
    /// <summary>
    /// 逻辑运算符
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// 空
        /// </summary>
        [Description("")]
        Null = 0,
        /// <summary>
        /// 与
        /// </summary>
        [Description("And")]
        And = 1,
        /// <summary>
        /// 或
        /// </summary>
        [Description("Or")]
        Or = 2,
        /// <summary>
        /// 非
        /// </summary>
        [Description("Not")]
        Not = 3
    }
}
