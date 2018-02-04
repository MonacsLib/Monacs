using System;
using System.Collections.Generic;

namespace Monacs.Core
{
    /// <summary>
    /// Encapsulates optional value. A value either contains a value of a type <see cref="T"/> or it is default of <see cref="T"/>.
    /// <para />An Option becomes Some if any value is provided. Otherwise it's None.
    /// </summary>
    /// <typeparam name="T">Type of optional value.</typeparam>
    public struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
    {
        internal Option(T value)
        {
            Value = value;
            IsSome = true;
        }
        
        /// <summary>
        /// Encapsulated valid value.
        /// <para /> !!! It is not recommended to use it directly !!!
        /// <para /> Use one of the following extension methods instead:
        /// <para /> * <see cref="Option.GetOrDefault{T}"/>, 
        /// <para /> * <see cref="Option.Map{TIn,TOut}"/>, 
        /// <para /> * <see cref="Option.Bind{TIn,TOut}"/>,
        /// <para /> * <see cref="Option.Match{TIn,TOut}"/> 
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Returns true if the option has a value that is not None.
        /// </summary>
        public bool IsSome { get; }

        /// <summary>
        /// Returns true if the option has the None value.
        /// </summary>
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

        public static bool operator ==(Option<T> a, Option<T> b) => a.Equals(b);

        public static bool operator !=(Option<T> a, Option<T> b) => !a.Equals(b);

        public static bool operator ==(Option<T> a, T b) => a.Equals(b);

        public static bool operator !=(Option<T> a, T b) => !a.Equals(b);

        public static bool operator ==(T a, Option<T> b) => b.Equals(a);

        public static bool operator !=(T a, Option<T> b) => !b.Equals(a);

        public static implicit operator T(Option<T> option) => option.IsSome ? option.Value : default;
    }
}