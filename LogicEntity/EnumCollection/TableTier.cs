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
        /// Null
        /// </summary>
        [Description("")]
        Null = 0,

        /// <summary>
        /// From
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
        RightJoin = 5,

        /// <summary>
        /// Full Join
        /// </summary>
        [Description("Full Join")]
        FullJoin = 6,

        /// <summary>
        /// Natural Join
        /// </summary>
        [Description("Natural Join")]
        NaturalJoin = 7,

        /// <summary>
        /// Union
        /// </summary>
        [Description("Union")]
        Union = 8,

        /// <summary>
        /// Union All
        /// </summary>
        [Description("Union All")]
        UnionAll = 9
    }
}
