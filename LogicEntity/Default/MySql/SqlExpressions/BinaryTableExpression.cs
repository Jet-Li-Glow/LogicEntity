using LogicEntity.Collections.Generic;
using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class BinaryTableExpression : SelectSql, ISelectSql, ISubQuerySql
    {
        HashSet<SelectNodeType> _nodes = new() { SelectNodeType.From };

        public ISelectSql Left { get; set; }

        public BinaryOperate Operate { get; set; }

        public bool IsDistinct { get; set; }

        public ISelectSql Right { get; set; }

        public OrderKeys OrderBy { get; set; }

        public OffsetLimit Limit { get; set; }

        public bool IsVector { get => Left.IsVector; }

        public IList<ColumnInfo> Columns => Left.Columns.AsReadOnly();

        public ISqlExpression[] GetOrderByParameters()
        {
            if (Left.IsVector)
                return new ISqlExpression[] { Empty };
            else
                return new ISqlExpression[] { new ColumnExpression(null, Left.Columns[0].Alias) };
        }

        public bool CanAddNode(SelectNodeType nodeType)
        {
            if (nodeType != SelectNodeType.OrderBy && nodeType != SelectNodeType.Limit)
                return false;

            return (int)nodeType >= _nodes.Max(n => (int)n);
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

        public SelectExpression AddJoin()
        {
            return new SelectExpression(this);
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
                _nodes.Add(SelectNodeType.OrderBy);

                OrderBy = null;

                return this;
            }

            return new SelectExpression(this).AddOrderBy();
        }

        public ISelectSql AddThenBy()
        {
            if (_nodes.Count == 0 || _nodes.Max(s => (int)s) != (int)SelectNodeType.OrderBy)
                throw new UnsupportedExpressionException();

            return this;
        }

        public ISelectSql AddLimit()
        {
            if (CanAddNode(SelectNodeType.Limit))
            {
                _nodes.Add(SelectNodeType.Limit);

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

        public SqlCommand BuildTableDefinition(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.SubQuery(BuildSelect(context).Text.Indent(2))
            };
        }

        public SelectSqlCommand BuildSelect(BuildContext context)
        {
            context = context with { Level = context.Level + 1, DataManipulationSql = this };

            //Left
            SelectSqlCommand leftSelectSqlCommand = Left.BuildSelect(context);

            string left = SqlNode.SubQuery(leftSelectSqlCommand.Text.Indent(2));

            //Operate
            string operate = "\n\n" + Operate.ToString();

            //Distinct
            string distinct = " " + (IsDistinct ? "Distinct" : "All");

            //Right
            string right = "\n\n" + SqlNode.SubQuery(Right.BuildSelect(context).Text.Indent(2));

            if (OrderBy is not null || Limit is not null)
                right += "\n";

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
                Text = with + left + operate + distinct + right + orderBy + limit,
                ColumnMembers = leftSelectSqlCommand.ColumnMembers,
                Constructors = leftSelectSqlCommand.Constructors,
                Readers = leftSelectSqlCommand.Readers
            };
        }

        public enum BinaryOperate
        {
            Union,
            Intersect,
            Except
        }
    }
}
