using System;
using System.Collections;
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
    /// 列
    /// </summary>
    public class Column : Description
    {
        private object _value;

        /// <summary>
        /// 列所属表
        /// </summary>
        public TableDescription Table { get; internal set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; internal set; }

        /// <summary>
        /// 全名
        /// </summary>
        public string FullName => Table.FinalTableName + "." + ColumnName;

        /// <summary>
        /// 是否设置值
        /// </summary>
        internal bool IsValueSet { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            internal get { return _value; }
            set { _value = value; IsValueSet = true; }
        }

        /// <summary>
        /// 主体内容
        /// </summary>
        protected override string Content => FullName;
    }
}
