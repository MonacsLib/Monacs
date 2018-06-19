using System;
using System.Collections.Generic;
using System.Linq;

namespace Monacs.Core
{
    public static class Either
    {
        /* Match */

        // E<L,R> -> Out
        public static TOut Match<TLeft, TRight, TOut>(this Either<TLeft, TRight> either, Func<TLeft, TOut> left, Func<TRight, TOut> right) =>
            either.IsLeft ? left(either.LeftValue) : right(either.RightValue);

        /* Map */

        // E<L,R> -> (L -> LOut, R -> ROut) -> E<LOut, ROut>
        public static Either<TLeftOut, TRightOut> Map<TLeft, TRight, TLeftOut, TRightOut>(this Either<TLeft, TRight> either, Func<TLeft, TLeftOut> left, Func<TRight, TRightOut> right) =>
            either.IsLeft ? Left<TLeftOut, TRightOut>(left(either.LeftValue)) : Right<TLeftOut, TRightOut>(right(either.RightValue));

        /* Bind */

        // E<L, R> -> (L -> LOut, R -> ROut) -> E<LOut, ROut>
        public static Either<TLeftOut, TRightOut> Bind<TLeft, TRight, TLeftOut, TRightOut>(this Either<TLeft, TRight> either, Func<TLeft, Either<TLeftOut, TRightOut>> left, Func<TRight, Either<TLeftOut, TRightOut>> right) =>
            either.IsLeft ? left(either.LeftValue) : right(either.RightValue);


        /* Side effects */

        // E<L, R> -> (L -> Unit) -> E<L, R>
        public static Either<TLeft, TRight> DoWhenLeft<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TLeft> action)
        {
            if (either.IsLeft)
                action(either.LeftValue);

            return either;
        }

        // E<L, R> -> (R -> Unit) -> E<L, R>
        public static Either<TLeft, TRight> DoWhenRight<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TRight> action)
        {
            if (either.IsRight)
                action(either.RightValue);

            return either;
        }

        /* Factories */

        // L -> E<L, R>
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);

        // R -> E<L, R>
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);

        // (L, R) -> E<L, R>
        public static Either<TLeft, TRight> ToLeftOrRight<TLeft, TRight>((TLeft left, TRight right) tuple) =>
            tuple.right == null ? Left<TLeft, TRight>(tuple.left) : Right<TLeft, TRight>(tuple.right);

        /* Collections */

        // List<E<L, R>> -> List<L>
        public static IEnumerable<TLeft> ChooseLeft<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> items) =>
            items.Where(x => x.LeftOrRight == LeftOrRight.Left).Select(x => x.LeftValue);

        // List<E<L, R>> -> List<R>
        public static IEnumerable<TRight> ChooseRight<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> items) =>
            items.Where(x => x.LeftOrRight == LeftOrRight.Right).Select(x => x.RightValue);

        // List<E<L, R>> -> Option<List<L>>
        public static Option<IEnumerable<TLeft>> SequenceLeft<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> items) =>
            items.Any(x => x.IsRight) ? Option.None<IEnumerable<TLeft>>() : Option.Some(items.ChooseLeft());

        // List<E<L, R>> -> Option<List<R>>
        public static Option<IEnumerable<TRight>> SequenceRight<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> items) =>
            items.Any(x => x.IsLeft) ? Option.None<IEnumerable<TRight>>() : Option.Some(items.ChooseRight());

    }
}
