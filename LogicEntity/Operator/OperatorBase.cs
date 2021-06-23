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
        public virtual Command GetCommand()
        {
            return new Command();
        }

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
