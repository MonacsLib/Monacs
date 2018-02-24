using System;

namespace Monacs.Core
{
    public static partial class Result
    {
        /// <summary>
        /// Applies railway pattern and binds two functions.
        /// <para />If the result of the previous function is on the success path, the received result is taken as an argument and the next function is invoked.
        /// <para />If the result of the previous function is on the failure path, the new error is created to match generic result type, but the error details remain the same.
        /// </summary>
        /// <typeparam name="TIn">Type of the output value received from previous function.</typeparam>
        /// <typeparam name="TOut">Type of the output value.</typeparam>
        /// <param name="result">Output of previous function</param>
        /// <param name="binder">Passes the output of first function to the next one.</param>
        /// <returns>Result of the second function or error received from the first function.</returns>
        public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value) : Error<TOut>(result.Error);

        /// <summary>
        /// Two-value tuple alternative for <see cref="Bind{TIn,TOut}"/>.
        /// Applies railway pattern and binds two functions.
        /// <para />If the result of the previous function is on the success path, the received result is taken as an argument and the next function is invoked.
        /// <para />If the result of the previous function is on the failure path, the new error is created to match generic result type, but the error details remain the same.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TFst">Type of the first value in function tuple argument.</typeparam>
        /// <typeparam name="TSnd">Type of the second value in function tuple argument.</typeparam>
        /// <param name="result">Output of previous function</param>
        /// <param name="binder">Passes the output of first function to the next one.</param>
        /// <returns>Result of the second function or error received from the first function.</returns>
        public static Result<TOut> Bind2<TOut, TFst, TSnd>(this Result<(TFst fst, TSnd snd)> result, Func<TFst, TSnd, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value.fst, result.Value.snd) : Error<TOut>(result.Error);

        /// <summary>
        /// Three-value tuple alternative for <see cref="Bind{TIn,TOut}"/>.
        /// Applies railway pattern and binds two functions.
        /// <para />If the result of the previous function is on the success path, the received result is taken as an argument and the next function is invoked.
        /// <para />If the result of the previous function is on the failure path, the new error is created to match generic result type, but the error details remain the same.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TFst">Type of the first value in function tuple argument.</typeparam>
        /// <typeparam name="TSnd">Type of the second value in function tuple argument.</typeparam>
        /// <typeparam name="TTrd">Type of the third value in function tuple argument.</typeparam>
        /// <param name="result">Output of previous function</param>
        /// <param name="binder">Passes the output of first function to the next one.</param>
        /// <returns>Result of the second function or error received from the first function.</returns>
        public static Result<TOut> Bind3<TOut, TFst, TSnd, TTrd>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Func<TFst, TSnd, TTrd, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value.fst, result.Value.snd, result.Value.trd) : Error<TOut>(result.Error);
    }
}
