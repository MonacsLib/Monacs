using System;

namespace Monacs.Core
{
    /// <summary>
    /// Contains factory methods to create instances of <see cref="ErrorDetails"/> in a more convenient way.
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// Creates <see cref="ErrorDetails"/> with level <see cref="ErrorLevel.Trace"/>.
        /// </summary>
        /// <param name="message">Optional error message</param>
        /// <param name="key">Optional key to identify the error</param>
        /// <param name="exception">Optional exception which caused the error</param>
        /// <param name="metadata">Optional error metadata</param>
        public static ErrorDetails Trace(string message = null, string key = null, Exception exception = null, object metadata = null) =>
            new ErrorDetails(ErrorLevel.Trace, message.ToOption(), key.ToOption(), exception.ToOption(), metadata.ToOption());

        /// <summary>
        /// Creates <see cref="ErrorDetails"/> with level <see cref="ErrorLevel.Debug"/>.
        /// </summary>
        /// <param name="message">Optional error message</param>
        /// <param name="key">Optional key to identify the error</param>
        /// <param name="exception">Optional exception which caused the error</param>
        /// <param name="metadata">Optional error metadata</param>
        public static ErrorDetails Debug(string message = null, string key = null, Exception exception = null, object metadata = null) =>
            new ErrorDetails(ErrorLevel.Debug, message.ToOption(), key.ToOption(), exception.ToOption(), metadata.ToOption());

        /// <summary>
        /// Creates <see cref="ErrorDetails"/> with level <see cref="ErrorLevel.Info"/>.
        /// </summary>
        /// <param name="message">Optional error message</param>
        /// <param name="key">Optional key to identify the error</param>
        /// <param name="exception">Optional exception which caused the error</param>
        /// <param name="metadata">Optional error metadata</param>
        public static ErrorDetails Info(string message = null, string key = null, Exception exception = null, object metadata = null) =>
            new ErrorDetails(ErrorLevel.Info, message.ToOption(), key.ToOption(), exception.ToOption(), metadata.ToOption());

        /// <summary>
        /// Creates <see cref="ErrorDetails"/> with level <see cref="ErrorLevel.Warn"/>.
        /// </summary>
        /// <param name="message">Optional error message</param>
        /// <param name="key">Optional key to identify the error</param>
        /// <param name="exception">Optional exception which caused the error</param>
        /// <param name="metadata">Optional error metadata</param>
        public static ErrorDetails Warn(string message = null, string key = null, Exception exception = null, object metadata = null) =>
            new ErrorDetails(ErrorLevel.Warn, message.ToOption(), key.ToOption(), exception.ToOption(), metadata.ToOption());

        /// <summary>
        /// Creates <see cref="ErrorDetails"/> with level <see cref="ErrorLevel.Error"/>.
        /// </summary>
        /// <param name="message">Optional error message</param>
        /// <param name="key">Optional key to identify the error</param>
        /// <param name="exception">Optional exception which caused the error</param>
        /// <param name="metadata">Optional error metadata</param>
        public static ErrorDetails Error(string message = null, string key = null, Exception exception = null, object metadata = null) =>
            new ErrorDetails(ErrorLevel.Error, message.ToOption(), key.ToOption(), exception.ToOption(), metadata.ToOption());

        /// <summary>
        /// Creates <see cref="ErrorDetails"/> with level <see cref="ErrorLevel.Fatal"/>.
        /// </summary>
        /// <param name="message">Optional error message</param>
        /// <param name="key">Optional key to identify the error</param>
        /// <param name="exception">Optional exception which caused the error</param>
        /// <param name="metadata">Optional error metadata</param>
        public static ErrorDetails Fatal(string message = null, string key = null, Exception exception = null, object metadata = null) =>
            new ErrorDetails(ErrorLevel.Fatal, message.ToOption(), key.ToOption(), exception.ToOption(), metadata.ToOption());
    }
}