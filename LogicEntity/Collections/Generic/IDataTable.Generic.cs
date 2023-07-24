using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Collections.Generic
{
    public interface IDataTable<T> : IDataTable, IEnumerable<T>
    {
        IEnumerable<T> AsEnumerable()
        {
            return this;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (object obj in (IEnumerable)this)
                yield return (T)obj;
        }
    }

    public interface IDataTable<T1, T2> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IDataTable
    {

    }

    public interface IDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IDataTable
    {

    }
}
