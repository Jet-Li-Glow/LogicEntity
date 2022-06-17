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
    public class CommonTableExpression : Table
    {
        string _tableName;

        ISelector _selector;

        List<Column> _columns = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public CommonTableExpression(string name)
        {
            _tableName = name;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public override string __TableName => _tableName;

        /// <summary>
        /// 定义列
        /// </summary>
        /// <param name="columnNames"></param>
        public void DefineColumns(params string[] columnNames)
        {
            if (columnNames is not null)
            {
                _columns.Clear();

                _columns.AddRange(columnNames.Select(name => new Column(this, name)));
            }
        }

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
        /// 查询器
        /// </summary>
        public ISelector Selector
        {
            set
            {
                _selector = value;

                if (_selector is not null)
                {
                    _selector.__Indent = 4;

                    if (_columns.Any() == false)
                    {
                        _columns.AddRange(_selector.Columns.Select(column => new Column(this, column.FinalColumnName)
                        {
                            Reader = column.Reader,
                            BytesReader = column.BytesReader,
                            CharsReader = column.CharsReader
                        }));
                    }
                }
            }
        }

        /// <summary>
        /// 列
        /// </summary>
        internal override IEnumerable<Column> Columns => _columns.AsEnumerable();

        /// <summary>
        /// 生成定义
        /// </summary>
        internal (string, IEnumerable<KeyValuePair<string, object>>) BuildDefinition()
        {
            string columnsDefinition = string.Empty;

            if (_columns.Any())
                columnsDefinition = $"({string.Join(", ", _columns.Select(column => column.ColumnName))})";

            (var cmd, var ps) = (_selector as IValueExpression)?.Build() ?? (null, null);

            return (_tableName + " " + columnsDefinition + $" As\n  (\n{cmd}\n  )", ps);
        }
    }
}
