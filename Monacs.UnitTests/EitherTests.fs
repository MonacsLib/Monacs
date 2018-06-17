namespace Monacs.UnitTests.Either

open Xunit
open FsUnit.Xunit
open Monacs.Core

module ``Constructors and equality`` =

    [<Fact>]
    let ``Left of Either<L, R> equals itself`` ()  =
         let left = Either.ToEitherLeft(42)
         left = left |> should equal true
         left <> left |> should equal false

    [<Fact>]
    let ``Right of Either<L, R> equals itself`` ()  =
         let right = Either.ToEitherRight(42)
         right = right |> should equal true
         right <> right |> should equal false

    [<Fact>]
    let ``Boxed Left of Either<L, R> is not null`` ()  =
        let left = box (Either.ToEitherLeft(42))
        isNull left |> should equal false
        not (isNull left) |> should equal true

    [<Fact>]
    let ``Boxed Right of Either<L, R> is not null`` ()  =
        let right = box (Either.ToEitherRight(42))
        isNull right |> should equal false
        not (isNull right) |> should equal true

    [<Fact>]
    let ``Either<L, R> equals Either<L, R> when the Left values are equal`` () =
        let value = 42
        Either.ToEitherLeft(value) = Either.ToEitherLeft(value) |> should equal true
        Either.ToEitherLeft(value) <> Either.ToEitherLeft(value) |> should equal false

    [<Fact>]
    let ``Either<L, R> equals Either<L, R> when the Right values are equal`` ()  =
        let value = 42
        Either.ToEitherRight(value) =  Either.ToEitherRight(value) |> should equal true
        Either.ToEitherRight(value) <> Either.ToEitherRight(value) |> should equal false
        
    [<Fact>]
    let ``Either<L, R> should be compared by values`` () =
        let e1 = Either.ToEitherLeft<int, int>(42)
        let e2 = Either.ToEitherLeft<obj, int>(42)
        e1 |> should equal e2


    [<Fact>]
    let ``Either<L, R> doesn't equal Either<L, R> when the Left values are not equal`` ()  =
        Either.ToEitherLeft(42) =  Either.ToEitherLeft(52) |> should equal false
        Either.ToEitherLeft(42) <> Either.ToEitherLeft(52) |> should equal true

    [<Fact>]
    let ``Either<L, R> doesn't equal Either<L, R> when the Right values are not equal`` () =
        Either.ToEitherRight(42) =  Either.ToEitherRight(52) |> should equal false
        Either.ToEitherRight(42) <> Either.ToEitherRight(52) |> should equal true

    [<Fact>]
    let ``Left side of Either<L,R> doesn't equal the Right side``()  =
        let value = 42
        Either.ToEitherRight(value) <> Either.ToEitherLeft(value) |> should equal true
        Either.ToEitherRight(value) = Either.ToEitherLeft(value) |> should equal false

module Match =

    [<Fact>]
    let ``Match goes with left when LeftValue is set`` () =
        let either = Either.ToEitherLeft(42)
        Either.Match(either, left = (fun _ -> "left"), right = (fun _ -> "right")) |> should equal "left"

    [<Fact>]
    let ``Match goes with right when RightValue is set`` () =
        let either = Either.ToEitherRight(42)
        Either.Match(either, left = (fun _ -> "left"), right = (fun _ -> "right")) |> should equal "right"

module Map =

    [<Fact>]
    let ``Maps left side when LeftValue is set`` () =
        let either = Either.ToEitherLeft(42)
        let expected = Either.ToEitherLeft(84)
        Either.Map(either, mapLeft = (fun l -> l * 2), mapRight = (fun r -> r * 3)) |> should equal expected

    
    [<Fact>]
    let ``Maps right side when RightValue is set`` () =
        let either = Either.ToEitherRight(42)
        let expected = Either.ToEitherRight(126)
        Either.Map(either, mapLeft = (fun l -> l * 2), mapRight = (fun r -> r * 3)) |> should equal expected

module ``Side effects`` =

    [<Fact>]
    let ``DoWhenLeft returns value and executes action when Left value is set`` () =
        let either = Either.ToEitherLeft(42)
        let expected = "42"
        let mutable output = ""
        Either.DoWhenLeft(either, fun l -> output <- l.ToString()) |> ignore
        output |> should equal expected
        
    [<Fact>]
    let ``DoWhenLeft returns value but does not execute action when Right value is set`` () =
        let either = Either.ToEitherRight(42)
        let expected = ""
        let mutable output = ""
        Either.DoWhenLeft(either, fun l -> output <- l.ToString()) |> ignore
        output |> should equal expected
        
    [<Fact>]
    let ``DoWhenRight returns value and executes action when Right value is set`` () =
        let either = Either.ToEitherRight(42)
        let expected = "42"
        let mutable output = ""
        Either.DoWhenRight(either, fun l -> output <- l.ToString()) |> ignore
        output |> should equal expected
        
    [<Fact>]
    let ``DoWhenRight returns value but does not execute action when Left value is set`` () =
        let either = Either.ToEitherLeft(42)
        let expected = ""
        let mutable output = ""
        Either.DoWhenRight(either, fun l -> output <- l.ToString()) |> ignore
        output |> should equal expected

module Collections =

    // Choose

    [<Fact>]
    let ``ChooseLeft<T> returns all items of type Either with Left value set`` () =
        let items = seq { yield Either.ToEitherLeft(21); yield Either.ToEitherLeft(42); yield Either.ToEitherRight(63) }
        let lefts = Either.ChooseLeft(items)
        let expected = [| 21; 42 |]
        lefts |> Seq.toArray |> should equal expected
        
    [<Fact>]
    let ``ChooseRight<T> returns all items of type Either with Right value set`` () =
        let items = seq { yield Either.ToEitherRight(21); yield Either.ToEitherRight(42); yield Either.ToEitherLeft(63) }
        let lefts = Either.ChooseRight(items)
        let expected = [| 21; 42 |]
        lefts |> Seq.toArray |> should equal expected


    // Sequence

    open System.Collections.Generic

    [<Fact>]
    let ``SequenceLeft<T> returns None if any item in collection has Right value set`` () =
        let items = seq { yield Either.ToEitherLeft(21); yield Either.ToEitherRight(42) }
        let lefts = Either.SequenceLeft(items)
        let expected = Option.None<IEnumerable<int>>()
        lefts |> should equal expected

    
    [<Fact>]
    let ``SequenceLeft<T> returns Some<T> when all items in collection has Left value set`` () =
        let items = seq { yield Either.ToEitherLeft(21) }
        let lefts = Either.SequenceLeft(items)
        lefts.IsSome |> should equal true
        lefts.Value |> Seq.length |> should equal 1

    [<Fact>]
    let ``SequenceRight<T> returns None if any item in collection has Left value set`` () =
        let items = seq { yield Either.ToEitherRight(21); yield Either.ToEitherLeft(42) }
        let rights = Either.SequenceRight(items)
        let expected = Option.None<IEnumerable<int>>()
        rights |> should equal expected

    
    [<Fact>]
    let ``SequenceRight<T> returns Some<T> when all items in collection has Right value set`` () =
        let items = seq { yield Either.ToEitherRight(21) }
        let lefts = Either.SequenceRight(items)
        lefts.IsSome |> should equal true
        lefts.Value |> Seq.length |> should equal 1
