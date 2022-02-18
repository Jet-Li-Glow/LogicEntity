using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface ISelect
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public IDistinct Select(params Description[] columnDescriptions);
    }
}
