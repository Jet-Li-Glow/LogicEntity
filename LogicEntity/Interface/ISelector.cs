using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    /// <summary>
    /// 查询操作器
    /// </summary>
    public interface ISelector : IAddParameterDbOperator
    {
        /// <summary>
        /// 生成嵌套表
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public NestedTable As(string alias);

        /// <summary>
        /// 列
        /// </summary>
        public IEnumerable<Column> Columns { get; }
    }
}
