namespace Monacs.Core
{
    ///<summary>
    /// Contains the set of extensions to work with the <see cref="Result{Unit}" /> type.
    ///</summary>
    public static class UnitResult
    {
        ///<summary>
        /// Creates the Ok case instance of the <see cref="Result{Unit}" />.
        ///</summary>
        public static Result<Unit.Unit> Ok() =>
            Result.Ok(Unit.Unit.Default);

        ///<summary>
        /// Creates the Error case instance of the <see cref="Result{Unit}" /> type, containing error instead of value.
        ///</summary>
        /// <param name="error">Details of the error.</param>
        public static Result<Unit.Unit> Error(ErrorDetails error) =>
            Result.Error<Unit.Unit>(error);

        ///<summary>
        /// Rejects the value of the <see cref="Result{T}" /> and returns <see cref="Result{Unit}" /> instead.
        /// If the input <see cref="Result{T}" /> is Error then the error details are preserved.
        ///</summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="result">The result of which the value should be ignored.</param>
        public static Result<Unit.Unit> Ignore<T>(this Result<T> result) =>
            result.Map(_ => Unit.Unit.Default);
    }
}
