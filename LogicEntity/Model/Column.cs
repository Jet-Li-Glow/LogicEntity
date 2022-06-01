using LogicEntity.Operator;

namespace LogicEntity.Model
{
    /// <summary>
    /// 列
    /// </summary>
    public sealed class Column : Description
    {
        private object _value;

        /// <summary>
        /// 列所属表
        /// </summary>
        public TableDescription Table { get; internal set; }

        /// <summary>
        /// 实体属性名称
        /// </summary>
        internal string EntityPropertyName { get; set; }

        /// <summary>
        /// 表达式的列名
        /// </summary>
        public override string Name => ColumnName;

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        public string FullName => Table.FinalTableName + $".`{ColumnName}`";

        /// <summary>
        /// 是否设置值
        /// </summary>
        internal bool IsValueSet { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; IsValueSet = true; }
        }

        /// <summary>
        /// 主体内容
        /// </summary>
        protected override string Content => FullName;
    }
}
