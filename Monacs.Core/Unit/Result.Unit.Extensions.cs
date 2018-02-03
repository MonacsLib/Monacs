using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core.Unit
{
    ///<summary>
    /// Extensions for Result<Unit> type.
    ///</summary>
    public static class Result
    {
        ///<summary>
        /// Creates successful Result<Unit>.
        ///</summary>
        public static Result<Unit> Ok() =>
            Core.Result.Ok(Unit.Default);

        ///<summary>
        /// Creates failed Result<Unit> with provided error details.
        ///</summary>
        public static Result<Unit> Error(ErrorDetails error) =>
            Core.Result.Error<Unit>(error);

        ///<summary>
        /// Rejects the value of the Result<T> and returns Result<Unit> instead.
        /// If the input Result<T> is Error then the error details are preserved.
        ///</summary>
        public static Result<Unit> Ignore<T>(this Result<T> result) =>
            result.Map(_ => Unit.Default);
    }
}
