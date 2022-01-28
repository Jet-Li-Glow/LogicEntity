using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
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
        internal TableTier TableTier { get; private set; }

        /// <summary>
        /// 关联表
        /// </summary>
        internal TableDescription Table { get; private set; }

        /// <summary>
        /// 关联条件
        /// </summary>
        private ConditionDescription _condition;

        /// <summary>
        /// 是否设置条件
        /// </summary>
        private bool _hasCondition;

        public Relation(TableDescription table, TableTier tableTier)
        {
            Table = table;

            if (table is not null)
                _parameters.AddRange(Table.GetParameters());

            TableTier = tableTier;
        }

        /// <summary>
        /// 设置关联条件
        /// </summary>
        /// <param name="condition"></param>
        public void SetCondition(ConditionDescription condition)
        {
            _hasCondition = true;

            _condition = condition;

            if (condition is not null)
                _parameters.AddRange(condition.GetParameters());
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TableTier.Description() + " " + Table + (_hasCondition ? "\n   On " + _condition : string.Empty);
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
