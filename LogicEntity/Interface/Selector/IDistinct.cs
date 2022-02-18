using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Interface
{
    public interface IDistinct : IFrom
    {
        /// <summary>
        /// 取唯一的结果
        /// </summary>
        /// <returns></returns>
        public IFrom Distinct();
    }
}
