using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public abstract partial class AbstractDataBase
    {
        /// <summary>
        /// 插入下一条数据，并返回自增主键
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public int InsertNext(Table row)
        {
            return 0;
        }
    }
}
