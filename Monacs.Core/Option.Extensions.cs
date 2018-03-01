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

        /// <summary>
        /// Does the pattern matching on the <see cref="Option{T}"/> type.
        /// If the <paramref name="option"/> is Some, calls <paramref name="some"/> function
        /// with the value from the option as a parameter and returns its result.
        /// Otherwise calls <paramref name="none"/> function and returns its result.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the option.</typeparam>
        /// <typeparam name="TOut">Type of the returned value.</typeparam>
        /// <param name="option">The option to match on.</param>
        /// <param name="some">Function called for the Some case.</param>
        /// <param name="none">Function called for the None case.</param>
        public static TOut Match<TIn, TOut>(this Option<TIn> option, Func<TIn, TOut> some, Func<TOut> none) =>
            option.IsSome ? some(option.Value) : none();

        /// <summary>
        /// Does the pattern matching on the <see cref="Option{T}"/> type.
        /// If the <paramref name="option"/> is Some, returns <paramref name="some"/> value.
        /// Otherwise returns <paramref name="none"/> value.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the option.</typeparam>
        /// <typeparam name="TOut">Type of the returned value.</typeparam>
        /// <param name="option">The option to match on.</param>
        /// <param name="some">Value returned for the Some case.</param>
        /// <param name="none">Value returned for the None case.</param>
        public static TOut MatchTo<TIn, TOut>(this Option<TIn> option, TOut some, TOut none) =>
            option.IsSome ? some : none;

        /* Bind */

        /// <summary>
        /// Transforms the <paramref name="option"/> into another <see cref="Option{T}"/> using the <paramref name="binder"/> function.
        /// If the input option is Some, returns the value of the binder call (which is <typeparamref name="TOut"/> option).
        /// Otherwise returns None case of the <typeparamref name="TOut"/> option.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input option.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned option.</typeparam>
        /// <param name="option">The option to bind with.</param>
        /// <param name="binder">Function called with the input option value if it's Some case.</param>
        public static Option<TOut> Bind<TIn, TOut>(this Option<TIn> option, Func<TIn, Option<TOut>> binder) =>
            option.IsSome ? binder(option.Value) : None<TOut>();

        /* Map */

        /// <summary>
        /// Maps the value of the <paramref name="option"/> into another <see cref="Option{T}"/> using the <paramref name="mapper"/> function.
        /// If the input option is Some, returns the Some case with the value of the mapper call (which is <typeparamref name="TOut"/>).
        /// Otherwise returns None case of the <typeparamref name="TOut"/> option.
        /// </summary>
        /// <typeparam name="TIn">Type of the value in the input option.</typeparam>
        /// <typeparam name="TOut">Type of the value in the returned option.</typeparam>
        /// <param name="option">The option to map on.</param>
        /// <param name="mapper">Function called with the input option value if it's Some case.</param>
        public static Option<TOut> Map<TIn, TOut>(this Option<TIn> option, Func<TIn, TOut> mapper) =>
            option.IsSome ? Some(mapper(option.Value)) : None<TOut>();

        /* Getters */

        /// <summary>
        /// Gets the value of the <paramref name="option"/> if it's Some case.
        /// If the option is None case returns value specified by the <paramref name="whenNone"/> parameter;
        /// if the parameter is not set returns the default value of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value in the option.</typeparam>
        /// <param name="option">The option to get a value from.</param>
        /// <param name="whenNone">Value to return if the option is the None case.</param>
        public static T GetOrDefault<T>(this Option<T> option, T whenNone = default(T)) =>
            option.IsSome ? option.Value : whenNone;

        /// <summary>
        /// Gets the value from the <paramref name="option"/> using the <paramref name="getter"/> function if it's Some case.
        /// If the option is None case returns value specified by the <paramref name="whenNone"/> parameter;
        /// if the parameter is not set returns the default value of the type <typeparamref name="TOut"/>.
        /// </summary>
        /// <remarks>Effectively the combination of <see cref="Option.Map{TIn, TOut}(Option{TIn}, Func{TIn, TOut})"/> and <see cref="Option.GetOrDefault{T}(Option{T}, T)"/> calls.</remarks>
        /// <typeparam name="TIn">Type of the value in the option.</typeparam>
        /// <typeparam name="TOut">Type of the return value.</typeparam>
        /// <param name="option">The option to get a value from.</param>
        /// <param name="getter">Function used to get the value if the option is the Some case.</param>
        /// <param name="whenNone">Value to return if the option is the None case.</param>
        public static TOut GetOrDefault<TIn, TOut>(this Option<TIn> option, Func<TIn, TOut> getter, TOut whenNone = default(TOut)) =>
            option.IsSome ? getter(option.Value) : whenNone;

        /* Side Effects */

        /// <summary>
        /// Performs the <paramref name="action"/> with the value of the <paramref name="option"/> if it's Some case.
        /// If the option is None case nothing happens.
        /// In both cases unmodified option is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the option.</typeparam>
        /// <param name="option">The option to check for a value.</param>
        /// <param name="action">Function executed if the option is Some case.</param>
        public static Option<T> Do<T>(this Option<T> option, Action<T> action)
        {
            if (option.IsSome)
                action(option.Value);
            return option;
        }

        /// <summary>
        /// Performs the <paramref name="action"/> if the <paramref name="option"/> is None case.
        /// If the option is Some case nothing happens.
        /// In both cases unmodified option is returned.
        /// </summary>
        /// <typeparam name="T">Type of the value in the option.</typeparam>
        /// <param name="option">The option to check for a value.</param>
        /// <param name="action">Function executed if the option is None case.</param>
        public static Option<T> DoWhenNone<T>(this Option<T> option, Action action)
        {
            if (option.IsNone)
                action();
            return option;
        }

        /* Collections */

        /// <summary>
        /// Returns the collection of values of elements from the <see cref="Option{T}"/> collection
        /// that are Some case (so contain some value).
        /// </summary>
        /// <typeparam name="T">Type of the value in the option.</typeparam>
        /// <param name="items">Collection to filter out and map.</param>
        public static IEnumerable<T> Choose<T>(this IEnumerable<Option<T>> items) =>
            items.Where(i => i.IsSome).Select(i => i.Value);

        /// <summary>
        /// If all elements in the input collection are Some case, returns the Some of the collection of underlying values.
        /// Otherwise returns None.
        /// </summary>
        /// <typeparam name="T">Type of the value in the option.</typeparam>
        /// <param name="items">Collection to check and map.</param>
        public static Option<IEnumerable<T>> Sequence<T>(this IEnumerable<Option<T>> items) =>
            items.Any(i => i.IsNone)
            ? None<IEnumerable<T>>()
            : Some(items.Select(i => i.Value));

        /// <summary>
        /// Tries to find the first element of the <paramref name="items"/> collection matching the <paramref name="predicate"/>.
        /// If element is found, returns Some with the value of that element.
        /// Otherwise returns None.
        /// </summary>
        /// <typeparam name="T">Type of the value in the collection and returned option.</typeparam>
        /// <param name="items">Collection to search in.</param>
        /// <param name="predicate">Function that checks if given element matches desired condition.</param>
        public static Option<T> TryFind<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items != null)
                foreach (T element in items) {
                    if (predicate(element)) return Some(element);
                }
            return None<T>();
        }

        /// <summary>
        /// Tries to get the the element in the <paramref name="items"/> collection.
        /// If element is found, returns Some with the value of that element.
        /// Otherwise returns None.
        /// </summary>
        /// <typeparam name="T">Type of the value in the collection and the returned option.</typeparam>
        /// <param name="items">Collection to search in.</param>
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

        /// <summary>
        /// Tries to get the element of the <paramref name="items"/> collection at the posision given by the <paramref name="index"/> parameter.
        /// If element is found, returns Some with the value of that element.
        /// Otherwise returns None.
        /// </summary>
        /// <typeparam name="T">Type of the value in the collection and returned option.</typeparam>
        /// <param name="items">Collection to search in.</param>
        /// <param name="index">Position at which to look for an element.</param>
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
