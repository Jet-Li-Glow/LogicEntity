using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.EnumCollection
{
    /// <summary>
    /// 表级别
    /// </summary>
    internal enum TableTier
    {
        /// <summary>
        /// 空
        /// </summary>
        [Description("")]
        Null = 0,
        /// <summary>
        /// 主表
        /// </summary>
        [Description("From")]
        From = 1,
        /// <summary>
        /// Join
        /// </summary>
        [Description("Join")]
        Join = 2,
        /// <summary>
        /// Inner Join
        /// </summary>
        [Description("Inner Join")]
        InnerJoin = 3,
        /// <summary>
        /// Left Join
        /// </summary>
        [Description("Left Join")]
        LeftJoin = 4,
        /// <summary>
        /// Right Join
        /// </summary>
        [Description("Right Join")]
        RightJoin = 5
    }
}
