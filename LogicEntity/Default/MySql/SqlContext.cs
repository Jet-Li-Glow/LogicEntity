using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal record SqlContext
    {
        public SqlContext(int level)
        {
            Level = level;
        }

        public int Level { get; init; }

        public Dictionary<ParameterExpression, LambdaParameterInfo> Parameters { get; init; }

        public bool GetTableExpression { get; init; } = false;
    }
}
