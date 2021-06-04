using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface IInsertorValues<T> where T : Table
    {
        public IOnDuplicateKeyUpdate<T> Row<TRow>(TRow row);

        public IOnDuplicateKeyUpdate<T> Rows<TRow>(IEnumerable<TRow> rows);

        public IOnDuplicateKeyUpdate<T> Rows(ISelector selector);
    }
}
