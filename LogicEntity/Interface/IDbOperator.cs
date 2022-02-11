using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    /// <summary>
    /// 数据库操作器
    /// </summary>
    public interface IDbOperator
    {
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        public Command GetCommand();

        /// <summary>
        /// 获取参数名称唯一的命令
        /// </summary>
        /// <returns></returns>
        public Command GetCommandWithUniqueParameterName();

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddParameter(string key, object value);

        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="seconds">超时时间（秒）</param>
        /// <returns></returns>
        public void SetCommandTimeout(int seconds);
    }
}
