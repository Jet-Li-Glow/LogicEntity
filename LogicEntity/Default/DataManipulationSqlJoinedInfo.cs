using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default
{
    internal class DataManipulationSqlJoinedInfo
    {
        public string Join { get; set; }

        public object Table { get; set; }

        public LambdaExpression LambdaExpression { get; set; }
    }
}
