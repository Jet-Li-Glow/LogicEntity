using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal class SelectSqlJoinedInfo
    {
        public string Join { get; set; }

        public TableExpression TableExpression { get; set; }

        public LambdaExpression Predicate { get; set; }
    }
}
