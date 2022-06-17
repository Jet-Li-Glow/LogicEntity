using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    /// <summary>
    /// Sql表达式
    /// </summary>
    public interface ISqlExpression
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        internal (string, IEnumerable<KeyValuePair<string, object>>) Build();
    }
}
