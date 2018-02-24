using System;

namespace Monacs.Core
{
    public static partial class Result
    {
        /// <summary>
        /// Invokes function in try/catch block and returns its result.
        /// If any <see cref="Exception"/> is raised during execution, error handler is invoked and error details are returned.
        /// </summary>
        /// <typeparam name="TValue">Type of value returned by invoked function.</typeparam>
        /// <param name="func">The function to be invoked in 'try' block.</param>
        /// <param name="errorHandler">Handler invoked in 'catch' block on any raised exception.</param>
        /// <returns><see cref="Result{TValue}"/> of invoked function in try block or <see cref="ErrorDetails"/> if any exception occurs.</returns>
        public static Result<TValue> TryCatch<TValue>(Func<TValue> func, Func<Exception, ErrorDetails> errorHandler)
        {
            try
            {
                return Ok(func());
            }
            catch (Exception ex)
            {
                return Error<TValue>(errorHandler(ex));
            }
        }

        /// <summary>
        /// Invokes function in try/catch block and returns its result.
        /// If any <see cref="Exception"/> is raised during execution, error handler is invoked and error details are returned.
        /// </summary>
        /// <typeparam name="TArg">Type of an argument accepted by invoked function.</typeparam>
        /// <typeparam name="TValue">Type of value returned by invoked function.</typeparam>
        /// <param name="result">Result of previous operation.</param>
        /// <param name="func">The function to be invoked in 'try' block.</param>
        /// <param name="errorHandler">Handler invoked in 'catch' block on any raised exception.</param>
        /// <returns><see cref="Result{TValue}"/> of invoked function in try block or <see cref="ErrorDetails"/> if any exception occurs.</returns>
        public static Result<TValue> TryCatch<TArg, TValue>(this Result<TArg> result, Func<TArg, TValue> func, Func<TArg, Exception, ErrorDetails> errorHandler) =>
            result.Bind(value => Result.TryCatch(() => func(value), e => errorHandler(value, e)));


        /* Tuple variations */

        /// <summary>
        /// Invokes function in try/catch block and returns its result.
        /// </summary>
        /// <typeparam name="TValue">Type of value returned by invoked function.</typeparam>
        /// <typeparam name="TFst">Type of first value in the function tuple argument.</typeparam>
        /// <typeparam name="TSnd">Type of second value in the function tuple argument.</typeparam>
        /// <param name="result">Result of previous operation.</param>
        /// <param name="tryFunc">The function to be invoked in 'try' block.</param>
        /// <param name="errorHandler">Handler invoked in 'catch' block on any raised exception.</param>
        /// <returns><see cref="Result{TValue}"/> of invoked function in try block or <see cref="ErrorDetails"/> if any exception occurs.</returns>
        public static Result<TValue> TryCatch2<TValue, TFst, TSnd>(
            this Result<(TFst fst, TSnd snd)> result,
                 Func<TFst, TSnd, TValue> tryFunc,
                 Func<TFst, TSnd, Exception, ErrorDetails> errorHandler) =>
                     result.Bind(value => TryCatch(func: () => tryFunc(value.fst, value.snd),
                                 errorHandler: err => errorHandler(value.fst, value.snd, err)));

        /// <summary>
        /// Invokes function in try/catch block and returns its result.
        /// </summary>
        /// <typeparam name="TValue">Type of value returned by invoked function.</typeparam>
        /// <typeparam name="TFst">Type of first value in the function tuple argument.</typeparam>
        /// <typeparam name="TSnd">Type of second value in the function tuple argument.</typeparam>
        /// <typeparam name="TTrd">Type of third value in the function tuple argument.</typeparam>
        /// <param name="result">Result of previous operation.</param>
        /// <param name="tryFunc">The function to be invoked in 'try' block.</param>
        /// <param name="errorHandler">Handler invoked in 'catch' block on any raised exception.</param>
        /// <returns><see cref="Result{TValue}"/> of invoked function in try block or <see cref="ErrorDetails"/> if any exception occurs.</returns>
        public static Result<TValue> TryCatch3<TValue, TFst, TSnd, TTrd>(
            this Result<(TFst fst, TSnd snd, TTrd trd)> result,
                 Func<TFst, TSnd, TTrd, TValue> tryFunc,
                 Func<TFst, TSnd, TTrd, Exception, ErrorDetails> errorHandler) =>
                     result.Bind(value => TryCatch(func: () => tryFunc(value.fst, value.snd, value.trd),
                                 errorHandler: err => errorHandler(value.fst, value.snd, value.trd, err)));
    }
}
