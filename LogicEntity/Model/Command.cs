using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Model
{
    /// <summary>
    /// 数据库命令
    /// </summary>
    public class Command
    {
        /// <summary>
        /// 命令文本
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// 命令参数
        /// </summary>
        public List<KeyValuePair<string, object>> Parameters { get; set; }

        /// <summary>
        /// 命令超时时间（秒）
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// 读取器
        /// </summary>
        public Dictionary<int, Func<object, object>> Readers { get; set; }

        /// <summary>
        /// 写入器
        /// </summary>
        public Dictionary<int, Func<object, object>> Writers { get; set; }
    }
}
