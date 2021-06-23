using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    /// <summary>
    /// 更新操作器
    /// </summary>
    public interface IChanger : IUpdater
    {
        public IUpdater On(Condition condition);
    }
}
