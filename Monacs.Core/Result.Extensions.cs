using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
    /// <summary>
    /// Contains the set of extensions to work with the <see cref="Result{T}"/> type.
    /// </summary>
    public static partial class Result
    {
        /* Constructors */

        /// <summary>
        /// Creates the Ok case instance of the <see cref="Result{T}"/> type, encapsulating provided value.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to encapsulate.</param>
        public static Result<T> Ok<T>(T value) => new Result<T>(value);

        /// <summary>
        /// Creates the Error case instance of the <see cref="Result{T}"/> type, containing error instead of value.
        /// </summary>
        /// <typeparam name="T">Desired type parameter for <see cref="Result{T}"/> type.</typeparam>
        /// <param name="error">Details of the error.</param>
        public static Result<T> Error<T>(ErrorDetails error) => new Result<T>(error);

        /* Converters */

        /// <summary>
        /// Converts the value of class T to the <see cref="Result{T}"/> type.
        /// If the value is null, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="error">Details of the error if the value is null.</param>
        public static Result<T> OfObject<T>(T value, ErrorDetails error) where T : class =>
            value != null ? Ok(value) : Error<T>(error);

        /// <summary>
        /// Converts the value of class T to the <see cref="Result{T}"/> type.
        /// If the value is null, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="errorFunc">Function yielding details of the error if the value is null.</param>
        public static Result<T> OfObject<T>(T value, Func<ErrorDetails> errorFunc) where T : class =>
            value != null ? Ok(value) : Error<T>(errorFunc());

        /// <summary>
        /// Converts the value of class T to the <see cref="Result{T}"/> type.
        /// If the value is null, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Result.OfObject{T}(T, ErrorDetails)"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="error">Details of the error if the value is null.</param>
        public static Result<T> ToResult<T>(this T value, ErrorDetails error) where T : class => OfObject(value, error);

        /// <summary>
        /// Converts the value of class T to the <see cref="Result{T}"/> type.
        /// If the value is null, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Result.OfObject{T}(T, Func{ErrorDetails})"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="errorFunc">Function yielding details of the error if the value is null.</param>
        public static Result<T> ToResult<T>(this T value, Func<ErrorDetails> errorFunc) where T : class => OfObject(value, errorFunc);

        /// <summary>
        /// Converts the value of <see cref="Nullable{T}"/> to the <see cref="Result{T}"/> type.
        /// If the value is null, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="error">Details of the error if the value is null.</param>
        public static Result<T> OfNullable<T>(T? value, ErrorDetails error) where T : struct =>
            value.HasValue ? Ok(value.Value) : Error<T>(error);

        /// <summary>
        /// Converts the value of <see cref="Nullable{T}"/> to the <see cref="Result{T}"/> type.
        /// If the value is null, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="errorFunc">Function yielding details of the error if the value is null.</param>
        public static Result<T> OfNullable<T>(T? value, Func<ErrorDetails> errorFunc) where T : struct =>
            value.HasValue ? Ok(value.Value) : Error<T>(errorFunc());

        /// <summary>
        /// Converts the value of <see cref="Nullable{T}"/> to the <see cref="Result{T}"/> type.
        /// If the value is null, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Result.OfNullable{T}(T?, ErrorDetails)"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="error">Details of the error if the value is null.</param>
        public static Result<T> ToResult<T>(this T? value, ErrorDetails error) where T : struct => OfNullable(value, error);

        /// <summary>
        /// Converts the value of <see cref="Nullable{T}"/> to the <see cref="Result{T}"/> type.
        /// If the value is null, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Result.OfNullable{T}(T?, Func{ErrorDetails})"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="errorFunc">Function yielding details of the error if the value is null.</param>
        public static Result<T> ToResult<T>(this T? value, Func<ErrorDetails> errorFunc) where T : struct => OfNullable(value, errorFunc);

        /// <summary>
        /// Converts the string value to the <see cref="Result{T}"/> type.
        /// If the value is null or empty string, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="error">Details of the error if the value is null or empty.</param>
        public static Result<string> OfString(string value, ErrorDetails error) =>
            string.IsNullOrEmpty(value) ? Error<string>(error) : Ok(value);

        /// <summary>
        /// Converts the string value to the <see cref="Result{T}"/> type.
        /// If the value is null or empty string, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="errorFunc">Function yielding details of the error if the value is null or empty.</param>
        public static Result<string> OfString(string value, Func<ErrorDetails> errorFunc) =>
            string.IsNullOrEmpty(value) ? Error<string>(errorFunc()) : Ok(value);

        /// <summary>
        /// Converts the string value to the <see cref="Result{T}"/> type.
        /// If the value is null or empty string, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Result.OfString(string, ErrorDetails)"/></remarks>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="error">Details of the error if the value is null or empty.</param>
        public static Result<string> ToResult(this string value, ErrorDetails error) => OfString(value, error);

        /// <summary>
        /// Converts the string value to the <see cref="Result{T}"/> type.
        /// If the value is null or empty string, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Result.OfString(string, Func{ErrorDetails})"/></remarks>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="errorFunc">Function yielding details of the error if the value is null or empty.</param>
        public static Result<string> ToResult(this string value, Func<ErrorDetails> errorFunc) => OfString(value, errorFunc);

        /// <summary>
        /// Converts the value of <see cref="Option{T}"/> to the <see cref="Result{T}"/> type.
        /// If the value is None case, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="error">Details of the error if the value is None.</param>
        public static Result<T> OfOption<T>(Option<T> value, ErrorDetails error) where T : struct =>
            value.IsSome ? Ok(value.Value) : Error<T>(error);

        /// <summary>
        /// Converts the value of <see cref="Option{T}"/> to the <see cref="Result{T}"/> type.
        /// If the value is None case, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="errorFunc">Function yielding details of the error if the value is None.</param>
        public static Result<T> OfOption<T>(Option<T> value, Func<ErrorDetails> errorFunc) where T : struct =>
            value.IsSome ? Ok(value.Value) : Error<T>(errorFunc());

        /// <summary>
        /// Converts the value of <see cref="Option{T}"/> to the <see cref="Result{T}"/> type.
        /// If the value is None case, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Result.OfOption{T}(Option{T}, ErrorDetails)"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="error">Details of the error if the value is None.</param>
        public static Result<T> ToResult<T>(this Option<T> value, ErrorDetails error) where T : struct => OfOption(value, error);

        /// <summary>
        /// Converts the value of <see cref="Option{T}"/> to the <see cref="Result{T}"/> type.
        /// If the value is None case, the Error case is yielded.
        /// Otherwise Ok case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Result.OfOption{T}(Option{T}, Func{ErrorDetails})"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Result{T}"/>.</param>
        /// <param name="errorFunc">Function yielding details of the error if the value is None.</param>
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

        public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> ok, Func<ErrorDetails, TOut> error) =>
            result.IsOk ? ok(result.Value) : error(result.Error);

        public static TOut MatchTo<TIn, TOut>(this Result<TIn> result, TOut ok, TOut error) =>
            result.IsOk ? ok : error;

        /* Bind */

        public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value) : Error<TOut>(result.Error);

        /* Map */

        public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper) =>
            result.IsOk ? Ok(mapper(result.Value)) : Error<TOut>(result.Error);

        /* Getters */

        public static T GetOrDefault<T>(this Result<T> result, T whenError = default(T)) =>
            result.IsOk ? result.Value : whenError;

        public static TOut GetOrDefault<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> getter, TOut whenError = default(TOut)) =>
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

        public static Result<TOut> TryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func, Func<TIn, Exception, ErrorDetails> errorHandler) =>
            result.Bind(value => Result.TryCatch(() => func(value), e => errorHandler(value, e)));
    }
}
