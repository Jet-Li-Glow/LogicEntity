using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.ValueConversion
{
    /// <summary>
    /// 值转换器
    /// </summary>
    internal class ValueConverter
    {
        /// <summary>
        /// 读取器
        /// </summary>
        public Delegate Reader { get; set; }

        /// <summary>
        /// 写入器
        /// </summary>
        public Delegate Writer { get; set; }
    }
}
