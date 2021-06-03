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
    public class Insertor : IInsertorColumns, IInsertorValues, IInsertor
    {
        private Table _table;

        private List<string> _columns = new();

        private List<List<object>> _rows = new();

        /// <summary>
        /// 插入操作器
        /// </summary>
        /// <param name="table"></param>
        public Insertor(Table table)
        {
            _table = table;
        }

        public IInsertorValues Columns(params Column[] columns)
        {
            if (columns is null)
                return this;

            _columns.AddRange(columns.Select(c => c?.ColumnName ?? string.Empty));

            return this;
        }

        public IInsertor Row<T>(T row)
        {
            Rows(Enumerable.Repeat(row, 1));

            return this;
        }

        public IInsertor Rows<T>(IEnumerable<T> rows)
        {
            if (rows is null)
                return this;

            Type type = typeof(T);

            List<PropertyInfo> properties = new List<PropertyInfo>();

            foreach (string columnName in _columns)
            {
                properties.Add(type.GetProperty(columnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase));
            }

            foreach (object obj in rows)
            {
                List<object> row = new();

                foreach (PropertyInfo property in properties)
                {
                    object v = property?.GetValue(obj);

                    if (v is Column)
                    {
                        v = (v as Column).Value;
                    }

                    row.Add(v);
                }

                _rows.Add(row);
            }

            return this;
        }

        /// <summary>
        /// 获取操作命令
        /// </summary>
        /// <returns></returns>
        public Command GetCommand()
        {
            Command command = new Command();

            command.Parameters = new();

            string columns = string.Join(", ", _columns);

            int index = 0;

            List<string> values = new();

            foreach (List<object> row in _rows)
            {
                List<string> parameters = new();

                foreach (object v in row)
                {
                    string key = "@param" + index.ToString();

                    parameters.Add(key);

                    command.Parameters.Add(KeyValuePair.Create(key, v));

                    index++;
                }

                values.Add("(" + string.Join(", ", parameters) + ")");
            }

            string rows = string.Join(",\n       ", values);

            command.CommandText = $"Insert Into {_table.TableName} ({columns})\nValues {rows}";

            return command;
        }
    }
}
