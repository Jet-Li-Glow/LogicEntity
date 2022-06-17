using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    public class Window : ISqlExpression
    {
        /// <summary>
        /// 节点
        /// </summary>
        List<SqlExpression> _nodes = new();

        /// <summary>
        /// 别名
        /// </summary>
        internal string Alias { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="alias"></param>
        public Window(string alias)
        {
            Alias = alias;
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        public Window PartitionBy(params IValueExpression[] valueExpressions)
        {
            if (valueExpressions is null)
                valueExpressions = Array.Empty<IValueExpression>();

            _nodes.Add(SqlExpression.PartitionBy(valueExpressions));

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public Window OrderBy(IValueExpression valueExpression)
        {
            _nodes.Add(SqlExpression.OrderBy(valueExpression));

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public Window OrderByDescending(IValueExpression valueExpression)
        {
            _nodes.Add(SqlExpression.OrderByDescending(valueExpression));

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public Window ThenBy(IValueExpression valueExpression)
        {
            _nodes.Add(SqlExpression.ThenBy(valueExpression));

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public Window ThenByDescending(IValueExpression valueExpression)
        {
            _nodes.Add(SqlExpression.ThenByDescending(valueExpression));

            return this;
        }

        /// <summary>
        /// 行
        /// </summary>
        /// <returns></returns>
        public Window Rows()
        {
            _nodes.Add(SqlExpression.Rows());

            return this;
        }

        /// <summary>
        /// 范围
        /// </summary>
        /// <returns></returns>
        public Window Range()
        {
            _nodes.Add(SqlExpression.Range());

            return this;
        }

        /// <summary>
        /// Between
        /// </summary>
        /// <returns></returns>
        public Window Between()
        {
            _nodes.Add(SqlExpression.Between());

            return this;
        }

        /// <summary>
        /// And
        /// </summary>
        /// <returns></returns>
        public Window And()
        {
            _nodes.Add(SqlExpression.And());

            return this;
        }

        /// <summary>
        /// 当前行
        /// </summary>
        /// <returns></returns>
        public Window CurrentRow()
        {
            _nodes.Add(SqlExpression.CurrentRow());

            return this;
        }

        /// <summary>
        /// 第一行作为开始
        /// </summary>
        /// <returns></returns>
        public Window UnboundedPreceding()
        {
            _nodes.Add(SqlExpression.UnboundedPreceding());

            return this;
        }

        /// <summary>
        /// 最后一行作为结束
        /// </summary>
        /// <returns></returns>
        public Window UnboundedFollowing()
        {
            _nodes.Add(SqlExpression.UnboundedFollowing());

            return this;
        }

        /// <summary>
        /// 向前
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public Window Preceding(IValueExpression valueExpression)
        {
            _nodes.Add(SqlExpression.Preceding(valueExpression));

            return this;
        }

        /// <summary>
        /// 向前
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Window Preceding(int count)
        {
            _nodes.Add(SqlExpression.Preceding(count));

            return this;
        }

        /// <summary>
        /// 向后
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public Window Following(IValueExpression valueExpression)
        {
            _nodes.Add(SqlExpression.Following(valueExpression));

            return this;
        }

        /// <summary>
        /// 向后
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Window Following(int count)
        {
            _nodes.Add(SqlExpression.Following(count));

            return this;
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        (string, IEnumerable<KeyValuePair<string, object>>) ISqlExpression.Build()
        {
            return (SqlExpression.__Join(" ", _nodes) as ISqlExpression).Build();
        }
    }
}
