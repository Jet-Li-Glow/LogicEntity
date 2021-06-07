using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IUpdaterJoin<T> : IUpdaterSet<T> where T : Table
    {
        public IUpdaterOn<T> Join(TableDescription table);

        public IUpdaterOn<T> InnerJoin(TableDescription table);

        public IUpdaterOn<T> LeftJoin(TableDescription table);

        public IUpdaterOn<T> RightJoin(TableDescription table);

        public IUpdaterOn<T> FullJoin(TableDescription table);

        public IUpdaterOn<T> NaturalJoin(TableDescription table);
    }
}
