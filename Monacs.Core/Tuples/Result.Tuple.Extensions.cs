using System;
using System.Collections.Generic;
using System.Linq;
using static Monacs.Core.Result;

namespace Monacs.Core.Tuples
{
    public static class Result
    {
        /* Bind2 */
        public static Result<TResult> Bind2<TResult, T1, T2>(this Result<(T1, T2)> result, Func<T1, T2, Result<TResult>> func) =>
            result.IsOk ? func(result.Value.Item1, result.Value.Item2) : Error<TResult>(result.Error);

        /* Bind3 */
        public static Result<TResult> Bind3<TResult, T1, T2, T3>(this Result<(T1, T2, T3)> result, Func<T1, T2, T3, Result<TResult>> func) =>
            result.IsOk ? func(result.Value.Item1, result.Value.Item2, result.Value.Item3) : Error<TResult>(result.Error);

        /* Map2 */
        public static Result<TResult> Map2<TResult, T1, T2>(this Result<(T1, T2)> result, Func<T1, T2, TResult> func) =>
            result.IsOk ? Ok(func(result.Value.Item1, result.Value.Item2)) : Error<TResult>(result.Error);

        /* Map3 */
        public static Result<TResult> Map3<TResult, T1, T2, T3>(this Result<(T1, T2, T3)> result, Func<T1, T2, T3, TResult> func) =>
            result.IsOk ? Ok(func(result.Value.Item1, result.Value.Item2, result.Value.Item3)) : Error<TResult>(result.Error);
    }
}
