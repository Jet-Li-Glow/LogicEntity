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
        public TableTier TableTier { get; set; }

        /// <summary>
        /// 关联表
        /// </summary>
        public TableDescription RelateTable { get; set; }

        /// <summary>
        /// 关联条件
        /// </summary>
        public Condition Condition { get; set; }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameters"></param>
        public void AddParameters(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            if (parameters is not null)
                _parameters.AddRange(parameters);
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        internal string Description()
        {
            return TableTier.Description() + " " + RelateTable?.Description() + "\n  On " + Condition.Description();
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
