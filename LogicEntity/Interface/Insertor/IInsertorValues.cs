using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface IInsertorValues
    {
        public IInsertor Row<T>(T row);

        public IInsertor Rows<T>(IEnumerable<T> rows);
    }
}
