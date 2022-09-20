using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal enum SqlValueValueType
    {
        GroupKey,

        GroupElement,

        /// <summary>
        /// 逻辑值
        /// </summary>
        Logic,

        /// <summary>
        /// 计算值（+、-、*、/...）
        /// </summary>
        Calculation
    }
}
