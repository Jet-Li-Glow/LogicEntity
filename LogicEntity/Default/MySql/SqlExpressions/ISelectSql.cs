using LogicEntity.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal interface ISelectSql : IDataManipulationSql, ITableExpression, IValueExpression, IValuesExpression, IInsertRowsExpression
    {
        SelectExpression AddJoin();

        OrderKeys OrderBy { get; set; }

        OffsetLimit Limit { get; set; }

        ISqlExpression SelectedObjectExpression { get; }

        SelectSqlCommand BuildSelect(BuildContext context);

        SqlCommand IValueExpression.BuildValue(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.SubQuery(BuildSelect(context).Text.Indent(2))
            };
        }

        SqlCommand IValuesExpression.BuildValues(BuildContext context)
        {
            return BuildValue(context);
        }

        InsertRowsSqlCommand IInsertRowsExpression.BuildRows(BuildContext context)
        {
            SelectSqlCommand selectSqlCommand = BuildSelect(context);

            return new()
            {
                Text = selectSqlCommand.Text,
                ColumnMembers = selectSqlCommand.ColumnMembers
            };
        }

        Command IDataManipulationSql.Build(LinqConvertProvider linqConvertProvider)
        {
            Command command = new();

            command.CommandTimeout = Timeout;

            CommandResult result = new()
            {
                Type = Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(IDataTable<>) ? Type.GetGenericArguments()[0] : Type
            };

            command.Results.Add(result);

            BuildContext context = new BuildContext()
            {
                Level = -1,
                LinqConvertProvider = linqConvertProvider,
            };

            SelectSqlCommand selectSqlCommand = BuildSelect(context);

            command.CommandText = selectSqlCommand.Text;

            command.Parameters.AddRange(context.SqlParameters);

            if (selectSqlCommand.Constructors is not null)
            {
                foreach (var s in selectSqlCommand.Constructors)
                {
                    result.Constructors[s.Key] = s.Value;
                }
            }

            if (selectSqlCommand.Readers is not null)
            {
                foreach (var s in selectSqlCommand.Readers)
                {
                    result.Readers[s.Key] = s.Value;
                }
            }

            return command;
        }
    }
}
