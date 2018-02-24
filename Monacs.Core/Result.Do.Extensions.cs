using System;

namespace Monacs.Core
{
    public static partial class Result
    {
        /// <summary>
        /// Performs action if the given result is on the succesful path.
        /// The action takes given result as an argument.
        /// </summary>
        /// <typeparam name="T">Type of result value.</typeparam>
        /// <param name="result">Given result.</param>
        /// <param name="action">Action to perform.</param>
        /// <returns>Result passed to the method as an argument.</returns>
        public static Result<T> Do<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsOk)
                action(result.Value);
            return result;
        }

        /// <summary>
        /// Performs action if the given result is on the failure path.
        /// The action takes given results error as an argument.
        /// </summary>
        /// <typeparam name="T">Type of result value.</typeparam>
        /// <param name="result">Given result.</param>
        /// <param name="action">Action to perform.</param>
        /// <returns>Result passed to the method as an argument.</returns>
        public static Result<T> DoWhenError<T>(this Result<T> result, Action<ErrorDetails> action)
        {
            if (result.IsError)
                action(result.Error);
            return result;
        }


        /* Tuple variations */

        /// <summary>
        /// Performs action if the given result is on the failure path.
        /// </summary>
        /// <typeparam name="TFst">Type of the first value in function tuple argument.</typeparam>
        /// <typeparam name="TSnd">Type of the second value in function tuple argument.</typeparam>
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
        /// Performs action if the given result is on the failure path.
        /// </summary>
        /// <typeparam name="TFst">Type of the first value in function tuple argument.</typeparam>
        /// <typeparam name="TSnd">Type of the second value in function tuple argument.</typeparam>
        /// <typeparam name="TTrd">Type of the third value in function tuple argument.</typeparam>
        /// <param name="result">Given result.</param>
        /// <param name="action">Action to perform.</param>
        /// <returns>Result passed to the method as an argument.</returns>
        public static Result<(TFst, TSnd, TTrd)> Do3<TFst, TSnd, TTrd>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Action<TFst, TSnd, TTrd> action)
        {
            if (result.IsOk)
                action(result.Value.fst, result.Value.snd, result.Value.trd);
            return result;
        }
    }
}
