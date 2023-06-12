using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public struct Value<T> : IValue, IComparable, IComparable<Value<T>>
    {
        T _value = default;

        Expression<Func<T>> _expression = default;

        Value(T value)
        {
            _value = value;

            ValueType = ValueType.Value;

            ValueSetted = true;
        }

        public Value(Expression<Func<T>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            _expression = expression;

            ValueType = ValueType.Expression;

            ValueSetted = true;
        }

        public bool ValueSetted { get; private set; } = false;

        public ValueType ValueType { get; private set; } = ValueType.Value;

        public object Object => ValueType == ValueType.Value ? _value : _expression;

        public override string ToString()
        {
            return Object?.ToString();
        }

        public int CompareTo(object obj)
        {
            return ((IComparable)_value).CompareTo(obj);
        }

        public int CompareTo(Value<T> other)
        {
            return ((IComparable<T>)_value).CompareTo(other._value);
        }

        public static implicit operator Value<T>(T value)
        {
            return new(value);
        }

        public static implicit operator T(Value<T> value)
        {
            return value.ValueType == ValueType.Value ? value._value : default;
        }
    }
}
