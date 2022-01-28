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

                column.ColumnName = property.Name;

                property.SetValue(this, column);

                _columns.Add(column);
            }
        }

        /// <summary>
        /// 库名
        /// </summary>
        public virtual string SchemaName => string.Empty;

        /// <summary>
        /// 表名
        /// </summary>
        public virtual string TableName => GetType().Name;

        /// <summary>
        /// 表全名
        /// </summary>
        public string FullName => SchemaName.IsValid() ? SchemaName + "." + TableName : TableName;

        /// <summary>
        /// 别名
        /// </summary>
        private string TableAlias { get; set; }

        /// <summary>
        /// 最后的表名
        /// </summary>
        internal override string FinalTableName => TableAlias ?? FullName;

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
            TableAlias = alias;

            return this;
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        public override string ToString()
        {
            return FullName + (TableAlias is null ? string.Empty : " As " + TableAlias);
        }
    }
}
