namespace Monacs.UnitTests.ResultTests

open System
open Xunit
open FsUnit.Xunit

open Monacs.Core

module ``Constructors and equality`` =

    [<Fact>]
    let ``Ok<T> equals Ok<T> when the Value is equal`` () =
        let value = "test"
        Result.Ok(value) = Result.Ok(value) |> should equal true
        Result.Ok(value) <> Result.Ok(value) |> should equal false

    [<Fact>]
    let ``Ok<T> doesn't equal Ok<T> when the Value is not equal`` () =
        Result.Ok(42) = Result.Ok(13) |> should equal false
        Result.Ok(42) <> Result.Ok(13) |> should equal true

    [<Fact>]
    let ``Error<T> equals Error<T> when the Error is equal`` () =
        let error = Errors.Error()
        Result.Error<string>(error) = Result.Error<string>(error) |> should equal true
        Result.Error<string>(error) <> Result.Error<string>(error) |> should equal false

    [<Fact>]
    let ``Error<T> doesn't equal Error<T> when the Error is not equal`` () =
        let error1 = Errors.Error("test1")
        let error2 = Errors.Error("test2")
        Result.Error<string>(error1) = Result.Error<string>(error2) |> should equal false
        Result.Error<string>(error1) <> Result.Error<string>(error2) |> should equal true

    [<Fact>]
    let ``Ok<T> doesn't equal Error<T>`` () =
        let error = Errors.Error()
        let value = 42
        Result.Ok(value) = Result.Error<int>(error) |> should equal false
        Result.Ok(value) <> Result.Error<int>(error) |> should equal true

module Converters =

    [<Fact>]
    let ``OfObject<T> returns Error<T> when value is null`` () =
        let error = Errors.Error()
        Result.OfObject<System.Object>(null, error) |> should equal (Result.Error<System.Object>(error))

    [<Fact>]
    let ``OfObject<T> returns Ok<T> when value is not null`` () =
        let object = obj()
        Result.OfObject(object, Errors.Error()) |> should equal (Result.Ok(object))

    [<Fact>]
    let ``OfObject<T> with func returns Error<T> when value is null`` () =
        let error = Errors.Error()
        Result.OfObject<System.Object>(null, (fun () -> error)) |> should equal (Result.Error<System.Object>(error))

    [<Fact>]
    let ``OfObject<T> with func returns Ok<T> when value is not null`` () =
        let object = obj()
        Result.OfObject(object, (fun () -> Errors.Error())) |> should equal (Result.Ok(object))

    [<Fact>]
    let ``OfNullable<T> returns Error<T> when value is null`` () =
        let empty = new Nullable<int>()
        let error = Errors.Error()
        Result.OfNullable<int>(empty, error) |> should equal (Result.Error<int>(error))

    [<Fact>]
    let ``OfNullable<T> returns Ok<T> when value is not null`` () =
        let value = Nullable(42)
        Result.OfNullable(value, Errors.Error()) |> should equal (Result.Ok(value.Value))

    [<Fact>]
    let ``OfNullable<T> with func returns Error<T> when value is null`` () =
        let empty = new Nullable<int>()
        let error = Errors.Error()
        Result.OfNullable<int>(empty, (fun () -> error)) |> should equal (Result.Error<int>(error))

    [<Fact>]
    let ``OfNullable<T> with func returns Ok<T> when value is not null`` () =
        let value = Nullable(42)
        Result.OfNullable(value, (fun () -> Errors.Error())) |> should equal (Result.Ok(value.Value))

    [<Fact>]
    let ``OfString<T> returns Error<T> when value is null`` () =
        let error = Errors.Error()
        Result.OfString(null, error) |> should equal (Result.Error<string>(error))

    [<Fact>]
    let ``OfString<T> returns Error<T> when value is empty`` () =
        let error = Errors.Error()
        Result.OfString("", error) |> should equal (Result.Error<string>(error))

    [<Fact>]
    let ``OfString<T> returns Ok<T> when value is not null and not empty`` () =
        let value = "test"
        Result.OfString(value, Errors.Error()) |> should equal (Result.Ok(value))

    [<Fact>]
    let ``OfString<T> with func returns Error<T> when value is null`` () =
        let error = Errors.Error()
        Result.OfString(null, (fun () -> error)) |> should equal (Result.Error<string>(error))

    [<Fact>]
    let ``OfString<T> with func returns Error<T> when value is empty`` () =
        let error = Errors.Error()
        Result.OfString("", (fun () -> error)) |> should equal (Result.Error<string>(error))

    [<Fact>]
    let ``OfString<T> with func returns Ok<T> when value is not null and not empty`` () =
        let value = "test"
        Result.OfString(value, (fun () -> Errors.Error())) |> should equal (Result.Ok(value))

    [<Fact>]
    let ``OfOption<T> returns Error<T> when value is None`` () =
        let empty = Option.None<int>()
        let error = Errors.Error()
        Result.OfOption<int>(empty, error) |> should equal (Result.Error<int>(error))

    [<Fact>]
    let ``OfOption<T> returns Ok<T> when value is Some`` () =
        let value = Option.Some(42)
        Result.OfOption(value, Errors.Error()) |> should equal (Result.Ok(value.Value))

    [<Fact>]
    let ``OfOption<T> with func returns Error<T> when value is None`` () =
        let empty = Option.None<int>()
        let error = Errors.Error()
        Result.OfOption<int>(empty, (fun () -> error)) |> should equal (Result.Error<int>(error))

    [<Fact>]
    let ``OfOption<T> with func returns Ok<T> when value is Some`` () =
        let value = Option.Some(42)
        Result.OfOption(value, (fun () -> Errors.Error())) |> should equal (Result.Ok(value.Value))

