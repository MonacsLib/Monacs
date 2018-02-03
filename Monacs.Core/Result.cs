using System;
using System.Collections.Generic;

namespace Monacs.Core
{
    /// <summary>
    /// Represents Success and Failure.
    /// Great for working in Railway Oriented way.
    /// </summary>
    /// <typeparam name="T">Expected returned value type.</typeparam>
    public struct Result<T> : IEquatable<Result<T>>
    {
        internal Result(T value)
        {
            Value = value;
            Error = default(ErrorDetails);
            IsOk = true;
        }

        internal Result(ErrorDetails error)
        {
            Value = default(T);
            Error = error;
            IsOk = false;
        }

        /// <summary>
        /// Wrapped result value. Only valid when result is successful.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Field filled on operation failure.
        /// </summary>
        public ErrorDetails Error { get; }

        /// <summary>
        /// Result is successful (Ok) when operation ends in optimistic way.
        /// I.e.: Dividing two numbers returns their quotient.
        /// </summary>
        public bool IsOk { get; }

        /// <summary>
        /// Result is failure (Error) when operation ends in pesimistic way.
        /// I.e.: Dividing by zero should always be forbidden and the result should be failure.
        /// </summary>
        public bool IsError => !IsOk;

        public override string ToString() =>
            IsOk
            ? $"Ok<{typeof(T).Name}>({Value})"
            : $"Error<{typeof(T).Name}>({Error})";


        /* Equality */

        public bool Equals(Result<T> other) =>
            (IsError && other.IsError && EqualityComparer<ErrorDetails>.Default.Equals(Error, other.Error))
            || (IsOk && other.IsOk && EqualityComparer<T>.Default.Equals(Value, other.Value));

        public override bool Equals(object obj) =>
            obj is Result<T> && Equals((Result<T>)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return IsError
                ? Error.GetHashCode() ^ 13
                : Value == null ? base.GetHashCode() ^ 29 : Value.GetHashCode() ^ 31;
            }
        }

        public static bool operator ==(Result<T> a, Result<T> b) =>
            a.Equals(b);

        public static bool operator !=(Result<T> a, Result<T> b) =>
            !a.Equals(b);
    }
}