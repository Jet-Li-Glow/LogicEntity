﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Collections;
using LogicEntity.Collections.Generic;
using LogicEntity.Default.MySql.Linq.Expressions;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Default.MySql
{
    public static class MySqlEnumerable
    {
        public static IDataTable<T> Value<T>(this AbstractDataBase db, System.Linq.Expressions.Expression<Func<T>> valueExpression)
        {
            return new DataTableImpl<T>(db, new SelectedTableExpression(null, valueExpression, typeof(T)));
        }

        public static System.Data.DataTable Select<TSource>(this IDataTable<TSource> source, int timeout, params System.Linq.Expressions.Expression<Func<TSource, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2>(this IDataTable<T1, T2> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3>(this IDataTable<T1, T2, T3> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4>(this IDataTable<T1, T2, T3, T4> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4, T5> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5, T6> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static System.Data.DataTable Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source, int timeout, params System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, object>>[] columnSelectors)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (columnSelectors is null)
                throw new ArgumentNullException(nameof(columnSelectors));

            return source.Db.QueryDataTable(new TimeoutTableExpression(new SelectedTableExpression(source.Expression, columnSelectors, typeof(System.Data.DataTable)), timeout));
        }

        public static IDataTable<TOuter, TInner> InnerJoin<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner, System.Linq.Expressions.Expression<Func<TOuter, TInner, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3> InnerJoin<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4> InnerJoin<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5> InnerJoin<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> InnerJoin<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> InnerJoin<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> InnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, "Inner Join", inner.Expression, predicate));
        }

        public static IDataTable<TOuter, TInner> CrossJoin<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner, System.Linq.Expressions.Expression<Func<TOuter, TInner, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3> CrossJoin<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4> CrossJoin<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5> CrossJoin<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> CrossJoin<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> CrossJoin<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CrossJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, "Cross Join", inner.Expression, predicate));
        }

        public static IDataTable<TOuter, TInner> LeftJoin<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner, System.Linq.Expressions.Expression<Func<TOuter, TInner, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3> LeftJoin<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4> LeftJoin<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5> LeftJoin<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> LeftJoin<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> LeftJoin<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> LeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, "Left Join", inner.Expression, predicate));
        }

        public static IDataTable<TOuter, TInner> RightJoin<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner, System.Linq.Expressions.Expression<Func<TOuter, TInner, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3> RightJoin<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4> RightJoin<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5> RightJoin<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> RightJoin<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> RightJoin<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> RightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner, System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>> predicate)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, "Right Join", inner.Expression, predicate));
        }

        public static IDataTable<TOuter, TInner> NaturalJoin<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3> NaturalJoin<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4> NaturalJoin<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5> NaturalJoin<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> NaturalJoin<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> NaturalJoin<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> NaturalJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Join", inner.Expression, null));
        }

        public static IDataTable<TOuter, TInner> NaturalInnerJoin<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3> NaturalInnerJoin<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4> NaturalInnerJoin<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5> NaturalInnerJoin<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> NaturalInnerJoin<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> NaturalInnerJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Inner Join", inner.Expression, null));
        }

        public static IDataTable<TOuter, TInner> NaturalLeftJoin<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3> NaturalLeftJoin<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4> NaturalLeftJoin<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5> NaturalLeftJoin<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> NaturalLeftJoin<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> NaturalLeftJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Left Join", inner.Expression, null));
        }

        public static IDataTable<TOuter, TInner> NaturalRightJoin<TOuter, TInner>(this IDataTable<TOuter> outer, IDataTable<TInner> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<TOuter, TInner>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3> NaturalRightJoin<T1, T2, T3>(this IDataTable<T1, T2> outer, IDataTable<T3> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4> NaturalRightJoin<T1, T2, T3, T4>(this IDataTable<T1, T2, T3> outer, IDataTable<T4> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5> NaturalRightJoin<T1, T2, T3, T4, T5>(this IDataTable<T1, T2, T3, T4> outer, IDataTable<T5> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6> NaturalRightJoin<T1, T2, T3, T4, T5, T6>(this IDataTable<T1, T2, T3, T4, T5> outer, IDataTable<T6> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7>(this IDataTable<T1, T2, T3, T4, T5, T6> outer, IDataTable<T7> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataTable<T1, T2, T3, T4, T5, T6, T7> outer, IDataTable<T8> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> outer, IDataTable<T9> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> outer, IDataTable<T10> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> outer, IDataTable<T11> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outer, IDataTable<T12> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outer, IDataTable<T13> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outer, IDataTable<T14> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outer, IDataTable<T15> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> NaturalRightJoin<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outer, IDataTable<T16> inner)
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));

            return new DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(outer.Db, new JoinedTableExpression(outer.Expression, "Natural Right Join", inner.Expression, null));
        }

        public static IDataTable<TSource> RecursiveConcat<TSource>(this IDataTable<TSource> first, System.Linq.Expressions.Expression<Func<IDataTable<TSource>, IDataTable<TSource>>> secondFactory)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (secondFactory is null)
                throw new ArgumentNullException(nameof(secondFactory));

            return new DataTableImpl<TSource>(first.Db, new RecursiveUnionedTableExpression(first.Expression, secondFactory, false));
        }

        public static IDataTable<TSource> RecursiveUnion<TSource>(this IDataTable<TSource> first, System.Linq.Expressions.Expression<Func<IDataTable<TSource>, IDataTable<TSource>>> secondFactory)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (secondFactory is null)
                throw new ArgumentNullException(nameof(secondFactory));

            return new DataTableImpl<TSource>(first.Db, new RecursiveUnionedTableExpression(first.Expression, secondFactory, true));
        }

        public static IDataTable<TSource> Timeout<TSource>(this IDataTable<TSource> source, int timeout)
        {
            return new DataTableImpl<TSource>(source.Db, new TimeoutTableExpression(source.Expression, timeout));
        }

        //Operate

        public static int Add<TSource>(this ITable<TSource> source, int timeout, params TSource[] elements)
        {
            return source.AddRange(timeout, elements);
        }

        public static int AddRange<TSource>(this ITable<TSource> source, int timeout, IEnumerable<TSource> elements)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            if (elements.Any() is false)
                throw new InvalidOperationException("No elements");

            return source.Db.ExecuteNonQuery(new TimeoutOperateExpression(new AddOperateExpression(source.Expression, elements.Cast<object>(), false), timeout));
        }

        public static int AddOrUpdate<TSource>(this ITable<TSource> source, int timeout, params TSource[] elements)
        {
            return source.AddRangeOrUpdate(elements, timeout, null);
        }

        public static int AddOrUpdate<TSource>(this ITable<TSource> source, System.Linq.Expressions.Expression<Func<TSource, TSource, TSource>> updateFactory, params TSource[] elements)
        {
            return source.AddRangeOrUpdate(elements, null, updateFactory);
        }

        public static int AddOrUpdate<TSource>(this ITable<TSource> source, int timeout, System.Linq.Expressions.Expression<Func<TSource, TSource, TSource>> updateFactory, params TSource[] elements)
        {
            return source.AddRangeOrUpdate(elements, timeout, updateFactory);
        }

        public static int AddRangeOrUpdate<TSource>(this ITable<TSource> source, IEnumerable<TSource> elements, int timeout)
        {
            return source.AddRangeOrUpdate(elements, timeout, null);
        }

        public static int AddRangeOrUpdate<TSource>(this ITable<TSource> source, IEnumerable<TSource> elements, System.Linq.Expressions.Expression<Func<TSource, TSource, TSource>> updateFactory)
        {
            return source.AddRangeOrUpdate(elements, null, updateFactory);
        }

        public static int AddRangeOrUpdate<TSource>(this ITable<TSource> source, IEnumerable<TSource> elements, int? timeout, System.Linq.Expressions.Expression<Func<TSource, TSource, TSource>> updateFactory)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            if (elements.Any() is false)
                throw new InvalidOperationException("No elements");

            OperateExpression operateExpression = new AddOrUpdateOperateExpression(source.Expression, elements.Cast<object>(), updateFactory);

            if (timeout.HasValue)
                operateExpression = new TimeoutOperateExpression(operateExpression, timeout.Value);

            return source.Db.ExecuteNonQuery(operateExpression);
        }

        public static int AddIgnore<TSource>(this ITable<TSource> source, params TSource[] elements)
        {
            return source.AddRangeIgnore(elements);
        }

        public static int AddIgnore<TSource>(this ITable<TSource> source, int timeout, params TSource[] elements)
        {
            return source.AddRangeIgnore(timeout, elements);
        }

        public static int AddRangeIgnore<TSource>(this ITable<TSource> source, IEnumerable<TSource> elements)
        {
            return source.AddRangeIgnore(null, elements);
        }

        public static int AddRangeIgnore<TSource>(this ITable<TSource> source, int? timeout, IEnumerable<TSource> elements)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            if (elements.Any() is false)
                throw new InvalidOperationException("No elements");

            OperateExpression operateExpression = new AddIgnoreOperateExpression(source.Expression, elements.Cast<object>());

            if (timeout.HasValue)
                operateExpression = new TimeoutOperateExpression(operateExpression, timeout.Value);

            return source.Db.ExecuteNonQuery(operateExpression);
        }

        public static int Replace<TSource>(this ITable<TSource> source, params TSource[] elements)
        {
            return source.ReplaceRange(elements);
        }

        public static int Replace<TSource>(this ITable<TSource> source, int timeout, params TSource[] elements)
        {
            return source.ReplaceRange(timeout, elements);
        }

        public static int ReplaceRange<TSource>(this ITable<TSource> source, IEnumerable<TSource> elements)
        {
            return source.ReplaceRange(null, elements);
        }

        public static int ReplaceRange<TSource>(this ITable<TSource> source, int? timeout, IEnumerable<TSource> elements)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            if (elements.Any() is false)
                throw new InvalidOperationException("No elements");

            OperateExpression operateExpression = new ReplaceOperateExpression(source.Expression, elements.Cast<object>());

            if (timeout.HasValue)
                operateExpression = new TimeoutOperateExpression(operateExpression, timeout.Value);

            return source.Db.ExecuteNonQuery(operateExpression);
        }
    }
}
