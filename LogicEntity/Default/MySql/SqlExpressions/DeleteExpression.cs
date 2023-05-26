using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class DeleteExpression : SqlExpression, IDataManipulationSql
    {
        public DeleteExpression(ITableExpression from, IValueExpression where, OrderKeys orderBy, OffsetLimit limit, int? timeout)
        {
            From = from;

            Where = where;

            OrderBy = orderBy;

            Limit = limit;

            Timeout = timeout;
        }

        public int? Timeout { get; set; }

        public List<CommonTableExpression> CommonTableExpressions { get; } = new();

        public List<ITableExpression> DeletedTables { get; } = new();

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

            command.CommandText = "Delete";

            string from = "\nFrom\n" + From.BuildFromNode(context, 0).Text.Indent(2);

            string deleteTables = string.Empty;

            if (DeletedTables.Count > 0)
                deleteTables = " " + string.Join(", ", DeletedTables.Select(s => s.ShortName));

            command.CommandText += deleteTables + from;

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

        public bool CanAddNode(SelectNodeType nodeType)
        {
            throw new NotImplementedException();
        }

        public SelectExpression AddIndex()
        {
            throw new NotImplementedException();
        }

        public DeleteExpression AddDelete()
        {
            throw new NotImplementedException();
        }

        public SelectExpression Distinct()
        {
            throw new NotImplementedException();
        }

        public SelectExpression AddGroupBy()
        {
            throw new NotImplementedException();
        }

        public SelectExpression AddHaving()
        {
            throw new NotImplementedException();
        }

        public ISelectSql AddLimit()
        {
            throw new NotImplementedException();
        }

        public ISelectSql AddOrderBy()
        {
            throw new NotImplementedException();
        }

        public ISelectSql AddThenBy()
        {
            throw new NotImplementedException();
        }

        public SelectExpression AddSelect()
        {
            throw new NotImplementedException();
        }

        public UpdateExpression AddUpdateSet()
        {
            throw new NotImplementedException();
        }

        public SelectExpression AddWhere()
        {
            throw new NotImplementedException();
        }
    }
}
