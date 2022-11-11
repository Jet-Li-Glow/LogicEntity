using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal class CommandExtend : Command
    {
        public List<PropertyInfo> ColumnProperties { get; set; }
    }
}
