using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    internal class DBOperatorImplement : IWith, IDistinct, IWhere, IThenBy, IHaving, ISelector, IUpdaterSet, IUpdaterOn, IUpdaterThenBy,
        IDeleterFrom, IDeleterWhere, IDeleterThenBy, IInsert, IInsertIgnore, IInsertInto, IInsertTable
    {
        /// <summary>
        /// 额外的参数
        /// </summary>
        List<KeyValuePair<string, object>> _addedParameters = new();

        /// <summary>
        /// 超时时间
        /// </summary>
        int? _commandTimeout;

        /// <summary>
        /// 节点
        /// </summary>
        protected List<Description> Nodes { get; } = new();

        /// <summary>
        /// 列
        /// </summary>
        public IEnumerable<Column> Columns { get; protected set; }

        //CTE

        public IOperate With(bool isRecursive, params CommonTableExpression[] commonTableExpressions)
        {
            if (commonTableExpressions is null)
                commonTableExpressions = Array.Empty<CommonTableExpression>();

            Nodes.Add(new Description($"With {(isRecursive ? "Recursive" : string.Empty)}\n  {string.Join(",\n  ", commonTableExpressions.Select((_, i) => "{" + i + "}"))}",
                commonTableExpressions.Select(c => new Description(c)).ToArray()));

            return this;
        }

        //Select

        public IDistinct Select()
        {
            Nodes.Add(new Description(nameof(Select)));

            return this;
        }

        public ISelectColumns Distinct()
        {
            Nodes.Add(new Description(nameof(Distinct)));

            return this;
        }

        public IFrom SetColumns(params Column[] columns)
        {
            if (columns is null || columns.Any() == false)
                return this;

            Columns = columns.AsEnumerable();

            Nodes.Add(new Description(string.Join(",\n", columns.Select((column, i) =>
            {
                return "  {" + i + "}" + (column.HasAlias ? $" As `{column.Alias}`" : string.Empty);
            })), columns));

            return this;
        }

        public IWhere From(params TableDescription[] tables)
        {
            if (Columns is null || Columns.Any() == false)
                SetColumns(tables.SelectMany(t => t.Columns).ToArray());

            Nodes.Add(new Description($"From{string.Join(string.Empty, tables.Select((_, i) => "\n  {" + i + "}"))}", tables));

            return this;
        }

        public IGroupBy Where(Description condition)
        {
            Nodes.Add(new Description("Where {0}", condition));

            return this;
        }

        public IHaving GroupBy(params Description[] columns)
        {
            if (columns is null)
                columns = Array.Empty<Description>();

            Nodes.Add(new Description($"Group By\n{string.Join(",\n", columns.Select((_, i) => "  {" + i + "}"))}", columns));

            return this;
        }

        public IWindow Having(Description condition)
        {
            Nodes.Add(new Description("Having\n  {0}", condition));

            return this;
        }

        public IUnion Window(params Window[] windows)
        {
            if (windows is null)
                windows = Array.Empty<Window>();

            Nodes.Add(new Description($"Window\n{string.Join(",\n", windows.Select((w, i) => "  " + w.Alias.PadLeft(4) + " As ({" + i + "})"))}", windows));

            return this;
        }

        public IUnion Union(ISelector selector)
        {
            Nodes.Add(new Description("\nUnion\n\n{0}", selector));

            return this;
        }

        public IUnion UnionAll(ISelector selector)
        {
            Nodes.Add(new Description("\nUnion All\n\n{0}", selector));

            return this;
        }

        public IThenBy OrderBy(Description description)
        {
            Nodes.Add(new Description("Order By\n  {0} Asc", description));

            return this;
        }

        public IThenBy OrderByDescending(Description description)
        {
            Nodes.Add(new Description("Order By\n  {0} Desc", description));

            return this;
        }

        public IThenBy ThenBy(Description description)
        {
            Nodes.Add(new Description(", {0} Asc", description));

            return this;
        }

        public IThenBy ThenByDescending(Description description)
        {
            Nodes.Add(new Description(", {0} Desc", description));

            return this;
        }

        public IForUpdate Limit(ulong limit)
        {
            Nodes.Add(new Description($"Limit {limit}"));

            return this;
        }

        public IForUpdate Limit(ulong offset, ulong limit)
        {
            Nodes.Add(new Description($"Limit {offset}, {limit}"));

            return this;
        }

        public ISelector ForUpdate()
        {
            Nodes.Add(new Description(nameof(ForUpdate)));

            return this;
        }

        public NestedTable As(string alias)
        {
            return new NestedTable(this, alias);
        }

        //Update

        public IUpdaterSet Update(JoinedTable table)
        {
            Nodes.Add(new Description("Update {0}", table));

            return this;
        }

        public IUpdaterOn ApplyChanges(Table table)
        {
            Nodes.Add(new Description($"Update {table?.FullName}"));

            return ApplyChanges(new Table[] { table });
        }

        public IUpdaterOn ApplyChanges(params Table[] tables)
        {
            if (tables is null)
                tables = Array.Empty<Table>();

            int index = 0;

            List<KeyValuePair<string, object>> commands = new();

            foreach (Table table in tables.Where(table => table is not null))
            {
                foreach (Column column in table.Columns.Where(column => column.IsValueSet))
                {
                    if (column.Writer is not null && column.Value is not Description)
                        column.Value = column.Writer(column.Value);

                    commands.Add(KeyValuePair.Create(column.FullName + " = {" + index + "}", column.Value));

                    index++;
                }
            }

            Nodes.Add(new Description($"Set\n    {string.Join(",\n    ", commands.Select(c => c.Key))}", commands.Select(c => c.Value).ToArray()));

            return this;
        }

        public IUpdaterOrderBy On(Description condition)
        {
            Nodes.Add(new Description("Where {0}", condition));

            return this;
        }

        IUpdaterThenBy IUpdaterOrderBy.OrderBy(Description description)
        {
            Nodes.Add(new Description("Order By {0} Asc", description));

            return this;
        }

        IUpdaterThenBy IUpdaterOrderBy.OrderByDescending(Description description)
        {
            Nodes.Add(new Description("Order By {0} Desc", description));

            return this;
        }

        IUpdaterThenBy IUpdaterThenBy.ThenBy(Description description)
        {
            Nodes.Add(new Description(", {0} Asc", description));

            return this;
        }

        IUpdaterThenBy IUpdaterThenBy.ThenByDescending(Description description)
        {
            Nodes.Add(new Description(", {0} Desc", description));

            return this;
        }

        IUpdater IUpdaterLimit.Limit(ulong limit)
        {
            Nodes.Add(new Description($"Limit {limit}"));

            return this;
        }

        //Delete

        public IDeleterFrom Delete(params Table[] tables)
        {
            if (tables is null)
                tables = Array.Empty<Table>();

            Nodes.Add(new Description($"Delete {string.Join(", ", tables.Select(table => table.FullName))}"));

            return this;
        }

        public IDeleterWhere From(Table table)
        {
            Nodes.Add(new Description($"From {table.FullName}"));

            return this;
        }

        public IDeleterWhere From(JoinedTable table)
        {
            Nodes.Add(new Description("From {0}", table));

            return this;
        }

        IDeleterOrderBy IDeleterWhere.Where(Description condition)
        {
            Nodes.Add(new Description("Where {0}", condition));

            return this;
        }

        IDeleterThenBy IDeleterOrderBy.OrderBy(Description description)
        {
            Nodes.Add(new Description("Order By {0} Asc", description));

            return this;
        }

        IDeleterThenBy IDeleterOrderBy.OrderByDescending(Description description)
        {
            Nodes.Add(new Description("Order By {0} Desc", description));

            return this;
        }

        IDeleterThenBy IDeleterThenBy.ThenBy(Description description)
        {
            Nodes.Add(new Description(", {0} Asc", description));

            return this;
        }

        IDeleterThenBy IDeleterThenBy.ThenByDescending(Description description)
        {
            Nodes.Add(new Description(", {0} Desc", description));

            return this;
        }

        IDeleter IDeleterLimit.Limit(ulong limit)
        {
            Nodes.Add(new Description($"Limit {limit}"));

            return this;
        }

        //Insert

        public IInsertIgnore Insert()
        {
            Nodes.Add(new Description(nameof(Insert)));

            return this;
        }

        public IInsertInto Replace()
        {
            Nodes.Add(new Description(nameof(Replace)));

            return this;
        }

        public IInsertInto Ignore()
        {
            Nodes.Add(new Description(nameof(Ignore)));

            return this;
        }

        public IInsertTable Into()
        {
            Nodes.Add(new Description(nameof(Into)));

            return this;
        }

        public IInsertorColumns<TTable> Table<TTable>(TTable table) where TTable : Table, new()
        {
            Nodes.Add(new Description(table.FullName));

            return new DBOperatorImplement<TTable>(Nodes, table);
        }

        //Operator

        public IAddParameterDbOperator AddParameter(string key, object value)
        {
            _addedParameters.Add(KeyValuePair.Create(key, value));

            return this;
        }

        public IDbOperator SetCommandTimeout(int seconds)
        {
            _commandTimeout = seconds;

            return this;
        }

        Command IDbOperator.GetCommandWithUniqueParameterName()
        {
            Command command = new();

            if (Columns is not null)
            {
                List<Column> columns = Columns.ToList();

                for (int i = 0; i < columns.Count; i++)
                {
                    Column column = columns[i];

                    if (column.Reader is not null)
                        command.Readers[i] = column.Reader;

                    if (column.BytesReader is not null)
                        command.BytesReaders[i] = column.BytesReader;

                    if (column.CharsReader is not null)
                        command.CharsReaders[i] = column.CharsReader;
                }
            }

            StringBuilder stringBuilder = new();

            int last = Nodes.Count - 1;

            for (int i = 0; i < Nodes.Count; i++)
            {
                Description node = Nodes[i];

                (var cmd, var ps) = node.Build();

                stringBuilder.Append(cmd + (i == last ? string.Empty : "\n"));

                if (ps is not null)
                    command.Parameters.AddRange(ps);
            }

            command.CommandText = stringBuilder.ToString();

            command.Parameters.AddRange(_addedParameters);

            command.CommandTimeout = _commandTimeout;

            return command;
        }
    }
}
