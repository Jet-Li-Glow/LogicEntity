using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal enum SelectNodeType
    {
        From,
        Where,
        GroupBy,
        Select,
        Having,
        OrderBy,
        Limit
    }
}
