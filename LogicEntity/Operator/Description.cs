﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.Interface;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 字符串描述
    /// </summary>
    public class Description
    {
        /// <summary>
        /// 生成
        /// </summary>
        protected virtual Func<(string, IEnumerable<KeyValuePair<string, object>>)> _build { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Description()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="args"></param>
        public Description(string command, params object[] objects)
        {
            _build = () =>
            {
                List<KeyValuePair<string, object>> parameters = new();

                List<string> formatArgs = new();

                if (objects is not null)
                {
                    foreach (object obj in objects)
                    {
                        if (obj is null)
                        {
                            formatArgs.Add(string.Empty);

                            continue;
                        }

                        if (obj is Description description)
                        {
                            (var cmd, var ps) = description.Build();

                            formatArgs.Add(cmd);

                            if (ps is not null)
                                parameters.AddRange(ps);

                            continue;
                        }

                        if (obj is TableDescription table)
                        {
                            (var cmd, var ps) = table.Build();

                            formatArgs.Add(cmd);

                            if (ps is not null)
                                parameters.AddRange(ps);

                            continue;
                        }

                        if (obj is Window window)
                        {
                            (var cmd, var ps) = window.Build();

                            formatArgs.Add(cmd);

                            if (ps is not null)
                                parameters.AddRange(ps);

                            continue;
                        }

                        if (obj is IDbOperator dbOperator)
                        {
                            Command cmd = dbOperator.GetCommandWithUniqueParameterName();

                            if (cmd is not null)
                            {
                                formatArgs.Add(cmd.CommandText);

                                if (cmd.Parameters is not null)
                                    parameters.AddRange(cmd.Parameters);
                            }

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
        /// 构造函数
        /// </summary>
        /// <param name="convertor"></param>
        public Description(Func<string> content)
        {
            _build = () => (content(), null);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commonTableExpression"></param>
        public Description(CommonTableExpression commonTableExpression)
        {
            _build = commonTableExpression.BuildDefinition;
        }

        /// <summary>
        /// 是否包含逻辑操作符
        /// </summary>
        internal bool HasLogicalOperator { get; private set; } = false;

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        internal (string, IEnumerable<KeyValuePair<string, object>>) Build()
        {
            return _build?.Invoke() ?? (null, null);
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator +(Description left, object right)
        {
            return new Description("{0} + {1}", left, right);
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator -(Description left, object right)
        {
            return new Description("{0} - {1}", left, right);
        }

        /// <summary>
        /// 乘
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator *(Description left, object right)
        {
            return new Description("{0} * {1}", left, right);
        }

        /// <summary>
        /// 除
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator /(Description left, object right)
        {
            return new Description("{0} / {1}", left, right);
        }

        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator ==(Description left, object right)
        {
            return new Description("{0} = {1}", left, right);
        }

        /// <summary>
        /// 不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator !=(Description left, object right)
        {
            return new Description("{0} != {1}", left, right);
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator >(Description left, object right)
        {
            return new Description("{0} > {1}", left, right);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator <(Description left, object right)
        {
            return new Description("{0} < {1}", left, right);
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator >=(Description left, object right)
        {
            return new Description("{0} >= {1}", left, right);
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator <=(Description left, object right)
        {
            return new Description("{0} <= {1}", left, right);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public Description In(IEnumerable vs)
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
        public Description In(params object[] vs)
        {
            return In(vs.AsEnumerable());
        }

        /// <summary>
        /// 枚举查找
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Description In(IDbOperator dbOperator)
        {
            return new Description("{0} In\n\n(\n{1}\n)\n", this, dbOperator);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public Description Between(object left, object right)
        {
            return new Description("{0} Between {1} And {2}", this, left, right);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public Description NotBetween(object left, object right)
        {
            return new Description("{0} Not Between {1} And {2}", this, left, right);
        }

        /// <summary>
        /// 相似
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Description Like(string str)
        {
            return new Description("{0} Like {1}", this, str);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator &(Description left, object right)
        {
            return LogicalDescription(left, "And", right);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator &(bool left, Description right)
        {
            return LogicalDescription(left, "And", right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator |(Description left, object right)
        {
            return LogicalDescription(left, "Or", right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Description operator |(bool left, Description right)
        {
            return LogicalDescription(left, "Or", right);
        }

        /// <summary>
        /// 逻辑表达式
        /// </summary>
        /// <param name="left"></param>
        /// <param name="logicalOperator"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        static Description LogicalDescription(object left, string logicalOperator, object right)
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

                if (right is Description rightDescription && rightDescription.HasLogicalOperator)
                    rightStr = "(" + rightStr + ")";

                index++;

                parameters.Add(right);
            }

            Description result = new Description($"{leftStr}\n{logicalOperator.PadLeft(5)} {rightStr}", parameters.ToArray());

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
