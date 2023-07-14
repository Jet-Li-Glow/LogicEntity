using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class JoinedTableExpression : SqlExpression, ITableExpression
    {
        public bool HasAlias => false;

        public string Alias { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public JoinedTableExpression(ITableExpression table)
        {
            Table = table;
        }

        public ITableExpression Table { get; private set; }

        public List<JoinedTable> JoinedTables { get; } = new();

        public int Count => JoinedTables.Count + 1;

        public IList<ColumnInfo> Columns => throw new NotImplementedException();

        public OrderKeys OrderBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public OffsetLimit Limit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsVector => throw new NotImplementedException();

        public int? Timeout { get; set; }

        public List<CommonTableExpression> CommonTableExpressions => throw new NotImplementedException();

        public bool CanAddNode(SelectNodeType nodeType)
        {
            return true;
        }

        public SelectExpression AddSelect()
        {
            return new SelectExpression(this);
        }

        public JoinedTableExpression AddJoin()
        {
            return this;
        }

        public SelectExpression Distinct()
        {
            return new SelectExpression(this).Distinct();
        }

        public SelectExpression AddIndex()
        {
            return new SelectExpression(this).AddIndex();
        }

        public SelectExpression AddWhere()
        {
            return new SelectExpression(this).AddWhere();
        }

        public SelectExpression AddAggregateFunction()
        {
            return AddSelect();
        }

        public SelectExpression AddGroupBy()
        {
            return new SelectExpression(this).AddGroupBy();
        }

        public SelectExpression AddHaving()
        {
            throw new NotImplementedException();
        }

        public ITableExpression AddOrderBy()
        {
            return new SelectExpression(this).AddOrderBy();
        }

        public ITableExpression AddThenBy()
        {
            throw new NotImplementedException();
        }

        public ITableExpression AddLimit()
        {
            return new SelectExpression(this).AddLimit();
        }

        public ISqlExpression[] GetOrderByParameters()
        {
            throw new NotImplementedException();
        }

        public DeleteExpression AddDelete()
        {
            return new SelectExpression(this).AddDelete();
        }

        public UpdateExpression AddUpdateSet()
        {
            return new SelectExpression(this).AddUpdateSet();
        }

        public SelectSqlCommand BuildSelect(BuildContext context)
        {
            throw new NotImplementedException();
        }

        public SqlCommand BuildTableDefinition(BuildContext context)
        {
            List<string> tableCommands = new()
            {
                Table.BuildFromNode(context, 0).Text
            };

            for (int i = 0; i < JoinedTables.Count; i++)
            {
                var joinedTableInfo = JoinedTables[i];

                string command = joinedTableInfo.Join + " " + joinedTableInfo.TableExpression.BuildFromNode(context, i + 1).Text;

                if (joinedTableInfo.Predicate is not null)
                    command += " On " + joinedTableInfo.Predicate.BuildValue(context).Text;

                tableCommands.Add(command);
            }

            return new()
            {
                Text = string.Join("\n", tableCommands)
            };
        }

        public SqlCommand BuildFromNode(BuildContext context, int index)
        {
            return BuildTableDefinition(context);
        }

        public ITableExpression GetTable(int i)
        {
            if (i == 0)
                return Table;

            return JoinedTables[i - 1].TableExpression;
        }

        public class JoinedTable
        {
            public string Join { get; set; }

            public ITableExpression TableExpression { get; set; }

            public IValueExpression Predicate { get; set; }
        }
    }
}
