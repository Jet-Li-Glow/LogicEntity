using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 更新操作器
    /// </summary>
    internal class Changer : OperatorBase, IChanger
    {
        /// <summary>
        /// 表
        /// </summary>
        Table _change;

        /// <summary>
        /// 条件
        /// </summary>
        Condition _condition;

        /// <summary>
        /// 是否有条件
        /// </summary>
        bool _hasConditons;

        /// <summary>
        /// 更新操作器
        /// </summary>
        /// <param name="change"></param>
        public Changer(Table change)
        {
            _change = change;
        }

        /// <summary>
        /// 添加更新条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IUpdater On(Condition condition)
        {
            _condition = condition;

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

            if (_change is null)
                return command;

            List<string> columns = new();

            command.Parameters = new();

            foreach (PropertyInfo property in _change.GetType().GetProperties())
            {
                Column column = property.GetValue(_change) as Column;

                if (column is null)
                    continue;

                if (column.IsValueSet == false)
                    continue;

                if (column.Writer is not null)
                    column.Value = column.Writer(column.Value);

                if (column.Value is Description)
                {
                    columns.Add(column.ToString() + " = " + (column.Value as Description).ToString());

                    continue;
                }

                string key = ToolService.UniqueName();

                columns.Add(column.ToString() + " = " + key);

                command.Parameters.Add(KeyValuePair.Create(key, column.Value));
            }

            string set = "\nSet " + string.Join(",\n    ", columns);

            string condition = string.Empty;

            if (_hasConditons)
            {
                condition = "\nWhere ";

                if (_condition is not null)
                {
                    condition += _condition;

                    command.Parameters.AddRange(_condition.Parameters ?? new List<KeyValuePair<string, object>>());
                }
            }

            command.CommandText = $"Update {_change.FullName}{set}{condition}";

            command.Parameters.AddRange(ExtraParameters);

            command.CommandTimeout = CommandTimeout;

            return command;
        }
    }
}
