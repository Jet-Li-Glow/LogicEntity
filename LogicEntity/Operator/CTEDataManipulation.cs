using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// CTE数据操作
    /// </summary>
    public class CTEDataManipulation : ICTEDataManipulation
    {
        /// <summary>
        /// 是否递归
        /// </summary>
        bool _isRecursive = false;

        /// <summary>
        /// 表达式
        /// </summary>
        CommonTableExpression[] _commonTableExpressions;

        /// <summary>
        /// 公共表格表达式
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public ICTEDataManipulation With(params CommonTableExpression[] commonTableExpressions)
        {
            _commonTableExpressions = commonTableExpressions;

            return this;
        }

        /// <summary>
        /// 公共表格表达式
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public ICTEDataManipulation WithRecursive(params CommonTableExpression[] commonTableExpressions)
        {
            With(commonTableExpressions);

            _isRecursive = true;

            return this;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public IDistinct Select(params Description[] columnDescriptions)
        {
            Selector selector = new();

            if (_isRecursive)
                selector.WithRecursive(_commonTableExpressions);
            else
                selector.With(_commonTableExpressions);

            selector.Select(columnDescriptions);

            return selector;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public IFrom SelectDistinct(params Description[] columnDescriptions)
        {
            return Select(columnDescriptions).Distinct();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterJoin<T> Update<T>(T table) where T : Table, new()
        {
            Updater<T> updater = new();

            if (_isRecursive)
                updater.WithRecursive(_commonTableExpressions);
            else
                updater.With(_commonTableExpressions);

            updater.Update(table);

            return updater;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public IDeleterFrom Delete(params Table[] tables)
        {
            Deleter deleter = new();

            if (_isRecursive)
                deleter.WithRecursive(_commonTableExpressions);
            else
                deleter.With(_commonTableExpressions);

            return deleter.Delete(tables);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public IDeleterJoin DeleterFrom(params TableDescription[] tables)
        {
            return Delete().From(tables);
        }
    }
}
