using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IHaving : IWindow
    {
        /// <summary>
        /// 分组筛选
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IWindow Having(IValueExpression condition);
    }
}
