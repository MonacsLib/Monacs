using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
    public struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
    {
        internal Option(T value)
        {
            Value = value;
            IsSome = true;
        }

        public T Value { get; }

        public bool IsSome { get; }

        public bool IsNone => !IsSome;

        public override string ToString() =>
            IsSome
            ? $"Some({Value})"
            : $"None<{typeof(T).Name}>";

        public bool Equals(Option<T> other) =>
            (IsNone && other.IsNone) || (IsSome && other.IsSome && EqualityComparer<T>.Default.Equals(Value, other.Value));

        public bool Equals(T other) =>
            IsSome && EqualityComparer<T>.Default.Equals(Value, other);

        public override bool Equals(object obj) =>
            obj is Option<T> && Equals((Option<T>)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return IsNone
                ? base.GetHashCode() ^ 13
                : Value == null ? base.GetHashCode() ^ 29 : Value.GetHashCode() ^ 31;
            }
        }

        public static bool operator ==(Option<T> a, Option<T> b) =>
            a.Equals(b);

        public static bool operator !=(Option<T> a, Option<T> b) =>
            !a.Equals(b);

        public static bool operator ==(Option<T> a, T b) =>
            a.Equals(b);

        public static bool operator !=(Option<T> a, T b) =>
            !a.Equals(b);

        public static bool operator ==(T a, Option<T> b) =>
            b.Equals(a);

        public static bool operator !=(T a, Option<T> b) =>
            !b.Equals(a);

        public static implicit operator T(Option<T> option) =>
            option.IsSome ? option.Value : default;
    }
}
