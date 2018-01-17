using System;
using static Monacs.Core.Option;

namespace Monacs.Core
{
    public struct ErrorDetails
    {
        public ErrorDetails(ErrorLevel level, Option<string> message)
        {
            Level = level;
            Message = message;
            Key = None<string>();
            Exception = None<Exception>();
        }

        public ErrorDetails(ErrorLevel level, Option<Exception> exception)
        {
            Level = level;
            Message = None<string>();
            Key = None<string>();
            Exception = exception;
        }

        public ErrorDetails(ErrorLevel level, Option<string> message, Option<string> key)
        {
            Level = level;
            Message = message;
            Key = key;
            Exception = None<Exception>();
        }

        public ErrorDetails(ErrorLevel level, Option<string> message, Option<Exception> exception)
        {
            Level = level;
            Message = message;
            Key = None<string>();
            Exception = exception;
        }

        public ErrorDetails(ErrorLevel level, Option<string> message, Option<string> key, Option<Exception> exception)
        {
            Level = level;
            Message = message;
            Key = key;
            Exception = exception;
        }

        public ErrorLevel Level { get; }

        public Option<string> Message { get; }

        public Option<string> Key { get; }

        public Option<Exception> Exception { get; }
    }

    public enum ErrorLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Fatal = 5
    }

    public static class Errors
    {
        public static ErrorDetails Trace(string message = null, string key = null, Exception exception = null) =>
            new ErrorDetails(ErrorLevel.Trace, message.ToOption(), key.ToOption(), exception.ToOption());

        public static ErrorDetails Debug(string message = null, string key = null, Exception exception = null) =>
            new ErrorDetails(ErrorLevel.Debug, message.ToOption(), key.ToOption(), exception.ToOption());

        public static ErrorDetails Info(string message = null, string key = null, Exception exception = null) =>
            new ErrorDetails(ErrorLevel.Info, message.ToOption(), key.ToOption(), exception.ToOption());

        public static ErrorDetails Warn(string message = null, string key = null, Exception exception = null) =>
            new ErrorDetails(ErrorLevel.Warn, message.ToOption(), key.ToOption(), exception.ToOption());

        public static ErrorDetails Error(string message = null, string key = null, Exception exception = null) =>
            new ErrorDetails(ErrorLevel.Error, message.ToOption(), key.ToOption(), exception.ToOption());

        public static ErrorDetails Fatal(string message = null, string key = null, Exception exception = null) =>
            new ErrorDetails(ErrorLevel.Fatal, message.ToOption(), key.ToOption(), exception.ToOption());
    }
}