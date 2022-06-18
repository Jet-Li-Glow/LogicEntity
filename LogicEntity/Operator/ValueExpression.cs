using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.Grammar;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 字符串描述
    /// </summary>
    public class ValueExpression : IValueExpression, ISqlExpression
    {
        /// <summary>
        /// 生成
        /// </summary>
        Func<(string, IEnumerable<KeyValuePair<string, object>>)> _build;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ValueExpression()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="args"></param>
        public ValueExpression(string command, params object[] objects)
        {
            _build = () =>
            {
                List<KeyValuePair<string, object>> parameters = new();

                List<string> formatArgs = new();

                if (objects is not null)
                {
                    foreach (object obj in objects)
                    {
                        if (obj is IValueExpression valueExpression)
                        {
                            if (valueExpression is IDbOperator dbOperator)
                                dbOperator.__Indent = 4;

                            (var cmd, var ps) = valueExpression.Build();

                            if (valueExpression is ISelector)
                                cmd = $"(\n{cmd}\n  )";

                            formatArgs.Add(cmd);

                            if (ps is not null)
                                parameters.AddRange(ps);

                            continue;
                        }

                        string key = ToolService.UniqueName();

                        formatArgs.Add(key);

                        parameters.Add(KeyValuePair.Create(key, obj));
                    }
                }

                return (string.Format(command, formatArgs.ToArray()), parameters);
            };
        }

        /// <summary>
        /// 是否包含逻辑操作符
        /// </summary>
        public bool HasLogicalOperator { get; private set; } = false;

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        (string, IEnumerable<KeyValuePair<string, object>>) ISqlExpression.Build()
        {
            return _build?.Invoke() ?? (null, null);
        }

        /// <summary>
        /// 加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator +(ValueExpression left, object right)
        {
            return new ValueExpression("{0} + {1}", left, right);
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator -(ValueExpression left, object right)
        {
            return new ValueExpression("{0} - {1}", left, right);
        }

        /// <summary>
        /// 乘
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator *(ValueExpression left, object right)
        {
            return new ValueExpression("{0} * {1}", left, right);
        }

        /// <summary>
        /// 除
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator /(ValueExpression left, object right)
        {
            return new ValueExpression("{0} / {1}", left, right);
        }

        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator ==(ValueExpression left, object right)
        {
            if (right is null)
                return new ValueExpression("{0} Is " + SqlValue.Null, left);

            return new ValueExpression("{0} = {1}", left, right);
        }

        /// <summary>
        /// 不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator !=(ValueExpression left, object right)
        {
            if (right is null)
                return new ValueExpression("{0} Is Not " + SqlValue.Null, left);

            return new ValueExpression("{0} != {1}", left, right);
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator >(ValueExpression left, object right)
        {
            return new ValueExpression("{0} > {1}", left, right);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator <(ValueExpression left, object right)
        {
            return new ValueExpression("{0} < {1}", left, right);
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator >=(ValueExpression left, object right)
        {
            return new ValueExpression("{0} >= {1}", left, right);
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator <=(ValueExpression left, object right)
        {
            return new ValueExpression("{0} <= {1}", left, right);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator &(ValueExpression left, object right)
        {
            return LogicalValueExpression(left, LogicalOperator.And, right);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator &(bool left, ValueExpression right)
        {
            return LogicalValueExpression(left, LogicalOperator.And, right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator |(ValueExpression left, object right)
        {
            return LogicalValueExpression(left, LogicalOperator.Or, right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ValueExpression operator |(bool left, ValueExpression right)
        {
            return LogicalValueExpression(left, LogicalOperator.Or, right);
        }

        /// <summary>
        /// 逻辑表达式
        /// </summary>
        /// <param name="left"></param>
        /// <param name="logicalOperator"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static ValueExpression LogicalValueExpression(object left, LogicalOperator logicalOperator, object right)
        {
            int index = 0;

            List<object> parameters = new();

            string leftStr;

            if (left is bool)
            {
                leftStr = left.ToString();
            }
            else
            {
                leftStr = "{" + index + "}";

                index++;

                parameters.Add(left);
            }

            string rightStr;

            if (right is bool)
            {
                rightStr = right.ToString();
            }
            else
            {
                rightStr = "{" + index + "}";

                if (right is ValueExpression valueExpression && valueExpression.HasLogicalOperator)
                    rightStr = "(" + rightStr + ")";

                index++;

                parameters.Add(right);
            }

            ValueExpression result = new ValueExpression($"{leftStr}\n{logicalOperator.ToString().PadLeft(5)} {rightStr}", parameters.ToArray());

            result.HasLogicalOperator = true;

            return result;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
