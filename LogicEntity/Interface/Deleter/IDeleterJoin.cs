using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IDeleterJoin : IDeleterWhere
    {
        public IDeleterOn Join(TableDescription table);

        public IDeleterOn InnerJoin(TableDescription table);

        public IDeleterOn LeftJoin(TableDescription table);

        public IDeleterOn RightJoin(TableDescription table);

        public IDeleterOn FullJoin(TableDescription table);

        public IDeleterOn NaturalJoin(TableDescription table);
    }
}
