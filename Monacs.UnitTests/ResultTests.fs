namespace Monacs.UnitTests.ResultTests

open System
open Xunit
open FsUnit.Xunit

open Monacs.Core

module ``Constructors and equality`` =

    [<Fact>]
    let ``Ok<T> equals itself`` () =
        let result = Result.Ok("test")
        result = result |> should equal true
        result <> result |> should equal false

    [<Fact>]
    let ``Ok<T> doesn't equal null`` () =
        let result = box (Result.Ok("test"))
        result = null |> should equal false
        result <> null |> should equal true

    [<Fact>]
    let ``Error<T> equals itself`` () =
        let result = Result.Error(Errors.Debug("test"))
        result = result |> should equal true
        result <> result |> should equal false

    [<Fact>]
    let ``Error<T> doesn't equal null`` () =
        let result = box (Result.Error(Errors.Debug("test")))
        result = null |> should equal false
        result <> null |> should equal true

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

module TryGetResult =
    open System.Collections.Generic
    open System.Linq

    [<Fact>]
    let ``TryGetResult<TKey, TValue> returns Error<TValue> when key is not present in dictionary`` () =
        let dict = new Dictionary<int, string>()
        let error = Errors.Error()
        Result.TryGetResult(dict, 1, error) |> should equal (Result.Error<string>(error))

    [<Fact>]
    let ``TryGetResult<TKey, TValue> returns Ok<TValue> when key is present in dictionary`` () =
        let dict = new Dictionary<int, string>()
        let key = 42
        let value = "hello"
        dict.Add(42, value)
        Result.TryGetResult(dict, key, Errors.Error()) |> should equal (Result.Ok(value))

    [<Fact>]
    let ``TryGetResult<TKey, TValue> with func returns Error<TValue> when key is not present in dictionary`` () =
        let dict = new Dictionary<string, int>()
        let key = "foo"
        Result.TryGetResult(dict, key, (fun k -> Errors.Error(k))) |> should equal (Result.Error<int>(Errors.Error(key)))

    [<Fact>]
    let ``TryGetResult<TKey, TValue> with func returns Ok<TValue> when key is present in dictionary`` () =
        let dict = new Dictionary<int, string>()
        let key = 42
        let value = "hello"
        dict.Add(42, value)
        Result.TryGetResult(dict, key, (fun _ -> Errors.Error())) |> should equal (Result.Ok(value))

    [<Fact>]
    let ``TryGetResult<TKey, TValue> returns Error<IEnumerable<TValue>> when key is not present in lookup`` () =
        let lookup = [1].ToLookup(fun k -> k)
        let error = Errors.Error()
        Result.TryGetResult(lookup, 2, error) |> should equal (Result.Error<IEnumerable<int>>(error))

    [<Fact>]
    let ``TryGetResult<TKey, TValue> returns Ok<IEnumerable<TValue>> when key is present in lookup`` () =
        let key = 42
        let value = "hello"
        let lookup = [(key, value)].ToLookup((fun (k, _) -> k), (fun (_, v) -> v))
        let result = Result.TryGetResult(lookup, key, Errors.Error())
        result.IsOk |> should equal true
        result.Value.Count() |> should equal 1
        result.Value |> should contain value

    [<Fact>]
    let ``TryGetResult<TKey, TValue> with func returns Error<IEnumerable<TValue>> when key is not present in lookup`` () =
        let lookup = ["bar"].ToLookup(fun k -> k)
        let key = "foo"
        Result.TryGetResult(lookup, key, (fun k -> Errors.Error(k))) |> should equal (Result.Error<IEnumerable<string>>(Errors.Error(key)))

    [<Fact>]
    let ``TryGetResult<TKey, TValue> with func returns Ok<IEnumerable<TValue>> when key is present in lookup`` () =
        let key = 42
        let value = "hello"
        let lookup = [(key, value)].ToLookup((fun (k, _) -> k), (fun (_, v) -> v))
        let result = Result.TryGetResult(lookup, key, (fun _ -> Errors.Error()))
        result.IsOk |> should equal true
        result.Value.Count() |> should equal 1
        result.Value |> should contain value

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
        Result.Do(value, (fun _ -> result <- expected.ToString())) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do<T> returns value and doesn't execute action when value is Error<T>`` () =
        let value = Result.Error<int>(Errors.Error())
        let expected = "test"
        let mutable result = expected
        Result.Do(value, (fun _ -> result <- "fail")) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``DoWhenError<T> returns value and doesn't execute action when value is Ok<T>`` () =
        let value = Result.Ok(42)
        let expected = "test"
        let mutable result = expected
        Result.DoWhenError(value, (fun _ -> result <- "fail")) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``DoWhenError<T> returns value and executes action when value is Error<T>`` () =
        let value = Result.Error<int>(Errors.Error())
        let expected = "42"
        let mutable result = ""
        Result.DoWhenError(value, (fun _ -> result <- expected.ToString())) |> should equal value
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

