using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Collections;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Collections.Generic
{
    public class DataTableImpl<T> : DataTableImplBase, ITable<T>, IOrderedDataTable<T>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (object obj in Db.Query(Expression))
                yield return obj;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (object obj in Db.Query(Expression))
                yield return (T)obj;
        }
    }

    public class DataTableImpl<T1, T2> : DataTableImplBase, IOrderedDataTable<T1, T2>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3> : DataTableImplBase, IOrderedDataTable<T1, T2, T3>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }

    public class DataTableImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : DataTableImplBase, IOrderedDataTable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {
        public DataTableImpl(AbstractDataBase db, TableExpression expression) : base(db, expression)
        {

        }
    }
}
