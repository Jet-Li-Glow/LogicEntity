using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Json
{
    public class JsonPathAttribute : Attribute
    {
        public JsonPathAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }
}
