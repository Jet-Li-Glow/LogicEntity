using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Linq;

namespace LogicEntity.Collections.Generic
{
    public interface IGroupingDataTable<TKey> : IGroupingDataTable
    {
        TKey Key { get; }
    }

    public interface IGroupingDataTable<TKey, TElement> : IGroupingDataTable<TKey>, IDataTable<TElement>
    {
        TElement Element => this.First();
    }

    public interface IGroupingDataTable<TKey, T1, T2> : IGroupingDataTable<TKey>, IDataTable<T1, T2>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression);
    }

    public interface IGroupingDataTable<TKey, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IGroupingDataTable<TKey>, IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {
        TResult Element<TResult>(System.Linq.Expressions.Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> expression);
    }
}
