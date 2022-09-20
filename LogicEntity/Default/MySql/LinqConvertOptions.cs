using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Default.MySql.ValueConversion;

namespace LogicEntity.Default.MySql
{
    public class LinqConvertOptions
    {
        /// <summary>
        /// 成员格式化
        /// </summary>
        public Dictionary<MemberInfo, object> MemberFormat { get; } = new();

        /// <summary>
        /// 属性值转换
        /// </summary>
        public ValueConverterCollection PropertyConverters { get; } = new();
    }
}
