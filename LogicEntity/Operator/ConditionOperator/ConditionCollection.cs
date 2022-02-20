using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.EnumCollection;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 条件集合
    /// </summary>
    public class ConditionCollection : ConditionDescription
    {
        /// <summary>
        /// 条件集合
        /// </summary>
        List<Condition> _conditions = new();

        /// <summary>
        /// 逻辑操作符
        /// </summary>
        public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.And;

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        public void Add(Condition condition)
        {
            _conditions.Add(condition);
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        internal override Command GetCommand()
        {
            Command command = new();

            List<KeyValuePair<string, object>> parameters = new();

            command.CommandText = string.Join($"\n   {LogicalOperator.Description()} ", _conditions.Select(s =>
            {
                string result = string.Empty;

                if (s is null)
                    return result;

                Command conditionCommand = s.GetCommand();

                if (conditionCommand is null)
                    return result;

                if (conditionCommand.Parameters is not null)
                    parameters.AddRange(conditionCommand.Parameters);

                result = conditionCommand.CommandText;

                if (s.IsMultiple)
                    result = $"({result})";

                return result;
            }));

            command.Parameters = parameters.AsEnumerable();

            return command;
        }
    }
}
