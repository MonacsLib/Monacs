using System;

namespace Monacs.Core
{
    public static partial class Result
    {
        public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> ok, Func<ErrorDetails, TOut> error) =>
            result.IsOk ? ok(result.Value) : error(result.Error);

        public static TVal Match2<TFst, TSnd, TVal>(
            this Result<(TFst fst, TSnd snd)> result,
            Func<TFst, TSnd, TVal> ok,
            Func<ErrorDetails, TVal> error) =>
            result.IsOk
                ? ok(result.Value.fst, result.Value.snd)
                : error(result.Error);

        public static TVal Match3<TFst, TSnd, TTrd, TVal>(this Result<(TFst fst, TSnd snd, TTrd trd)> result, Func<TFst, TSnd, TTrd, TVal> ok, Func<ErrorDetails, TVal> error) =>
            result.IsOk ? ok(result.Value.fst, result.Value.snd, result.Value.trd) : error(result.Error);

        public static TOut MatchTo<TIn, TOut>(this Result<TIn> result, TOut ok, TOut error) =>
            result.IsOk ? ok : error;
        

        public static TVal MatchTo2<TFst, TSnd, TVal>(this Result<(TFst fst, TSnd snd)> result, TVal some, TVal none) =>
            result.IsOk ? some : none;
    }
}
