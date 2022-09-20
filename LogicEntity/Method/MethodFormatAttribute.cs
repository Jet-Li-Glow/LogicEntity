using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Method
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MethodFormatAttribute : Attribute
    {
        public MethodFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; private set; }
    }
}
