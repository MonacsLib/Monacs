using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core.Unit
{
    public static class Result
    {
        public static Result<Unit> Ok() =>
            Core.Result.Ok(Unit.Default);

        public static Result<Unit> Error(ErrorDetails error) =>
            Core.Result.Error<Unit>(error);

        public static Result<Unit> Ignore<T>(this Result<T> result) =>
            result.Map(_ => Unit.Default);
    }
}
