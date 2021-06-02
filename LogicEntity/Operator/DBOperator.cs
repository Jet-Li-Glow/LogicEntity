using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Extension;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 数据库操作器
    /// </summary>
    public abstract class DBOperator
    {
        /// <summary>
        /// 查询操作器
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public static IFrom Select(params Description[] columnDescriptions)
        {
            return new Selector(columnDescriptions);
        }

        /// <summary>
        /// 查询操作器
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public static IFrom SelectDistinct(params Description[] columnDescriptions)
        {
            return new Selector(columnDescriptions).Distinct();
        }

        /// <summary>
        /// 插入操作器
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IInsertor Insert(Table table)
        {
            return new Insertor(table);
        }

        /// <summary>
        /// 更新操作器
        /// </summary>
        /// <returns></returns>
        public static IUpdaterJoin<T> Update<T>(T table) where T : Table, new()
        {
            return new Updater<T>(table);
        }

        /// <summary>
        /// 更新操作器
        /// </summary>
        /// <returns></returns>
        public static Changer ApplyChanges(Table change)
        {
            return new Changer(change);
        }

        /// <summary>
        /// 删除操作器
        /// </summary>
        /// <returns></returns>
        public static IDeleteFrom Delete()
        {
            return new Deleter();
        }
    }
}
