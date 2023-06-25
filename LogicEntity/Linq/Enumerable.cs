using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LogicEntity.Collections;
using LogicEntity.Collections.Generic;
using LogicEntity.Linq;
using LogicEntity.Linq.Expressions;
using LogicEntity.Method;

namespace LogicEntity.Linq
{
    public static class Enumerable
    {
        public static ITable<T> Create<T>(this ITable<T> source, Func<string, string, (string, string)> getTableName)
        {
            OriginalTableExpression table = (OriginalTableExpression)source.Expression;

            (string schema, string name) = getTableName(table.Schema, table.Name);

            return new DataTableImpl<T>(source.Db, new OriginalTableExpression(schema, name, typeof(T)));
        }

#nullable enable

        public static IDataTable<TResult> Select<TSource, TResult>(this IDataTable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, TResult>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, TResult>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, TResult>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, TResult>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TSource, TResult>(this IDataTable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, TResult>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, TResult>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, TResult>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, TResult>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, int, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static DataTable Select<TSource>(this IDataTable<TSource> source, params Expression<Func<TSource, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2>(this IDataTable<T1, T2> source, params Expression<Func<T1, T2, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3>(this IDataTable<T1, T2, T3> source, params Expression<Func<T1, T2, T3, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source, params Expression<Func<T1, T2, T3, T4, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source, params Expression<Func<T1, T2, T3, T4, T5, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source, params Expression<Func<T1, T2, T3, T4, T5, T6, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new SelectedTableExpression(source.Expression, columnSelectors, typeof(DataTable)));
        }

        public static IDataTable<TResult> Select<TKey, TSource, TResult>(this IGroupedDataTable<TKey, TSource> source, Expression<Func<IGroupingDataTable<TKey, TSource>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, TResult>(this IGroupedDataTable<TKey, T1, T2> source, Expression<Func<IGroupingDataTable<TKey, T1, T2>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, TResult>(this IGroupedDataTable<TKey, T1, T2, T3> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TResult> Select<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, TResult>> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return new DataTableImpl<TResult>(source.Db, new SelectedTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IDataTable<TOuter, TInner> Join<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner, Expression<Func<TOuter, TInner, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3> Join<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner, Expression<Func<T1, T2, T3, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4> Join<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner, Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5> Join<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner, Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> Join<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> Join<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> Join<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Join<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, inner.Expression, predicate));
        }

        public static IDataTable<TSource> Where<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<TSource>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2> Where<T1, T2>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3> Where<T1, T2, T3>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4> Where<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5> Where<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> Where<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> Where<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> Where<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(source.Db, new RowFilteredTableExpression(source.Expression, predicate));
        }

        [HasIndex]
        public static IDataTable<TSource> Where<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<TSource>(source.Db, new RowFilteredTableExpression(source.Expression, predicate, true));
        }

        public static IGroupedDataTable<TKey, TSource> GroupBy<TSource, TKey>(this IDataTable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, TSource>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2> GroupBy<T1, T2, TKey>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3> GroupBy<T1, T2, T3, TKey>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4> GroupBy<T1, T2, T3, T4, TKey>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5> GroupBy<T1, T2, T3, T4, T5, TKey>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6> GroupBy<T1, T2, T3, T4, T5, T6, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7> GroupBy<T1, T2, T3, T4, T5, T6, T7, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IGroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> GroupBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(source.Db, new GroupedTableExpression(source.Expression, keySelector));
        }

        public static IDataTable<TSource> Concat<TSource>(this IDataTable<TSource> first, IDataTable<TSource> second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return new DataTableImpl<TSource>(first.Db, new UnionedTableExpression(first.Expression, second.Expression, false));
        }

        public static IDataTable<TSource> Union<TSource>(this IDataTable<TSource> first, IDataTable<TSource> second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return new DataTableImpl<TSource>(first.Db, new UnionedTableExpression(first.Expression, second.Expression, true));
        }

        public static IDataTable<TSource> UnionBy<TSource, TKey>(this IDataTable<TSource> first, IDataTable<TSource> second, Expression<Func<TSource, TKey>> keySelector)
        {
            return first.Concat(second).DistinctBy(keySelector);
        }

        public static IOrderedDataTable<TSource> OrderBy<TSource, TKey>(this IDataTable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<TSource>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2> OrderBy<T1, T2, TKey>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3> OrderBy<T1, T2, T3, TKey>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4> OrderBy<T1, T2, T3, T4, TKey>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5> OrderBy<T1, T2, T3, T4, T5, TKey>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6> OrderBy<T1, T2, T3, T4, T5, T6, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7> OrderBy<T1, T2, T3, T4, T5, T6, T7, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> OrderBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, false));
        }

