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
