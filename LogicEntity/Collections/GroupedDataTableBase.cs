using LogicEntity.Collections.Generic;
using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Collections
{
    public class GroupedDataTableBase : IGroupedDataTable
    {
        public GroupedDataTableBase(AbstractDataBase db, TableExpression expression)
        {
            Db = db;

            Expression = expression;
        }

        public AbstractDataBase Db { get; private set; }

        public TableExpression Expression { get; private set; }
    }
}
