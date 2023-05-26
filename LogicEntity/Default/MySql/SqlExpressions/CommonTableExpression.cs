using LogicEntity.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class CommonTableExpression : SqlExpression, ISelectSql
    {
        public CommonTableExpression(ISelectSql selectSql, bool isRecursive)
        {
            SelectSql = selectSql;

            IsRecursive = isRecursive;
        }

        public ISelectSql SelectSql { get; private set; }

        public bool CanModify { get; set; } = false;

        public bool IsRecursive { get; private set; }

        public string Name { get; private set; }

        public IDataManipulationSql DataManipulationSql { get; private set; }

        public OrderKeys OrderBy
        {
            get
            {
                return SelectSql.OrderBy;
            }

            set
            {
                SelectSql.OrderBy = value;
            }
        }

        public OffsetLimit Limit
        {
            get
            {
                return SelectSql.Limit;
            }

            set
            {
                SelectSql.Limit = value;
            }
        }

        public ISqlExpression SelectedObjectExpression => SelectSql.SelectedObjectExpression;

        public Dictionary<string, ConstructorInfo> Constructors { get; set; }

        public Dictionary<int, Delegate> Readers { get; set; }

        public int? Timeout
        {
            get
            {
                return SelectSql.Timeout;
            }

            set
            {
                SelectSql.Timeout = value;
            }
        }

        public List<CommonTableExpression> CommonTableExpressions => SelectSql.CommonTableExpressions;

        public bool HasAlias => throw new NotImplementedException();

        public string Alias { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string ShortName => Name;

        public IReadOnlyCollection<MemberInfo> ColumnMembers { get; set; }

        public SqlCommand BuildCTE(BuildContext context)
        {
            SelectSqlCommand selectSqlCommand = SelectSql.BuildSelect(context with { Level = -1 });

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

                Name = SqlNode.GetCTEAlias(context.Level, DataManipulationSql.CommonTableExpressions.Count);

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

        public bool CanAddNode(SelectNodeType nodeType)
        {
            if (CanModify)
                return (nodeType == SelectNodeType.OrderBy || nodeType == SelectNodeType.Limit) && SelectSql.CanAddNode(nodeType);

            return false;
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

        public SelectExpression AddJoin()
        {
            return new SelectExpression(this).AddJoin();
        }

        public SelectExpression AddWhere()
        {
            return new SelectExpression(this).AddWhere();
        }

        public SelectExpression AddGroupBy()
        {
            return new SelectExpression(this).AddGroupBy();
        }

        public SelectExpression AddHaving()
        {
            return new SelectExpression(this).AddHaving();
        }

        public ISelectSql AddOrderBy()
        {
            if (CanAddNode(SelectNodeType.OrderBy))
            {
                SelectSql.AddOrderBy();

                return this;
            }

            return new SelectExpression(this).AddOrderBy();
        }

        public ISelectSql AddThenBy()
        {
            SelectSql.AddThenBy();

            return this;
        }

        public ISelectSql AddLimit()
        {
            if (CanAddNode(SelectNodeType.Limit))
            {
                SelectSql.AddLimit();

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
