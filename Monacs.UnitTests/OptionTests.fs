namespace Monacs.UnitTests.OptionTests

open System
open Xunit
open FsUnit.Xunit

open Monacs.Core

module ``Constructors and equality`` =

    [<Fact>]
    let ``Some<T> equals itself`` () =
        let opt = Option.Some("test")
        opt = opt |> should equal true
        opt <> opt |> should equal false

    [<Fact>]
    let ``Some<T> doesn't equal null`` () =
        let opt = box (Option.Some("test"))
        opt = null |> should equal false
        opt <> null |> should equal true

    [<Fact>]
    let ``None<T> equals itself`` () =
        let opt = Option.None<string>()
        opt = opt |> should equal true
        opt <> opt |> should equal false

    [<Fact>]
    let ``None<T> doesn't equal null`` () =
        let opt = box (Option.None<string>())
        opt = null |> should equal false
        opt <> null |> should equal true

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
        let object = obj()
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
    let ``ToNullable<T> returns null when value is None<T>`` () =
        let none = Option.None<int>()
        Option.ToNullable(none) |> should equal (new Nullable<int>())

    [<Fact>]
    let ``ToNullable<T> returns value when value is Some<T>`` () =
        let some = Option.Some(42)
        Option.ToNullable(some) |> should equal (Nullable(some.Value))

    [<Fact>]
    let ``OfResult<T> returns None<T> when value Error<T>`` () =
        let error = Result.Error<int>(Errors.Info())
        Option.OfResult<int>(error) |> should equal (Option.None<int>())

    [<Fact>]
    let ``OfResult<T> returns Some<T> when value is Ok<T>`` () =
        let result = Result.Ok(42)
        Option.OfResult(result) |> should equal (Option.Some(result.Value))

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

module Bind =

    [<Fact>]
    let ``Bind<T1, T2> returns result of binder when value is Some<T1>`` () =
        let value = Option.Some(42)
        let expected = Option.Some("42")
        Option.Bind(value, (fun x -> Option.Some(x.ToString()))) |> should equal expected

    [<Fact>]
    let ``Bind<T1, T2> returns None<T2> when value is None<T1>`` () =
        let value = Option.None<int>()
        let expected = Option.None<string>()
        Option.Bind(value, (fun x -> Option.Some(x.ToString()))) |> should equal expected

module Map =

    [<Fact>]
    let ``Map<T1, T2> returns result of mapper when value is Some<T1>`` () =
        let value = Option.Some(42)
        let expected = Option.Some("42")
        Option.Map(value, (fun x -> x.ToString())) |> should equal expected

    [<Fact>]
    let ``Map<T1, T2> returns None<T2> when value is None<T1>`` () =
        let value = Option.None<int>()
        let expected = Option.None<string>()
        Option.Map(value, (fun x -> x.ToString())) |> should equal expected

module GetOrDefault =

    [<Fact>]
    let ``GetOrDefault<T> returns encapsulated value when value is Some<T>`` () =
        let value = Option.Some("test")
        let expected = "test"
        Option.GetOrDefault(value) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T> returns type default when value is None<T>`` () =
        let value = Option.None<obj>()
        let expected = null
        Option.GetOrDefault(value) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T> returns given default when value is None<T>`` () =
        let value = Option.None<string>()
        let expected = "test"
        Option.GetOrDefault(value, whenNone = expected) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T1, T2> returns getter result when value is Some<T1>`` () =
        let value = Option.Some((1, "test"))
        let expected = "test"
        Option.GetOrDefault(value, fun (_, x) -> x) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T1, T2> returns type default when value is None<T1>`` () =
        let value = Option.None<obj>()
        let expected = 0
        Option.GetOrDefault(value, getter = (fun x -> x.GetHashCode())) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T1, T2> returns given default when value is None<T1>`` () =
        let value = Option.None<string>()
        let expected = 42
        Option.GetOrDefault(value, getter = (fun x -> x.GetHashCode()), whenNone = expected) |> should equal expected

module ``Side effects`` =

    [<Fact>]
    let ``Do<T> returns value and executes action when value is Some<T>`` () =
        let value = Option.Some(42)
        let expected = "42"
        let mutable result = ""
        Option.Do(value, (fun x -> result <- expected.ToString())) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do<T> returns value and doesn't execute action when value is None<T>`` () =
        let value = Option.None<int>()
        let expected = "test"
        let mutable result = expected
        Option.Do(value, (fun x -> result <- "fail")) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``DoWhenNone<T> returns value and doesn't execute action when value is Some<T>`` () =
        let value = Option.Some(42)
        let expected = "test"
        let mutable result = expected
        Option.DoWhenNone(value, (fun x -> result <- "fail")) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do<T> returns value and executes action when value is None<T>`` () =
        let value = Option.None<int>()
        let expected = "42"
        let mutable result = ""
        Option.DoWhenNone(value, (fun x -> result <- expected.ToString())) |> should equal value
        result |> should equal expected

module Collections =

    [<Fact>]
    let ``Choose<T> returns collection of values of items which are not None<T>`` () =
        let values = seq { yield Option.Some(42); yield Option.None(); yield Option.Some(123) }
        let expected = [| 42; 123 |]
        Option.Choose(values) |> Seq.toArray |> should equal expected

    [<Fact>]
    let ``Sequence<T> returns collection of values wrapped into Option when all items are not None<T>`` () =
        let values = seq { yield Option.Some(42); yield Option.Some(123) }
        let expected = [| 42; 123 |]
        let result = Option.Sequence(values)
        result.IsSome |> should equal true
        result.Value |> Seq.toArray |> should equal expected

    [<Fact>]
    let ``Sequence<T> returns None<IEnumerable<T>> when any item is None<T>`` () =
        let values = seq { yield Option.Some(42); yield Option.Some(123); yield Option.None() }
        let expected = Option.None<int seq>()
        Option.Sequence(values) |> should equal expected
