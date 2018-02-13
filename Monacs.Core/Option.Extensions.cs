using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
    /// <summary>
    /// Contains the set of extensions to work with the <see cref="Option{T}"/> type.
    /// </summary>
    public static class Option
    {
        /* Constructors */

        /// <summary>
        /// Creates the Some case instance of the <see cref="Option{T}"/> type, encapsulating provided value.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to encapsulate.</param>
        public static Option<T> Some<T>(T value) => new Option<T>(value);

        /// <summary>
        /// Creates the None case instance of the <see cref="Option{T}"/> type, containing no value.
        /// </summary>
        /// <typeparam name="T">Desired type parameter for <see cref="Option{T}"/> type.</typeparam>
        public static Option<T> None<T>() => default(Option<T>);

        /* Converters */

        /// <summary>
        /// Converts the value of class T to the <see cref="Option{T}"/> type.
        /// If the value is null, the None case is yielded.
        /// Otherwise Some case with provided value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Option{T}"/>.</param>
        public static Option<T> OfObject<T>(T value) where T : class =>
            value != null ? Some(value) : None<T>();

        /// <summary>
        /// Converts the value of class T to the <see cref="Option{T}"/> type.
        /// If the value is null, the None case is yielded.
        /// Otherwise Some case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Option.OfObject{T}(T)"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Option{T}"/>.</param>
        public static Option<T> ToOption<T>(this T value) where T : class => OfObject(value);

        /// <summary>
        /// Converts the value of <see cref="Nullable{T}"/> to the <see cref="Option{T}"/> type.
        /// If the value is null, the None case is yielded.
        /// Otherwise Some case with provided value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Option{T}"/>.</param>
        public static Option<T> OfNullable<T>(T? value) where T : struct =>
            value.HasValue ? Some(value.Value) : None<T>();

        /// <summary>
        /// Converts the value of <see cref="Nullable{T}"/> to the <see cref="Option{T}"/> type.
        /// If the value is null, the None case is yielded.
        /// Otherwise Some case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Option.OfNullable{T}(T?)"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Option{T}"/>.</param>
        public static Option<T> ToOption<T>(this T? value) where T : struct => OfNullable(value);

        /// <summary>
        /// Converts the value of <see cref="Option{T}"/> to the <see cref="Nullable{T}"/> type.
        /// If the option is the None case, null is yielded.
        /// Otherwise encapsulated value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Nullable{T}"/>.</param>
        public static T? ToNullable<T>(Option<T> value) where T : struct =>
            value.IsSome ? value.Value : (T?)null;

        /// <summary>
        /// Converts the string value to the <see cref="Option{T}"/> type.
        /// If the value is null or empty string, the None case is yielded.
        /// Otherwise Some case with provided value is returned.
        /// </summary>
        /// <param name="value">The value to convert to <see cref="Option{T}"/>.</param>
        public static Option<string> OfString(string value) =>
            string.IsNullOrEmpty(value) ? None<string>() : Some(value);

        /// <summary>
        /// Converts the string value to the <see cref="T:Option{string}"/> type.
        /// If the value is null or empty string, the None case is yielded.
        /// Otherwise Some case with provided value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Option.OfString(string)"/></remarks>
        /// <param name="value">The value to convert to <see cref="T:Option{string}"/>.</param>
        public static Option<string> ToOption(this string value) => OfString(value);

        /// <summary>
        /// Converts the value of <see cref="Result{T}"/> to the <see cref="Option{T}"/> type.
        /// If the value is the Error case, the None case is yielded.
        /// Otherwise Some case with encapsulated value is returned.
        /// </summary>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Option{T}"/>.</param>
        public static Option<T> OfResult<T>(Result<T> value) where T : struct =>
            value.IsOk ? Some(value.Value) : None<T>();

        /// <summary>
        /// Converts the value of <see cref="Result{T}"/> to the <see cref="Option{T}"/> type.
        /// If the value is the Error case, the None case is yielded.
        /// Otherwise Some case with encapsulated value is returned.
        /// </summary>
        /// <remarks>Extension method variant of <see cref="Option.OfResult{T}(Result{T})"/></remarks>
        /// <typeparam name="T">Type of the encapsulated value.</typeparam>
        /// <param name="value">The value to convert to <see cref="Option{T}"/>.</param>
        public static Option<T> ToOption<T>(this Result<T> value) where T : struct => OfResult(value);

        /* TryGetOption */

        /// <summary>
        /// Tries to get the element with the given <paramref name="key"/> from the <paramref name="dictionary"/>.
        /// If the value is found, returns Some case of the <see cref="Option{T}"/> type with the value from the dictionary.
        /// Otherwise returns None case of the <see cref="Option{T}"/> type.
        /// </summary>
        /// <typeparam name="TKey">Type of the key in the dictionary.</typeparam>
        /// <typeparam name="TValue">Type of the value in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search in.</param>
        /// <param name="key">The key to look for.</param>
        public static Option<TValue> TryGetOption<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
            dictionary.TryGetValue(key, out TValue value) ? Some(value) : None<TValue>();

        /// <summary>
        /// Tries to get the elements with the given <paramref name="key"/> from the <paramref name="lookup"/>.
        /// If any value is found, returns Some case of the <see cref="Option{T}"/> type with the values from the lookup.
        /// Otherwise returns None case of the <see cref="Option{T}"/> type.
        /// </summary>
        /// <typeparam name="TKey">Type of the key in the lookup.</typeparam>
        /// <typeparam name="TValue">Type of the value in the lookup.</typeparam>
        /// <param name="lookup">The lookup to search in.</param>
        /// <param name="key">The key to look for.</param>
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
