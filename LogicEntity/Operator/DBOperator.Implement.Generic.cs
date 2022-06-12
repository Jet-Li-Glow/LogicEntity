using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    internal class DBOperatorImplement<T> : DBOperatorImplement, IUpdaterSet<T>, IUpdaterWhere<T>, IApplyChanges<T>, IInsertorColumns<T>,
        IInsertorSet<T>, IInsertorValues<T>, IOnDuplicateKeyUpdate<T> where T : Table, new()
    {
        public DBOperatorImplement()
        {

        }

        public DBOperatorImplement(IEnumerable<Description> nodes)
        {
            Nodes.AddRange(nodes);
        }

        //Insert

        IInsertorValues<T> IInsertorColumns<T>.Columns(params Column[] columns)
        {
            if (columns is null)
                columns = Array.Empty<Column>();

            Columns = columns.AsEnumerable();

            Nodes.Add(new Description($"({string.Join(", ", columns.Select(column => column.FullName))})"));

            return this;
        }

        IOnDuplicateKeyUpdate<T> IInsertorSet<T>.Set(Action<T> setValue)
        {
            return (DBOperatorImplement<T>)Set(setValue);
        }

        public IOnDuplicateKeyUpdate<T> Rows<TRow>(params TRow[] rows)
        {
            Type rowType = typeof(TRow);

            var properties = (Columns ?? Enumerable.Empty<Column>()).Select(column => new
            {
                PropertyInfo = rowType.GetProperty(column?.EntityPropertyName ?? string.Empty, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase),
                Writer = column.Writer
            }).ToList();

            int index = 0;

            List<object> values = new();

            var rowCommands = rows.Select((row, i) =>
            {
                List<string> pNames = new();

                foreach (var property in properties)
                {
                    object v = property.PropertyInfo?.GetValue(row);

                    if (v is Column column)
                    {
                        v = column.Value;
                    }

                    if (property.Writer is not null)
                        v = property.Writer(v);

                    pNames.Add("{" + index + "}");

                    index++;

                    values.Add(v);
                }

                return $"({string.Join(", ", pNames)})";
            });

            Nodes.Add(new Description($"Values\n  {string.Join(",\n  ", rowCommands)}", values.ToArray()));

            return this;
        }

        public IOnDuplicateKeyUpdate<T> SelectRows(ISelector selector)
        {
            Nodes.Add(new Description("{0}", selector));

            return this;
        }

        public IInsertor OnDuplicateKeyUpdate(Action<T> setValue)
        {
            T t = new();

            setValue?.Invoke(t);

            return OnDuplicateKeyUpdate(t);
        }

        public IInsertor OnDuplicateKeyUpdate(Action<T, T> setValueWithRow)
        {
            T t = new();

            T row = new();

            string rowAlias = "rowData";

            row.As(rowAlias);

            setValueWithRow?.Invoke(t, row);

            Nodes.Add(new Description($"As {rowAlias}"));

            return OnDuplicateKeyUpdate(t);
        }

        DBOperatorImplement<T> OnDuplicateKeyUpdate(T t)
        {
            List<KeyValuePair<string, object>> commands = t.Columns.Where(column => column.IsValueSet)
                .Select((column, i) => KeyValuePair.Create(column.FullName + " = {" + i + "}", column.Value)).ToList();

            Nodes.Add(new Description($"On Duplicate Key Update\n  {string.Join(",\n  ", commands.Select(c => c.Key))}", commands.Select(c => c.Value).ToArray()));

            return this;
        }

        //Update

        public IChangerOn ApplyChanges(T table)
        {
            Update(table);

            return Set(table);
        }

        public IUpdaterWhere<T> Set(Action<T> setValue)
        {
            return (DBOperatorImplement<T>)Set<T>(setValue);
        }

        IUpdaterOrderBy IUpdaterWhere<T>.Where(Description condition)
        {
            Nodes.Add(new Description("Where {0}", condition));

            return this;
        }
    }
}
