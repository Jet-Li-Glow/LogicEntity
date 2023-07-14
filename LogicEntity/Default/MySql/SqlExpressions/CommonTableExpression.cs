using LogicEntity.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class CommonTableExpression : SqlExpression, ITableExpression, ISubQuerySql
    {
        public CommonTableExpression(ITableExpression tableExpression, bool isRecursive)
        {
            TableExpression = tableExpression;

            IsRecursive = isRecursive;
        }

        public ITableExpression TableExpression { get; private set; }

        public bool CanModify { get; set; } = false;

        public bool IsRecursive { get; private set; }

        public string Name { get; private set; }

        public IDataManipulationSql DataManipulationSql { get; private set; }

        public OrderKeys OrderBy
        {
            get
            {
                return TableExpression.OrderBy;
            }

            set
            {
                TableExpression.OrderBy = value;
            }
        }

        public OffsetLimit Limit
        {
            get
            {
                return TableExpression.Limit;
            }

            set
            {
                TableExpression.Limit = value;
            }
        }

        public Dictionary<string, ConstructorInfo> Constructors { get; set; }

        public Dictionary<int, Delegate> Readers { get; set; }

        public int? Timeout
        {
            get
            {
                return TableExpression.Timeout;
            }

            set
            {
                TableExpression.Timeout = value;
            }
        }

        public List<CommonTableExpression> CommonTableExpressions => TableExpression.CommonTableExpressions;

        public bool HasAlias => throw new NotImplementedException();

        public string Alias { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string ShortName => Name;

        public IReadOnlyCollection<MemberInfo> ColumnMembers { get; set; }

        public bool IsVector { get => TableExpression.IsVector; }

        public IList<ColumnInfo> Columns => TableExpression.Columns.AsReadOnly();

        public SqlCommand BuildCTE(BuildContext context)
        {
            SelectSqlCommand selectSqlCommand = TableExpression.BuildSelect(context with { Level = -1 });

            ColumnMembers = selectSqlCommand.ColumnMembers;

            Constructors = selectSqlCommand.Constructors;

            Readers = selectSqlCommand.Readers;

            return new()
            {
                Text = Name.As(SqlNode.SubQuery(selectSqlCommand.Text.Indent(2)))
            };
        }

        public SqlCommand BuildTableDefinition(BuildContext context)
        {
            if (DataManipulationSql is null)
            {
                DataManipulationSql = context.DataManipulationSql;

                Name = SqlNode.GetCTEAlias(DataManipulationSql.CommonTableExpressions.Count, context.Level);

                DataManipulationSql.CommonTableExpressions.Add(this);
            }

            return new()
            {
                Text = Name
            };
        }

        SqlCommand ITableExpression.BuildFromNode(BuildContext context, int index)
        {
            return BuildTableDefinition(context);
        }

        SelectSqlCommand ISelectSql.BuildSelect(BuildContext context)
        {
            SelectExpression selectExpression = new SelectExpression(this);

            selectExpression.Columns.Add(new()
            {
                ValueExpression = AllColumns()
            });

            SelectSqlCommand selectSqlCommand = selectExpression.BuildSelect(context);

            selectSqlCommand.ColumnMembers = ColumnMembers;

            selectSqlCommand.Constructors = Constructors;

            selectSqlCommand.Readers = Readers;

            return selectSqlCommand;
        }

        public ISqlExpression[] GetOrderByParameters()
        {
            return TableExpression.GetOrderByParameters();
        }

        public bool CanAddNode(SelectNodeType nodeType)
        {
            if (CanModify)
                return (nodeType == SelectNodeType.OrderBy || nodeType == SelectNodeType.Limit) && TableExpression.CanAddNode(nodeType);

            return false;
        }

        public SelectExpression ChangeColumns()
        {
            return new SelectExpression(this);
        }

        public SelectExpression AddSelect()
        {
            return new SelectExpression(this).AddSelect();
        }

        public SelectExpression Distinct()
        {
            return new SelectExpression(this).Distinct();
        }

        public SelectExpression AddIndex()
        {
            return new SelectExpression(this).AddIndex();
        }

        public JoinedTableExpression AddJoin()
        {
            return new(this);
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
            return new SelectExpression(this).AddHaving();
        }

        public ITableExpression AddOrderBy()
        {
            if (CanAddNode(SelectNodeType.OrderBy))
            {
                TableExpression.AddOrderBy();

                return this;
            }

            return new SelectExpression(this).AddOrderBy();
        }

        public ITableExpression AddThenBy()
        {
            TableExpression.AddThenBy();

            return this;
        }

        public ITableExpression AddLimit()
        {
            if (CanAddNode(SelectNodeType.Limit))
            {
                TableExpression.AddLimit();

                return this;
            }

            return new SelectExpression(this).AddLimit();
        }

        public DeleteExpression AddDelete()
        {
            throw new NotImplementedException();
        }

        public UpdateExpression AddUpdateSet()
        {
            throw new NotImplementedException();
        }
    }
}
