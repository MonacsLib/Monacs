using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
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

        public T Value { get; }

        public ErrorDetails Error { get; }

        public bool IsOk { get; }

        public bool IsError => !IsOk;

        public override string ToString() =>
            IsOk
            ? $"Ok<{typeof(T).Name}>({Value})"
            : $"Error<{typeof(T).Name}>({Error})";

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

    public static class Result
    {
        /* Constructors */

        public static Result<T> Ok<T>(T value) => new Result<T>(value);

        public static Result<T> Error<T>(ErrorDetails error) => new Result<T>(error);

        /* Converters */

        public static Result<T> OfObject<T>(T value, ErrorDetails error) where T : class =>
            value != null ? Ok(value) : Error<T>(error);

        public static Result<T> OfObject<T>(T value, Func<ErrorDetails> errorFunc) where T : class =>
            value != null ? Ok(value) : Error<T>(errorFunc());

        public static Result<T> ToResult<T>(this T value, ErrorDetails error) where T : class => OfObject(value, error);

        public static Result<T> ToResult<T>(this T value, Func<ErrorDetails> errorFunc) where T : class => OfObject(value, errorFunc);

        public static Result<T> OfNullable<T>(T? value, ErrorDetails error) where T : struct =>
            value.HasValue ? Ok(value.Value) : Error<T>(error);

        public static Result<T> OfNullable<T>(T? value, Func<ErrorDetails> errorFunc) where T : struct =>
            value.HasValue ? Ok(value.Value) : Error<T>(errorFunc());

        public static Result<T> ToResult<T>(this T? value, ErrorDetails error) where T : struct => OfNullable(value, error);

        public static Result<T> ToResult<T>(this T? value, Func<ErrorDetails> errorFunc) where T : struct => OfNullable(value, errorFunc);

        public static Result<string> OfString(string value, ErrorDetails error) =>
            string.IsNullOrEmpty(value) ? Error<string>(error) : Ok(value);

        public static Result<string> OfString(string value, Func<ErrorDetails> errorFunc) =>
            string.IsNullOrEmpty(value) ? Error<string>(errorFunc()) : Ok(value);

        public static Result<string> ToResult(this string value, ErrorDetails error) => OfString(value, error);

        public static Result<string> ToResult(this string value, Func<ErrorDetails> errorFunc) => OfString(value, errorFunc);

        public static Result<T> OfOption<T>(Option<T> value, ErrorDetails error) where T : struct =>
            value.IsSome ? Ok(value.Value) : Error<T>(error);

        public static Result<T> OfOption<T>(Option<T> value, Func<ErrorDetails> errorFunc) where T : struct =>
            value.IsSome ? Ok(value.Value) : Error<T>(errorFunc());

        public static Result<T> ToResult<T>(this Option<T> value, ErrorDetails error) where T : struct => OfOption(value, error);

        public static Result<T> ToResult<T>(this Option<T> value, Func<ErrorDetails> errorFunc) where T : struct => OfOption(value, errorFunc);

        /* TryGetResult */

        public static Result<TValue> TryGetResult<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, ErrorDetails error) =>
            dict.TryGetValue(key, out TValue value) ? Ok(value) : Error<TValue>(error);

        public static Result<TValue> TryGetResult<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, ErrorDetails> errorFunc) =>
            dict.TryGetValue(key, out TValue value) ? Ok(value) : Error<TValue>(errorFunc(key));

        public static Result<IEnumerable<TValue>> TryGetResult<TKey, TValue>(this ILookup<TKey, TValue> lookup, TKey key, ErrorDetails error) =>
            lookup.Contains(key) ? Ok(lookup[key]) : Error<IEnumerable<TValue>>(error);

        public static Result<IEnumerable<TValue>> TryGetResult<TKey, TValue>(this ILookup<TKey, TValue> lookup, TKey key, Func<TKey, ErrorDetails> errorFunc) =>
            lookup.Contains(key) ? Ok(lookup[key]) : Error<IEnumerable<TValue>>(errorFunc(key));

        /* Match */

        public static T2 Match<T1, T2>(this Result<T1> result, Func<T1, T2> ok, Func<ErrorDetails, T2> error) =>
            result.IsOk ? ok(result.Value) : error(result.Error);

        public static T2 MatchTo<T1, T2>(this Result<T1> result, T2 ok, T2 error) =>
            result.IsOk ? ok : error;

        /* Bind */

        public static Result<T2> Bind<T1, T2>(this Result<T1> result, Func<T1, Result<T2>> binder) =>
            result.IsOk ? binder(result.Value) : Error<T2>(result.Error);

        /* Map */

        public static Result<T2> Map<T1, T2>(this Result<T1> result, Func<T1, T2> mapper) =>
            result.IsOk ? Ok(mapper(result.Value)) : Error<T2>(result.Error);

        /* Getters */

        public static T GetOrDefault<T>(this Result<T> result, T whenError = default(T)) =>
            result.IsOk ? result.Value : whenError;

        public static T2 GetOrDefault<T1, T2>(this Result<T1> result, Func<T1, T2> getter, T2 whenError = default(T2)) =>
            result.IsOk ? getter(result.Value) : whenError;

        /* Side Effects */

        public static Result<T> Do<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsOk)
                action(result.Value);
            return result;
        }

        public static Result<T> DoWhenError<T>(this Result<T> result, Action<ErrorDetails> action)
        {
            if (result.IsError)
                action(result.Error);
            return result;
        }

        /* Collections */

        public static IEnumerable<T> Choose<T>(this IEnumerable<Result<T>> items) =>
            items.Where(i => i.IsOk).Select(i => i.Value);

        public static IEnumerable<ErrorDetails> ChooseErrors<T>(this IEnumerable<Result<T>> items) =>
            items.Where(r => r.IsError).Select(x => x.Error);

        public static Result<IEnumerable<T>> Sequence<T>(this IEnumerable<Result<T>> items) =>
            items.Any(i => i.IsError)
            ? Error<IEnumerable<T>>(items.First(i => i.IsError).Error)
            : Ok(items.Select(i => i.Value));

        /* TryCatch */

        public static Result<T> TryCatch<T>(Func<T> func, Func<Exception, ErrorDetails> errorHandler)
        {
            try
            {
                var result = func();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Error<T>(errorHandler(ex));
            }
        }

        public static Result<T2> TryCatch<T1, T2>(this Result<T1> result, Func<T1, T2> func, Func<T1, Exception, ErrorDetails> errorHandler) =>
            result.Bind(value => Result.TryCatch(() => func(value), e => errorHandler(value, e)));
    }
}
