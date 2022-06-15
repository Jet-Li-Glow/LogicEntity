using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;

namespace LogicEntity.Operator
{
    public interface IValueExpression
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        internal (string, IEnumerable<KeyValuePair<string, object>>) Build();

        /// <summary>
        /// 加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator +(IValueExpression left, object right)
        {
            return new Description("{0} + {1}", left, right);
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator -(IValueExpression left, object right)
        {
            return new Description("{0} - {1}", left, right);
        }

        /// <summary>
        /// 乘
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator *(IValueExpression left, object right)
        {
            return new Description("{0} * {1}", left, right);
        }

        /// <summary>
        /// 除
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator /(IValueExpression left, object right)
        {
            return new Description("{0} / {1}", left, right);
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator >(IValueExpression left, object right)
        {
            return new Description("{0} > {1}", left, right);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator <(IValueExpression left, object right)
        {
            return new Description("{0} < {1}", left, right);
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator >=(IValueExpression left, object right)
        {
            return new Description("{0} >= {1}", left, right);
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator <=(IValueExpression left, object right)
        {
            return new Description("{0} <= {1}", left, right);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public IValueExpression In(IEnumerable vs)
        {
            if (vs is null)
                vs = Enumerable.Empty<object>();

            object[] objs = vs.OfType<object>().ToArray();

            return new Description("{0} In (" + string.Join(", ", objs.Select((_, i) => "{" + (i + 1) + "}")) + ")", new object[] { this }.Concat(objs).ToArray());
        }

        /// <summary>
        /// 枚举查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public IValueExpression In(params object[] vs)
        {
            return In(vs.AsEnumerable());
        }

        /// <summary>
        /// 枚举查找
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IValueExpression In(ISelector selector)
        {
            if (selector is not null)
                selector.Indent = 4;

            return new Description("{0} In\n  (\n{1}\n  )", this, selector);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValueExpression Between(object left, object right)
        {
            return new Description("{0} Between {1} And {2}", this, left, right);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValueExpression NotBetween(object left, object right)
        {
            return new Description("{0} Not Between {1} And {2}", this, left, right);
        }

        /// <summary>
        /// 相似
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IValueExpression Like(string str)
        {
            return new Description("{0} Like {1}", this, str);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator &(IValueExpression left, object right)
        {
            return Description.LogicalDescription(left, "And", right);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator &(bool left, IValueExpression right)
        {
            return Description.LogicalDescription(left, "And", right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator |(IValueExpression left, object right)
        {
            return Description.LogicalDescription(left, "Or", right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator |(bool left, IValueExpression right)
        {
            return Description.LogicalDescription(left, "Or", right);
        }
    }
}
