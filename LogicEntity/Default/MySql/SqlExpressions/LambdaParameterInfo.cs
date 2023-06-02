using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class LambdaParameterInfo
    {
        public static LambdaParameterInfo Table(ITableExpression tableExpression)
        {
            return new()
            {
                ParameterType = LambdaParameterType.Table,
                SqlExpression = tableExpression
            };
        }

        public static LambdaParameterInfo IndexColumn(IndexColumnExpression indexColumnExpression)
        {
            return new()
            {
                ParameterType = LambdaParameterType.IndexColumn,
                SqlExpression = indexColumnExpression
            };
        }

        public static LambdaParameterInfo ColumnIndexValue { get; } = new()
        {
            ParameterType = LambdaParameterType.ColumnIndexValue,
            SqlExpression = new RawSqlExpression(SqlNode.ColumnIndexValue)
        };

        public static LambdaParameterInfo Value(IValueExpression valueExpression)
        {
            return new()
            {
                ParameterType = LambdaParameterType.Value,
                SqlExpression = valueExpression
            };
        }

        public static LambdaParameterInfo GroupingDataTable(ITableExpression from, Dictionary<MemberInfo, IValueExpression> members)
        {
            return new()
            {
                ParameterType = LambdaParameterType.GroupingDataTable,
                SqlExpression = new GroupingDataTableExpression(from, members)
            };
        }

        public static LambdaParameterInfo LambdaParameter(ITableExpression tableExpression)
        {
            if (tableExpression is ISelectSql subQuery && subQuery.IsVector.Value == false)
            {
                return Value(new ColumnExpression(subQuery, subQuery.Columns[0].Alias));
            }

            return Table(tableExpression);
        }

        public LambdaParameterType ParameterType { get; private set; }

        public ISqlExpression SqlExpression { get; private set; }
    }
}
