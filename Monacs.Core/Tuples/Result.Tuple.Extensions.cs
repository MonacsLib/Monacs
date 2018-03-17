using System;
using static Monacs.Core.Result;

namespace Monacs.Core.Tuples
{
    public static class Result
    {
        /* Map */

        /// <summary>
        /// Maps the value of the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="mapper"/> function. 
        /// <para /> If the input result is Ok, returns the Ok case with the value of the mapper call (which is <typeparamref name="TResult"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of the value in the returned result.</typeparam>
        /// <typeparam name="TFst">Type of first value in input result.</typeparam>
        /// <typeparam name="TSnd">Type of second value in input result.</typeparam>
        /// <param name="result">The result to map on.</param>
        /// <param name="mapper">Function called with the input result value if it's Ok case.</param>
        public static Result<TResult> Map2<TResult, TFst, TSnd>(this Result<(TFst fst, TSnd snd)> result, Func<TFst, TSnd, TResult> mapper) =>
            result.IsOk ? Ok(mapper(result.Value.fst, result.Value.snd)) : Error<TResult>(result.Error);

        /// <summary>
        /// Maps the value of the <paramref name="result"/> into another <see cref="Result{T}"/> using the <paramref name="mapper"/> function. 
        /// <para /> If the input result is Ok, returns the Ok case with the value of the mapper call (which is <typeparamref name="TResult"/>).
        /// Otherwise returns Error case of the Result of <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of the value in the returned result.</typeparam>
        /// <typeparam name="TFst">Type of first value in input result.</typeparam>
        /// <typeparam name="TSnd">Type of second value in input result.</typeparam>
        /// <typeparam name="TTrd">Type of third value in input result.</typeparam>
        /// <param name="result">The result to map on.</param>
        /// <param name="mapper">Function called with the input result value if it's Ok case.</param>
        public static Result<TResult> Map3<TResult, TFst, TSnd, TTrd>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Func<TFst, TSnd, TTrd, TResult> mapper) =>
            result.IsOk ? Ok(mapper(result.Value.fst, result.Value.snd, result.Value.trd)) : Error<TResult>(result.Error);

        /* Match */

        /// <summary>
        /// Does the pattern matching on the <see cref="Result{T}"/> type.
        /// If the <paramref name="result"/> is Ok, calls <paramref name="ok"/> function
        /// with the value from the result as a parameter and returns its result.
        /// Otherwise calls <paramref name="error"/> function and returns its result.
        /// </summary>
        /// <typeparam name="TResult">Type of the returned value.</typeparam>
        /// <typeparam name="TFst">Type of the first tuple value in the result.</typeparam>
        /// <typeparam name="TSnd">Type of the second tuple value in the result.</typeparam>
        /// <param name="result">The result to match on.</param>
        /// <param name="ok">Function called for the Ok case.</param>
        /// <param name="error">Function called for the Error case.</param>
        public static TResult Match2<TResult, TFst, TSnd>(this Result<(TFst fst, TSnd snd)> result, Func<TFst, TSnd, TResult> ok, Func<ErrorDetails, TResult> error) =>
            result.IsOk ? ok(result.Value.fst, result.Value.snd) : error(result.Error);

