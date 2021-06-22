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
        /// 添加别名
        /// </summary>
        /// <param name="alias"></param>
        internal NestedTable As(string alias)
        {
            _alias = alias;

            return this;
        }

        /// <summary>
        /// 最后的表名
        /// </summary>
        internal override string FinalTableName => _alias;

        /// <summary>
        /// 列
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Column Column(string columnName)
        {
            return new Column() { Table = this, ColumnName = columnName };
        }

        /// <summary>
        /// 所有的列
        /// </summary>
        /// <returns></returns>
        public override Description All()
        {
            return new AllColumnDescription(this);
        }

        /// <summary>
        /// 转为字符串（延迟加载）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_command is null)
                _command = _selector?.GetCommand();

            return "(\n    " + _command?.CommandText.Replace("\n", "\n    ") + "\n  ) As " + _alias;
        }

        /// <summary>
        /// 参数（延迟加载）
        /// </summary>
        internal override IEnumerable<KeyValuePair<string, object>> GetParameters()
        {
            if (_command is null)
                _command = _selector?.GetCommand();

            return _command?.Parameters;
        }
    }
}
