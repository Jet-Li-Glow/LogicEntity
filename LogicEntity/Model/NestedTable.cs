using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Operator;

namespace LogicEntity.Model
{
    /// <summary>
    /// 嵌套表
    /// </summary>
    public class NestedTable : TableDescription
    {
        ISelector _selector;

        string _alias;

        IEnumerable<Column> _columns;

        /// <summary>
        /// 嵌套表
        /// </summary>
        /// <param name="selector"></param>
        public NestedTable(ISelector selector, string alias)
        {
            _selector = selector;

            _alias = alias;

            _columns = _selector?.Columns ?? Enumerable.Empty<Column>();
        }

        /// <summary>
        /// 全名
        /// </summary>
        internal override string FullName => string.Empty;

        /// <summary>
        /// 是否有别名
        /// </summary>
        internal override bool HasAlias => true;

        /// <summary>
        /// 别名
        /// </summary>
        internal override string Alias => _alias;

        /// <summary>
        /// 列
        /// </summary>
        internal override IEnumerable<Column> Columns => _columns;

        /// <summary>
        /// 列
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Column Column(string columnName)
        {
            Column column = new Column(this, columnName);

            column.Read(_columns.SingleOrDefault(s => s.FinalColumnName == columnName)?.Reader);

            return column;
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal override (string, IEnumerable<KeyValuePair<string, object>>) Build()
        {
            Command selectorCommand = _selector?.GetCommandWithUniqueParameterName();

            return ($"(\n    {selectorCommand?.CommandText?.Replace("\n", "\n    ")}\n  ) As `{_alias}`", selectorCommand?.Parameters);
        }
    }
}
