using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 值表达式
    /// </summary>
    public interface IValueExpression : ISqlExpression, IEnumerable<Column>
    {
        /// <summary>
        /// 枚举
        /// </summary>
        /// <returns></returns>
        IEnumerator<Column> IEnumerable<Column>.GetEnumerator()
        {
            yield return new Column(this);
        }

        /// <summary>
        /// 枚举
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 是否包含逻辑操作符
        /// </summary>
        public bool HasLogicalOperator => false;

        /// <summary>
        /// 加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator +(IValueExpression left, object right)
        {
            return new ValueExpression("{0} + {1}", left, right);
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator -(IValueExpression left, object right)
        {
            return new ValueExpression("{0} - {1}", left, right);
        }

        /// <summary>
        /// 乘
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator *(IValueExpression left, object right)
        {
            return new ValueExpression("{0} * {1}", left, right);
        }

        /// <summary>
        /// 除
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator /(IValueExpression left, object right)
        {
            return new ValueExpression("{0} / {1}", left, right);
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator >(IValueExpression left, object right)
        {
            return new ValueExpression("{0} > {1}", left, right);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator <(IValueExpression left, object right)
        {
            return new ValueExpression("{0} < {1}", left, right);
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator >=(IValueExpression left, object right)
        {
            return new ValueExpression("{0} >= {1}", left, right);
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator <=(IValueExpression left, object right)
        {
            return new ValueExpression("{0} <= {1}", left, right);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator &(IValueExpression left, object right)
        {
            return ValueExpression.LogicalValueExpression(left, LogicalOperator.And, right);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator &(bool left, IValueExpression right)
        {
            return ValueExpression.LogicalValueExpression(left, LogicalOperator.And, right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator |(IValueExpression left, object right)
        {
            return ValueExpression.LogicalValueExpression(left, LogicalOperator.Or, right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression operator |(bool left, IValueExpression right)
        {
            return ValueExpression.LogicalValueExpression(left, LogicalOperator.Or, right);
        }
    }
}
