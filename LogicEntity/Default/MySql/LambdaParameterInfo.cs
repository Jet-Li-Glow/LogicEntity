using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    /// <summary>
    /// Lambda表达式参数信息
    /// </summary>
    internal record LambdaParameterInfo
    {
        public static LambdaParameterInfo Table(TableType tableType)
        {
            return new()
            {
                ParameterType = LambdaParameterType.Table,
                TableType = tableType
            };
        }

        public static LambdaParameterInfo Table(TableInfo tableInfo)
        {
            return new()
            {
                ParameterType = LambdaParameterType.Table,
                CommandText = tableInfo.CommandText,
                TableType = tableInfo.TableType
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

        public static LambdaParameterInfo GroupingDataTable(Dictionary<MemberInfo, string> groupKeys, List<TableInfo> fromTables)
        {
            return new()
            {
                ParameterType = LambdaParameterType.GroupingDataTable,
                GroupKeys = groupKeys,
                FromTables = fromTables
            };
        }

        public LambdaParameterType ParameterType { get; private set; }

        public string CommandText { get; private set; }

        public TableType? TableType { get; private set; }

        public Dictionary<MemberInfo, string> GroupKeys { get; private set; }

        public List<TableInfo> FromTables { get; private set; }
    }
}
