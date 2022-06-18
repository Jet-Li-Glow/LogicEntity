using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Grammar;
using LogicEntity.Model;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    internal class DBOperatorImplement : IWith, IDistinct, IWhere, IThenBy, IHaving, ISelector, IUpdaterSet, IUpdaterOn, IUpdaterThenBy,
        IDeleterFrom, IDeleterWhere, IDeleterThenBy, IInsert, IInsertIgnore, IInsertInto, IInsertTable
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        int? _commandTimeout;

        /// <summary>
        /// 缩进
        /// </summary>
        int _indent = 0;

        /// <summary>
        /// 节点
        /// </summary>
        protected List<ISqlExpression> Nodes { get; } = new();

        /// <summary>
        /// 缩进
        /// </summary>
        int IDbOperator.__Indent
        {
            set
            {
                _indent = value;
            }
        }

        /// <summary>
        /// 列
        /// </summary>
        public IEnumerable<Column> Columns { get; protected set; }

        //CTE

        public IOperate With(bool isRecursive, params CommonTableExpression[] commonTableExpressions)
        {
            Nodes.Add(SqlExpression.With(isRecursive, commonTableExpressions));

            return this;
        }

        //Select

        public IDistinct Select()
        {
            Nodes.Add(SqlExpression.Select());

            return this;
        }

        public ISelectColumns Distinct()
        {
            Nodes.Add(SqlExpression.Distinct());

            return this;
        }

        public IFrom SetColumns(params Column[] columns)
        {
            if (columns is null || columns.Any() == false)
                return this;

            Columns = columns.AsEnumerable();

            Nodes.Add(SqlExpression.__Join(",\n",
                columns.Select(column => new SqlExpression("  {0}" + (column.HasAlias ? $" As `{column.Alias}`" : string.Empty), column))
                )
                );

            return this;
        }

        public IWhere From(params TableExpression[] tables)
        {
            if (tables is null)
                tables = Array.Empty<TableExpression>();

            if (Columns is null || Columns.Any() == false)
                SetColumns(tables.SelectMany(t => t.Columns).ToArray());

            Nodes.Add(SqlExpression.From(tables));

            return this;
        }

        public IGroupBy Where(IValueExpression condition)
        {
            Nodes.Add(SqlExpression.Where(condition));

            return this;
        }

        public IHaving GroupBy(params IValueExpression[] columns)
        {
            if (columns is null)
                columns = Array.Empty<IValueExpression>();

            Nodes.Add(SqlExpression.GroupBy(columns));

            return this;
        }

        public IWindow Having(IValueExpression condition)
        {
            Nodes.Add(SqlExpression.Having(condition));

            return this;
        }

        public IUnion Window(params Window[] windows)
        {
            if (windows is null)
                windows = Array.Empty<Window>();

            Nodes.Add(SqlExpression.Window(windows));

            return this;
        }

        public IUnion Union(ISelector selector)
        {
            Nodes.Add(SqlExpression.Union(selector));

            return this;
        }

        public IUnion UnionAll(ISelector selector)
        {
            Nodes.Add(SqlExpression.UnionAll(selector));

            return this;
        }

        public IThenBy OrderBy(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.OrderBy(valueExpression));

            return this;
        }

        public IThenBy OrderByDescending(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.OrderByDescending(valueExpression));

            return this;
        }

        public IThenBy ThenBy(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.ThenBy(valueExpression));

            return this;
        }

        public IThenBy ThenByDescending(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.ThenByDescending(valueExpression));

            return this;
        }

        public IForUpdate Limit(ulong limit)
        {
            Nodes.Add(SqlExpression.Limit(limit));

            return this;
        }

        public IForUpdate Limit(ulong offset, ulong limit)
        {
            Nodes.Add(SqlExpression.Limit(offset, limit));

            return this;
        }

        public ISelector ForUpdate()
        {
            Nodes.Add(SqlExpression.ForUpdate());

            return this;
        }

        public NestedTable As(string alias)
        {
            return new NestedTable(this, alias);
        }

        //Update

        public IUpdaterSet Update(TableExpression table)
        {
            Nodes.Add(SqlExpression.Update(table));

            return this;
        }

        public IUpdaterOn ApplyChanges(Table table)
        {
            Nodes.Add(new SqlExpression($"Update\n  {(table as TableExpression)?.FullName}"));

            return ApplyChanges(new Table[] { table });
        }

        public IUpdaterOn ApplyChanges(params Table[] tables)
        {
            if (tables is null)
                tables = Array.Empty<Table>();

            List<SqlExpression> expressions = new();

            foreach (Table table in tables.Where(table => table is not null))
            {
                foreach (Column column in (table as TableExpression).Columns.Where(column => column.IsValueSet))
                {
                    if (column.Writer is not null && column.Value is not ISqlExpression)
                        column.Value = column.Writer(column.Value);

                    expressions.Add(new SqlExpression(column.FullName + " = {0}", column.Value));
                }
            }

            Nodes.Add(new SqlExpression("Set\n  {0}", SqlExpression.__Join(",\n  ", expressions)));

            return this;
        }

        public IUpdaterOrderBy On(IValueExpression condition)
        {
            Nodes.Add(SqlExpression.Where(condition));

            return this;
        }

        IUpdaterThenBy IUpdaterOrderBy.OrderBy(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.OrderBy(valueExpression));

            return this;
        }

        IUpdaterThenBy IUpdaterOrderBy.OrderByDescending(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.OrderByDescending(valueExpression));

            return this;
        }

        IUpdaterThenBy IUpdaterThenBy.ThenBy(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.ThenBy(valueExpression));

            return this;
        }

        IUpdaterThenBy IUpdaterThenBy.ThenByDescending(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.ThenByDescending(valueExpression));

            return this;
        }

        IUpdater IUpdaterLimit.Limit(ulong limit)
        {
            Nodes.Add(SqlExpression.Limit(limit));

            return this;
        }

        //Delete

        public IDeleterFrom Delete(params Table[] tables)
        {
            if (tables is null)
                tables = Array.Empty<Table>();

            Nodes.Add(SqlExpression.Delete(tables));

            return this;
        }

        public IDeleterWhere From(TableExpression table)
        {
            Nodes.Add(SqlExpression.From(table));

            return this;
        }

        IDeleterOrderBy IDeleterWhere.Where(IValueExpression condition)
        {
            Nodes.Add(SqlExpression.Where(condition));

            return this;
        }

        IDeleterThenBy IDeleterOrderBy.OrderBy(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.OrderBy(valueExpression));

            return this;
        }

        IDeleterThenBy IDeleterOrderBy.OrderByDescending(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.OrderByDescending(valueExpression));

            return this;
        }

        IDeleterThenBy IDeleterThenBy.ThenBy(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.ThenBy(valueExpression));

            return this;
        }

        IDeleterThenBy IDeleterThenBy.ThenByDescending(IValueExpression valueExpression)
        {
            Nodes.Add(SqlExpression.ThenByDescending(valueExpression));

            return this;
        }

        IDeleter IDeleterLimit.Limit(ulong limit)
        {
            Nodes.Add(SqlExpression.Limit(limit));

            return this;
        }

        //Insert

        public IInsertIgnore Insert()
        {
            Nodes.Add(SqlExpression.Insert());

            return this;
        }

        public IInsertInto Replace()
        {
            Nodes.Add(SqlExpression.Replace());

            return this;
        }

        public IInsertInto Ignore()
        {
            Nodes.Add(SqlExpression.Ignore());

            return this;
        }

        public IInsertTable Into()
        {
            Nodes.Add(SqlExpression.Into());

            return this;
        }

        public IInsertorColumns<TTable> Table<TTable>(TTable table) where TTable : Table, new()
        {
            Nodes.Add(new SqlExpression((table as TableExpression).FullName));

            return new DBOperatorImplement<TTable>(Nodes, table);
        }

        //Operator

        public void SetTimeout(int seconds)
        {
            _commandTimeout = seconds;
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

            string indentStr = new(Enumerable.Repeat(' ', _indent).ToArray());

            (var cmd, var ps) = (SqlExpression.__Join("\n", Nodes) as ISqlExpression).Build();

            command.CommandText = indentStr + cmd.Replace("\n", "\n" + indentStr);

            if (ps is not null)
                command.Parameters.AddRange(ps);

            command.CommandTimeout = _commandTimeout;

            return command;
        }

        (string, IEnumerable<KeyValuePair<string, object>>) ISqlExpression.Build()
        {
            Command command = (this as IDbOperator).GetCommandWithUniqueParameterName();

            return (command.CommandText, command.Parameters);
        }
    }
}
