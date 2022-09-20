using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;

namespace LogicEntity
{
    public interface ILinqConvertProvider
    {
        Command Convert(Expression expression);
    }
}
