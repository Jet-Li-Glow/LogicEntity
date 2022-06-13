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
    internal class DBOperatorImplement : IWith, IDistinct, IWhere, IThenBy, IHaving, ISelector, IUpdaterSet, IUpdaterWhere, IUpdaterOrderBy, IUpdaterThenBy, IChangerOn,
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

        public IUpdaterSet<T> Update<T>(T table) where T : Table, new()
        {
            Nodes.Add(new Description($"Update {table.FullName}"));

            return new DBOperatorImplement<T>(Nodes, table);
        }

        //Update

        public IUpdaterSet Update(JoinedTable table)
        {
            Nodes.Add(new Description("Update {0}", table));

            return this;
        }

        internal DBOperatorImplement Set(params Table[] tables)
        {
            if (tables is null)
                tables = Array.Empty<Table>();

            int index = 0;

            List<KeyValuePair<string, object>> commands = new();

            foreach (Table table in tables)
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

        public IUpdaterWhere Set<Table1>(Action<Table1> setValue) where Table1 : Table, new()
        {
            Table1 table1 = new();

            setValue?.Invoke(table1);

            return Set(table1);
        }

        public IUpdaterWhere Set<Table1, Table2>(Action<Table1, Table2> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            setValue?.Invoke(table1, table2);

            return Set(table1, table2);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3>(Action<Table1, Table2, Table3> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            setValue?.Invoke(table1, table2, table3);

            return Set(table1, table2, table3);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4>(Action<Table1, Table2, Table3, Table4> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            setValue?.Invoke(table1, table2, table3, table4);

            return Set(table1, table2, table3, table4);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5>(Action<Table1, Table2, Table3, Table4, Table5> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5);

            return Set(table1, table2, table3, table4, table5);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6>(Action<Table1, Table2, Table3, Table4, Table5, Table6> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6);

            return Set(table1, table2, table3, table4, table5, table6);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7);

            return Set(table1, table2, table3, table4, table5, table6, table7);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
            where Table9 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            Table9 table9 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8, table9);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8, table9);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
            where Table9 : Table, new()
            where Table10 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            Table9 table9 = new();

            Table10 table10 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
            where Table9 : Table, new()
            where Table10 : Table, new()
            where Table11 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            Table9 table9 = new();

            Table10 table10 = new();

            Table11 table11 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
            where Table9 : Table, new()
            where Table10 : Table, new()
            where Table11 : Table, new()
            where Table12 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            Table9 table9 = new();

            Table10 table10 = new();

            Table11 table11 = new();

            Table12 table12 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
            where Table9 : Table, new()
            where Table10 : Table, new()
            where Table11 : Table, new()
            where Table12 : Table, new()
            where Table13 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            Table9 table9 = new();

            Table10 table10 = new();

            Table11 table11 = new();

            Table12 table12 = new();

            Table13 table13 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12, table13);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12, table13);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
            where Table9 : Table, new()
            where Table10 : Table, new()
            where Table11 : Table, new()
            where Table12 : Table, new()
            where Table13 : Table, new()
            where Table14 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            Table9 table9 = new();

            Table10 table10 = new();

            Table11 table11 = new();

            Table12 table12 = new();

            Table13 table13 = new();

            Table14 table14 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12, table13, table14);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12, table13, table14);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14, Table15>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14, Table15> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
            where Table9 : Table, new()
            where Table10 : Table, new()
            where Table11 : Table, new()
            where Table12 : Table, new()
            where Table13 : Table, new()
            where Table14 : Table, new()
            where Table15 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            Table9 table9 = new();

            Table10 table10 = new();

            Table11 table11 = new();

            Table12 table12 = new();

            Table13 table13 = new();

            Table14 table14 = new();

            Table15 table15 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12, table13, table14, table15);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12, table13, table14, table15);
        }

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14, Table15, Table16>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14, Table15, Table16> setValue)
            where Table1 : Table, new()
            where Table2 : Table, new()
            where Table3 : Table, new()
            where Table4 : Table, new()
            where Table5 : Table, new()
            where Table6 : Table, new()
            where Table7 : Table, new()
            where Table8 : Table, new()
            where Table9 : Table, new()
            where Table10 : Table, new()
            where Table11 : Table, new()
            where Table12 : Table, new()
            where Table13 : Table, new()
            where Table14 : Table, new()
            where Table15 : Table, new()
            where Table16 : Table, new()
        {
            Table1 table1 = new();

            Table2 table2 = new();

            Table3 table3 = new();

            Table4 table4 = new();

            Table5 table5 = new();

            Table6 table6 = new();

            Table7 table7 = new();

            Table8 table8 = new();

            Table9 table9 = new();

            Table10 table10 = new();

            Table11 table11 = new();

            Table12 table12 = new();

            Table13 table13 = new();

            Table14 table14 = new();

            Table15 table15 = new();

            Table16 table16 = new();

            setValue?.Invoke(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12, table13, table14, table15, table16);

            return Set(table1, table2, table3, table4, table5, table6, table7, table8, table9, table10, table11, table12, table13, table14, table15, table16);
        }

        IUpdater IUpdaterWhere.Where(Description condition)
        {
            Nodes.Add(new Description("Where {0}", condition));

            return this;
        }

        public IUpdater On(Description condition)
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
