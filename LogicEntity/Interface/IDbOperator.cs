﻿using System;
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

                string key = "@param" + i;

                command.CommandText = command.CommandText.Replace(parameter.Key, key);

                parameters.Add(KeyValuePair.Create(key, parameter.Value));
            }

            command.Parameters.Clear();

            command.Parameters.AddRange(parameters);

#if DEBUG
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                string key = ("@param" + i);

                if (command.Parameters[i].Key != key)
                    throw new Exception("参数名称错误");

                if (command.CommandText.Contains(key) == false)
                    throw new Exception("参数名称错误");
            }

            if (command.CommandText.Contains(" @param" + command.Parameters.Count + " "))
                throw new Exception("参数名称错误");
#endif

            return command;
        }

        /// <summary>
        /// 获取参数名称唯一的命令
        /// </summary>
        /// <returns></returns>
        internal Command GetCommandWithUniqueParameterName();
    }
}
