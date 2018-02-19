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

        /// <summary>
        /// Tries to get the element with the given <paramref name="key"/> from the <paramref name="dictionary"/>.
        /// If the value is found, returns Ok case of the <see cref="Result{T}"/> type with the value from the dictionary.
        /// Otherwise returns Error case of the <see cref="Result{T}"/> type.
        /// </summary>
        /// <typeparam name="TKey">Type of the key in the dictionary.</typeparam>
        /// <typeparam name="TValue">Type of the value in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search in.</param>
        /// <param name="key">The key to look for.</param>
        /// <param name="error">Details of the error if the key is not found.</param>
        public static Result<TValue> TryGetResult<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, ErrorDetails error) =>
            dictionary.TryGetValue(key, out TValue value) ? Ok(value) : Error<TValue>(error);

        /// <summary>
        /// Tries to get the element with the given <paramref name="key"/> from the <paramref name="dictionary"/>.
        /// If the value is found, returns Ok case of the <see cref="Result{T}"/> type with the value from the dictionary.
        /// Otherwise returns Error case of the <see cref="Result{T}"/> type.
        /// </summary>
        /// <typeparam name="TKey">Type of the key in the dictionary.</typeparam>
        /// <typeparam name="TValue">Type of the value in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search in.</param>
        /// <param name="key">The key to look for.</param>
        /// <param name="errorFunc">Function yielding details of the error if the key is not found.</param>
        public static Result<TValue> TryGetResult<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, ErrorDetails> errorFunc) =>
            dictionary.TryGetValue(key, out TValue value) ? Ok(value) : Error<TValue>(errorFunc(key));

        /// <summary>
        /// Tries to get the elements with the given <paramref name="key"/> from the <paramref name="lookup"/>.
        /// If any value is found, returns Ok case of the <see cref="Result{T}"/> type with the values from the lookup.
        /// Otherwise returns Error case of the <see cref="Result{T}"/> type.
        /// </summary>
        /// <typeparam name="TKey">Type of the key in the lookup.</typeparam>
        /// <typeparam name="TValue">Type of the value in the lookup.</typeparam>
        /// <param name="lookup">The lookup to search in.</param>
        /// <param name="key">The key to look for.</param>
        /// <param name="error">Details of the error if the key is not found.</param>
        public static Result<IEnumerable<TValue>> TryGetResult<TKey, TValue>(this ILookup<TKey, TValue> lookup, TKey key, ErrorDetails error) =>
            lookup.Contains(key) ? Ok(lookup[key]) : Error<IEnumerable<TValue>>(error);

        /// <summary>
        /// Tries to get the elements with the given <paramref name="key"/> from the <paramref name="lookup"/>.
        /// If any value is found, returns Ok case of the <see cref="Result{T}"/> type with the values from the lookup.
        /// Otherwise returns Error case of the <see cref="Result{T}"/> type.
        /// </summary>
        /// <typeparam name="TKey">Type of the key in the lookup.</typeparam>
        /// <typeparam name="TValue">Type of the value in the lookup.</typeparam>
        /// <param name="lookup">The lookup to search in.</param>
        /// <param name="key">The key to look for.</param>
        /// <param name="errorFunc">Function yielding details of the error if the key is not found.</param>
        public static Result<IEnumerable<TValue>> TryGetResult<TKey, TValue>(this ILookup<TKey, TValue> lookup, TKey key, Func<TKey, ErrorDetails> errorFunc) =>
            lookup.Contains(key) ? Ok(lookup[key]) : Error<IEnumerable<TValue>>(errorFunc(key));

        /* Match */

        /// <summary>
        /// Does the pattern matching on the <see cref="Result{T}"/> type.
        /// If the <paramref name="result"/> is Ok, calls <paramref name="ok"/> function
        /// with the value from the result as a parameter and returns its result.
        /// Otherwise calls <paramref name="error"/> function and returns its result.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the result.</typeparam>
        /// <typeparam name="TOut">Type of the returned value.</typeparam>
        /// <param name="result">The result to match on.</param>
        /// <param name="ok">Function called for the Ok case.</param>
        /// <param name="error">Function called for the Error case.</param>
        public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> ok, Func<ErrorDetails, TOut> error) =>
            result.IsOk ? ok(result.Value) : error(result.Error);

        /// <summary>
        /// Does the pattern matching on the <see cref="Result{T}"/> type.
        /// If the <paramref name="result"/> is Ok, returns <paramref name="ok"/> value.
        /// Otherwise returns <paramref name="error"/> value.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the result.</typeparam>
        /// <typeparam name="TOut">Type of the returned value.</typeparam>
        /// <param name="result">The result to match on.</param>
        /// <param name="ok">Value returned for the Ok case.</param>
        /// <param name="error">Value returned for the Error case.</param>
        public static TOut MatchTo<TIn, TOut>(this Result<TIn> result, TOut ok, TOut error) =>
            result.IsOk ? ok : error;

        /* Bind */

        /// <summary>
        /// Transforms the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="binder"/> function.
        /// If the input result is Ok, returns the value of the binder call (which is <see cref="Result{T}"/> of <typeparamref name="TOut"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned result.</typeparam>
        /// <param name="result">The result to bind with.</param>
        /// <param name="binder">Function called with the input result value if it's Ok case.</param>
        public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value) : Error<TOut>(result.Error);

        /* Map */

        /// <summary>
        /// Maps the value of the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="mapper"/> function.
        /// If the input result is Ok, returns the Ok case with the value of the mapper call (which is <typeparamref name="TOut"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned result.</typeparam>
        /// <param name="result">The result to map on.</param>
        /// <param name="mapper">Function called with the input result value if it's Ok case.</param>
        public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper) =>
            result.IsOk ? Ok(mapper(result.Value)) : Error<TOut>(result.Error);

        /* Getters */

        /// <summary>
        /// Gets the value of the <paramref name="result"/> if it's Ok case.
        /// If the result is Error case returns value specified by the <paramref name="whenError"/> parameter;
        /// if the parameter is not set returns the default value of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">The result to get a value from.</param>
        /// <param name="whenError">Value to return if the result is the Error case.</param>
        public static T GetOrDefault<T>(this Result<T> result, T whenError = default(T)) =>
            result.IsOk ? result.Value : whenError;

        /// <summary>
        /// Gets the value from the <paramref name="result"/> using the <paramref name="getter"/> function if it's Ok case.
        /// If the result is Error case returns value specified by the <paramref name="whenError"/> parameter;
        /// if the parameter is not set returns the default value of the type <typeparamref name="TOut"/>.
        /// </summary>
        /// <remarks>Effectively the combination of <see cref="Result.Map{TIn, TOut}(Result{TIn}, Func{TIn, TOut})"/> and <see cref="Result.GetOrDefault{T}(Result{T}, T)"/> calls.</remarks>
        /// <typeparam name="TIn">Type of the value in the result.</typeparam>
        /// <typeparam name="TOut">Type of the return value.</typeparam>
        /// <param name="result">The result to get a value from.</param>
        /// <param name="getter">Function used to get the value if the result is the Ok case.</param>
        /// <param name="whenError">Value to return if the result is the Error case.</param>
        public static TOut GetOrDefault<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> getter, TOut whenError = default(TOut)) =>
            result.IsOk ? getter(result.Value) : whenError;

        /* Side Effects */

        /// <summary>
        /// Performs the <paramref name="action"/> with the value of the <paramref name="result"/> if it's Ok case.
        /// If the result is Error case nothing happens.
        /// In both cases unmodified result is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">The result to check for a value.</param>
        /// <param name="action">Function executed if the result is Ok case.</param>
        public static Result<T> Do<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsOk)
                action(result.Value);
            return result;
        }

        /// <summary>
        /// Performs the <paramref name="action"/> if the <paramref name="result"/> is Error case.
        /// If the result is Ok case nothing happens.
        /// In both cases unmodified result is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">The result to check for a value.</param>
        /// <param name="action">Function executed if the result is Error case.</param>
        public static Result<T> DoWhenError<T>(this Result<T> result, Action<ErrorDetails> action)
        {
            if (result.IsError)
                action(result.Error);
            return result;
        }

        /* Collections */

        /// <summary>
        /// Returns the collection of values of elements from the <see cref="Result{T}"/> collection
        /// that are Ok case.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="items">Collection to filter out and map.</param>
        public static IEnumerable<T> Choose<T>(this IEnumerable<Result<T>> items) =>
            items.Where(i => i.IsOk).Select(i => i.Value);

        /// <summary>
        /// Returns the collection of values of elements from the <see cref="Result{T}"/> collection
        /// that are Error case.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="items">Collection to filter out and map.</param>
        public static IEnumerable<ErrorDetails> ChooseErrors<T>(this IEnumerable<Result<T>> items) =>
            items.Where(r => r.IsError).Select(x => x.Error);

        /// <summary>
        /// If all elements in the input collection are Ok case, returns the Ok of the collection of underlying values.
        /// Otherwise returns Error from the first element.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="items">Collection to check and map.</param>
        public static Result<IEnumerable<T>> Sequence<T>(this IEnumerable<Result<T>> items) =>
            items.Any(i => i.IsError)
            ? Error<IEnumerable<T>>(items.First(i => i.IsError).Error)
            : Ok(items.Select(i => i.Value));

        /* TryCatch */

        /// <summary>
        /// Tries to execute <paramref name="func"/>.
        /// If the execution completes without exception, returns Ok with the function result.
        /// Otherwise returns Error with details generated by <paramref name="errorHandler"/> based on the thrown exception.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="func">Function to execute.</param>
        /// <param name="errorHandler">Function that generates error details in case of exception.</param>
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

        /// <summary>
        /// Tries to execute <paramref name="func"/> with the value from the <paramref name="result"/> as an input.
        /// If the execution completes without exception, returns Ok with the function result.
        /// Otherwise returns Error with details generated by <paramref name="errorHandler"/> based on the thrown exception.
        /// If the <paramref name="result"/> is Error function is not executed and the Error is returned.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the output result.</typeparam>
        /// <param name="result">Result to take the value from.</param>
        /// <param name="func">Function to execute.</param>
        /// <param name="errorHandler">Function that generates error details in case of exception.</param>
        public static Result<TOut> TryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func, Func<TIn, Exception, ErrorDetails> errorHandler) =>
            result.Bind(value => Result.TryCatch(() => func(value), e => errorHandler(value, e)));
    }
}
