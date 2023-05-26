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
