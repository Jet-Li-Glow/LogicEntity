using LogicEntity.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal abstract class SelectSql : SqlExpression
    {
        public int? Timeout { get; set; }

        public List<CommonTableExpression> CommonTableExpressions { get; } = new();

        string _alisas;

        public string Alias
        {
            get
            {
                return _alisas;
            }

            set
            {
                _alisas = value;

                HasAlias = true;
            }
        }

        public bool HasAlias { get; private set; } = false;

        protected Type GetResultType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDataTable<>))
                return type.GetGenericArguments()[0];

            return type;
        }
    }
}
