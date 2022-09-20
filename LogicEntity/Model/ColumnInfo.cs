using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Json;

namespace LogicEntity.Model
{
    internal class ColumnInfo
    {
        public int Index { get; set; }

        public MemberAccess[] ColumnName { get; set; }
    }
}
