using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.Operator;

namespace LogicEntity.Model
{
    /// <summary>
    /// 表
    /// </summary>
    public abstract class Table : TableDescription
    {
        List<Column> _columns = new();

        bool _hasAlias;

        string _alias;

        /// <summary>
        /// 构造
        /// </summary>
        public Table()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(Column));

            foreach (PropertyInfo property in properties)
            {
                Column column = new Column(this, property.Name);

                property.SetValue(this, column);

                _columns.Add(column);
            }
        }

        /// <summary>
        /// 库名
        /// </summary>
        public virtual string __SchemaName => string.Empty;

        /// <summary>
        /// 表名
        /// </summary>
        public virtual string __TableName => GetType().Name;

        /// <summary>
        /// 全名
        /// </summary>
        internal override string FullName => (__SchemaName.IsValid() ? $"`{__SchemaName}`." : string.Empty) + $"`{__TableName}`";

        /// <summary>
        /// 列
        /// </summary>
        internal override IEnumerable<Column> Columns => _columns.AsEnumerable();

        /// <summary>
        /// 是否有别名
        /// </summary>
        internal override bool HasAlias => _hasAlias;

        /// <summary>
        /// 别名
        /// </summary>
        internal override string Alias => _alias;

        /// <summary>
        /// 添加别名
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public Table As(string alias)
        {
            _alias = alias;

            _hasAlias = true;

            return this;
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal override (string, IEnumerable<KeyValuePair<string, object>>) Build()
        {
            return (FullName + (_hasAlias ? $" As `{_alias}`" : string.Empty), null);
        }
    }
}
