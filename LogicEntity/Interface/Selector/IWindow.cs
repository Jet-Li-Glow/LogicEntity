using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    /// <summary>
    /// 窗口
    /// </summary>
    public interface IWindow : IUnion
    {
        /// <summary>
        /// 窗口
        /// </summary>
        /// <param name="windows"></param>
        /// <returns></returns>
        public IUnion Window(params Window[] windows);
    }
}
