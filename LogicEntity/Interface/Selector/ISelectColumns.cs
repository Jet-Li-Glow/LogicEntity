using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface ISelectColumns : IFrom
    {
        public IFrom SetColumns(params IEnumerable<Column>[] columns)
        {
            return SetColumns(columns.SelectMany(c => c).ToArray());
        }

        public IFrom SetColumns(params Column[] columns);
    }
}
