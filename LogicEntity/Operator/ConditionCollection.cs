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
    public class ConditionCollection
    {
        /// <summary>
        /// 条件集合
        /// </summary>
        List<Description> _conditions = new();

        /// <summary>
        /// 逻辑操作符
        /// </summary>
        public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.And;

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        public void Add(Description condition)
        {
            _conditions.Add(condition);
        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="conditionCollection"></param>
        public static implicit operator Description(ConditionCollection conditionCollection)
        {
            return new Description(
                string.Join($"\n   {conditionCollection.LogicalOperator} ", conditionCollection._conditions.Select((condition, i) =>
                {
                    string str = "{" + i + "}";

                    if (condition.HasLogicalOperator)
                        str = "(" + str + ")";

                    return str;
                })),
                conditionCollection._conditions.ToArray()
                );
        }
    }
}
