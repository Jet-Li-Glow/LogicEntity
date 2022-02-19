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
    /// CTE数据操作
    /// </summary>
    public interface ICTEDataManipulation
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public IDistinct Select(params Description[] columnDescriptions);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public IFrom SelectDistinct(params Description[] columnDescriptions);

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterJoin<T> Update<T>(T table) where T : Table, new();

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public IDeleterFrom Delete(params Table[] tables);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public IDeleterJoin DeleterFrom(params TableDescription[] tables);
    }
}
