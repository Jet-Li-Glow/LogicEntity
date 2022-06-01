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
        private readonly List<Column> _columns = new();

        private bool _hasAlias;

        private string _alias;

        /// <summary>
        /// 表
        /// </summary>
        public Table()
        {
            var properties = GetType().GetProperties().Where(p => p.PropertyType == typeof(Column) || p.PropertyType.IsSubclassOf(typeof(Column)));

            foreach (PropertyInfo property in properties)
            {
                Column column = Activator.CreateInstance(property.PropertyType) as Column;

                column.Table = this;

                column.EntityPropertyName = property.Name;

                column.ColumnName = property.Name;

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
        /// 表全名
        /// </summary>
        public string __FullName => __SchemaName.IsValid() ? $"`{__SchemaName}`.`{__TableName}`" : $"`{__TableName}`";

        /// <summary>
        /// 最后的表名
        /// </summary>
        internal override string FinalTableName => _hasAlias ? $"`{_alias}`" : __FullName;

        /// <summary>
        /// 列
        /// </summary>
        internal override IEnumerable<Description> Columns => _columns.AsEnumerable();

        /// <summary>
        /// 添加别名
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public Table As(string alias)
        {
            _hasAlias = true;

            _alias = alias;

            return this;
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        internal override Command GetCommand()
        {
            Command command = new();

            command.CommandText = __FullName + (_hasAlias ? $" As `{_alias}`" : string.Empty);

            command.Parameters = Enumerable.Empty<KeyValuePair<string, object>>();

            return command;
        }
    }
}
