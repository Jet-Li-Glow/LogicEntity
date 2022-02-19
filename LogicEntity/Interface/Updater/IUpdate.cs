using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    /// <summary>
    /// 更新
    /// </summary>
    public interface IUpdate<T> where T : Table, new()
    {
        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterJoin<T> Update(T table);
    }
}
