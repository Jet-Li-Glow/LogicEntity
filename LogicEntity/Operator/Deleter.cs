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
    internal class Deleter : OperatorBase, IDeleteWhere
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

        /// <summary>
        /// 获取参数名称唯一的命令
        /// </summary>
        /// <returns></returns>
        public override Command GetCommandWithUniqueParameterName()
        {
            Command command = new Command();

            command.Parameters = new();

            string conditions = string.Empty;

            if (_hasConditons)
            {
                conditions = "\nWhere " + _conditions;

                command.Parameters.AddRange(_conditions?.Parameters ?? new List<KeyValuePair<string, object>>());
            }

            command.CommandText = $"Delete From {_mainTable?.FullName}{conditions}";

            command.Parameters.AddRange(ExtraParameters);

            command.CommandTimeout = CommandTimeout;

            return command;
        }
    }
}
