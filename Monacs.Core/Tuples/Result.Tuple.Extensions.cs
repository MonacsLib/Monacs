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
        public static TVal Match2<TFst, TSnd, TVal>(this Result<(TFst fst, TSnd snd)> result, Func<TFst, TSnd, TVal> ok, Func<ErrorDetails, TVal> error) =>
            result.IsOk ? ok(result.Value.fst, result.Value.snd) : error(result.Error);

        /* Match3 */
        public static TVal Match3<TFst, TSnd, TTrd, TVal>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Func<TFst, TSnd, TTrd, TVal> ok, Func<ErrorDetails, TVal> error) =>
            result.IsOk ? ok(result.Value.fst, result.Value.snd, result.Value.trd) : error(result.Error);

        /* MatchTo2 */
        public static TVal MatchTo2<TFst, TSnd, TVal>(this Result<(TFst fst, TSnd snd)> result, TVal some, TVal none) =>
            result.IsOk ? some : none;

        /* Do2 */
        public static Result<(TFst, TSnd)> Do2<TFst, TSnd>(this Result<(TFst fst, TSnd snd)> result, Action<TFst, TSnd> action)
        {
            if (result.IsOk)
                action(result.Value.fst, result.Value.snd);
            return result;
        }

        /* Do3 */
        public static Result<(TFst, TSnd, TTrd)> Do3<TFst, TSnd, TTrd>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Action<TFst, TSnd, TTrd> action)
        {
            if (result.IsOk)
                action(result.Value.fst, result.Value.snd, result.Value.trd);
            return result;
        }

        /* TryCatch2 */
        public static Result<TValue> TryCatch2<TValue, TFst, TSnd>(
            this Result<(TFst fst, TSnd snd)> result,
                 Func<TFst, TSnd, TValue> tryFunc,
                 Func<TFst, TSnd, Exception, ErrorDetails> errorHandler) =>
                     result.Bind(value => TryCatch(func: () => tryFunc(value.fst, value.snd),
                                 errorHandler: err => errorHandler(value.fst, value.snd, err)));

        /* TryCatch3 */
        public static Result<TValue> TryCatch3<TValue, TFst, TSnd, TTrd>(
            this Result<(TFst fst, TSnd snd, TTrd trd)> result,
                 Func<TFst, TSnd, TTrd, TValue> tryFunc,
                 Func<TFst, TSnd, TTrd, Exception, ErrorDetails> errorHandler) =>
                     result.Bind(value => TryCatch(func: () => tryFunc(value.fst, value.snd, value.trd),
                                 errorHandler: err => errorHandler(value.fst, value.snd, value.trd, err)));

        /* Bind2 */
        public static Result<TOut> Bind2<TOut, TFst, TSnd>(this Result<(TFst fst, TSnd snd)> result, Func<TFst, TSnd, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value.fst, result.Value.snd) : Error<TOut>(result.Error);

        /* Bind3 */
        public static Result<TOut> Bind3<TOut, TFst, TSnd, TTrd>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Func<TFst, TSnd, TTrd, Result<TOut>> binder) =>
            result.IsOk ? binder(result.Value.fst, result.Value.snd, result.Value.trd) : Error<TOut>(result.Error);

    }
}