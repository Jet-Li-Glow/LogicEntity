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
        /// <param name="table">数据实体</param>
        /// <returns></returns>
        public static IInsertor Insert<T>(T row) where T : Table, new()
        {
            var properties = row.GetType().GetProperties().Where(p => p.PropertyType == typeof(Column));

            List<Column> colums = new();

            foreach (PropertyInfo property in properties)
            {
                Column column = property.GetValue(row) as Column;

                if (column is null)
                    continue;

                if (column.IsValueSet == false)
                    continue;

                colums.Add(column);
            }

            return InsertInto(row).Columns(colums.ToArray()).Row(row);
        }

        /// <summary>
        /// 插入操作器（当主键冲突时更新）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public IInsertor Save<T>(T row) where T : Table, new()
        {
            return new Insertor<T>(row);
        }

        /// <summary>
        /// 插入操作器
        /// </summary>
        /// <param name="table">插入的表</param>
        /// <returns></returns>
        public static IInsertorColumns<T> InsertInto<T>(T table) where T : Table, new()
        {
            return new Insertor<T>(table);
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
        public static IDeleteWhere Delete(Table table)
        {
            return new Deleter(table);
        }
    }
}
