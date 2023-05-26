using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class ColumnInfo
    {
        string _alias;

        public bool HasAlias { get; private set; } = false;

        public string Alias
        {
            get
            {
                return _alias;
            }

            set
            {
                _alias = value;

                HasAlias = true;
            }
        }

        public IValueExpression ValueExpression { get; set; }

        public MemberInfo Member { get; set; }

        public Delegate Reader { get; set; }
    }
}
