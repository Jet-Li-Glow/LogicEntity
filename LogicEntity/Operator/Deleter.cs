using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 删除操作器
    /// </summary>
    public class Deleter : IDeleteWhere
    {
        /// <summary>
        /// 主表
        /// </summary>
        private Table _mainTable;

        /// <summary>
        /// 条件
        /// </summary>
        private ConditionDescription _conditions;

        /// <summary>
        /// 是否有条件
        /// </summary>
        private bool _hasConditons;

        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        private int _commandTimeout = 0;

        /// <summary>
        /// 删除操作器
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Deleter(Table table)
        {
            _mainTable = table;
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IDeleter Where(Condition condition)
        {
            _conditions = condition;

            _hasConditons = true;

            return this;
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IDeleter With(ConditionCollection conditions)
        {
            _conditions = conditions;

            _hasConditons = true;

            return this;
        }

        public IDeleter SetCommandTimeout(int seconds)
        {
            _commandTimeout = seconds;

            return this;
        }

        /// <summary>
        /// 获取操作命令
        /// </summary>
        /// <returns></returns>
        public Command GetCommand()
        {
            Command command = new Command();

            List<KeyValuePair<string, object>> parameters = new();

            string conditions = string.Empty;

            if (_hasConditons)
            {
                conditions = "\nWhere " + _conditions;

                parameters.AddRange(_conditions?.GetParameters() ?? new List<KeyValuePair<string, object>>());
            }

            command.CommandText = $"Delete From {_mainTable?.FullName}{conditions}";

            command.Parameters = new();

            for (int i = 0; i < parameters.Count; i++)
            {
                string key = "@param" + i.ToString();

                command.CommandText = command.CommandText.Replace(parameters[i].Key, key);

                command.Parameters.Add(KeyValuePair.Create(key, parameters[i].Value));
            }

            command.CommandTimeout = _commandTimeout;

            return command;
        }
    }
}