module TryCatch =

    [<Fact>]
    let ``TryCatch<T> returns Ok<T> when function call doesn't throw`` () =
        let value = 42
        Result.TryCatch((fun () -> value), (fun _ -> Errors.Error())) |> should equal (Result.Ok(value))

    [<Fact>]
    let ``TryCatch<T> returns Error<T> when function call throws`` () =
        let message = "This should not happen!"
        Result.TryCatch((fun () -> raise(Exception(message))), (fun e -> Errors.Error(e.Message))) |> should equal (Result.Error(Errors.Error(message)))

    [<Fact>]
    let ``TryCatch<T1, T2> returns Ok<T2> when previous result is Ok<T1> and function call doesn't throw`` () =
        let result = Result.Ok(42)
        let value = "42"
        Result.TryCatch(result, (fun v -> v.ToString()), (fun _ _ -> Errors.Error())) |> should equal (Result.Ok(value))

    [<Fact>]
    let ``TryCatch<T1, T2> returns Error<T2> when previous result is Ok<T1> and function call throws`` () =
        let value = "OK"
        let message = "This should not happen!"
        let result = Result.Ok(value)
        Result.TryCatch(result, (fun _ -> raise(Exception(message))), (fun v e -> Errors.Error(v + e.Message))) |> should equal (Result.Error(Errors.Error(value + message)))

    [<Fact>]
    let ``TryCatch<T1, T2> returns Error<T2> when previous result is Error<T1>`` () =
        let error = Errors.Error("Oh no!")
        let result = Result.Error<int>(error)
        Result.TryCatch(result, (fun v -> v.ToString()), (fun _ _ -> Errors.Error())) |> should equal (Result.Error<string>(error))

module Match2 =
    open Monacs.Core.Tuples

    let testTuple = ("Meaning of Life", 42).ToValueTuple()
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Match<TFst, TSnd> returns result of ok when value is Ok<(TFst, TSnd)>`` () =
        let result = Result.Ok(testTuple)
        let expected = testTuple.ToString()
        Result.Match2(result,
                      ok = (fun a b -> (a,b).ToString()),
                      error = (fun e -> e.Message.Value))
        |> should equal expected

    [<Fact>]
    let ``Match<TFst, TSnd> returns result of error when value is Error<TFst, TSnd>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<ValueTuple<int, string>>(error)
        Result.Match2(result,
                      ok = (fun a b -> (a, b).ToString()),
                      error = (fun e -> e.Message.Value))
        |> should equal errorMessage

    [<Fact>]
    let ``MatchTo2<TFst, TSnd, TVal> returns result of ok when value is Ok<(TFst, TSnd)>`` () =
        let result = Result.Ok(testTuple)
        Result.MatchTo2(result, "Success", "Failure") |> should equal "Success"

    [<Fact>]
    let ``MatchTo2<TFst, TSnd, TVal> returns result of error when value is Error<(TFst, TSnd)>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<ValueTuple<int, string>>(error)
        Result.MatchTo2(result, "Success", "Failure") |> should equal "Failure"

module Match3 =
    open Monacs.Core.Tuples

    let testTuple = ("Some stringo", 101, 2.0).ToValueTuple()
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Match<TFst, TSnd, TTrd> returns result of ok when value is Ok<(TFst, TSnd, TTrd)>`` () =
        let result = Result.Ok(testTuple)
        let expected = testTuple.ToString()
        Result.Match3(result,
                      ok = (fun a b c -> (a, b, c).ToString()),
                      error = (fun e -> e.Message.Value))
        |> should equal expected

    [<Fact>]
    let ``Match<TFst, TSnd, TTrd> returns result of error when value is Error<TFst, TSnd>`` () =
        let error = Errors.Error(errorMessage)
        let value = Result.Error<ValueTuple<string, int, double>>(error)
        Result.Match3(value,
                      ok = (fun a b c -> (a, b).ToString()),
                      error = (fun e -> e.Message.Value))
        |> should equal errorMessage

    [<Fact>]
    let ``MatchTo2<TFst, TSnd, TVal> returns result of ok when value is Ok<(TFst, TSnd)>`` () =
        let result = Result.Ok(testTuple)
        Result.MatchTo3(result, "Success", "Failure") |> should equal "Success"

    [<Fact>]
    let ``MatchTo2<TFst, TSnd, TVal> returns result of error when value is Error<(TFst, TSnd)>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<ValueTuple<int, string, double>>(error)
        Result.MatchTo3(result, "Success", "Failure") |> should equal "Failure"

