using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
    public readonly struct Either<L, R> : IEquatable<Either<L, R>>
    {
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

        public override bool Equals(object obj) =>
            !ReferenceEquals(null, obj)
            && obj is Either<L, R> either
            && Equals(either);

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

        public static Out Match<L, R, Out>(this Either<L, R> target, Func<L, Out> left, Func<R, Out> right) =>
            target.IsLeft ? left(target.LeftValue) : right(target.RightValue);

        /* Map */

        public static Out MapRight<L, R, Out>(this Either<L, R> target, Func<R, Out> map) =>
            map(target.RightValue);

        public static Out MapLeft<L, R, Out>(this Either<L, R> target, Func<L, Out> map) =>
            map(target.LeftValue);

        /* Bind */

        public static Either<LOut, ROut> Bind<L, R, LOut, ROut>(
            this Either<L, R> target, Func<L, LOut> leftBinder, Func<R, ROut> rightBinder) =>
            target.IsLeft ? Left<LOut, ROut>(leftBinder(target.LeftValue)) : Right<LOut, ROut>(rightBinder(target.RightValue));


        /* Side effects */

        public static Either<L, R> DoWhenLeft<L, R>(this Either<L, R> target, Action<L> action)
        {
            if (target.IsLeft)
                action(target.LeftValue);

            return target;
        }

        public static Either<L, R> DoWhenRight<L, R>(this Either<L, R> target, Action<R> action)
        {
            if (target.IsRight)
                action(target.RightValue);

            return target;
        }

        /* Factories */

        public static Either<L, R> Left<L, R>(L left) => new Either<L, R>(left);

        public static Either<L, R> Right<L, R>(R right) => new Either<L, R>(right);

        public static Either<L, R> FromTuple<L, R>((L left, R right) tuple) =>
            tuple.right == null ? Left<L, R>(tuple.left) : Right<L, R>(tuple.right);

        /* Collections */

        public static IEnumerable<L> ChooseLeft<L, R>(this IEnumerable<Either<L, R>> target) =>
            target.Where(x => x.LeftOrRight == LeftOrRight.Left).Select(x => x.LeftValue);

        public static IEnumerable<R> ChooseRight<L, R>(this IEnumerable<Either<L, R>> target) =>
            target.Where(x => x.LeftOrRight == LeftOrRight.Right).Select(x => x.RightValue);

        public static Option<IEnumerable<L>> SequenceLeft<L, R>(this IEnumerable<Either<L, R>> target) =>
            target.Any(x => x.IsRight)
                ? Option.Some(target.ChooseLeft())
                : Option.None<IEnumerable<L>>();

        public static Option<IEnumerable<R>> SequenceRight<L, R>(this IEnumerable<Either<L, R>> target) =>
            target.Any(x => x.IsLeft)
                ? Option.Some(target.ChooseRight())
                : Option.None<IEnumerable<R>>();


    }
}