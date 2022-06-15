using System;
using System.Collections.Generic;
using LogicEntity.Operator;

namespace LogicEntity.Model
{
    /// <summary>
    /// 列
    /// </summary>
    public sealed class Column : Description
    {
        string _columnName;

        string _alias;

        object _value;

        Func<(string, IEnumerable<KeyValuePair<string, object>>)> _columnBuild;

        protected override Func<(string, IEnumerable<KeyValuePair<string, object>>)> _build => _columnBuild;

        public Column(TableDescription table, string columnName)
        {
            Table = table;

            EntityPropertyName = columnName;

            _columnName = columnName;

            _columnBuild = () => ($"{Table?.FinalTableName}.`{ColumnName}`", null);
        }

        public Column(IValueExpression valueExpression)
        {
            _columnBuild = valueExpression.Build;
        }

        /// <summary>
        /// 列所属表
        /// </summary>
        internal TableDescription Table { get; set; }

        /// <summary>
        /// 实体属性名称（Table构造时，ColumnName还没有被重新赋值）
        /// </summary>
        internal string EntityPropertyName { get; private set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get
            {
                return _columnName;
            }

            set
            {
                _columnName = value;

                if (_columnName != EntityPropertyName && HasAlias == false)
                    Alias = EntityPropertyName;
            }
        }

        /// <summary>
        /// 全名
        /// </summary>
        internal string FullName => Table.FullName + $".`{ColumnName}`";

        /// <summary>
        /// 最后的列名
        /// </summary>
        internal string FinalColumnName => HasAlias ? Alias : ColumnName;

        /// <summary>
        /// 是否有别名
        /// </summary>
        internal bool HasAlias { get; private set; } = false;

        /// <summary>
        /// 别名
        /// </summary>
        internal string Alias
        {
            get
            {
                return _alias;
            }

            set
            {
                _alias = value;

                HasAlias = true;
            }
        }

        /// <summary>
        /// 是否设置值
        /// </summary>
        internal bool IsValueSet { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;

                IsValueSet = true;
            }
        }

        /// <summary>
        /// 读取器
        /// </summary>
        public Func<object, object> Reader { get; set; }

        /// <summary>
        /// 字节读取器
        /// </summary>
        public Func<Func<long, byte[], int, int, long>, object> BytesReader { get; set; }

        /// <summary>
        /// 字符读取器
        /// </summary>
        public Func<Func<long, char[], int, int, long>, object> CharsReader { get; set; }

        /// <summary>
        /// 写入器
        /// </summary>
        public Func<object, object> Writer { get; set; }
    }
}
