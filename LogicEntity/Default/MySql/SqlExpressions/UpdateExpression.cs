using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class UpdateExpression : SqlExpression, IDataManipulationSql
    {
        public UpdateExpression(ITableExpression from, IValueExpression where, OrderKeys orderBy, OffsetLimit limit, int? timeout)
        {
            From = from;

            Where = where;

            OrderBy = orderBy;

            Limit = limit;

            Timeout = timeout;
        }

        public int? Timeout { get; set; }

        public List<CommonTableExpression> CommonTableExpressions { get; } = new();

        public List<AssignmentExpression> Assignments { get; } = new();

        public ITableExpression From { get; private set; }

        public IValueExpression Where { get; private set; }

        public OrderKeys OrderBy { get; private set; }

        public OffsetLimit Limit { get; private set; }

        public Command Build(LinqConvertProvider linqConvertProvider)
        {
            Command command = new Command();

            BuildContext context = new BuildContext()
            {
                Level = 0,
                DataManipulationSql = this,
                LinqConvertProvider = linqConvertProvider
            };

            command.CommandText = "Update";

            command.CommandText += "\n" + From.BuildFromNode(context, 0).Text.Indent(2);

            command.CommandText += "\nSet\n" + string.Join(",\n", Assignments.Select(s => s.BuildValue(context).Text)).Indent(2);

            if (Where is not null)
                command.CommandText += "\nWhere\n" + Where.BuildValue(context).Text.Indent(2);

            if (OrderBy is not null)
                command.CommandText += "\nOrder By\n  " + OrderBy.Build(context);

            if (Limit is not null)
                command.CommandText += "\nLimit\n  " + Limit.Build();

            if (CommonTableExpressions.Count > 0)
            {
                string with = "With";

                if (CommonTableExpressions.Any(s => s.IsRecursive))
                    with += " Recursive";

                with += "\n" + string.Join(",\n", CommonTableExpressions.Select(s => s.BuildCTE(context).Text)).Indent(2) + "\n";

                command.CommandText = with + command.CommandText;
            }

            command.Parameters.AddRange(context.SqlParameters);

            command.CommandTimeout = Timeout;

            return command;
        }
    }
}
