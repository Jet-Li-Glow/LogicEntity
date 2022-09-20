using LogicEntity.Linq.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public abstract class DataTableImplBase : IDataTable
    {
        public DataTableImplBase(AbstractDataBase db, TableExpression expression)
        {
            Db = db;

            Expression = expression;
        }

        public AbstractDataBase Db { get; private set; }

        public TableExpression Expression { get; private set; }
    }
}
