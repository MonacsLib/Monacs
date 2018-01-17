using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monacs.Core
{
    public static partial class Result
    {
        public static class Unit
        {
            public static Result<Monacs.Core.Unit.Unit> Ok() =>
                Result.Ok(Monacs.Core.Unit.Unit.Default);

            public static Result<Monacs.Core.Unit.Unit> Error(ErrorDetails error) =>
                Result.Error<Monacs.Core.Unit.Unit>(error);
        }

        public static Result<Monacs.Core.Unit.Unit> Ignore<TValue>(this Result<TValue> result) =>
            result.Map(_ => Monacs.Core.Unit.Unit.Default);
    }
}
