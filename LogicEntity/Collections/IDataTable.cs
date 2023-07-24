using LogicEntity.Linq.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Collections
{
    public interface IDataTable : IEnumerable
    {
        AbstractDataBase Db { get; }

        TableExpression Expression { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Db.Query(Expression).GetEnumerator();
        }
    }
}
