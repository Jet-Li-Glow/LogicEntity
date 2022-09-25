using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Default.MySql
{
    /// <summary>
    /// Lambda表达式参数信息
    /// </summary>
    internal record LambdaParameterInfo
    {
        public static LambdaParameterInfo Entity(EntitySource entitySource)
        {
            return new()
            {
                ParameterType = LambdaParameterType.Entity,
                EntitySource = entitySource
            };
        }

        public static LambdaParameterInfo Entity(EntityInfo entityInfo)
        {
            return new()
            {
                ParameterType = LambdaParameterType.Entity,
                CommandText = entityInfo.CommandText,
                EntitySource = entityInfo.EntitySource
            };
        }

        public static LambdaParameterInfo ColumnIndexValue { get; } = new()
        {
            ParameterType = LambdaParameterType.ColumnIndexValue,
            CommandText = SqlNode.ColumnIndexValue
        };

        public static LambdaParameterInfo IndexColumnName { get; } = new()
        {
            ParameterType = LambdaParameterType.IndexColumnName,
            CommandText = SqlNode.IndexColumnName
        };

        public static LambdaParameterInfo GroupingDataTable(Dictionary<MemberInfo, string> groupKeys, List<EntityInfo> fromTables)
        {
            return new()
            {
                ParameterType = LambdaParameterType.GroupingDataTable,
                GroupKeys = groupKeys,
                FromTables = fromTables
            };
        }

        public static LambdaParameterInfo DataTable(TableExpression tableExpression)
        {
            return new()
            {
                ParameterType = LambdaParameterType.DataTable,
                TableExpression = tableExpression
            };
        }

        public LambdaParameterType ParameterType { get; init; }

        public string CommandText { get; init; }

        public EntitySource? EntitySource { get; init; }

        public Dictionary<MemberInfo, string> GroupKeys { get; init; }

        public List<EntityInfo> FromTables { get; init; }

        public TableExpression TableExpression { get; init; }
    }
}
