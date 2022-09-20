using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Json
{
    public class InvalidJsonPathException : Exception
    {
        public InvalidJsonPathException(string path) : base($"Invalid json path: {path}")
        {
        }
    }
}
