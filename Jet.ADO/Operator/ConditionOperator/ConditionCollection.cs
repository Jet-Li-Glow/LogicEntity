using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Extension;
using LogicEntity.EnumCollection;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 条件集合
    /// </summary>
    public class ConditionCollection : ConditionDescription
    {
        private List<Condition> Conditions = new List<Condition>();

        /// <summary>
        /// 逻辑操作符
        /// </summary>
        public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.And;

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        public void Add(Condition condition)
        {
            Conditions.Add(condition);
        }

        /// <summary>
        /// 获取条件集合字符串
        /// </summary>
        /// <returns></returns>
        internal override string Description()
        {
            return string.Join($" {LogicalOperator.Description()} ", Conditions.Select(c =>
            {
                if (c is null)
                    return string.Empty;

                return c.IsMultiple ? "(" + c.Description() + ")" : c.Description();
            }));
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<KeyValuePair<string, object>> GetParameters()
        {
            return Conditions.SelectMany(c => c?.GetParameters() ?? new List<KeyValuePair<string, object>>());
        }
    }
}
