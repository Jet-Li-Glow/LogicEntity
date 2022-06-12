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
        public List<KeyValuePair<string, object>> Parameters { get; } = new();

        /// <summary>
        /// 命令超时时间（秒）
        /// </summary>
        public int? CommandTimeout { get; set; }

        /// <summary>
        /// 读取器
        /// </summary>
        public Dictionary<int, Func<object, object>> Readers { get; } = new();

        /// <summary>
        /// 字节读取器
        /// </summary>
        public Dictionary<int, Func<Func<long, byte[], int, int, long>, object>> BytesReaders { get; } = new();

        /// <summary>
        /// 字符读取器
        /// </summary>
        public Dictionary<int, Func<Func<long, char[], int, int, long>, object>> CharsReaders { get; } = new();
    }
}