        public static IOrderedDataTable<TSource> OrderByDescending<TSource, TKey>(this IDataTable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<TSource>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2> OrderByDescending<T1, T2, TKey>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3> OrderByDescending<T1, T2, T3, TKey>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4> OrderByDescending<T1, T2, T3, T4, TKey>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5> OrderByDescending<T1, T2, T3, T4, T5, TKey>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6> OrderByDescending<T1, T2, T3, T4, T5, T6, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> OrderByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(source.Db, new OrderedTableExpression(source.Expression, false, keySelector, true));
        }

        public static IOrderedDataTable<TSource> ThenBy<TSource, TKey>(this IOrderedDataTable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<TSource>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2> ThenBy<T1, T2, TKey>(this IOrderedDataTable<T1, T2> source, Expression<Func<T1, T2, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3> ThenBy<T1, T2, T3, TKey>(this IOrderedDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4> ThenBy<T1, T2, T3, T4, TKey>(this IOrderedDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5> ThenBy<T1, T2, T3, T4, T5, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6> ThenBy<T1, T2, T3, T4, T5, T6, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7> ThenBy<T1, T2, T3, T4, T5, T6, T7, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ThenBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, false));
        }

        public static IOrderedDataTable<TSource> ThenByDescending<TSource, TKey>(this IOrderedDataTable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<TSource>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2> ThenByDescending<T1, T2, TKey>(this IOrderedDataTable<T1, T2> source, Expression<Func<T1, T2, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3> ThenByDescending<T1, T2, T3, TKey>(this IOrderedDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4> ThenByDescending<T1, T2, T3, T4, TKey>(this IOrderedDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5> ThenByDescending<T1, T2, T3, T4, T5, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6> ThenByDescending<T1, T2, T3, T4, T5, T6, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ThenByDescending<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>(this IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TKey>> keySelector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(source.Db, new OrderedTableExpression(source.Expression, true, keySelector, true));
        }

        public static IDataTable<TSource> Skip<TSource>(this IDataTable<TSource> source, int count)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new DataTableImpl<TSource>(source.Db, new SkippedTableExpression(source.Expression, count));
        }

        public static IDataTable<TSource> Take<TSource>(this IDataTable<TSource> source, int count)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new DataTableImpl<TSource>(source.Db, new TakedTableExpression(source.Expression, count));
        }

        public static bool All<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool All<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            return (bool)source.Db.ExecuteScalar(new AllTableExpression(source.Expression, predicate));
        }

        public static bool Any<TSource>(this IDataTable<TSource> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2>(this IDataTable<T1, T2> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3>(this IDataTable<T1, T2, T3> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source)
        {
            return (bool)source.Db.ExecuteScalar(new AnyTableExpression(source.Expression));
        }

        public static bool Any<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static bool Any<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            return source.Where(predicate).Any();
        }

        public static TResult Average<TSource, TResult>(this IDataTable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, TResult>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, TResult>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, TResult>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, TResult>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Average<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new AverageTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static IEnumerable<TSource[]> Chunk<TSource>(this IDataTable<TSource> source, int size)
        {
            int offset = 0;

            TSource[]? elements;

            do
            {
                elements = source.Skip(offset).Take(size).ToArray();

                if (elements.Length == 0)
                    yield break;

                yield return elements;

                offset += size;
            }
            while (elements.Length == size);
        }

        public static int Count<TSource>(this IDataTable<TSource> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2>(this IDataTable<T1, T2> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3>(this IDataTable<T1, T2, T3> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static int Count<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source)
        {
            return (int)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(int)));
        }

        public static IDataTable<TSource> Distinct<TSource>(this IDataTable<TSource> source)
        {
            return new DataTableImpl<TSource>(source.Db, new DistinctTableExpression(source.Expression));
        }

        public static IDataTable<TSource> DistinctBy<TSource, TKey>(this IDataTable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return source.GroupBy(keySelector).Select(g => g.Element);
        }

        public static TSource ElementAt<TSource>(this IDataTable<TSource> source, int index)
        {
            return source.Skip(index).Take(1).AsEnumerable().ElementAt(0);
        }

        public static TSource? ElementAtOrDefault<TSource>(this IDataTable<TSource> source, int index)
        {
            return source.Skip(index).Take(1).AsEnumerable().ElementAtOrDefault(0);
        }

        public static IDataTable<TSource> Except<TSource>(this IDataTable<TSource> first, IDataTable<TSource> second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return new DataTableImpl<TSource>(first.Db, new ExceptTableExpression(first.Expression, second.Expression));
        }

        public static TSource First<TSource>(this IDataTable<TSource> source)
        {
            return source.Take(1).AsEnumerable().First();
        }

        public static TSource First<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Where(predicate).Take(1).AsEnumerable().First();
        }

        public static TSource? FirstOrDefault<TSource>(this IDataTable<TSource> source)
        {
            return source.Take(1).AsEnumerable().FirstOrDefault();
        }

        public static TSource FirstOrDefault<TSource>(this IDataTable<TSource> source, TSource defaultValue)
        {
            return source.Take(1).AsEnumerable().FirstOrDefault(defaultValue);
        }

        public static TSource? FirstOrDefault<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Where(predicate).Take(1).AsEnumerable().FirstOrDefault();
        }

        public static TSource FirstOrDefault<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue)
        {
            return source.Where(predicate).Take(1).AsEnumerable().FirstOrDefault(defaultValue);
        }

        public static IDataTable<TSource> Intersect<TSource>(this IDataTable<TSource> first, IDataTable<TSource> second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return new DataTableImpl<TSource>(first.Db, new IntersectTableExpression(first.Expression, second.Expression));
        }

        public static long LongCount<TSource>(this IDataTable<TSource> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2>(this IDataTable<T1, T2> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3>(this IDataTable<T1, T2, T3> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static long LongCount<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source)
        {
            return (long)source.Db.ExecuteScalar(new CountTableExpression(source.Expression, typeof(long)));
        }

        public static TResult Max<TSource, TResult>(this IDataTable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, TResult>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, TResult>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, TResult>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, TResult>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Max<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MaxTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TSource? MaxBy<TSource, TKey>(this IDataTable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return source.OrderByDescending(keySelector).Take(1).AsEnumerable().FirstOrDefault();
        }

        public static TResult Min<TSource, TResult>(this IDataTable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, TResult>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, TResult>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, TResult>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, TResult>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Min<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new MinTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TSource? MinBy<TSource, TKey>(this IDataTable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return source.OrderBy(keySelector).Take(1).AsEnumerable().FirstOrDefault();
        }

        public static TResult Sum<TSource, TResult>(this IDataTable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, TResult>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, TResult>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, TResult>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, TResult>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TResult Sum<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> selector)
        {
            return (TResult)source.Db.ExecuteScalar(new SumTableExpression(source.Expression, selector, typeof(TResult)));
        }

        public static TSource Single<TSource>(this IDataTable<TSource> source)
        {
            return source.Take(2).Single();
        }

        public static TSource Single<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Where(predicate).Take(2).Single();
        }

        public static TSource? SingleOrDefault<TSource>(this IDataTable<TSource> source)
        {
            return source.Take(2).SingleOrDefault();
        }

        public static TSource SingleOrDefault<TSource>(this IDataTable<TSource> source, TSource defaultValue)
        {
            return source.Take(2).SingleOrDefault(defaultValue);
        }

        public static TSource? SingleOrDefault<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Where(predicate).Take(2).SingleOrDefault();
        }

        public static TSource SingleOrDefault<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue)
        {
            return source.Where(predicate).Take(2).SingleOrDefault(defaultValue);
        }

        public static IDataTable<TSource> SkipWhile<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Where(
                System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(
                    System.Linq.Expressions.Expression.Not(predicate.Body),
                    predicate.Parameters
                    )
                );
        }

        public static IDataTable<TSource> SkipWhile<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            return source.Where(
                System.Linq.Expressions.Expression.Lambda<Func<TSource, int, bool>>(
                    System.Linq.Expressions.Expression.Not(predicate.Body),
                    predicate.Parameters
                    )
                );
        }

