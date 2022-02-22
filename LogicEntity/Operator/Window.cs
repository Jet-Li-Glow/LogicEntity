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
        /// 转换函数
        /// </summary>
        List<Func<string, string>> _beforeConvertors = new();

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

            _beforeConvertors.Add(s => s + $"Partition By {string.Join<Description>(", ", descriptions)}");

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window OrderBy(Description description)
        {
            _beforeConvertors.Add(s => s + $" Order By {description}");

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window OrderByDescending(Description description)
        {
            _beforeConvertors.Add(s => s + $" Order By {description} Desc");

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window ThenBy(Description description)
        {
            _beforeConvertors.Add(s => s + $", {description}");

            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window ThenByDescending(Description description)
        {
            _beforeConvertors.Add(s => s + $", {description} Desc");

            return this;
        }

        /// <summary>
        /// 行
        /// </summary>
        /// <returns></returns>
        public Window Rows()
        {
            _beforeConvertors.Add(s => s + " Rows");

            return this;
        }

        /// <summary>
        /// 范围
        /// </summary>
        /// <returns></returns>
        public Window Range()
        {
            _beforeConvertors.Add(s => s + " Range");

            return this;
        }

        /// <summary>
        /// Between
        /// </summary>
        /// <returns></returns>
        public Window Between()
        {
            _beforeConvertors.Add(s => s + " Between");

            return this;
        }

        /// <summary>
        /// And
        /// </summary>
        /// <returns></returns>
        public Window And()
        {
            _beforeConvertors.Add(s => s + " And");

            return this;
        }

        /// <summary>
        /// 当前行
        /// </summary>
        /// <returns></returns>
        public Window CurrentRow()
        {
            _beforeConvertors.Add(s => s + " Current Row");

            return this;
        }

        /// <summary>
        /// 第一行作为开始
        /// </summary>
        /// <returns></returns>
        public Window UnboundedPreceding()
        {
            _beforeConvertors.Add(s => s + " Unbounded Preceding");

            return this;
        }

        /// <summary>
        /// 最后一行作为结束
        /// </summary>
        /// <returns></returns>
        public Window UnboundedFollowing()
        {
            _beforeConvertors.Add(s => s + " Unbounded Following");

            return this;
        }

        /// <summary>
        /// 向前
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window Preceding(Description description)
        {
            _beforeConvertors.Add(s => s + $" {description} Preceding");

            return this;
        }

        /// <summary>
        /// 向后
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Window Following(Description description)
        {
            _beforeConvertors.Add(s => s + $" {description} Following");

            return this;
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = string.Empty;

            foreach (Func<string, string> convert in _beforeConvertors)
            {
                if (convert is null)
                    continue;

                result = convert(result);
            }

            return result;
        }
    }
}
