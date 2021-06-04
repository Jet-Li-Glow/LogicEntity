using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Extension;
using LogicEntity.Operator;

namespace LogicEntity.Model
{
    /// <summary>
    /// 表
    /// </summary>
    public abstract class Table : TableDescription
    {
        /// <summary>
        /// 表
        /// </summary>
        public Table()
        {
            Type type = GetType();

            var properties = type.GetProperties().Where(p => p.PropertyType == typeof(Column));

            foreach (PropertyInfo property in properties)
            {
                Column column = Activator.CreateInstance(property.PropertyType) as Column;

                column.Table = this;
                column.ColumnName = property.Name;

                property.SetValue(this, column);
            }
        }

        /// <summary>
        /// 库名
        /// </summary>
        public virtual string schemaName => string.Empty;

        /// <summary>
        /// 表名
        /// </summary>
        public virtual string TableName => GetType().Name;

        /// <summary>
        /// 表全名
        /// </summary>
        public string FullName => schemaName.IsValid() ? schemaName + "." + TableName : TableName;

        /// <summary>
        /// 别名
        /// </summary>
        private string TableAlias { get; set; }

        /// <summary>
        /// 最后的表名
        /// </summary>
        internal override string FinalTableName => TableAlias ?? FullName;

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
        /// 描述
        /// </summary>
        internal override string Description()
        {
            return FullName + (TableAlias is null ? string.Empty : " As " + TableAlias);
        }

        /// <summary>
        /// 所有的列
        /// </summary>
        /// <returns></returns>
        public override Description All()
        {
            return new AllColumnDescription(this);
        }
    }
}
