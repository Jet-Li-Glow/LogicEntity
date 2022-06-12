using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    /// <summary>
    /// CTE数据操作
    /// </summary>
    public interface IWith : IOperate
    {
        public IOperate With(bool isRecursive, params CommonTableExpression[] commonTableExpressions);
    }
}
