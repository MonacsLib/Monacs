using System;
using System.Threading.Tasks;
using static Monacs.Core.Result;

namespace Monacs.Core
{
    ///<summary>
    /// Contains the set of async extensions to work with the <see cref="Result{T}" /> type.
    ///</summary>
    public static class AsyncResult
    {
        /* BindAsync */

        /// <summary>
        /// Transforms the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="binder"/> function.
        /// If the input result is Ok, returns the value of the binder call (which is <see cref="Result{T}"/> of <typeparamref name="TOut"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned result.</typeparam>
        /// <param name="result">The result to bind with.</param>
        /// <param name="binder">Function called with the input result value if it's Ok case.</param>
        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> binder) =>
            result.IsOk ? await binder(result.Value) : Error<TOut>(result.Error);

        /// <summary>
        /// Transforms the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="binder"/> function.
        /// If the input result is Ok, returns the value of the binder call (which is <see cref="Result{T}"/> of <typeparamref name="TOut"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned result.</typeparam>
        /// <param name="result">The result to bind with.</param>
        /// <param name="binder">Function called with the input result value if it's Ok case.</param>
        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<Result<TOut>>> binder) =>
            await (await result).BindAsync(binder);

        /// <summary>
        /// Transforms the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="binder"/> function.
        /// If the input result is Ok, returns the value of the binder call (which is <see cref="Result{T}"/> of <typeparamref name="TOut"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned result.</typeparam>
        /// <param name="result">The result to bind with.</param>
        /// <param name="binder">Function called with the input result value if it's Ok case.</param>
        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Result<TOut>> binder) =>
            (await result).Bind(binder);

        /* MapAsync */

        /// <summary>
        /// Maps the value of the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="mapper"/> function.
        /// If the input result is Ok, returns the Ok case with the value of the mapper call (which is <typeparamref name="TOut"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned result.</typeparam>
        /// <param name="result">The result to map on.</param>
        /// <param name="mapper">Function called with the input result value if it's Ok case.</param>
        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> mapper) =>
            (result.IsOk ? Ok(await mapper(result.Value)) : Error<TOut>(result.Error));

        /// <summary>
        /// Maps the value of the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="mapper"/> function.
        /// If the input result is Ok, returns the Ok case with the value of the mapper call (which is <typeparamref name="TOut"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned result.</typeparam>
        /// <param name="result">The result to map on.</param>
        /// <param name="mapper">Function called with the input result value if it's Ok case.</param>
        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> mapper) =>
            await (await result).MapAsync(mapper);

        /// <summary>
        /// Maps the value of the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="mapper"/> function.
        /// If the input result is Ok, returns the Ok case with the value of the mapper call (which is <typeparamref name="TOut"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input result.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned result.</typeparam>
        /// <param name="result">The result to map on.</param>
        /// <param name="mapper">Function called with the input result value if it's Ok case.</param>
        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> mapper) =>
            (await result).Map(mapper);

        /* MatchAsync */

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
        public static async Task<TOut> MatchAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> ok, Func<ErrorDetails, Task<TOut>> error) =>
            result.IsOk ? await ok(result.Value) : await error(result.Error);

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
        public static async Task<TOut> MatchAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> ok, Func<ErrorDetails, TOut> error) =>
            result.IsOk ? await ok(result.Value) : error(result.Error);

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
        public static async Task<TOut> MatchAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> ok, Func<ErrorDetails, Task<TOut>> error) =>
            result.IsOk ? ok(result.Value) : await error(result.Error);

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
        public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> ok, Func<ErrorDetails, TOut> error) =>
            (await result).Match(ok, error);

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
        public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> ok, Func<ErrorDetails, Task<TOut>> error) =>
            await (await result).MatchAsync(ok, error);

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
        public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> ok, Func<ErrorDetails, TOut> error) =>
            await (await result).MatchAsync(ok, error);

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
        public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> ok, Func<ErrorDetails, Task<TOut>> error) =>
            await (await result).MatchAsync(ok, error);

        /* IgnoreAsync */

        ///<summary>
        /// Rejects the value of the <see cref="Result{T}" /> and returns <see cref="Result{Unit}" /> instead.
        /// If the input <see cref="Result{T}" /> is Error then the error details are preserved.
        ///</summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="result">The result of which the value should be ignored.</param>
        public static async Task<Result<Monacs.Core.Unit.Unit>> IgnoreAsync<T>(this Task<Result<T>> result) =>
            (await result).Map(_ => Monacs.Core.Unit.Unit.Default);

        /* Side Effects */

        /// <summary>
        /// Performs the <paramref name="action"/> with the value of the <paramref name="result"/> if it's Ok case.
        /// If the result is Error case nothing happens.
        /// In both cases unmodified result is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">The result to check for a value.</param>
        /// <param name="action">Function executed if the result is Ok case.</param>
        public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Action<T> action) =>
            (await result).Do(action);

        /// <summary>
        /// Performs the <paramref name="action"/> with the value of the <paramref name="result"/> if it's Ok case.
        /// If the result is Error case nothing happens.
        /// In both cases unmodified result is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">The result to check for a value.</param>
        /// <param name="action">Function executed if the result is Ok case.</param>
        public static async Task<Result<T>> DoAsync<T>(this Result<T> result, Func<T, Task> action)
        {
            if (result.IsOk)
                await action(result.Value);
            return result;
        }

        /// <summary>
        /// Performs the <paramref name="action"/> with the value of the <paramref name="result"/> if it's Ok case.
        /// If the result is Error case nothing happens.
        /// In both cases unmodified result is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">The result to check for a value.</param>
        /// <param name="action">Function executed if the result is Ok case.</param>
        public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<T, Task> action) =>
            await (await result).DoAsync(action);

        /// <summary>
        /// Performs the <paramref name="action"/> if the <paramref name="result"/> is Error case.
        /// If the result is Ok case nothing happens.
        /// In both cases unmodified result is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">The result to check for a value.</param>
        /// <param name="action">Function executed if the result is Error case.</param>
        public static async Task<Result<T>> DoWhenErrorAsync<T>(this Task<Result<T>> result, Action<ErrorDetails> action) =>
            (await result).DoWhenError(action);

        /// <summary>
        /// Performs the <paramref name="action"/> if the <paramref name="result"/> is Error case.
        /// If the result is Ok case nothing happens.
        /// In both cases unmodified result is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">The result to check for a value.</param>
        /// <param name="action">Function executed if the result is Error case.</param>
        public static async Task<Result<T>> DoWhenErrorAsync<T>(this Result<T> result, Func<ErrorDetails, Task> action)
        {
            if (result.IsError)
                await action(result.Error);
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
        public static async Task<Result<T>> DoWhenErrorAsync<T>(this Task<Result<T>> result, Func<ErrorDetails, Task> action) =>
            await (await result).DoWhenErrorAsync(action);

        /* Flip */

        /// <summary>
        /// Transforms <see cref="Result{T}"/> with async value inside to <see cref="Task{T}"/> of the result,
        /// preserving original result's state and value.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="result">Result to take the value from.</param>
        public static async Task<Result<T>> FlipAsync<T>(this Result<Task<T>> result) =>
            result.IsOk
            ? Ok(await result.Value)
            : Error<T>(result.Error);

        /* TryCatchAsync */

        /// <summary>
        /// Tries to execute <paramref name="func"/>.
        /// If the execution completes without exception, returns Ok with the function result.
        /// Otherwise returns Error with details generated by <paramref name="errorHandler"/> based on the thrown exception.
        /// </summary>
        /// <typeparam name="T">Type of the value in the result.</typeparam>
        /// <param name="func">Function to execute.</param>
        /// <param name="errorHandler">Function that generates error details in case of exception.</param>
        public static async Task<Result<T>> TryCatchAsync<T>(Func<Task<T>> func, Func<Exception, ErrorDetails> errorHandler)
        {
            try
            {
                var result = await func();
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
        public static async Task<Result<TOut>> TryCatchAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> func, Func<TIn, Exception, ErrorDetails> errorHandler) =>
            await result.BindAsync(value => TryCatchAsync(() => func(value), e => errorHandler(value, e)));
    }
}
