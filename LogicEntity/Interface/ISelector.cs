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
    public interface ISelector : IDbOperator
    {
        /// <summary>
        /// 联合
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public ISelector Union(ISelector selector);

        /// <summary>
        /// 联合所有
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public ISelector UnionAll(ISelector selector);

        /// <summary>
        /// 生成嵌套表
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public NestedTable As(string alias);

        /// <summary>
        /// 列
        /// </summary>
        public IEnumerable<Description> Columns { get; }
    }
}
