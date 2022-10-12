using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.ExpressionVisit
{
    internal class ColumnVisitor : ExpressionVisitor
    {
        static MethodInfo _Values = typeof(DbFunction).GetMethod(nameof(DbFunction.Values), BindingFlags.Static | BindingFlags.NonPublic);

        ParameterExpression _parameterExpression;

        public ColumnVisitor(ParameterExpression parameterExpression)
        {
            _parameterExpression = parameterExpression;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == _parameterExpression)
                return Expression.Call(null, _Values.MakeGenericMethod(node.Type), node);

            return node;
        }
    }
}
