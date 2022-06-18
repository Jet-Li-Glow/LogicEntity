using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Grammar
{
    public interface IOnDuplicateKeyUpdate<T> : IInsertor where T : Table, new()
    {
        public IInsertor OnDuplicateKeyUpdate(Action<T> setValue);

        public IInsertor OnDuplicateKeyUpdate(Action<T, T> setValueWithRow);
    }
}
