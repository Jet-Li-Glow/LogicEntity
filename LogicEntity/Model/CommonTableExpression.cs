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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
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
            Column column = new Column(this, columnName);

            column.Read(Columns.SingleOrDefault(s => s.FinalColumnName == columnName)?.Reader);

            return column;
        }

        /// <summary>
        /// 查询器
        /// </summary>
        public ISelector Selector { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        internal override string FullName => _name;

        /// <summary>
        /// 是否有别名
        /// </summary>
        internal override bool HasAlias => false;

        /// <summary>
        /// 别名
        /// </summary>
        internal override string Alias => string.Empty;

        /// <summary>
        /// 列
        /// </summary>
        internal override IEnumerable<Column> Columns => Selector?.Columns ?? Enumerable.Empty<Column>();

        /// <summary>
        /// 生成定义
        /// </summary>
        internal (string, IEnumerable<KeyValuePair<string, object>>) BuildDefinition()
        {
            string columnsDefinition = string.Empty;

            if (_columnNames.Any())
                columnsDefinition = $"({string.Join(", ", _columnNames)})";

            return new Description(_name + " " + columnsDefinition + " As\n (\n    {0}\n  )", Selector).Build();
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        internal override (string, IEnumerable<KeyValuePair<string, object>>) Build()
        {
            return (_name, null);
        }
    }
}
