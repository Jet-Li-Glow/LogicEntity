using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public static class OperatorFunction
    {
        public static T Assign<T>(this T left, T right)
        {
            return right;
        }

        public static T Assign<T>(this Value<T> left, T right)
        {
            return right;
        }

        public static T Assign<T>(this T left, Value<T> right)
        {
            return right;
        }
    }
}
