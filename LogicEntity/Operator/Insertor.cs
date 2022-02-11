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
    /// 插入操作器
    /// </summary>
    internal class Insertor<T> : OperatorBase, IInsertorColumns<T>, IInsertorValues<T>, IOnDuplicateKeyUpdate<T> where T : Table, new()
    {
        /// <summary>
        /// 表
        /// </summary>
        T _table;

        /// <summary>
        /// 列
        /// </summary>
        List<Column> _columns = new();

        /// <summary>
        /// 是否忽略冲突
        /// </summary>
        bool _isIgnore = false;

        /// <summary>
        /// 是否替换
        /// </summary>
        bool _isReplace = false;

        /// <summary>
        /// 值描述
        /// </summary>
        ValueDescription _valueDescription = new();

        /// <summary>
        /// 更新描述
        /// </summary>
        OnDuplicateKeyUpdateDescription _updateDescription;

        /// <summary>
        /// 插入操作器
        /// </summary>
        /// <param name="table"></param>
        public Insertor(T table)
        {
            _table = table;
        }

        /// <summary>
        /// 插入操作器
        /// </summary>
        /// <param name="table"></param>
        /// <param name="isIgnore"></param>
        /// <param name="isReplace"></param>
        internal Insertor(T table, bool isIgnore, bool isReplace)
        {
            _table = table;

            _isIgnore = isIgnore;

            _isReplace = isReplace;
        }

        /// <summary>
        /// 列
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IInsertorValues<T> Columns(params Column[] columns)
        {
            if (columns is null)
                return this;

            _columns.AddRange(columns);

            return this;
        }

        public IOnDuplicateKeyUpdate<T> Row<TRow>(params TRow[] rows)
        {
            if (rows is null)
                return this;

            Rows(rows);

            return this;
        }

        public IOnDuplicateKeyUpdate<T> Rows<TRow>(IEnumerable<TRow> rows)
        {
            if (rows is null)
                return this;

            Type type = typeof(TRow);

            var properties = _columns.Select(c => new
            {
                PropertyInfo = type.GetProperty(c?.ColumnName ?? string.Empty, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase),
                Writer = c.Writer
            }).ToList();

            _valueDescription.Parameters = new();

            List<string> values = new();

            foreach (object row in rows)
            {
                List<string> parameters = new();

                foreach (var property in properties)
                {
                    object v = property.PropertyInfo?.GetValue(row);

                    if (v is Column)
                    {
                        v = (v as Column).Value;
                    }

                    if (property.Writer is not null)
                        v = property.Writer(v);

                    string key = ToolService.UniqueName();

                    parameters.Add(key);

                    _valueDescription.Parameters.Add(KeyValuePair.Create(key, v));
                }

                values.Add("(" + string.Join(", ", parameters) + ")");
            }

            _valueDescription.Description = "\nValues " + string.Join(",\n       ", values);

            return this;
        }

        /// <summary>
        /// 查询结果
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IOnDuplicateKeyUpdate<T> Rows(ISelector selector)
        {
            Command command = selector.GetCommandWithUniqueParameterName();

            _valueDescription.Description = "\n" + command.CommandText;

            _valueDescription.Parameters = command.Parameters;

            return this;
        }

        /// <summary>
        /// 当键值冲突时更新
        /// </summary>
        /// <param name="updateValue"></param>
        /// <returns></returns>
        public IInsertor OnDuplicateKeyUpdate(Action<T> updateValue)
        {
            _updateDescription = new OnDuplicateKeyUpdateDescription();

            _updateDescription.Parameters = new();

            updateValue?.Invoke(_table);

            List<string> updateSets = new List<string>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                Column column = property.GetValue(_table) as Column;

                if (column is null)
                    continue;

                if (column.IsValueSet == false)
                    continue;

                if (column.Value is Description)
                {
                    updateSets.Add($"{column} = {column.Value as Description}");

                    continue;
                }

                string key = ToolService.UniqueName();

                updateSets.Add(column.ToString() + " = " + key);

                _updateDescription.Parameters.Add(KeyValuePair.Create(key, column.Value));
            }

            _updateDescription.Description = $"\nON DUPLICATE KEY UPDATE\n{string.Join(",\n", updateSets)}";

            return this;
        }

        /// <summary>
        /// 当键值冲突时更新
        /// </summary>
        /// <param name="updateValue"></param>
        /// <returns></returns>
        public IInsertor OnDuplicateKeyUpdate(Action<T, T> updateValueWithRow)
        {
            _updateDescription = new OnDuplicateKeyUpdateDescription();

            _updateDescription.Parameters = new();

            Type type = typeof(T);

            T row = new();

            updateValueWithRow?.Invoke(_table, row);

            string rowName = type.Name + "Data";

            row.As(rowName);

            List<string> updateSets = new List<string>();

            foreach (PropertyInfo property in type.GetProperties())
            {
                Column column = property.GetValue(_table) as Column;

                if (column is null)
                    continue;

                if (column.IsValueSet == false)
                    continue;

                if (column.Value is Description)
                {
                    updateSets.Add($"{column} = {column.Value as Description}");

                    continue;
                }

                string key = ToolService.UniqueName();

                updateSets.Add(column.ToString() + " = " + key);

                _updateDescription.Parameters.Add(KeyValuePair.Create(key, column.Value));
            }

            _updateDescription.Description = $"\nAs {rowName}\nON DUPLICATE KEY UPDATE\n{string.Join(",\n", updateSets)}";

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

            //操作
            string operate = "Insert Into";

            if (_isIgnore)
                operate = "Insert Ignore";

            if (_isReplace)
                operate = "Replace Into";

            //列
            string columns = string.Join(", ", _columns.Select(c => c?.ColumnName ?? string.Empty));

            //值
            string valueDescription = _valueDescription.Description ?? string.Empty;

            if (_valueDescription.Parameters is not null)
                command.Parameters.AddRange(_valueDescription.Parameters);

            //更新
            string update = string.Empty;

            if (_updateDescription is not null)
            {
                update = _updateDescription.Description ?? string.Empty;

                if (_updateDescription.Parameters is not null)
                    command.Parameters.AddRange(_updateDescription.Parameters);
            }

            command.CommandText = $"{operate} {_table.FullName} ({columns}){valueDescription}{update}";

            command.Parameters.AddRange(ExtraParameters);

            command.CommandTimeout = CommandTimeout;

            return command;
        }

        /// <summary>
        /// 值描述
        /// </summary>
        class ValueDescription
        {
            /// <summary>
            /// 值描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 参数
            /// </summary>
            public List<KeyValuePair<string, object>> Parameters { get; set; }
        }

        /// <summary>
        /// 更新描述
        /// </summary>
        class OnDuplicateKeyUpdateDescription
        {
            /// <summary>
            /// 更新描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 参数
            /// </summary>
            public List<KeyValuePair<string, object>> Parameters { get; set; }
        }
    }
}
