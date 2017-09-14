using System;

namespace Monacs.Core
{
    public struct Error
    {
        public Error(Option<string> message, Option<string> key, Option<Exception> exception)
        {
            Message = message;
            Key = key;
            Exception = exception;
        }

        public Option<string> Message { get; }

        public Option<string> Key { get; }

        public Option<Exception> Exception { get; }
    }

    public static class Errors
    {
        public static Error Error(string message = null, string key = null, Exception exception = null) =>
            new Error(message.ToOption(), key.ToOption(), exception.ToOption());
    }
}