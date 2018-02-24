using System;
using static Monacs.Core.Result;

namespace Monacs.Core.Tuples
{
    public static class Result
    {
        /* Map2 */
        public static Result<TResult> Map2<TResult, T1, T2>(this Result<(T1, T2)> result, Func<T1, T2, TResult> func) =>
            result.IsOk ? Ok(func(result.Value.Item1, result.Value.Item2)) : Error<TResult>(result.Error);

        /* Map3 */
        public static Result<TResult> Map3<TResult, T1, T2, T3>(this Result<(T1, T2, T3)> result, Func<T1, T2, T3, TResult> func) =>
            result.IsOk ? Ok(func(result.Value.Item1, result.Value.Item2, result.Value.Item3)) : Error<TResult>(result.Error);


        /* Match2 */

        public static TVal Match2<TFst, TSnd, TVal>(
            this Result<(TFst fst, TSnd snd)> result,
                 Func<TFst, TSnd, TVal> ok,
                 Func<ErrorDetails, TVal> error) =>
                      result.IsOk
                      ? ok(result.Value.fst, result.Value.snd)
                      : error(result.Error);

        public static TVal MatchTo2<TFst, TSnd, TVal>(this Result<(TFst fst, TSnd snd)> result, TVal some, TVal none) =>
            result.IsOk ? some : none;


        /* Match3 */

        public static TVal Match3<TFst, TSnd, TTrd, TVal>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Func<TFst, TSnd, TTrd, TVal> ok, Func<ErrorDetails, TVal> error) =>
            result.IsOk ? ok(result.Value.fst, result.Value.snd, result.Value.trd) : error(result.Error);
    }
}