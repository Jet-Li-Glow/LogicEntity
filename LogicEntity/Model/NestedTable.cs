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
        private ISelector _selector;

        private Command _command;

        private string _alias;

        /// <summary>
        /// 嵌套表
        /// </summary>
        /// <param name="selector"></param>
        public NestedTable(ISelector selector, string alias)
        {
            _selector = selector;

            _alias = alias;
        }

        /// <summary>
        /// 最后的表名
        /// </summary>
        internal override string FinalTableName => _alias;

        /// <summary>
        /// 列
        /// </summary>
        internal override IEnumerable<Description> Columns => _selector?.Columns ?? Enumerable.Empty<Description>();

        /// <summary>
        /// 列
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Column Column(string columnName)
        {
            Column column = new Column() { Table = this, ColumnName = columnName };

            column.Read(Columns.SingleOrDefault(s => s.Name == columnName)?.Reader);

            return column;
        }

        /// <summary>
        /// 转为字符串（延迟加载）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_command is null)
                _command = _selector?.GetCommandWithUniqueParameterName();

            return "(\n    " + _command?.CommandText.Replace("\n", "\n    ") + "\n  ) As " + _alias;
        }

        /// <summary>
        /// 参数（延迟加载）
        /// </summary>
        internal override IEnumerable<KeyValuePair<string, object>> Parameters
        {
            get
            {
                if (_command is null)
                    _command = _selector?.GetCommandWithUniqueParameterName();

                return _command?.Parameters;
            }
        }
    }
}
