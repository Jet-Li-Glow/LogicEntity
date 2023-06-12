using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public static class SetOperatorFunction
    {
        public static T SetValue<T>(this T left, T right)
        {
            return right;
        }

        public static T SetValue<T>(this T left, Value<T> right)
        {
            return right;
        }

        public static T SetValue<T>(this Value<T> left, T right)
        {
            return right;
        }

        public static T SetValue<T>(this T? left, T right) where T : struct
        {
            return right;
        }

        public static T SetValue<T>(this T? left, Value<T> right) where T : struct
        {
            return right;
        }

        public static T SetValue<T>(this Value<T?> left, T right) where T : struct
        {
            return right;
        }

        public static T SetValue<T>(this Value<T?> left, Value<T> right) where T : struct
        {
            return right;
        }
    }
}
