using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 条件集合
    /// </summary>
    public class ConditionCollection : IValueExpression
    {
        /// <summary>
        /// 条件集合
        /// </summary>
        List<IValueExpression> _conditions = new();

        /// <summary>
        /// 逻辑操作符
        /// </summary>
        public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.And;

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        public void Add(IValueExpression condition)
        {
            _conditions.Add(condition);
        }

        (string, IEnumerable<KeyValuePair<string, object>>) ISqlExpression.Build()
        {
            return (new ValueExpression(string.Join($"\n   {LogicalOperator} ", _conditions.Select((condition, i) =>
                {
                    string str = "{" + i + "}";

                    if (condition.HasLogicalOperator)
                        str = "(" + str + ")";

                    return str;
                })),
                _conditions.ToArray()
                ) as IValueExpression).Build();
        }
    }
}
