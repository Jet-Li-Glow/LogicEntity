using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 条件描述
    /// </summary>
    public abstract class ConditionDescription
    {
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        internal abstract Command GetCommand();

        /// <summary>
        /// 直接获取字符串（没有参数）
        /// </summary>
        internal string ConditionStr
        {
            get
            {
                string result = string.Empty;

                Command command = GetCommand();

                if (command is null)
                    return result;

                result = command.CommandText;

                if (command.Parameters is null)
                {
                    foreach (KeyValuePair<string, object> p in command.Parameters)
                    {
                        result = result.Replace(p.Key, p.Value.ToSqlParam());
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 命令
        /// </summary>
        internal class Command
        {
            public string CommandText { get; set; }

            public IEnumerable<KeyValuePair<string, object>> Parameters { get; set; }
        }
    }
}
