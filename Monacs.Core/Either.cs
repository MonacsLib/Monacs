using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
    public interface IEither
    {
        LeftOrRight LeftOrRight { get; }
        object Left { get; }
        object Right { get; }
    }

    public readonly struct Either<L, R> : IEither, IEquatable<Either<L, R>>, IEquatable<IEither>
    {
        object IEither.Left => LeftValue;
        object IEither.Right => RightValue;

        public L LeftValue { get; }
        public R RightValue { get; }
        public LeftOrRight LeftOrRight { get; }
        public bool IsLeft => LeftOrRight == LeftOrRight.Left;
        public bool IsRight => LeftOrRight == LeftOrRight.Right;

        private Either(L left, R right, LeftOrRight leftOrRight)
        {
            LeftValue = left;
            RightValue = right;
            LeftOrRight = leftOrRight;
        }

        public Either(L left) : this(left, default, LeftOrRight.Left) { }
        public Either(R right) : this(default, right, LeftOrRight.Right) { }

        public bool Equals(Either<L, R> other) =>
            LeftOrRight == other.LeftOrRight
            && IsLeft ? EqualityComparer<L>.Default.Equals(LeftValue, other.LeftValue)
                      : EqualityComparer<R>.Default.Equals(RightValue, other.RightValue);

        public bool Equals(IEither other) =>
            !ReferenceEquals(null, other)
            && LeftOrRight == other.LeftOrRight
            && (other.Left != null && other.Left.Equals(LeftValue)
                || other.Right != null && other.Right.Equals(RightValue));

        public override bool Equals(object obj) =>
            !ReferenceEquals(null, obj)
            && obj is IEither either && Equals(either)
            || obj is Either<L, R> eitherGeneric && Equals(eitherGeneric);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<L>.Default.GetHashCode(LeftValue);
                hashCode = (hashCode * 397) ^ EqualityComparer<R>.Default.GetHashCode(RightValue);
                hashCode = (hashCode * 397) ^ (int)LeftOrRight;
                return hashCode;
            }
        }

        public static bool operator ==(in Either<L, R> a, in Either<L, R> b) => a.Equals(b);

        public static bool operator !=(in Either<L, R> a, in Either<L, R> b) => !a.Equals(b);

        public override string ToString()
        {
            var side = IsLeft ? "Left" : "Right";
            var value = IsLeft ? LeftValue.ToString() : RightValue.ToString();
            return $"Either<{typeof(L).Name}, {typeof(R).Name}>({side} = {value})";
        }
    }

    public enum LeftOrRight { Left, Right }

    public static partial class Either
    {
        /* Match */

        // E<L,R> -> Out
        public static Out Match<L, R, Out>(this Either<L, R> either, Func<L, Out> left, Func<R, Out> right) =>
            either.IsLeft ? left(either.LeftValue) : right(either.RightValue);

        /* Map */

        // E<L,R> -> (L -> LOut, R -> ROut) -> E<LOut, ROut>
        public static Either<LOut, ROut> Map<L, R, LOut, ROut>(this Either<L, R> either, Func<L, LOut> mapLeft, Func<R, ROut> mapRight) =>
            either.IsLeft ? Left<LOut, ROut>(mapLeft(either.LeftValue)) : Right<LOut, ROut>(mapRight(either.RightValue));

        /* Bind */

        // E<L, R> -> (L -> LOut, R -> ROut) -> E<LOut, ROut>
        public static Either<LOut, ROut> Bind<L, R, LOut, ROut>(this Either<L, R> either, Func<L, Either<LOut, ROut>> leftBinder, Func<R, Either<LOut, ROut>> rightBinder) =>
            either.IsLeft ? leftBinder(either.LeftValue) : rightBinder(either.RightValue);


        /* Side effects */

        public static Either<L, R> DoWhenLeft<L, R>(this Either<L, R> either, Action<L> action)
        {
            if (either.IsLeft)
                action(either.LeftValue);

            return either;
        }

        public static Either<L, R> DoWhenRight<L, R>(this Either<L, R> either, Action<R> action)
        {
            if (either.IsRight)
                action(either.RightValue);

            return either;
        }

        /* Factories */

        public static Either<L, R> Left<L, R>(L left) => new Either<L, R>(left);

        public static Either<L, R> Right<L, R>(R right) => new Either<L, R>(right);

        public static Either<L, R> FromTuple<L, R>((L left, R right) tuple) =>
            tuple.right == null ? Left<L, R>(tuple.left) : Right<L, R>(tuple.right);

        /* Collections */

        public static IEnumerable<L> ChooseLeft<L, R>(this IEnumerable<Either<L, R>> items) =>
            items.Where(x => x.LeftOrRight == LeftOrRight.Left).Select(x => x.LeftValue);

        public static IEnumerable<R> ChooseRight<L, R>(this IEnumerable<Either<L, R>> items) =>
            items.Where(x => x.LeftOrRight == LeftOrRight.Right).Select(x => x.RightValue);

        public static Option<IEnumerable<L>> SequenceLeft<L, R>(this IEnumerable<Either<L, R>> items) =>
            items.Any(x => x.IsRight)
                ? Option.Some(items.ChooseLeft())
                : Option.None<IEnumerable<L>>();

        public static Option<IEnumerable<R>> SequenceRight<L, R>(this IEnumerable<Either<L, R>> items) =>
            items.Any(x => x.IsLeft)
                ? Option.Some(items.ChooseRight())
                : Option.None<IEnumerable<R>>();


    }
}