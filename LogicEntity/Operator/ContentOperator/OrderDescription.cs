using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 排序描述
    /// </summary>
    internal class OrderDescription
    {
        private Description _description;

        private bool _isAscending;

        /// <summary>
        /// 排序描述
        /// </summary>
        /// <param name="description">列</param>
        /// <param name="isAscending">是否为升序</param>
        public OrderDescription(Description description, bool isAscending)
        {
            _description = description;
            _isAscending = isAscending;
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        public string Description()
        {
            return _description?.ToString() + (_isAscending ? string.Empty : " Desc");
        }
    }
}