module Match =

    [<Fact>]
    let ``Match<T1, T2> returns result of error when value is Error<T1>`` () =
        let expected = "test"
        let error = Errors.Error(expected)
        let value = Result.Error<int>(error)
        Result.Match(value, ok = (fun _ -> ""), error = (fun e -> e.Message.Value)) |> should equal expected

    [<Fact>]
    let ``Match<T1, T2> returns result of ok when value is Ok<T1>`` () =
        let value = Result.Ok(42)
        let expected = "test"
        Result.Match(value, ok = (fun _ -> expected), error = (fun _ -> "")) |> should equal expected

    [<Fact>]
    let ``MatchTo<T1, T2> returns result of error when value is Error<T1>`` () =
        let expected = "test"
        let value = Result.Error<int>(Errors.Error())
        Result.MatchTo(value, ok = "", error = expected) |> should equal expected

    [<Fact>]
    let ``MatchTo<T1, T2> returns result of ok when value is Ok<T1>`` () =
        let value = Result.Ok(42)
        let expected = "test"
        Result.MatchTo(value, ok = expected, error = "") |> should equal expected

module Bind =

    [<Fact>]
    let ``Bind<T1, T2> returns result of binder when value is Ok<T1>`` () =
        let value = Result.Ok(42)
        let expected = Result.Ok("42")
        Result.Bind(value, (fun x -> Result.Ok(x.ToString()))) |> should equal expected

    [<Fact>]
    let ``Bind<T1, T2> returns Error<T2> when value is Error<T1>`` () =
        let error = Errors.Error()
        let value = Result.Error<int>(error)
        let expected = Result.Error<string>(error)
        Result.Bind(value, (fun x -> Result.Ok(x.ToString()))) |> should equal expected

module Map =

    [<Fact>]
    let ``Map<T1, T2> returns result of mapper when value is Ok<T1>`` () =
        let value = Result.Ok(42)
        let expected = Result.Ok("42")
        Result.Map(value, (fun x -> x.ToString())) |> should equal expected

    [<Fact>]
    let ``Map<T1, T2> returns Error<T2> when value is Error<T1>`` () =
        let error = Errors.Error()
        let value = Result.Error<int>(error)
        let expected = Result.Error<string>(error)
        Result.Map(value, (fun x -> x.ToString())) |> should equal expected

