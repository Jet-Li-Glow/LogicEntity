using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class OffsetLimit
    {
        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = int.MaxValue;

        public string Build()
        {
            return (Offset > 0 ? (Offset + ", ") : null) + Limit;
        }
    }
}
