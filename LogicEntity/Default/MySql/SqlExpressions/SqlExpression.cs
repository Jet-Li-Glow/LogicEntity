using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal abstract class SqlExpression : ISqlExpression
    {
        public Type Type { get; set; }

        public static EmptyExpression Empty { get; } = new();

        public static RawSqlExpression DefaultValue { get; } = new RawSqlExpression(SqlNode.Default);

        public static RawSqlExpression AllColumns()
        {
            return new("*");
        }

        public static RawSqlExpression CountExpression()
        {
            return new("Count(*)");
        }

        public static IValueExpression AndAlso(params IValueExpression[] valueExpressions)
        {
            AndAlsoExpression expression = new(valueExpressions[0], valueExpressions[1]);

            for (int i = 2; i < valueExpressions.Length; i++)
            {
                expression = new(expression, valueExpressions[i]);
            }

            return expression;
        }

        public static bool NeedLeftBracket(SqlOperator? left, SqlOperator right)
        {
            if (left is null)
                return false;

            return (int)left.Value > (int)right;
        }

        public static bool NeedRightBracket(SqlOperator left, SqlOperator? right)
        {
            if (right is null)
                return false;

            return (int)right.Value >= (int)left;
        }
    }
}
