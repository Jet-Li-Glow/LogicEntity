using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal interface ITableExpression : ISqlExpression, ISelectSql
    {
        public bool HasAlias { get; }

        public string Alias { get; set; }

        public string ShortName => Alias;

        int Count => 1;

        IList<ColumnInfo> Columns { get; }

        bool CanAddNode(SelectNodeType nodeType);

        SelectExpression AddSelect();

        JoinedTableExpression AddJoin();

        SelectExpression Distinct();

        SelectExpression AddIndex();

        SelectExpression AddWhere();

        SelectExpression AddGroupBy();

        SelectExpression AddHaving();

        ITableExpression AddOrderBy();

        ITableExpression AddThenBy();

        ITableExpression AddLimit();

        ISqlExpression[] GetOrderByParameters();

        OrderKeys OrderBy { get; set; }

        OffsetLimit Limit { get; set; }

        DeleteExpression AddDelete();

        UpdateExpression AddUpdateSet();

        SqlCommand BuildTableDefinition(BuildContext context);

        SqlCommand BuildFromNode(BuildContext context, int index)
        {
            Alias = SqlNode.GetTableAlias(index, context.Level);

            return new()
            {
                Text = BuildTableDefinition(context).Text.As(Alias)
            };
        }

        ITableExpression GetTable(int i)
        {
            if (i == 0)
                return this;

            throw new NotImplementedException();
        }
    }
}
