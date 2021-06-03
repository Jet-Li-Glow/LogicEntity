using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Extension;
using LogicEntity.Operator;
using LogicEntity.EnumCollection;

namespace LogicEntity.Model
{
    /// <summary>
    /// 关联关系
    /// </summary>
    internal class Relation
    {
        /// <summary>
        /// 参数
        /// </summary>
        private List<KeyValuePair<string, object>> _parameters = new();

        /// <summary>
        /// 表级别
        /// </summary>
        internal TableTier TableTier { get; set; }

        /// <summary>
        /// 关联表
        /// </summary>
        private TableDescription RelateTable { get; set; }

        /// <summary>
        /// 关联条件
        /// </summary>
        private ConditionDescription Condition { get; set; }

        /// <summary>
        /// 设置关联表
        /// </summary>
        /// <param name="table"></param>
        public void SetTable(TableDescription table)
        {
            RelateTable = table;

            if (table is not null)
                _parameters.AddRange(table.GetParameters());
        }

        /// <summary>
        /// 设置关联条件
        /// </summary>
        /// <param name="condition"></param>
        public void SetCondition(ConditionDescription condition)
        {
            Condition = condition;

            if (condition is not null)
                _parameters.AddRange(condition.GetParameters());
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        internal string Description()
        {
            return TableTier.Description() + " " + RelateTable?.Description() + "\n   On " + Condition.Description();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<KeyValuePair<string, object>> GetParameters()
        {
            return _parameters.Select(s => KeyValuePair.Create(s.Key, s.Value));
        }
    }
}
