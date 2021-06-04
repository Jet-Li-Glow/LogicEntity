﻿using System;
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
    public class Insertor<T> : IInsertorColumns<T>, IInsertorValues<T>, IOnDuplicateKeyUpdate<T> where T : Table, new()
    {
        private T _table;

        private List<string> _columns = new();

        private ValueDescription _valueDescription = new();

        private bool _isUpdateOnDuplicateKey;

        private Action<T> _updateValue;

        /// <summary>
        /// 插入操作器
        /// </summary>
        /// <param name="table"></param>
        public Insertor(T table)
        {
            _table = table;
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

            _columns.AddRange(columns.Select(c => c?.ColumnName ?? string.Empty));

            return this;
        }

        /// <summary>
        /// 数据行
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public IOnDuplicateKeyUpdate<T> Row<TRow>(TRow row)
        {
            if (row is null)
                return this;

            Rows(Enumerable.Repeat(row, 1));

            return this;
        }

        /// <summary>
        /// 多数据行
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IOnDuplicateKeyUpdate<T> Rows<TRow>(IEnumerable<TRow> rows)
        {
            if (rows is null)
                return this;

            Type type = typeof(TRow);

            List<PropertyInfo> properties = new List<PropertyInfo>();

            foreach (string columnName in _columns)
            {
                properties.Add(type.GetProperty(columnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase));
            }

            _valueDescription.Parameters = new();

            List<string> values = new();

            foreach (object row in rows)
            {
                List<string> parameters = new();

                foreach (PropertyInfo property in properties)
                {
                    object v = property?.GetValue(row);

                    if (v is Column)
                    {
                        v = (v as Column).Value;
                    }

                    string key = "@param" + DateTime.Now.Ticks;

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
            Command command = selector.GetCommand();

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
            _isUpdateOnDuplicateKey = true;

            _updateValue = updateValue;

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

            int index = 0;

            //列
            string columns = string.Join(", ", _columns);

            //值
            if (_valueDescription.Parameters is not null)
            {
                if (_valueDescription.Description is null)
                    _valueDescription.Description = string.Empty;

                foreach (KeyValuePair<string, object> parameter in _valueDescription.Parameters)
                {
                    string key = "@param" + index.ToString();

                    _valueDescription.Description = _valueDescription.Description.Replace(parameter.Key, key);

                    command.Parameters.Add(KeyValuePair.Create(key, parameter.Value));

                    index++;
                }
            }

            //更新
            string update = string.Empty;

            if (_isUpdateOnDuplicateKey)
            {
                T t = new();

                _updateValue?.Invoke(t);

                var properties = t.GetType().GetProperties().Where(p => p.PropertyType == typeof(Column));

                List<string> updateSets = new List<string>();

                foreach (PropertyInfo property in properties)
                {
                    Column column = property.GetValue(t) as Column;

                    if (column is null)
                        continue;

                    if (column.IsValueSet == false)
                        continue;

                    if (column.Value is Description)
                    {
                        updateSets.Add(column.FullContent + " = VALUES (" + (column.Value as Description).FullContent + ")");

                        continue;
                    }

                    string key = "@param" + index.ToString();

                    updateSets.Add(column.FullContent + " = " + key);

                    command.Parameters.Add(KeyValuePair.Create(key, column.Value));

                    index++;
                }

                update = "\nON DUPLICATE KEY UPDATE\n" + string.Join(",\n", updateSets);
            }


            command.CommandText = $"Insert Into {_table.FullName} ({columns}){_valueDescription.Description}{update}";

            return command;
        }

        private class ValueDescription
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
    }
}
