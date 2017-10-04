using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
    public struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
    {
        internal Option(T value)
        {
            Value = value;
            IsSome = true;
        }

        public T Value { get; }

        public bool IsSome { get; }

        public bool IsNone => !IsSome;

        public override string ToString() =>
            IsSome
            ? $"Some({Value})"
            : $"None<{typeof(T).Name}>";

        public bool Equals(Option<T> other) =>
            (IsNone && other.IsNone) || (IsSome && other.IsSome && EqualityComparer<T>.Default.Equals(Value, other.Value));

        public bool Equals(T other) =>
            IsSome && EqualityComparer<T>.Default.Equals(Value, other);

        public override bool Equals(object obj) =>
            obj is Option<T> && Equals((Option<T>)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return IsNone
                ? base.GetHashCode() ^ 13
                : Value == null ? base.GetHashCode() ^ 29 : Value.GetHashCode() ^ 31;
            }
        }

        public static bool operator ==(Option<T> a, Option<T> b) =>
            a.Equals(b);

        public static bool operator !=(Option<T> a, Option<T> b) =>
            !a.Equals(b);

        public static bool operator ==(Option<T> a, T b) =>
            a.Equals(b);

        public static bool operator !=(Option<T> a, T b) =>
            !a.Equals(b);

        public static bool operator ==(T a, Option<T> b) =>
            b.Equals(a);

        public static bool operator !=(T a, Option<T> b) =>
            !b.Equals(a);
    }

    public static class Option
    {
        /* Constructors */

        public static Option<T> Some<T>(T value) => new Option<T>(value);

        public static Option<T> None<T>() => default(Option<T>);

        /* Converters */

        public static Option<T> OfObject<T>(T value) where T : class =>
            value != null ? Some(value) : None<T>();

        public static Option<T> ToOption<T>(this T value) where T : class => OfObject(value);

        public static Option<T> OfNullable<T>(T? value) where T : struct =>
            value.HasValue ? Some(value.Value) : None<T>();

        public static Option<T> ToOption<T>(this T? value) where T : struct => OfNullable(value);

        public static Option<string> OfString(string value) =>
            string.IsNullOrEmpty(value) ? None<string>() : Some(value);

        public static Option<string> ToOption(this string value) => OfString(value);

        public static Option<T> OfResult<T>(Result<T> value) where T : struct =>
            value.IsOk ? Some(value.Value) : None<T>();

        public static Option<T> ToOption<T>(this Result<T> value) where T : struct => OfResult(value);

        /* Match */

        public static T2 Match<T1, T2>(this Option<T1> option, Func<T1, T2> some, Func<T2> none) =>
            option.IsSome ? some(option.Value) : none();

        public static T2 MatchTo<T1, T2>(this Option<T1> option, T2 some, T2 none) =>
            option.IsSome ? some : none;

        /* Bind */

        public static Option<T2> Bind<T1, T2>(this Option<T1> option, Func<T1, Option<T2>> binder) =>
            option.IsSome ? binder(option.Value) : None<T2>();

        /* Map */

        public static Option<T2> Map<T1, T2>(this Option<T1> option, Func<T1, T2> mapper) =>
            option.IsSome ? Some(mapper(option.Value)) : None<T2>();

        /* Getters */

        public static T GetOrDefault<T>(this Option<T> option, T whenNone = default(T)) =>
            option.IsSome ? option.Value : whenNone;

        public static T2 GetOrDefault<T1, T2>(this Option<T1> option, Func<T1, T2> getter, T2 whenNone = default(T2)) =>
            option.IsSome ? getter(option.Value) : whenNone;

        /* Side Effects */

        public static Option<T> Do<T>(this Option<T> option, Action<T> action)
        {
            if (option.IsSome)
                action(option.Value);
            return option;
        }

        public static Option<T> DoWhenNone<T>(this Option<T> option, Action action)
        {
            if (option.IsNone)
                action();
            return option;
        }

        /* Collections */

        public static IEnumerable<T> Choose<T>(this IEnumerable<Option<T>> items) =>
            items.Where(i => i.IsSome).Select(i => i.Value);

        public static Option<IEnumerable<T>> Sequence<T>(this IEnumerable<Option<T>> items) =>
            items.Any(i => i.IsNone)
            ? None<IEnumerable<T>>()
            : Some(items.Select(i => i.Value));
    }
}
