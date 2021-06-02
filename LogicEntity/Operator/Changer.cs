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
    public class Changer : IChangerOn
    {
        private Table _change;

        private Condition _conditions;

        private bool _hasConditons;

        public Changer(Table change)
        {
            _change = change;
        }

        public IUpdater On(Condition condition)
        {
            _conditions = condition;

            _hasConditons = true;

            return this;
        }

        public Command GetCommand()
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

                string key = "@param" + index.ToString();

                columns.Add(column.ColumnName + " = " + key);

                command.Parameters.Add(KeyValuePair.Create(key, column.Value));

                index++;
            }

            string set = "\nSet " + string.Join(",\n    ", columns);

            string conditions = string.Empty;

            if (_hasConditons)
            {
                conditions = "\nWhere ";

                if (_conditions is not null)
                {
                    conditions += _conditions.Description();

                    foreach (KeyValuePair<string, object> parameter in _conditions.GetParameters())
                    {
                        string key = "@param" + index.ToString();

                        conditions = conditions.Replace(parameter.Key, key);

                        command.Parameters.Add(KeyValuePair.Create(key, parameter.Value));

                        index++;
                    }
                }
            }

            command.CommandText = $"Update {_change.TableName}{set}{conditions}";

            return command;
        }
    }
}
