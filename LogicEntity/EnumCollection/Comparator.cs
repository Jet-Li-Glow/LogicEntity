using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.EnumCollection
{
    /// <summary>
    /// 比较运算符
    /// </summary>
    public enum Comparator
    {
        [Description("")]
        Null = 0,
        /// <summary>
        /// 相等
        /// </summary>
        [Description("=")]
        Equal = 1,
        /// <summary>
        /// 不相等
        /// </summary>
        [Description("<>")]
        NotEqual = 2,
        /// <summary>
        /// 相似
        /// </summary>
        [Description("Like")]
        Like = 3,
        /// <summary>
        /// 大于
        /// </summary>
        [Description(">")]
        GreaterThan = 4,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description(">=")]
        GreaterThanOrEqual = 5,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("<")]
        LessThan = 6,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("<=")]
        LessThanOrEqual = 7,
        /// <summary>
        /// 枚举值内
        /// </summary>
        [Description("In")]
        In = 8
    }
}
