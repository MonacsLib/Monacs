using System;
using static Monacs.Core.Option;

namespace Monacs.Core
{
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