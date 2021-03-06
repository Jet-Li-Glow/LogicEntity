using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Grammar
{
    public interface IInsertorValues<T> where T : Table, new()
    {
        /// <summary>
        /// 数据行
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IOnDuplicateKeyUpdate<T> Row<TRow>(params TRow[] rows);

        /// <summary>
        /// 多数据行
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IOnDuplicateKeyUpdate<T> Rows<TRow>(IEnumerable<TRow> rows)
        {
            return Row(rows.ToArray());
        }

        /// <summary>
        /// 查询操作器
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IOnDuplicateKeyUpdate<T> Rows(ISelector selector);
    }
}
