using LogicEntity.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class SelectExpression : SelectSql, ISelectSql
    {
        HashSet<SelectNodeType> _nodes = new();

        public SelectExpression()
        {
        }

        public SelectExpression(ITableExpression table)
        {
            From = table;

            if (table is ISelectSql selectSql)
                Timeout = selectSql.Timeout;

            _nodes.Add(SelectNodeType.From);
        }

        public bool IsDistinct { get; set; } = false;

        public List<ColumnInfo> Columns { get; } = new();

        public bool HasIndex { get; private set; }

        public Dictionary<string, ConstructorInfo> Constructors { get; set; }

        public Dictionary<int, Delegate> Readers
        {
            get
            {
                Dictionary<int, Delegate> readers = new();

                for (int i = 0; i < Columns.Count; i++)
                {
                    ColumnInfo column = Columns[i];

                    if (column.Reader is not null)
                        readers[i] = column.Reader;
                }

                return readers;
            }
        }

        public ITableExpression From { get; private set; }

        public void AddJoinedTable(JoinedTableExpression.JoinedTable joinedTable)
        {
            if (From is not JoinedTableExpression)
                From = new JoinedTableExpression(From);

            ((JoinedTableExpression)From).JoinedTables.Add(joinedTable);
        }

        public IValueExpression Where { get; set; }

        public Dictionary<MemberInfo, IValueExpression> GroupBy { get; set; }

        public IValueExpression Having { get; set; }

        public OrderKeys OrderBy { get; set; }

        public OffsetLimit Limit { get; set; }

        ISqlExpression _selectedObjectExpression;

        public ISqlExpression SelectedObjectExpression
        {
            get
            {
                return _selectedObjectExpression ?? (From.Count == 1 ? From : null);
            }

            set
            {
                _selectedObjectExpression = value;
            }
        }

        public bool CanAddNode(SelectNodeType nodeType)
        {
            if (_nodes.Count == 0)
                return true;

            int max = _nodes.Max(n => (int)n);

            if (nodeType == SelectNodeType.Where
                || nodeType == SelectNodeType.Having
                || nodeType == SelectNodeType.OrderBy
                || nodeType == SelectNodeType.Limit
                )
            {
                return (int)nodeType >= max;
            }

            return (int)nodeType > max;
        }

        public SelectExpression AddSelect()
        {
            var selectExpression = this;

            if (selectExpression.CanAddNode(SelectNodeType.Select) == false)
            {
                selectExpression = new(this);
            }

            selectExpression._nodes.Add(SelectNodeType.Select);

            return selectExpression;
        }

        public SelectExpression Distinct()
        {
            IsDistinct = true;

            return this;
        }

        public SelectExpression AddIndex()
        {
            HasIndex = true;

            return this;
        }

        public SelectExpression AddJoin()
        {
            if (_nodes.SingleOrDefault() == SelectNodeType.From)
                return this;

            return new SelectExpression(this);
        }

        public SelectExpression AddWhere()
        {
            var selectExpression = this;

            if (selectExpression.CanAddNode(SelectNodeType.Where) == false)
            {
                selectExpression = new(this);
            }

            selectExpression._nodes.Add(SelectNodeType.Where);

            return selectExpression;
        }

        public SelectExpression AddGroupBy()
        {
            var selectExpression = this;

            if (selectExpression.CanAddNode(SelectNodeType.GroupBy) == false)
            {
                selectExpression = new(this);
            }

            selectExpression._nodes.Add(SelectNodeType.GroupBy);

            return selectExpression;
        }

        public SelectExpression AddHaving()
        {
            var selectExpression = this;

            if (selectExpression.CanAddNode(SelectNodeType.Having) == false)
            {
                selectExpression = new(this);
            }

            selectExpression._nodes.Add(SelectNodeType.Having);

            return selectExpression;
        }

        public ISelectSql AddOrderBy()
        {
            var selectExpression = this;

            if (selectExpression.CanAddNode(SelectNodeType.OrderBy) == false)
            {
                selectExpression = new(this);
            }

            selectExpression._nodes.Add(SelectNodeType.OrderBy);

            selectExpression.OrderBy = null;

            return selectExpression;
        }

        public ISelectSql AddThenBy()
        {
            if (_nodes.Count == 0 || _nodes.Max(s => (int)s) != (int)SelectNodeType.OrderBy)
                throw new UnsupportedExpressionException();

            return this;
        }

        public ISelectSql AddLimit()
        {
            var selectExpression = this;

            if (selectExpression.CanAddNode(SelectNodeType.Limit) == false)
            {
                selectExpression = new(this);
            }

            selectExpression._nodes.Add(SelectNodeType.Limit);

            return selectExpression;
        }

        public DeleteExpression AddDelete()
        {
            return new DeleteExpression(From, Where, OrderBy, Limit, Timeout);
        }

        public UpdateExpression AddUpdateSet()
        {
            return new UpdateExpression(From, Where, OrderBy, Limit, Timeout);
        }

        public SelectSqlCommand BuildSelect(BuildContext context)
        {
            context = context with { Level = context.Level + 1, DataManipulationSql = this };

            //From
            string from = null;

            if (From is not null)
                from = "\nFrom\n" + From.BuildFromNode(context, 0).Text.Indent(2);

            //Select
            List<string> columns = new();

            if (HasIndex)
                columns.Add(SqlNode.ColumnIndexValue.AsColumn(SqlNode.IndexColumnName));

            if (Columns.Count == 0)
            {
                Dictionary<string, ConstructorInfo> constructors = new();

                Columns.AddRange(context.LinqConvertProvider.GetSelectColumns(this, null, ref constructors, new()));

                Constructors = constructors;
            }

            columns.AddRange(Columns.Select(column =>
            {
                string text = column.ValueExpression.BuildValue(context).Text;

                if (column.HasAlias && SqlNode.NameEqual(column.Alias, text) == false)
                    text = text.AsColumn(column.Alias);

                return text;
            }));

            string select = "Select";

            if (IsDistinct)
                select += " Distinct";

            select += "\n" + string.Join(",\n", columns).Indent(2);

            //Where
            string where = null;

            if (Where is not null)
                where = "\nWhere\n" + Where.BuildValue(context).Text.Indent(2);

            //Group By
            string groupBy = null;

            if (GroupBy is not null)
                groupBy = "\nGroup By\n  " + string.Join(", ", GroupBy.Select(s => s.Value.BuildValue(context).Text));

            //Having
            string having = null;

            if (Having is not null)
                having = "\nHaving\n  " + Having.BuildValue(context).Text;

            //Order By
            string orderBy = null;

            if (OrderBy is not null)
                orderBy = "\nOrder By\n  " + OrderBy.Build(context);

            //Limit
            string limit = null;

            if (Limit is not null)
                limit = "\nLimit\n  " + Limit.Build();

            //With
            string with = null;

            if (CommonTableExpressions.Count > 0)
            {
                with = "With";

                if (CommonTableExpressions.Any(s => s.IsRecursive))
                    with += " Recursive";

                with += "\n" + string.Join(",\n", CommonTableExpressions.Select(s => s.BuildCTE(context).Text)).Indent(2) + "\n";
            }

            //合并
            return new()
            {
                Text = with + select + from + where + groupBy + having + orderBy + limit,
                ColumnMembers = Columns.Select(s => s.Member).ToList(),
                Constructors = Constructors,
                Readers = Readers
            };
        }

        public SqlCommand BuildTableDefinition(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.SubQuery(BuildSelect(context).Text.Indent(2))
            };
        }
    }
}
