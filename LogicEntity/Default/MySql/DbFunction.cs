using LogicEntity.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    public static class DbFunction
    {
        public static T As<T>(this T t, string alias)
        {
            return default;
        }

        public static TResult Read<TSource, TResult>(this object source, Func<TSource, TResult> reader)
        {
            return default;
        }

        public static TResult ReadBytes<TResult>(this object source, Func<Func<long, byte[], int, int, long>, TResult> bytesReader)
        {
            return default;
        }

        public static TResult ReadChars<TResult>(this object source, Func<Func<long, char[], int, int, long>, TResult> charsReader)
        {
            return default;
        }

        //-----------------------------------------------------------------------------------

        [MethodFormat("Any{1}")]
        public static T Any<T>(IDataTable<T> source)
        {
            return default;
        }

        [MethodFormat("All{1}")]
        public static T All<T>(IDataTable<T> source)
        {
            return default;
        }

        [MethodFormat("Avg({1})")]
        internal static T Average<T>(T t)
        {
            return default;
        }

        [MethodFormat("Count(*)")]
        internal static int Count()
        {
            return default;
        }

        [MethodFormat("Max({1})")]
        internal static T Max<T>(T t)
        {
            return default;
        }

        [MethodFormat("Min({1})")]
        internal static T Min<T>(T t)
        {
            return default;
        }

        [MethodFormat("Sum({1})")]
        internal static T Sum<T>(T t)
        {
            return default;
        }
    }
}
