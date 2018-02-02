using System;
using static Monacs.Core.Option;

namespace Monacs.Core
{
    public struct ErrorDetails
    {
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
}