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
    public class NestedTable : TableExpression
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

            if (_selector is not null)
            {
                _selector.__Indent = 4;

                _columns = _selector.Columns.Select(column => new Column(this, column.FinalColumnName)
                {
                    Reader = column.Reader,
                    BytesReader = column.BytesReader,
                    CharsReader = column.CharsReader
                });
            }
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
            return _columns.Single(column => column.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal protected override (string, IEnumerable<KeyValuePair<string, object>>) Build()
        {
            return (new SqlExpression("\n  (\n{0}\n  ) As `" + _alias + "`", _selector) as ISqlExpression).Build();
        }
    }
}