        public static IDataTable<TSource> TakeWhile<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2> TakeWhile<T1, T2>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3> TakeWhile<T1, T2, T3>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4> TakeWhile<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5> TakeWhile<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> TakeWhile<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> TakeWhile<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> TakeWhile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            return source.Where(predicate);
        }

        public static IDataTable<TSource> TakeWhile<TSource>(this IDataTable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            return source.Where(predicate);
        }

        //Operate

        public static int Add<TSource>(this ITable<TSource> source, params TSource[] elements)
        {
            return source.AddRange(elements);
        }

        public static int Add<TSource>(this ITable<TSource> source, params Expression<Func<TSource>>[] elements)
        {
            return source.AddRange(elements);
        }

        public static int AddRange<TSource>(this ITable<TSource> source, IEnumerable<TSource> elements)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            return source.Db.ExecuteNonQuery(new AddOperateExpression(source.Expression, elements));
        }

        public static int AddRange<TSource>(this ITable<TSource> source, IEnumerable<Expression<Func<TSource>>> elements)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            return source.Db.ExecuteNonQuery(new AddOperateExpression(source.Expression, elements));
        }

        public static ulong AddNext<TSource>(this ITable<TSource> source, Expression<Func<TSource>> element)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (element is null)
                throw new ArgumentNullException(nameof(element));

            return (ulong)source.Db.ExecuteScalar(new AddNextTableExpression(source.Expression, element));
        }

        public static int AddOrUpdate<TSource>(this ITable<TSource> source, params TSource[] elements)
        {
            return source.AddRangeOrUpdate(elements);
        }

        public static int AddOrUpdate<TSource>(this ITable<TSource> source, params Expression<Func<TSource>>[] elements)
        {
            return source.AddRangeOrUpdate(elements);
        }

        public static int AddRangeOrUpdate<TSource>(this ITable<TSource> source, IEnumerable<TSource> elements)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            return source.Db.ExecuteNonQuery(new AddOrUpdateOperateExpression(source.Expression, elements));
        }

        public static int AddRangeOrUpdate<TSource>(this ITable<TSource> source, IEnumerable<Expression<Func<TSource>>> elements)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            return source.Db.ExecuteNonQuery(new AddOrUpdateOperateExpression(source.Expression, elements));
        }

        public static int Remove<TSource>(this IDataTable<TSource> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, null));
        }

        public static int Remove<T1, T2>(this IDataTable<T1, T2> source, Expression<Func<T1, T2, object>> selector, params Expression<Func<T1, T2, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3>(this IDataTable<T1, T2, T3> source, Expression<Func<T1, T2, T3, object>> selector, params Expression<Func<T1, T2, T3, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source, Expression<Func<T1, T2, T3, T4, object>> selector, params Expression<Func<T1, T2, T3, T4, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Func<T1, T2, T3, T4, T5, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Func<T1, T2, T3, T4, T5, T6, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Remove<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, object>> selector, params Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, object>>[] selectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            List<LambdaExpression> removeSelectors = new();

            removeSelectors.Add(selector);

            if (selectors is not null)
                removeSelectors.AddRange(selectors);

            return source.Db.ExecuteNonQuery(new RemoveOperateExpression(source.Expression, removeSelectors.ToArray()));
        }

        public static int Set<TSource>(this IDataTable<TSource> source, Expression<Action<TSource>> assignment, params Expression<Action<TSource>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2>(this IDataTable<T1, T2> source, Expression<Action<T1, T2>> assignment, params Expression<Action<T1, T2>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3>(this IDataTable<T1, T2, T3> source, Expression<Action<T1, T2, T3>> assignment, params Expression<Action<T1, T2, T3>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source, Expression<Action<T1, T2, T3, T4>> assignment, params Expression<Action<T1, T2, T3, T4>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source, Expression<Action<T1, T2, T3, T4, T5>> assignment, params Expression<Action<T1, T2, T3, T4, T5>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source, Expression<Action<T1, T2, T3, T4, T5, T6>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }

        public static int Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> assignment, params Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>[] assignments)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (assignment is null)
                throw new ArgumentNullException(nameof(assignment));

            List<LambdaExpression> setAssignments = new();

            setAssignments.Add(assignment);

            if (assignments is not null)
            {
                setAssignments.AddRange(assignments);
            }

            return source.Db.ExecuteNonQuery(new SetOperateExpression(source.Expression, setAssignments.ToArray()));
        }
    }
}
