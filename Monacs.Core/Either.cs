using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
    public enum LeftOrRight { Left, Right }

    public readonly struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        public TLeft LeftValue { get; }
        public TRight RightValue { get; }
        public LeftOrRight LeftOrRight { get; }
        public bool IsLeft => LeftOrRight == LeftOrRight.Left;
        public bool IsRight => LeftOrRight == LeftOrRight.Right;

        private Either(TLeft left, TRight right, LeftOrRight leftOrRight)
        {
            LeftValue = left;
            RightValue = right;
            LeftOrRight = leftOrRight;
        }

        public Either(TLeft left) : this(left, default, LeftOrRight.Left) { }
        public Either(TRight right) : this(default, right, LeftOrRight.Right) { }

        public bool Equals(Either<TLeft, TRight> other) =>
            LeftOrRight == other.LeftOrRight
            && IsLeft ? EqualityComparer<TLeft>.Default.Equals(LeftValue, other.LeftValue)
                      : EqualityComparer<TRight>.Default.Equals(RightValue, other.RightValue);

        public override bool Equals(object obj) =>
            !ReferenceEquals(null, obj)
            && obj is Either<TLeft, TRight> either && Equals(either);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<TLeft>.Default.GetHashCode(LeftValue);
                hashCode = (hashCode * 397) ^ EqualityComparer<TRight>.Default.GetHashCode(RightValue);
                hashCode = (hashCode * 397) ^ (int)LeftOrRight;
                return hashCode;
            }
        }

        public static bool operator ==(in Either<TLeft, TRight> a, in Either<TLeft, TRight> b) => a.Equals(b);

        public static bool operator !=(in Either<TLeft, TRight> a, in Either<TLeft, TRight> b) => !a.Equals(b);

        public override string ToString()
        {
            var side = IsLeft ? "Left" : "Right";
            var value = IsLeft ? LeftValue.ToString() : RightValue.ToString();
            return $"Either<{typeof(TLeft).Name}, {typeof(TRight).Name}>({side} = {value})";
        }
    }
}