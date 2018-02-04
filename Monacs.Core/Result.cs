using System;
using System.Collections.Generic;

namespace Monacs.Core
{
    /// <summary>
    /// Represents the result of the operation that may succeed or fail.
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
        /// Contains the computed value of the operation if it ends successfully. 
        /// <para /> 
        /// <para /> !!! It is not recommended to use it directly !!!
        /// <para /> 
        /// <para /> Use one of the following extension methods instead:
        /// <para /> * <see cref="Result.GetOrDefault{T}"/>, 
        /// <para /> * <see cref="Result.Map{TIn,TOut}"/>, 
        /// <para /> * <see cref="Result.Bind{TIn,TOut}"/>,
        /// <para /> * <see cref="Result.Match{TIn,TOut}"/> 
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Field filled on operation failure.
        /// </summary>
        public ErrorDetails Error { get; }

        /// <summary>
        /// Indicates that the result is on the success path 
        /// and you should expect the output in the <see cref="Value"/> field.
        /// </summary>
        public bool IsOk { get; }

        /// <summary>
        /// Indicates that the result is on the failure path 
        /// and you should expect error in <see cref="Error"/> field 
        /// and no value set on <see cref="Value"/> field.
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