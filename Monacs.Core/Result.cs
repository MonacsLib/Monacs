using System;
using System.Collections.Generic;

namespace Monacs.Core
{
    /// <summary>
    /// Represents the result of the operation that may succeed or fail.
    /// It is recommended to use provided extension methods and not to use properties of the <see cref="Result{T}"/> directly.
    /// <para />If the operation succeeded it will contain a value of a type <typeparamref name="T"/> and it's called Ok in such case.
    /// <para />If the operation failed it will contain error information of type <see cref="ErrorDetails"/> and it's called Error.
    /// </summary>
    /// <typeparam name="T">Expected return value type.</typeparam>
    public readonly struct Result<T> : IEquatable<Result<T>>
    {
        internal Result(T value)
        {
            Value = value;
            Error = default(ErrorDetails);
            IsOk = true;
        }

        internal Result(in ErrorDetails error)
        {
            Value = default;
            Error = error;
            IsOk = false;
        }

        /// <summary>
        /// Contains the computed value of the operation if it ends successfully.
        /// <para /> It is not recommended to use it directly.
        /// <para /> Use extension methods to access the value instead, like:
        /// <para /> * <see cref="Result.GetOrDefault{T}"/>,
        /// <para /> * <see cref="Result.Map{TIn,TOut}"/>,
        /// <para /> * <see cref="Result.Bind{TIn,TOut}"/>,
        /// <para /> * <see cref="Result.Match{TIn,TOut}"/>
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Contains error details when operation ended up with failure.
        /// <para /> It is not recommended to use it directly.
        /// <para /> Use extension methods to access the value instead, like:
        /// <para /> * <see cref="Result.Match{TIn,TOut}"/>
        /// <para /> * <see cref="Result.DoWhenError{T}"/>
        /// </summary>
        public ErrorDetails Error { get; }

        /// <summary>
        /// Indicates that the Result is on the success path (Ok case).
        /// You should expect the output in the <see cref="Value"/> field.
        /// </summary>
        public bool IsOk { get; }

        /// <summary>
        /// Indicates that the result is on the failure path (Error case).
        /// You should expect error in <see cref="Error"/> field
        /// and no value in the <see cref="Value"/> field.
        /// </summary>
        public bool IsError => !IsOk;

        /// <summary>
        /// Creates a string representation of the <see cref="Result{T}"/>.
        /// </summary>
        public override string ToString() =>
            IsOk
            ? $"Ok<{typeof(T).Name}>({Value})"
            : $"Error<{typeof(T).Name}>({Error})";

        /* Equality */

        /// <summary>
        /// Compares two instances of the <see cref="Result{T}"/>.
        /// Two results are equal if both are of the same type, the same case
        /// and the underlying values (of errors) are equal.
        /// </summary>
        /// <param name="other">Result to compare with</param>
        public bool Equals(Result<T> other) =>
            (IsError && other.IsError && EqualityComparer<ErrorDetails>.Default.Equals(Error, other.Error))
            || (IsOk && other.IsOk && EqualityComparer<T>.Default.Equals(Value, other.Value));

        /// <summary>
        /// Compares the <see cref="Result{T}"/> with other object.
        /// Result is only equal to other result given the conditions described in <see cref="Result{T}.Equals(Result{T})"/>.
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        public override bool Equals(object obj) =>
            obj is Result<T> && Equals((Result<T>)obj);

        /// <summary>
        /// Computes the hashcode for the <see cref="Result{T}"/> instance.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return IsError
                ? Error.GetHashCode() ^ 13
                : Value == null ? base.GetHashCode() ^ 29 : Value.GetHashCode() ^ 31;
            }
        }

        public static bool operator ==(in Result<T> a, in Result<T> b) =>
            a.Equals(b);

        public static bool operator !=(in Result<T> a, in Result<T> b) =>
            !a.Equals(b);
    }
}