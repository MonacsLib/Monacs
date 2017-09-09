namespace Monacs.UnitTests.OptionTests

open System
open Xunit
open FsUnit.Xunit

open Monacs.Core

module ``Constructors and equality`` =

    [<Fact>]
    let ``None<T> equals None<T>`` () =
        Option.None<string>() = Option.None<string>() |> should equal true
        Option.None<string>() <> Option.None<string>() |> should equal false

    [<Fact>]
    let ``Some<T> equals Some<T> when the Value is equal`` () =
        let value = "test"
        Option.Some(value) = Option.Some(value) |> should equal true
        Option.Some(value) <> Option.Some(value) |> should equal false

    [<Fact>]
    let ``Some<T> doesn't equal Some<T> when the Value is not equal`` () =
        Option.Some(42) = Option.Some(13) |> should equal false
        Option.Some(42) <> Option.Some(13) |> should equal true

    [<Fact>]
    let ``Some<T> doesn't equal None<T>`` () =
        Option.Some(42) = Option.None<int>() |> should equal false
        Option.Some(42) <> Option.None<int>() |> should equal true

module Converters =

    [<Fact>]
    let ``OfObject<T> returns None<T> when value is null`` () =
        Option.OfObject<System.Object>(null) |> should equal (Option.None<System.Object>())

    [<Fact>]
    let ``OfObject<T> returns Some<T> when value is not null`` () =
        let object = System.Object
        Option.OfObject(object) |> should equal (Option.Some(object))
    
    [<Fact>]
    let ``OfNullable<T> returns None<T> when value is null`` () =
        let empty = new Nullable<int>()
        Option.OfNullable<int>(empty) |> should equal (Option.None<int>())

    [<Fact>]
    let ``OfNullable<T> returns Some<T> when value is not null`` () =
        let value = Nullable(42)
        Option.OfNullable(value) |> should equal (Option.Some(value.Value))
    
    [<Fact>]
    let ``OfString<T> returns None<T> when value is null`` () =
        Option.OfString(null) |> should equal (Option.None<string>())
    
    [<Fact>]
    let ``OfString<T> returns None<T> when value is empty`` () =
        Option.OfString("") |> should equal (Option.None<string>())

    [<Fact>]
    let ``OfString<T> returns Some<T> when value is not null and not empty`` () =
        let value = "test"
        Option.OfString(value) |> should equal (Option.Some(value))

module Match =

    [<Fact>]
    let ``Match<T1, T2> returns result of none when value is None<T1>`` () =
        let value = Option.None<int>()
        let expected = "test"
        Option.Match(value, some = (fun _ -> ""), none = (fun () -> expected)) |> should equal expected
        
    [<Fact>]
    let ``Match<T1, T2> returns result of some when value is Some<T1>`` () =
        let value = Option.Some(42)
        let expected = "test"
        Option.Match(value, some = (fun _ -> expected), none = (fun () -> "")) |> should equal expected
    
    [<Fact>]
    let ``MatchTo<T1, T2> returns result of none when value is None<T1>`` () =
        let value = Option.None<int>()
        let expected = "test"
        Option.MatchTo(value, some = "", none = expected) |> should equal expected
        
    [<Fact>]
    let ``MatchTo<T1, T2> returns result of some when value is Some<T1>`` () =
        let value = Option.Some(42)
        let expected = "test"
        Option.MatchTo(value, some = expected, none = "") |> should equal expected