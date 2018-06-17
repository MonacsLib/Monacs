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
    let ``Either<L, R> should be compared by values`` () =
        let e1 = Either.Left<int, int>(42)
        let e2 = Either.Left<obj, int>(42)
        e1 |> should equal e2


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
        let either = Either.Left(42)
        let expected = Either.Left(84)
        Either.Map(either, mapLeft = (fun l -> l * 2), mapRight = (fun r -> r * 3)) |> should equal expected

    
    [<Fact>]
    let ``Maps right side when RightValue is set`` () =
        let either = Either.Right(42)
        let expected = Either.Right(126)
        Either.Map(either, mapLeft = (fun l -> l * 2), mapRight = (fun r -> r * 3)) |> should equal expected