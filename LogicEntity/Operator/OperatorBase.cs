using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    internal abstract class OperatorBase : IDbOperator
    {
        /// <summary>
        /// 额外的参数
        /// </summary>
        protected List<KeyValuePair<string, object>> ExtraParameters = new();

        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        protected int CommandTimeout = 0;

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        public Command GetCommand()
        {
            Command command = GetCommandWithUniqueParameterName();

            List<KeyValuePair<string, object>> parameters = new();

            for (int i = 0; i < command.Parameters.Count; i++)
            {
                KeyValuePair<string, object> parameter = command.Parameters[i];

#if DEBUG
                if (parameter.Key.Contains("param"))
                    throw new Exception("参数名称错误");
#endif

                string key = "@param" + i.ToString();

                command.CommandText = command.CommandText.Replace(parameter.Key, key);

                parameters.Add(KeyValuePair.Create(key, parameter.Value));
            }

            command.Parameters = parameters;

            return command;
        }

        /// <summary>
        /// 获取参数名称唯一的命令
        /// </summary>
        /// <returns></returns>
        public abstract Command GetCommandWithUniqueParameterName();

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddParameter(string key, object value)
        {
            ExtraParameters.Add(KeyValuePair.Create(key, value));
        }

        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="seconds"></param>
        public void SetCommandTimeout(int seconds)
        {
            CommandTimeout = seconds;
        }
    }
}
