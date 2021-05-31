using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IJoin : IWhere
    {
        public IOn Join(TableDescription table);

        public IOn InnerJoin(TableDescription table);

        public IOn LeftJoin(TableDescription table);

        public IOn RightJoin(TableDescription table);
    }
}