        /// <summary>
        /// Does the pattern matching on the <see cref="Result{T}"/> type.
        /// If the <paramref name="result"/> is Ok, calls <paramref name="ok"/> function
        /// with the value from the result as a parameter and returns its result.
        /// Otherwise calls <paramref name="error"/> function and returns its result.
        /// </summary>
        /// <typeparam name="TResult">Type of the returned value.</typeparam>
        /// <typeparam name="TFst">Type of the first tuple value in the result.</typeparam>
        /// <typeparam name="TSnd">Type of the second tuple value in the result.</typeparam>
        /// <typeparam name="TTrd">Type of the third tuple value in the result.</typeparam>
        /// <param name="result">The result to match on.</param>
        /// <param name="ok">Function called for the Ok case.</param>
        /// <param name="error">Function called for the Error case.</param>
        public static TResult Match3<TResult, TFst, TSnd, TTrd>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Func<TFst, TSnd, TTrd, TResult> ok, Func<ErrorDetails, TResult> error) =>
            result.IsOk ? ok(result.Value.fst, result.Value.snd, result.Value.trd) : error(result.Error);

        /// <summary>
        /// Does the pattern matching on the <see cref="Result{T}"/> type.
        /// If the <paramref name="result"/> is Ok, returns <paramref name="ok"/> value.
        /// Otherwise returns <paramref name="error"/> value.
        /// </summary>
        /// <typeparam name="TResult">Type of the returned value.</typeparam>
        /// <typeparam name="TFst">Type of the first tuple value in the result.</typeparam>
        /// <typeparam name="TSnd">Type of the second tuple value in the result.</typeparam>
        /// <param name="result">The result to match on.</param>
        /// <param name="ok">Value returned for the Ok case.</param>
        /// <param name="error">Value returned for the Error case.</param>
        public static TResult MatchTo2<TResult, TFst, TSnd>(this Result<(TFst fst, TSnd snd)> result, TResult ok, TResult error) =>
            result.IsOk ? ok : error;

        /* Do */

        /// <summary>
        /// Performs action if the given result is on the succesful path.
        /// The action takes given result as an argument.
        /// </summary>
        /// <typeparam name="TFst">Type of first tuple result value.</typeparam>
        /// <typeparam name="TSnd">Type of second tuple result value.</typeparam>
        /// <param name="result">Given result.</param>
        /// <param name="action">Action to perform.</param>
        /// <returns>Result passed to the method as an argument.</returns>
        public static Result<(TFst, TSnd)> Do2<TFst, TSnd>(this Result<(TFst fst, TSnd snd)> result, Action<TFst, TSnd> action)
        {
            if (result.IsOk)
                action(result.Value.fst, result.Value.snd);
            return result;
        }

        /// <summary>
        /// Performs action if the given result is on the succesful path.
        /// The action takes given result as an argument.
        /// </summary>
        /// <typeparam name="TFst">Type of first tuple result value.</typeparam>
        /// <typeparam name="TSnd">Type of second tuple result value.</typeparam>
        /// <typeparam name="TTrd">Type of third tuple result value.</typeparam>
        /// <param name="result">Given result.</param>
        /// <param name="action">Action to perform.</param>
        /// <returns>Result passed to the method as an argument.</returns>
        public static Result<(TFst, TSnd, TTrd)> Do3<TFst, TSnd, TTrd>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Action<TFst, TSnd, TTrd> action)
        {
            if (result.IsOk)
                action(result.Value.fst, result.Value.snd, result.Value.trd);
            return result;
        }

        /* TryCatch */

        /// <summary>
        /// Invokes function in try/catch block and returns its result.
        /// If any <see cref="Exception"/> is raised during execution, error handler is invoked and error details are returned.
        /// </summary>
        /// <typeparam name="TResult">Type of value returned by invoked function.</typeparam>
        /// <typeparam name="TFst">Type of first tuple result value.</typeparam>
        /// <typeparam name="TSnd">Type of second tuple result value.</typeparam>
        /// <param name="tryFunc">The function to be invoked in 'try' block.</param>
        /// <param name="errorHandler">Handler invoked in 'catch' block on any raised exception.</param>
        /// <returns><see cref="Result{TValue}"/> of invoked function in try block or <see cref="ErrorDetails"/> if any exception occurs.</returns>
        public static Result<TResult> TryCatch2<TResult, TFst, TSnd>(
            this Result<(TFst fst, TSnd snd)> result,
                 Func<TFst, TSnd, TResult> tryFunc,
                 Func<TFst, TSnd, Exception, ErrorDetails> errorHandler) =>
                     result.Bind(value => TryCatch(func: () => tryFunc(value.fst, value.snd),
                                 errorHandler: err => errorHandler(value.fst, value.snd, err)));
        
        /// <summary>
        /// Invokes function in try/catch block and returns its result.
        /// If any <see cref="Exception"/> is raised during execution, error handler is invoked and error details are returned.
        /// </summary>
        /// <typeparam name="TResult">Type of value returned by invoked function.</typeparam>
        /// <typeparam name="TFst">Type of first tuple result value.</typeparam>
        /// <typeparam name="TSnd">Type of second tuple result value.</typeparam>
        /// <typeparam name="TTrd">Type of third tuple result value.</typeparam>
        /// <param name="tryFunc">The function to be invoked in 'try' block.</param>
        /// <param name="errorHandler">Handler invoked in 'catch' block on any raised exception.</param>
        /// <returns><see cref="Result{TValue}"/> of invoked function in try block or <see cref="ErrorDetails"/> if any exception occurs.</returns>
        public static Result<TResult> TryCatch3<TResult, TFst, TSnd, TTrd>(
            this Result<(TFst fst, TSnd snd, TTrd trd)> result,
                 Func<TFst, TSnd, TTrd, TResult> tryFunc,
                 Func<TFst, TSnd, TTrd, Exception, ErrorDetails> errorHandler) =>
                     result.Bind(value => TryCatch(func: () => tryFunc(value.fst, value.snd, value.trd),
                                 errorHandler: err => errorHandler(value.fst, value.snd, value.trd, err)));

        /* Bind */

        /// <summary>
        /// Applies railway pattern and binds two functions.
        /// <para />If the result of the previous function is on the success path, the received result is taken as an argument and the next function is invoked.
        /// <para />If the result of the previous function is on the failure path, the new error is created to match generic result type, but the error details remain the same.
        /// </summary>
        /// <typeparam name="TFst">Type of the first output tuple value received from previous function.</typeparam>
        /// <typeparam name="TSnd">Type of the second output tuple value received from previous function.</typeparam>
        /// <typeparam name="TOut">Type of the output value.</typeparam>
        /// <param name="result">Output of previous function</param>
        /// <param name="binder">Passes the output of first function to the next one.</param>
        /// <returns>Result of the second function or error received from the first function.</returns>
        public static Result<TOut> Bind2<TOut, TFst, TSnd>(this Result<(TFst fst, TSnd snd)> result, Func<TFst, TSnd, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value.fst, result.Value.snd) : Error<TOut>(result.Error);

        /// <summary>
        /// Applies railway pattern and binds two functions.
        /// <para />If the result of the previous function is on the success path, the received result is taken as an argument and the next function is invoked.
        /// <para />If the result of the previous function is on the failure path, the new error is created to match generic result type, but the error details remain the same.
        /// </summary>
        /// <typeparam name="TFst">Type of the first output tuple value received from previous function.</typeparam>
        /// <typeparam name="TSnd">Type of the second output tuple value received from previous function.</typeparam>
        /// <typeparam name="TTrd">Type of the second output tuple value received from previous function.</typeparam>
        /// <typeparam name="TOut">Type of the output value.</typeparam>
        /// <param name="result">Output of previous function</param>
        /// <param name="binder">Passes the output of first function to the next one.</param>
        /// <returns>Result of the second function or error received from the first function.</returns>
        public static Result<TOut> Bind3<TOut, TFst, TSnd, TTrd>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Func<TFst, TSnd, TTrd, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value.fst, result.Value.snd, result.Value.trd) : Error<TOut>(result.Error);

    }
}