using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal interface ITableExpression : ISqlExpression
    {
        public bool HasAlias { get; }

        public string Alias { get; set; }

        public string ShortName => Alias;

        int Count => 1;

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
