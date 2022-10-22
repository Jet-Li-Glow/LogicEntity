using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    /// <summary>
    /// 数据库命令
    /// </summary>
    public class Command
    {
        /// <summary>
        /// 构造
        /// </summary>
        public Command()
        {

        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public Command(Type resultType, string commandText, params object[] parameters)
        {
            List<KeyValuePair<string, object>> keyValues = new();

            if (parameters is not null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    keyValues.Add(KeyValuePair.Create("@param" + i.ToString(), parameters[i]));
                }
            }

            CommandText = string.Format(commandText, keyValues.Select(s => s.Key).ToArray());

            Parameters.AddRange(keyValues);

            Results.Add(new() { Type = resultType });
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public Command(Type resultType, string commandText, IEnumerable<KeyValuePair<string, object>> parameters) : this(resultType, commandText, parameters, null)
        {

        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public Command(Type resultType, string commandText, IEnumerable<KeyValuePair<string, object>> parameters, int? commandTimeout)
        {
            CommandText = commandText;

            if (parameters is not null)
                Parameters.AddRange(parameters);

            CommandTimeout = commandTimeout;

            Results.Add(new() { Type = resultType });
        }

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
        /// 结果类型
        /// </summary>
        public List<CommandResult> Results { get; } = new();
    }
}
