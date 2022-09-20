using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public interface IValue
    {
        public bool ValueSetted { get; }

        public ValueType ValueType { get; }

        public object Object { get; }
    }
}
