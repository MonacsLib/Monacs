using System;
using System.Collections.Generic;

namespace Monacs.Core
{
    public readonly struct Either<L, R> : IEquatable<Either<L, R>>
    {
        public L Left { get; }
        public R Right { get; }
        public LeftOrRight LeftOrRight { get; }
        public bool IsLeft => LeftOrRight == LeftOrRight.Left;
        public bool IsRight => LeftOrRight == LeftOrRight.Right;

        private Either(L left, R right, LeftOrRight leftOrRight)
        {
            Left = left;
            Right = right;
            LeftOrRight = leftOrRight;
        }

        public Either(L left) : this(left, default, LeftOrRight.Left) { }
        public Either(R right) : this(default, right, LeftOrRight.Right) { }

        public bool Equals(Either<L, R> other) =>
            LeftOrRight == other.LeftOrRight
            && IsLeft ? EqualityComparer<L>.Default.Equals(Left, other.Left)
                      : EqualityComparer<R>.Default.Equals(Right, other.Right);

        public override bool Equals(object obj) =>
            !ReferenceEquals(null, obj)
            && obj is Either<L, R> either
            && Equals(either);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<L>.Default.GetHashCode(Left);
                hashCode = (hashCode * 397) ^ EqualityComparer<R>.Default.GetHashCode(Right);
                hashCode = (hashCode * 397) ^ (int)LeftOrRight;
                return hashCode;
            }
        }

        public static bool operator ==(in Either<L, R> a, in Either<L, R> b) => a.Equals(b);

        public static bool operator !=(in Either<L, R> a, in Either<L, R> b) => !a.Equals(b);

        public override string ToString()
        {
            var side = IsLeft ? "Left" : "Right";
            var value = IsLeft ? Left.ToString() : Right.ToString();
            return $"Either<{typeof(L).Name}, {typeof(R).Name}>({side} = {value})";
        }
    }

    public enum LeftOrRight { Left, Right }

    public static class EitherExtensions
    {
        public static Result Match<L, R, Result>(this Either<L, R> target, Func<L, Result> left, Func<R, Result> right) =>
            target.IsLeft ? left(target.Left) : right(target.Right);
    }
}