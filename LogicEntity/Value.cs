using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity
{
    public struct Value<T> : IValue
    {
        T _value = default;

        Expression<Func<T>> _expression = default;

        Value(T vlaue)
        {
            _value = vlaue;

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

        public static implicit operator Value<T>(T vlaue)
        {
            return new(vlaue);
        }

        public static implicit operator T(Value<T> value)
        {
            return value.ValueType == ValueType.Value ? value._value : default;
        }
    }
}
