using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Tool
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    internal static class ToolService
    {
        public static string Description(this Enum enumValue)
        {
            return enumValue?.GetType().GetField(enumValue.ToString())?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValid(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 唯一名称
        /// </summary>
        /// <returns></returns>
        public static string UniqueName()
        {
            return $" @Guid_{Guid.NewGuid().ToString("N")} ";
        }
    }
}
