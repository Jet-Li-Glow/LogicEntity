using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Grammar;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    internal class DBOperatorImplement<T> : DBOperatorImplement, IInsertorColumns<T>,
        IInsertorSet<T>, IInsertorValues<T>, IOnDuplicateKeyUpdate<T> where T : Table, new()
    {
        T _instance;

        public DBOperatorImplement()
        {

        }

        public DBOperatorImplement(IEnumerable<ISqlExpression> nodes, T instance)
        {
            Nodes.AddRange(nodes);

            _instance = instance;
        }

        //Insert

        IInsertorValues<T> IInsertorColumns<T>.Columns(params Column[] columns)
        {
            if (columns is null)
                columns = Array.Empty<Column>();

            Columns = columns.AsEnumerable();

            Nodes.Add(new SqlExpression($"({string.Join(", ", columns.Select(column => column.FullName))})"));

            return this;
        }

        IOnDuplicateKeyUpdate<T> IInsertorSet<T>.Set(Action<T> setValue)
        {
            T t = new();

            foreach (Column column in (t as TableExpression).Columns)
            {
                column.Table = _instance;
            }

            setValue?.Invoke(t);

            return (DBOperatorImplement<T>)ApplyChanges(new Table[] { t });
        }

        public IOnDuplicateKeyUpdate<T> Row<TRow>(params TRow[] rows)
        {
            Type rowType = typeof(TRow);

            var properties = (Columns ?? Enumerable.Empty<Column>()).Select(column => new
            {
                PropertyInfo = rowType.GetProperty(column?.EntityPropertyName ?? string.Empty, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase),
                Writer = column.Writer
            }).ToList();

            var rowExpressions = rows.Select(row =>
            {
                List<KeyValuePair<string, object>> ps = new();

                for (int i = 0; i < properties.Count; i++)
                {
                    var property = properties[i];

                    object v = property.PropertyInfo?.GetValue(row);

                    if (v is Column column)
                    {
                        v = column.Value;
                    }

                    if (property.Writer is not null && v is not ISqlExpression && v is not null)
                        v = property.Writer(v);

                    ps.Add(KeyValuePair.Create("{" + i + "}", v));
                }

                return new SqlExpression($"({string.Join(", ", ps.Select(p => p.Key))})", ps.Select(p => p.Value).ToArray());
            });

            Nodes.Add(new SqlExpression("Values\n  {0}", SqlExpression.__Join(",\n  ", rowExpressions)));

            return this;
        }

        public IOnDuplicateKeyUpdate<T> Rows(ISelector selector)
        {
            Nodes.Add(new SqlExpression("{0}", selector));

            return this;
        }

        public IInsertor OnDuplicateKeyUpdate(Action<T> setValue)
        {
            T t = new();

            foreach (Column column in (t as TableExpression).Columns)
            {
                column.Table = _instance;
            }

            setValue?.Invoke(t);

            return OnDuplicateKeyUpdate(t);
        }

        public IInsertor OnDuplicateKeyUpdate(Action<T, T> setValueWithRow)
        {
            T t = new();

            foreach (Column column in (t as TableExpression).Columns)
            {
                column.Table = _instance;
            }

            T row = new();

            string rowAlias = "rowData";

            row.As(rowAlias);

            setValueWithRow?.Invoke(t, row);

            Nodes.Add(new SqlExpression($"As {rowAlias}"));

            return OnDuplicateKeyUpdate(t);
        }

        DBOperatorImplement<T> OnDuplicateKeyUpdate(T t)
        {
            IEnumerable<SqlExpression> expressions = (t as TableExpression).Columns.Where(column => column.IsValueSet)
                .Select(column => new SqlExpression(column.FullName + " = {0}", column.Value));

            Nodes.Add(new SqlExpression("On Duplicate Key Update\n  {0}", SqlExpression.__Join(",\n  ", expressions)));

            return this;
        }
    }
}
