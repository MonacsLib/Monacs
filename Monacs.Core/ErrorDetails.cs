using System;

namespace Monacs.Core
{
    /// <summary>
    /// Represents the details of the error in case of failed operation.
    /// To create the instances use the factory methods from the <see cref="Errors"/> class.
    /// </summary>
    public readonly struct ErrorDetails
    {
        internal ErrorDetails(in ErrorLevel level, in Option<string> message, in Option<string> key, in Option<Exception> exception, in Option<object> metadata)
        {
            Level = level;
            Message = message;
            Key = key;
            Exception = exception;
            Metadata = metadata;
        }

        /// <summary>
        /// Contains the error severity described by <see cref="ErrorLevel"/>.
        /// </summary>
        public ErrorLevel Level { get; }

        /// <summary>
        /// Contains optional message to describe the error details.
        /// </summary>
        public Option<string> Message { get; }

        /// <summary>
        /// Contains optional error key to identify the error.
        /// </summary>
        public Option<string> Key { get; }

        /// <summary>
        /// Contains optional exception which operation ended up with.
        /// Set to None if no exception occured.
        /// </summary>
        public Option<Exception> Exception { get; }

        /// <summary>
        /// Contains optional error metadata.
        /// </summary>
        public Option<object> Metadata { get; }
    }

    /// <summary>
    /// Represents the severity of the error.
    /// </summary>
    public enum ErrorLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Fatal = 5
    }
}