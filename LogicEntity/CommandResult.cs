using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public class CommandResult
    {
        public Type Type { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Dictionary<string, ConstructorInfo> Constructors { get; } = new();

        /// <summary>
        /// 读取器
        /// </summary>
        public Dictionary<int, Delegate> Readers { get; } = new();
    }
}
