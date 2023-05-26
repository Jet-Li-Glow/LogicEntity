using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal record SqlContext
    {
        public Dictionary<System.Linq.Expressions.ParameterExpression, LambdaParameterInfo> LambdaParameters { get; init; }

        public SqlContext ConcatParameters(IEnumerable<KeyValuePair<System.Linq.Expressions.ParameterExpression, LambdaParameterInfo>> parameters)
        {
            return this with { LambdaParameters = new(LambdaParameters is null ? parameters : LambdaParameters.Concat(parameters)) };
        }
    }
}
