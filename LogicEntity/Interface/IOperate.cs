using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IOperate
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public IDistinct Select();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IFrom Select(params ColumnCollection[] columns)
        {
            return Select().SetColumns(columns);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IFrom SelectDistinct(params ColumnCollection[] columns)
        {
            return Select().Distinct().SetColumns(columns);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterSet Update(JoinedTable table);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn ApplyChanges(Table table);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public IDeleterFrom Delete(params Table[] tables);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IDeleterWhere DeleteFrom(Table table)
        {
            return Delete().From(table);
        }
    }
}
