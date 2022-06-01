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
            Column column = new Column() { Table = this, EntityPropertyName = columnName, ColumnName = columnName };

            column.Read(Columns.SingleOrDefault(s => s.Name == columnName)?.Reader);

            return column;
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        internal override Command GetCommand()
        {
            Command command = new();

            Model.Command selectorCommand = _selector?.GetCommandWithUniqueParameterName();

            command.CommandText = "(\n    " + selectorCommand?.CommandText.Replace("\n", "\n    ") + "\n  ) As " + _alias;

            command.Parameters = selectorCommand?.Parameters ?? Enumerable.Empty<KeyValuePair<string, object>>();

            return command;
        }
    }
}