module ``Side effects (2 value tuple)`` =
    open Monacs.Core.Tuples

    let testTuple = ("Meaning of Life", 42).ToValueTuple()
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Do<TFst, TSnd> returns value and executes action when value is Ok<TFst, TSnd>`` () =
        let value = Result.Ok(testTuple)
        let expected = testTuple.ToString()
        let mutable result = ""
        Result.Do2(value, (fun a b -> result <- (a, b).ToValueTuple().ToString())) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do<TFst, TSnd> returns value and doesn't execute action when value is Error<TFst, TSnd>`` () =
        let value = Result.Error<ValueTuple<string, int>>(Errors.Error())
        let expected = "expected"
        let mutable result = expected
        Result.Do2(value, (fun a b -> result <- errorMessage)) |> should equal value
        result |> should equal expected

module ``Side effects (3 value tuple)`` =
    open Monacs.Core.Tuples

    let testTuple = ("Some stringo", 101, 2.0).ToValueTuple()
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Do<TFst, TSnd, TTrd> returns value and executes action when value is Ok<TFst, TSnd, TTrd>`` () =
        let value = Result.Ok(testTuple)
        let expected = testTuple.ToString()
        let mutable result = ""
        Result.Do3(value, (fun a b c -> result <- (a, b, c).ToValueTuple().ToString())) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do<TFst, TSnd, TTrd> returns value and doesn't execute action when value is Error<TFst, TSnd, TTrd>`` () =
        let value = Result.Error<ValueTuple<string, int, double>>(Errors.Error())
        let expected = "expected"
        let mutable result = expected
        Result.Do3(value, (fun a b c -> result <- errorMessage)) |> should equal value
        result |> should equal expected

module TryCatch2 =
    open Monacs.Core.Tuples

    let testTuple = ("Meaning of Life", 42).ToValueTuple()
    let errorMessage = "Some error message."

    [<Fact>]
    let ``TryCatch<TValue, TFst, TSnd> returns Ok<TValue> when previous result is Ok<(TFst, TSnd)> and function call doesn't throw`` () =
        let result = Result.Ok(testTuple)
        Result.TryCatch2(result, (fun a b -> (a, b).ToValueTuple()), (fun _ _ _ -> Errors.Error())) |> should equal (Result.Ok(testTuple))

    [<Fact>]
    let ``TryCatch<TValue, TFst, TSnd>  returns Error<TValue> when previous result is Ok<TFst, TSnd> and function call throws`` () =
        let message = errorMessage
        let result = Result.Ok(testTuple)
        Result.TryCatch2(result,
                         tryFunc = (fun _ _ -> raise(Exception(message))),
                         errorHandler = (fun a b e -> Errors.Error((a, b).ToValueTuple().ToString() + e.Message)))
        |> should equal (Result.Error(Errors.Error(testTuple.ToString() + message)))

    [<Fact>]
    let ``TryCatch<TValue, TFst, TSnd>  returns Error<TValue> when previous result is Error<TFst, TSnd>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<ValueTuple<string, int>>(error)
        Result.TryCatch2(result,
                         tryFunc = (fun _ _ -> "This should be omitted."),
                         errorHandler = (fun _ _ _ -> Errors.Error())) |> should equal (Result.Error<string>(error))

module TryCatch3 =
    open Monacs.Core.Tuples

    let testTuple = ("Some stringo", 101, 2.0).ToValueTuple()
    let errorMessage = "Some error message."

    [<Fact>]
    let ``TryCatch<TValue, TFst, TSnd> returns Ok<TValue> when previous result is Ok<(TFst, TSnd)> and function call doesn't throw`` () =
        let result = Result.Ok(testTuple)
        Result.TryCatch3(result, (fun a b c -> (a, b, c).ToValueTuple()), (fun _ _ _ _ -> Errors.Error())) |> should equal (Result.Ok(testTuple))

    [<Fact>]
    let ``TryCatch<TValue, TFst, TSnd>  returns Error<TValue> when previous result is Ok<TFst, TSnd> and function call throws`` () =
        let message = errorMessage
        let result = Result.Ok(testTuple)
        Result.TryCatch3(result,
                         tryFunc = (fun _ _ _ -> raise(Exception(message))),
                         errorHandler = (fun a b c e -> Errors.Error((a, b, c).ToValueTuple().ToString() + e.Message)))
        |> should equal (Result.Error(Errors.Error(testTuple.ToString() + message)))

    [<Fact>]
    let ``TryCatch<TValue, TFst, TSnd>  returns Error<TValue> when previous result is Error<TFst, TSnd>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<ValueTuple<string, int>>(error)
        Result.TryCatch2(result,
                         tryFunc = (fun _ _ -> "This should be omitted."),
                         errorHandler = (fun _ _ _ -> Errors.Error())) |> should equal (Result.Error<string>(error))
