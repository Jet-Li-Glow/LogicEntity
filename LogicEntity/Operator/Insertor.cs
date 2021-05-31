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
    /// 插入操作器
    /// </summary>
    public class Insertor : IInsertor
    {
        private Table _entity;

        /// <summary>
        /// 插入操作器
        /// </summary>
        /// <param name="table"></param>
        public Insertor(Table table)
        {
            _entity = table;
        }

        /// <summary>
        /// 获取操作命令
        /// </summary>
        /// <returns></returns>
        public Command GetCommand()
        {
            Command command = new Command();

            if (_entity is null)
                return command;

            var properties = _entity.GetType().GetProperties().Where(p => p.PropertyType == typeof(Column));

            List<string> colums = new();

            command.Parameters = new();

            int index = 0;

            foreach (PropertyInfo property in properties)
            {
                Column column = property.GetValue(_entity) as Column;

                if (column is null)
                    continue;

                if (column.IsValueSet == false)
                    continue;

                colums.Add(column.ColumnName);

                command.Parameters.Add(KeyValuePair.Create("@param" + index.ToString(), column.Value));

                index++;
            }

            command.CommandText = $"Insert Into {_entity.TableName} ({string.Join(", ", colums)}) Values ({string.Join(", ", command.Parameters.Select(p => p.Key))})";

            return command;
        }
    }
}
