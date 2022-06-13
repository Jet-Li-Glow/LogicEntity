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
        public static IOperate With(params CommonTableExpression[] commonTableExpressions)
        {
            return new DBOperatorImplement().With(false, commonTableExpressions);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public static IOperate WithRecursive(params CommonTableExpression[] commonTableExpressions)
        {
            return new DBOperatorImplement().With(true, commonTableExpressions);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static IFrom Select(params ColumnCollection[] columns)
        {
            return ((IOperate)new DBOperatorImplement()).Select(columns);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static IFrom SelectDistinct(params ColumnCollection[] columns)
        {
            return ((IOperate)new DBOperatorImplement()).SelectDistinct(columns);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static IInsertor Insert<T>(T row) where T : Table, new()
        {
            return new DBOperatorImplement().Insert().Table(row)
                .Columns(row.Columns.Where(c => c.IsValueSet).ToArray())
                .Rows(row);
        }

        /// <summary>
        /// 插入（当主键冲突时更新）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static IInsertor Save<T>(T row) where T : Table, new()
        {
            Column[] settedColumns = row.Columns.Where(c => c.IsValueSet).ToArray();

            return new DBOperatorImplement().Insert().Table(row)
                .Columns(settedColumns)
                .Rows(row)
                .OnDuplicateKeyUpdate(r =>
                {
                    foreach (Column column in settedColumns)
                    {
                        Column col = r.Columns.FirstOrDefault(c => c.EntityPropertyName == column.EntityPropertyName);

                        if (col is not null)
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
            return new DBOperatorImplement().Insert().Into().Table(table);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">插入的表</param>
        /// <returns></returns>
        public static IInsertorColumns<T> InsertIgnore<T>(T table) where T : Table, new()
        {
            return new DBOperatorImplement().Insert().Ignore().Table(table);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">插入的表</param>
        /// <returns></returns>
        public static IInsertorColumns<T> ReplaceInto<T>(T table) where T : Table, new()
        {
            return new DBOperatorImplement().Replace().Into().Table(table);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public static IUpdaterSet Update(JoinedTable table)
        {
            return new DBOperatorImplement().Update(table);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public static IUpdaterOn ApplyChanges(Table row)
        {
            return new DBOperatorImplement().ApplyChanges(row);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static IDeleterFrom Delete(params Table[] tables)
        {
            return new DBOperatorImplement().Delete(tables);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static IDeleterWhere DeleteFrom(Table table)
        {
            return ((IOperate)new DBOperatorImplement()).DeleteFrom(table);
        }
    }
}
