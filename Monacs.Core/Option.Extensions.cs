using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
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

        public static T? ToNullable<T>(Option<T> value) where T : struct =>
            value.IsSome ? value.Value : (T?)null;

        public static Option<string> OfString(string value) =>
            string.IsNullOrEmpty(value) ? None<string>() : Some(value);

        public static Option<string> ToOption(this string value) => OfString(value);

        public static Option<T> OfResult<T>(Result<T> value) where T : struct =>
            value.IsOk ? Some(value.Value) : None<T>();

        public static Option<T> ToOption<T>(this Result<T> value) where T : struct => OfResult(value);

        /* TryGetOption */

        public static Option<TValue> TryGetOption<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) =>
            dict.TryGetValue(key, out TValue value) ? Some(value) : None<TValue>();

        public static Option<IEnumerable<TValue>> TryGetOption<TKey, TValue>(this ILookup<TKey, TValue> lookup, TKey key) =>
            lookup.Contains(key) ? Some(lookup[key]) : None<IEnumerable<TValue>>();

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

        public static Option<T> TryFind<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items != null)
                foreach (T element in items) {
                    if (predicate(element)) return Some(element);
                }
            return None<T>();
        }

        public static Option<T> TryFirst<T>(this IEnumerable<T> items)
        {
            if (items == null) return None<T>();
            IList<T> list = items as IList<T>;
            if (list != null) {
                if (list.Count > 0) return Some(list[0]);
            } else {
                foreach (T element in items) {
                    return Some(element);
                }
            }
            return None<T>();
        }

        public static Option<T> TryElementAt<T>(this IEnumerable<T> items, int index)
        {
            if (items == null || index < 0) return None<T>();
            IList<T> list = items as IList<T>;
            if (list != null) {
                if (list.Count >= index+1) return Some(list[index]);
            } else {
                int idx = 0;
                foreach (T element in items) {
                    if (idx == index) return Some(element);
                    idx++;
                }
            }
            return None<T>();
        }
    }
}
