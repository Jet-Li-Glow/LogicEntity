using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public class UnSupportedClientReaderException : Exception
    {
        public UnSupportedClientReaderException(Delegate reader) : base($"{reader} is not supported")
        {

        }
    }
}
