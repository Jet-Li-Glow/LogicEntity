using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;

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

        public override Command GetCommand()
        {
            Command command = new Command();

            if (_change is null)
                return command;

            var properties = _change.GetType().GetProperties().Where(p => p.PropertyType == typeof(Column));

            List<string> columns = new();

            command.Parameters = new();

            int index = 0;

            foreach (PropertyInfo property in properties)
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

                string key = "@param" + index.ToString();

                columns.Add(column.ToString() + " = " + key);

                command.Parameters.Add(KeyValuePair.Create(key, column.Value));

                index++;
            }

            string set = "\nSet " + string.Join(",\n    ", columns);

            string condition = string.Empty;

            if (_hasConditons)
            {
                condition = "\nWhere ";

                if (_condition is not null)
                {
                    condition += _condition;

                    foreach (KeyValuePair<string, object> parameter in _condition.GetParameters())
                    {
                        string key = "@param" + index.ToString();

                        condition = condition.Replace(parameter.Key, key);

                        command.Parameters.Add(KeyValuePair.Create(key, parameter.Value));

                        index++;
                    }
                }
            }

            command.CommandText = $"Update {_change.FullName}{set}{condition}";

            command.Parameters.AddRange(ExtraParameters);

            command.CommandTimeout = CommandTimeout;

            return command;
        }
    }
}
