using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 数据库操作器
    /// </summary>
    public static class DBOperator
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public static ICTEDataManipulation With(params CommonTableExpression[] commonTableExpressions)
        {
            return new CTEDataManipulation().With(commonTableExpressions);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public static ICTEDataManipulation WithRecursive(params CommonTableExpression[] commonTableExpressions)
        {
            return new CTEDataManipulation().WithRecursive(commonTableExpressions);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public static IDistinct Select(params Description[] columnDescriptions)
        {
            return With(null).Select(columnDescriptions);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public static IFrom SelectDistinct(params Description[] columnDescriptions)
        {
            return With(null).SelectDistinct(columnDescriptions);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="table">数据实体</param>
        /// <returns></returns>
        public static IInsertor Insert<T>(T row) where T : Table, new()
        {
            List<Column> colums = new();

            foreach (PropertyInfo property in row.GetType().GetProperties())
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
        /// 插入（当主键冲突时更新）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static IInsertor Save<T>(T row) where T : Table, new()
        {
            T table = new T();

            List<Column> colums = new();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                Column column = property.GetValue(row) as Column;

                if (column is null)
                    continue;

                if (column.IsValueSet == false)
                    continue;

                colums.Add(property.GetValue(table) as Column);
            }

            return InsertInto(table).Columns(colums.ToArray()).Row(row).OnDuplicateKeyUpdate(t =>
            {
                Type type = t.GetType();

                foreach (Column column in colums)
                {
                    Column col = type.GetProperty(column.ColumnName)?.GetValue(t) as Column;

                    if (col is null)
                        continue;

                    col.Value = col.Values();
                }
            });
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">插入的表</param>
        /// <returns></returns>
        public static IInsertorColumns<T> InsertInto<T>(T table) where T : Table, new()
        {
            return new Insertor<T>(table);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">插入的表</param>
        /// <returns></returns>
        public static IInsertorColumns<T> InsertIgnore<T>(T table) where T : Table, new()
        {
            return new Insertor<T>(table, true, false);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">插入的表</param>
        /// <returns></returns>
        public static IInsertorColumns<T> ReplaceInto<T>(T table) where T : Table, new()
        {
            return new Insertor<T>(table, false, true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public static IUpdaterJoin<T> Update<T>(T table) where T : Table, new()
        {
            return With(null).Update(table);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public static IChanger ApplyChanges(Table change)
        {
            return new Changer(change);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static IDeleterFrom Delete(params Table[] tables)
        {
            return With(null).Delete(tables);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static IDeleterJoin DeleterFrom(params TableDescription[] tables)
        {
            return With(null).DeleterFrom(tables);
        }
    }
}
