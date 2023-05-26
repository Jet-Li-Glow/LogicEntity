using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class SelectSqlCommand : SqlCommand
    {
        public IReadOnlyCollection<MemberInfo> ColumnMembers { get; set; }

        public Dictionary<string, ConstructorInfo> Constructors { get; set; }

        public Dictionary<int, Delegate> Readers { get; set; }
    }
}
