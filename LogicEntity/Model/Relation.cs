using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.Operator;
using LogicEntity.EnumCollection;

namespace LogicEntity.Model
{
    /// <summary>
    /// 关联关系
    /// </summary>
    internal class Relation
    {
        /// <summary>
        /// 表级别
        /// </summary>
        internal TableTier TableTier { get; private set; }

        /// <summary>
        /// 关联表
        /// </summary>
        internal TableDescription Table { get; private set; }

        /// <summary>
        /// 关联条件
        /// </summary>
        private ConditionDescription _condition;

        /// <summary>
        /// 是否设置条件
        /// </summary>
        private bool _hasCondition;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        /// <param name="tableTier"></param>
        public Relation(TableDescription table, TableTier tableTier)
        {
            Table = table;

            TableTier = tableTier;
        }

        /// <summary>
        /// 设置关联条件
        /// </summary>
        /// <param name="condition"></param>
        public void SetCondition(ConditionDescription condition)
        {
            _hasCondition = true;

            _condition = condition;
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        internal Command GetCommand()
        {
            Command command = new();

            List<KeyValuePair<string, object>> parameters = new();

            TableDescription.Command tableCommand = Table?.GetCommand();

            ConditionDescription.Command conditionCommand = _condition?.GetCommand();

            command.CommandText = TableTier.Description() + " ";

            if (tableCommand is not null)
            {
                command.CommandText += tableCommand.CommandText;

                if (tableCommand.Parameters is not null)
                    parameters.AddRange(tableCommand.Parameters);
            }

            if (_hasCondition)
            {
                command.CommandText += "\n   On ";

                if (conditionCommand is not null)
                {
                    command.CommandText += conditionCommand.CommandText;

                    if (conditionCommand.Parameters is not null)
                    {
                        parameters.AddRange(conditionCommand.Parameters);
                    }
                }
            }

            command.Parameters = parameters.AsEnumerable();

            return command;
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
