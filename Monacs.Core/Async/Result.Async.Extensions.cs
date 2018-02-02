using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Monacs.Core.Result;

namespace Monacs.Core.Async
{
    public static class Result
    {
        /* BindAsync */

        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func) =>
            result.IsOk ? await func(result.Value) : Error<TOut>(result.Error);

        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> resultAsync, Func<TIn, Task<Result<TOut>>> func) =>
            await (await resultAsync).BindAsync(func);

        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> resultAsync, Func<TIn, Result<TOut>> func) =>
            (await resultAsync).Bind(func);

        /* MapAsync */

        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> func) =>
            (result.IsOk ? Ok(await func(result.Value)) : Error<TOut>(result.Error));

        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> resultAsync, Func<TIn, Task<TOut>> func) =>
            await (await resultAsync).MapAsync(func);

        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> resultAsync, Func<TIn, TOut> func) =>
            (await resultAsync).Map(func);

        /* MatchAsync */

        public static async Task<TOut> MatchAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> ok, Func<ErrorDetails, Task<TOut>> error) =>
            result.IsOk ? await ok(result.Value) : await error(result.Error);

        public static async Task<TOut> MatchAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> ok, Func<ErrorDetails, TOut> error) =>
            result.IsOk ? await ok(result.Value) : error(result.Error);

        public static async Task<TOut> MatchAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> ok, Func<ErrorDetails, Task<TOut>> error) =>
            result.IsOk ? ok(result.Value) : await error(result.Error);

        public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> ok, Func<ErrorDetails, TOut> error) =>
            (await result).Match(ok, error);

        public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> ok, Func<ErrorDetails, Task<TOut>> error) =>
            await (await result).MatchAsync(ok, error);

        public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> ok, Func<ErrorDetails, TOut> error) =>
            await (await result).MatchAsync(ok, error);

        public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> ok, Func<ErrorDetails, Task<TOut>> error) =>
            await (await result).MatchAsync(ok, error);

        /* IgnoreAsync */

        public static async Task<Result<Monacs.Core.Unit.Unit>> IgnoreAsync<T>(this Task<Result<T>> result) =>
            (await result).Map(_ => Monacs.Core.Unit.Unit.Default);

        /* Side Effects */

        public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> resultAsync, Action<T> action) =>
            (await resultAsync).Do(action);

        public static async Task<Result<T>> DoAsync<T>(this Result<T> result, Func<T, Task> action)
        {
            if (result.IsOk)
                await action(result.Value);
            return result;
        }

        public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> resultAsync, Func<T, Task> action) =>
            await (await resultAsync).DoAsync(action);

        public static async Task<Result<T>> DoWhenErrorAsync<T>(this Task<Result<T>> resultAsync, Action<ErrorDetails> action) =>
            (await resultAsync).DoWhenError(action);

        public static async Task<Result<T>> DoWhenErrorAsync<T>(this Result<T> result, Func<ErrorDetails, Task> action)
        {
            if (result.IsError)
                await action(result.Error);
            return result;
        }

        public static async Task<Result<T>> DoWhenErrorAsync<T>(this Task<Result<T>> resultAsync, Func<ErrorDetails, Task> action) =>
            await (await resultAsync).DoWhenErrorAsync(action);

        /* Flip */

        public static async Task<Result<T>> FlipAsync<T>(this Result<Task<T>> result) =>
            result.IsOk
            ? Ok(await result.Value)
            : Error<T>(result.Error);

        /* TryCatchAsync */

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

        public static async Task<Result<TOut>> TryCatchAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> func, Func<TIn, Exception, ErrorDetails> errorHandler) =>
            await result.BindAsync(value => TryCatchAsync(() => func(value), e => errorHandler(value, e)));
    }
}
