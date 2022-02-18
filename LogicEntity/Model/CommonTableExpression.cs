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
    /// 公共表格表达式
    /// </summary>
    public class CommonTableExpression : TableDescription
    {
        string _name;

        List<string> _columnNames = new();

        public CommonTableExpression(string name)
        {
            _name = name;
        }

        /// <summary>
        /// 定义列
        /// </summary>
        /// <param name="columnNames"></param>
        public void DefineColumns(params string[] columnNames)
        {
            if (columnNames is not null)
                _columnNames.AddRange(columnNames);
        }

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
        /// 查询器
        /// </summary>
        public ISelector Selector { get; set; }

        /// <summary>
        /// 最后的表名
        /// </summary>
        internal override string FinalTableName => _name;

        /// <summary>
        /// 列
        /// </summary>
        internal override IEnumerable<Description> Columns => Selector?.Columns ?? Enumerable.Empty<Description>();

        /// <summary>
        /// 获取命令
        /// </summary>
        internal Command GetCommonTableExpressionCommand()
        {
            Command command = new();

            Model.Command selectorCommand = Selector?.GetCommandWithUniqueParameterName();

            string cols = string.Empty;

            if (_columnNames.Any())
                cols = $" ({string.Join(", ", _columnNames)})";

            command.CommandText = $"  {_name}{cols} As\n  (\n    {selectorCommand?.CommandText.Replace("\n", "\n    ")}\n  )";

            command.Parameters = selectorCommand?.Parameters?.AsEnumerable() ?? Enumerable.Empty<KeyValuePair<string, object>>();

            return command;
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FinalTableName;
        }

        /// <summary>
        /// 命令
        /// </summary>
        internal class Command
        {
            public string CommandText { get; set; }

            public IEnumerable<KeyValuePair<string, object>> Parameters { get; set; }
        }
    }
}
