using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    public class Window
    {
        /// <summary>
        /// 节点
        /// </summary>
        List<Description> _nodes = new();

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
        public Window PartitionBy(params Description[] descriptions)
        {
            if (descriptions is null)
                descriptions = new Description[0];

            _nodes.Add(new Description($"Partition By {string.Join(", ", descriptions.Select((_, i) => "{" + i + "}"))}", descriptions));

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window OrderBy(Description description)
        {
            _nodes.Add(new Description("Order By {0} Asc", description));

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window OrderByDescending(Description description)
        {
            _nodes.Add(new Description("Order By {0} Desc", description));

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window ThenBy(Description description)
        {
            _nodes.Add(new Description(", {0} Asc", description));

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window ThenByDescending(Description description)
        {
            _nodes.Add(new Description(", {0} Desc", description));

            return this;
        }

        /// <summary>
        /// 行
        /// </summary>
        /// <returns></returns>
        public Window Rows()
        {
            _nodes.Add(new Description(nameof(Rows)));

            return this;
        }

        /// <summary>
        /// 范围
        /// </summary>
        /// <returns></returns>
        public Window Range()
        {
            _nodes.Add(new Description(nameof(Range)));

            return this;
        }

        /// <summary>
        /// Between
        /// </summary>
        /// <returns></returns>
        public Window Between()
        {
            _nodes.Add(new Description(nameof(Between)));

            return this;
        }

        /// <summary>
        /// And
        /// </summary>
        /// <returns></returns>
        public Window And()
        {
            _nodes.Add(new Description(nameof(And)));

            return this;
        }

        /// <summary>
        /// 当前行
        /// </summary>
        /// <returns></returns>
        public Window CurrentRow()
        {
            _nodes.Add(new Description("Current Row"));

            return this;
        }

        /// <summary>
        /// 第一行作为开始
        /// </summary>
        /// <returns></returns>
        public Window UnboundedPreceding()
        {
            _nodes.Add(new Description("Unbounded Preceding"));

            return this;
        }

        /// <summary>
        /// 最后一行作为结束
        /// </summary>
        /// <returns></returns>
        public Window UnboundedFollowing()
        {
            _nodes.Add(new Description("Unbounded Following"));

            return this;
        }

        /// <summary>
        /// 向前
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window Preceding(Description description)
        {
            _nodes.Add(new Description("{0} Preceding", description));

            return this;
        }

        /// <summary>
        /// 向前
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Window Preceding(int count)
        {
            _nodes.Add(new Description($"{count} Preceding"));

            return this;
        }

        /// <summary>
        /// 向后
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window Following(Description description)
        {
            _nodes.Add(new Description("{0} Following", description));

            return this;
        }

        /// <summary>
        /// 向后
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Window Following(int count)
        {
            _nodes.Add(new Description($"{count} Following"));

            return this;
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        internal (string, IEnumerable<KeyValuePair<string, object>>) Build()
        {
            return new Description(string.Join(" ", _nodes.Select((_, i) => "{" + i + "}")), _nodes.ToArray()).Build();
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
