using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal class TableInfo
    {
        public string CommandText { get; init; }

        public TableType TableType { get; init; }
    }
}