module GetOrDefault =

    [<Fact>]
    let ``GetOrDefault<T> returns encapsulated value when value is Ok<T>`` () =
        let value = Result.Ok("test")
        let expected = "test"
        Result.GetOrDefault(value) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T> returns type default when value is Error<T>`` () =
        let value = Result.Error<obj>(Errors.Error())
        let expected = null
        Result.GetOrDefault(value) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T> returns given default when value is Error<T>`` () =
        let value = Result.Error<string>(Errors.Error())
        let expected = "test"
        Result.GetOrDefault(value, whenError = expected) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T1, T2> returns getter result when value is Ok<T1>`` () =
        let value = Result.Ok((1, "test"))
        let expected = "test"
        Result.GetOrDefault(value, fun (_, x) -> x) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T1, T2> returns type default when value is Error<T1>`` () =
        let value = Result.Error<obj>(Errors.Error())
        let expected = 0
        Result.GetOrDefault(value, getter = (fun x -> x.GetHashCode())) |> should equal expected

    [<Fact>]
    let ``GetOrDefault<T1, T2> returns given default when value is Error<T1>`` () =
        let value = Result.Error<string>(Errors.Error())
        let expected = 42
        Result.GetOrDefault(value, getter = (fun x -> x.GetHashCode()), whenError = expected) |> should equal expected

module ``Side effects`` =

    [<Fact>]
    let ``Do<T> returns value and executes action when value is Ok<T>`` () =
        let value = Result.Ok(42)
        let expected = "42"
        let mutable result = ""
        Result.Do(value, (fun x -> result <- expected.ToString())) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do<T> returns value and doesn't execute action when value is Error<T>`` () =
        let value = Result.Error<int>(Errors.Error())
        let expected = "test"
        let mutable result = expected
        Result.Do(value, (fun x -> result <- "fail")) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``DoWhenError<T> returns value and doesn't execute action when value is Ok<T>`` () =
        let value = Result.Ok(42)
        let expected = "test"
        let mutable result = expected
        Result.DoWhenError(value, (fun x -> result <- "fail")) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do<T> returns value and executes action when value is Error<T>`` () =
        let value = Result.Error<int>(Errors.Error())
        let expected = "42"
        let mutable result = ""
        Result.DoWhenError(value, (fun x -> result <- expected.ToString())) |> should equal value
        result |> should equal expected

module Collections =

    [<Fact>]
    let ``Choose<T> returns collection of values of items which are not Error<T>`` () =
        let values = seq { yield Result.Ok(42); yield Result.Error(Errors.Error()); yield Result.Ok(123) }
        let expected = [| 42; 123 |]
        Result.Choose(values) |> Seq.toArray |> should equal expected

    [<Fact>]
    let ``ChooseErrors<T> returns collection of errors from items which are Error<T>`` () =
        let expected = [| Errors.Error("1"); Errors.Info("2") |]
        let values = seq { yield Result.Ok(42); yield Result.Error(expected.[0]); yield Result.Ok(123); yield Result.Error(expected.[1]); }
        Result.ChooseErrors(values) |> Seq.toArray |> should equal expected

    [<Fact>]
    let ``Sequence<T> returns collection of values wrapped into Result when all items are not Error<T>`` () =
        let values = seq { yield Result.Ok(42); yield Result.Ok(123) }
        let expected = [| 42; 123 |]
        let result = Result.Sequence(values)
        result.IsOk |> should equal true
        result.Value |> Seq.toArray |> should equal expected

    [<Fact>]
    let ``Sequence<T> returns Error<IEnumerable<T>> with first error when any item is Error<T>`` () =
        let error = Errors.Error("error!")
        let values = seq { yield Result.Ok(42); yield Result.Error(error); yield Result.Error(Errors.Error()) }
        let expected = Result.Error<int seq>(error)
        Result.Sequence(values) |> should equal expected
