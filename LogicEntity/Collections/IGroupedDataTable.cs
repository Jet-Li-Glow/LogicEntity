using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Collections
{
    public interface IGroupedDataTable
    {
        AbstractDataBase Db { get; }

        TableExpression Expression { get; }
    }
}
