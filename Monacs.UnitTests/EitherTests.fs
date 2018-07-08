namespace Monacs.UnitTests.Either

open Xunit
open FsUnit.Xunit
open Monacs.Core

module ``Constructors and equality`` =

    [<Fact>]
    let ``Left of Either<L, R> equals itself`` ()  =
         let left = Either.Left(42)
         left = left |> should equal true
         left <> left |> should equal false

    [<Fact>]
    let ``Right of Either<L, R> equals itself`` ()  =
         let right = Either.Right(42)
         right = right |> should equal true
         right <> right |> should equal false

    [<Fact>]
    let ``Boxed Left of Either<L, R> is not null`` ()  =
        let left = box (Either.Left(42))
        isNull left |> should equal false
        not (isNull left) |> should equal true

    [<Fact>]
    let ``Boxed Right of Either<L, R> is not null`` ()  =
        let right = box (Either.Right(42))
        isNull right |> should equal false
        not (isNull right) |> should equal true

    [<Fact>]
    let ``Either<L, R> equals Either<L, R> when the Left values are equal`` () =
        let value = 42
        Either.Left(value) = Either.Left(value) |> should equal true
        Either.Left(value) <> Either.Left(value) |> should equal false

    [<Fact>]
    let ``Either<L, R> equals Either<L, R> when the Right values are equal`` ()  =
        let value = 42
        Either.Right(value) =  Either.Right(value) |> should equal true
        Either.Right(value) <> Either.Right(value) |> should equal false
        
    [<Fact>]
    let ``Either<L, R> doesn't equal Either<L, R> when the Left values are not equal`` ()  =
        Either.Left(42) =  Either.Left(52) |> should equal false
        Either.Left(42) <> Either.Left(52) |> should equal true

    [<Fact>]
    let ``Either<L, R> doesn't equal Either<L, R> when the Right values are not equal`` () =
        Either.Right(42) =  Either.Right(52) |> should equal false
        Either.Right(42) <> Either.Right(52) |> should equal true

    [<Fact>]
    let ``Left side of Either<L,R> doesn't equal the Right side``()  =
        let value = 42
        Either.Right(value) <> Either.Left(value) |> should equal true
        Either.Right(value) = Either.Left(value) |> should equal false

module Match =

    [<Fact>]
    let ``Match goes with left when LeftValue is set`` () =
        let either = Either.Left(42)
        Either.Match(either, left = (fun _ -> "left"), right = (fun _ -> "right")) |> should equal "left"

    [<Fact>]
    let ``Match goes with right when RightValue is set`` () =
        let either = Either.Right(42)
        Either.Match(either, left = (fun _ -> "left"), right = (fun _ -> "right")) |> should equal "right"

module Map =

    [<Fact>]
    let ``Maps left side when LeftValue is set`` () =
        let either = Either.Left<int, int>(42)
        let expected = Either.Left<int, int>(84)
        Either.Map(either, left = (fun l -> l * 2), right = (fun r -> r * 3)) |> should equal expected

    
    [<Fact>]
    let ``Maps right side when RightValue is set`` () =
        let either = Either.Right<int, int>(42)
        let expected = Either.Right<int, int>(126)
        Either.Map(either, left = (fun l -> l * 2), right = (fun r -> r * 3)) |> should equal expected

module ``Side effects`` =

    [<Fact>]
    let ``DoWhenLeft returns value and executes action when Left value is set`` () =
        let either = Either.Left(42)
        let expected = "42"
        let mutable output = ""
        Either.DoWhenLeft(either, fun l -> output <- l.ToString()) |> ignore
        output |> should equal expected
        
    [<Fact>]
    let ``DoWhenLeft returns value but does not execute action when Right value is set`` () =
        let either = Either.Right(42)
        let expected = ""
        let mutable output = ""
        Either.DoWhenLeft(either, fun l -> output <- l.ToString()) |> ignore
        output |> should equal expected
        
    [<Fact>]
    let ``DoWhenRight returns value and executes action when Right value is set`` () =
        let either = Either.Right(42)
        let expected = "42"
        let mutable output = ""
        Either.DoWhenRight(either, fun l -> output <- l.ToString()) |> ignore
        output |> should equal expected
        
    [<Fact>]
    let ``DoWhenRight returns value but does not execute action when Left value is set`` () =
        let either = Either.Left(42)
        let expected = ""
        let mutable output = ""
        Either.DoWhenRight(either, fun l -> output <- l.ToString()) |> ignore
        output |> should equal expected

module Collections =

    // Choose

    [<Fact>]
    let ``ChooseLeft<T> returns all items of type Either with Left value set`` () =
        let items = seq { yield Either.Left(21); yield Either.Left(42); yield Either.Right(63) }
        let lefts = Either.ChooseLeft(items)
        let expected = [| 21; 42 |]
        lefts |> Seq.toArray |> should equal expected
        
    [<Fact>]
    let ``ChooseRight<T> returns all items of type Either with Right value set`` () =
        let items = seq { yield Either.Right(21); yield Either.Right(42); yield Either.Left(63) }
        let lefts = Either.ChooseRight(items)
        let expected = [| 21; 42 |]
        lefts |> Seq.toArray |> should equal expected


    // Sequence

    open System.Collections.Generic

    [<Fact>]
    let ``SequenceLeft<T> returns None if any item in collection has Right value set`` () =
        let items = seq { yield Either.Left(21); yield Either.Right(42) }
        let lefts = Either.SequenceLeft(items)
        let expected = Option.None<IEnumerable<int>>()
        lefts |> should equal expected

    
    [<Fact>]
    let ``SequenceLeft<T> returns Some<T> when all items in collection has Left value set`` () =
        let items = seq { yield Either.Left(21) }
        let lefts = Either.SequenceLeft(items)
        lefts.IsSome |> should equal true
        lefts.Value |> Seq.length |> should equal 1

    [<Fact>]
    let ``SequenceRight<T> returns None if any item in collection has Left value set`` () =
        let items = seq { yield Either.Right(21); yield Either.Left(42) }
        let rights = Either.SequenceRight(items)
        let expected = Option.None<IEnumerable<int>>()
        rights |> should equal expected

    
    [<Fact>]
    let ``SequenceRight<T> returns Some<T> when all items in collection has Right value set`` () =
        let items = seq { yield Either.Right(21) }
        let lefts = Either.SequenceRight(items)
        lefts.IsSome |> should equal true
        lefts.Value |> Seq.length |> should equal 1
