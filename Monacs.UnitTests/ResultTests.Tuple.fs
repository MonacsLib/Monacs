namespace Monacs.UnitTests.ResultTests

open System
open Xunit
open FsUnit.Xunit

open Monacs.Core

module Map2 =

    let testTuple = struct ("Meaning of Life", 42)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Map2<TFst, TSnd, TResult> returns result of mapper when value is Ok<(TFst, TSnd)>`` () =
        let value = Result.Ok(testTuple)
        let expected = Result.Ok(testTuple.ToString())
        TupleResult.Map2(value, (fun x y -> (x, y).ToString())) |> should equal expected

    [<Fact>]
    let ``Map2<TFst, TSnd, TResult> returns Error<TResult> when value is Error<(TFst, TSnd)>`` () =
        let error = Errors.Error(errorMessage)
        let value = Result.Error<struct (int * string)>(error)
        let expected = Result.Error<string>(error)
        TupleResult.Map2(value, (fun _ _ -> "")) |> should equal expected

module Map3 =

    let testTuple = struct ("Meaning of Life", 42, 0.1)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Map3<TFst, TSnd, TTrd, TResult> returns result of mapper when value is Ok<(TFst, TSnd, TTrd)>`` () =
        let value = Result.Ok(testTuple)
        let expected = Result.Ok(testTuple.ToString())
        TupleResult.Map3(value, (fun x y z -> (x, y, z).ToString())) |> should equal expected

    [<Fact>]
    let ``Map3<TFst, TSnd, TTrd, TResult> returns Error<TResult> when value is Error<(TFst, TSnd, TTrd)>`` () =
        let error = Errors.Error(errorMessage)
        let value = Result.Error<struct (int * string * float)>(error)
        let expected = Result.Error<string>(error)
        TupleResult.Map3(value, (fun _ _ _ -> "")) |> should equal expected

module Bind2 =

    let testTuple = struct ("Meaning of Life", 42)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Bind2<TFst, TSnd, TResult> returns result of binder when value is Ok<(TFst, TSnd)>`` () =
        let value = Result.Ok(testTuple)
        let expected = Result.Ok(testTuple.ToString())
        TupleResult.Bind2(value, (fun x y -> Result.Ok((x, y).ToString()))) |> should equal expected

    [<Fact>]
    let ``Bind2<TFst, TSnd, TResult> returns Error<TResult> when value is Error<(TFst, TSnd)>`` () =
        let error = Errors.Error(errorMessage)
        let value = Result.Error<struct (int * string)>(error)
        let expected = Result.Error<string>(error)
        TupleResult.Bind2(value, (fun _ _ -> Result.Ok(""))) |> should equal expected

module Bind3 =

    let testTuple = struct ("Meaning of Life", 42, 0.1)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Bind3<TFst, TSnd, TTrd, TResult> returns result of binder when value is Ok<(TFst, TSnd, TTrd)>`` () =
        let value = Result.Ok(testTuple)
        let expected = Result.Ok(testTuple.ToString())
        TupleResult.Bind3(value, (fun x y z -> Result.Ok((x, y, z).ToString()))) |> should equal expected

    [<Fact>]
    let ``Bind3<TFst, TSnd, TTrd, TResult> returns Error<TResult> when value is Error<(TFst, TSnd, TTrd)>`` () =
        let error = Errors.Error(errorMessage)
        let value = Result.Error<struct (int * string * float)>(error)
        let expected = Result.Error<string>(error)
        TupleResult.Bind3(value, (fun _ _ _ -> Result.Ok(""))) |> should equal expected

module Match2 =

    let testTuple = struct ("Meaning of Life", 42)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Match2<TFst, TSnd, TResult> returns result of ok when value is Ok<(TFst, TSnd)>`` () =
        let result = Result.Ok(testTuple)
        let expected = testTuple.ToString()
        TupleResult.Match2(result,
                      ok = (fun a b -> (a,b).ToString()),
                      error = (fun e -> e.Message.Value))
        |> should equal expected

    [<Fact>]
    let ``Match2<TFst, TSnd, TResult> returns result of error when value is Error<(TFst, TSnd)>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<struct (int * string)>(error)
        TupleResult.Match2(result,
                      ok = (fun _ _ -> ""),
                      error = (fun e -> e.Message.Value))
        |> should equal errorMessage

    [<Fact>]
    let ``MatchTo2<TFst, TSnd, TResult> returns result of ok when value is Ok<(TFst, TSnd)>`` () =
        let result = Result.Ok(testTuple)
        TupleResult.MatchTo2(result, "Success", "Failure") |> should equal "Success"

    [<Fact>]
    let ``MatchTo2<TFst, TSnd, TResult> returns result of error when value is Error<(TFst, TSnd)>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<struct (int * string)>(error)
        TupleResult.MatchTo2(result, "Success", "Failure") |> should equal "Failure"

module Match3 =

    let testTuple = struct ("Some stringo", 101, 2.0)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Match3<TFst, TSnd, TTrd, TResult> returns result of ok when value is Ok<(TFst, TSnd, TTrd)>`` () =
        let result = Result.Ok(testTuple)
        let expected = testTuple.ToString()
        TupleResult.Match3(result,
                      ok = (fun a b c -> (a, b, c).ToString()),
                      error = (fun e -> e.Message.Value))
        |> should equal expected

    [<Fact>]
    let ``Match3<TFst, TSnd, TTrd, TResult> returns result of error when value is Error<TFst, TSnd, TTrd>`` () =
        let error = Errors.Error(errorMessage)
        let value = Result.Error<struct (string * int * double)>(error)
        TupleResult.Match3(value,
                      ok = (fun _ _ _ -> ""),
                      error = (fun e -> e.Message.Value))
        |> should equal errorMessage

    [<Fact>]
    let ``MatchTo3<TFst, TSnd, TTrd, TResult> returns result of ok when value is Ok<(TFst, TSnd, TTrd)>`` () =
        let result = Result.Ok(testTuple)
        TupleResult.MatchTo3(result, "Success", "Failure") |> should equal "Success"

    [<Fact>]
    let ``MatchTo2<TFst, TSnd, TTrd, TResult> returns result of error when value is Error<(TFst, TSnd, TTrd)>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<struct (int * string * double)>(error)
        TupleResult.MatchTo3(result, "Success", "Failure") |> should equal "Failure"

module ``Side effects (2 value tuple)`` =

    let testTuple = struct ("Meaning of Life", 42)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Do2<TFst, TSnd> returns value and executes action when value is Ok<(TFst, TSnd)>`` () =
        let value = Result.Ok(testTuple)
        let expected = testTuple.ToString()
        let mutable result = ""
        TupleResult.Do2(value, (fun a b -> result <- (struct (a, b)).ToString())) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do2<TFst, TSnd> returns value and doesn't execute action when value is Error<(TFst, TSnd)>`` () =
        let value = Result.Error<struct (string * int)>(Errors.Error())
        let expected = "expected"
        let mutable result = expected
        TupleResult.Do2(value, (fun _ _ -> result <- errorMessage)) |> should equal value
        result |> should equal expected

module ``Side effects (3 value tuple)`` =

    let testTuple = struct ("Some stringo", 101, 2.0)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``Do3<TFst, TSnd, TTrd> returns value and executes action when value is Ok<(TFst, TSnd, TTrd)>`` () =
        let value = Result.Ok(testTuple)
        let expected = testTuple.ToString()
        let mutable result = ""
        TupleResult.Do3(value, (fun a b c -> result <- (struct (a, b, c)).ToString())) |> should equal value
        result |> should equal expected

    [<Fact>]
    let ``Do3<TFst, TSnd, TTrd> returns value and doesn't execute action when value is Error<(TFst, TSnd, TTrd)>`` () =
        let value = Result.Error<struct (string * int * double)>(Errors.Error())
        let expected = "expected"
        let mutable result = expected
        TupleResult.Do3(value, (fun _ _ _ -> result <- errorMessage)) |> should equal value
        result |> should equal expected

module TryCatch2 =

    let testTuple = struct ("Meaning of Life", 42)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``TryCatch2<TFst, TSnd, TResult> returns Ok<TResult> when previous result is Ok<(TFst, TSnd)> and function call doesn't throw`` () =
        let result = Result.Ok(testTuple)
        TupleResult.TryCatch2(result, (fun a b -> struct (a, b)), (fun _ _ _ -> Errors.Error())) |> should equal (Result.Ok(testTuple))

    [<Fact>]
    let ``TryCatch2<TFst, TSnd, TResult> returns Error<TResult> when previous result is Ok<(TFst, TSnd)> and function call throws`` () =
        let message = errorMessage
        let result = Result.Ok(testTuple)
        TupleResult.TryCatch2(result,
                         tryFunc = (fun _ _ -> raise(Exception(message))),
                         errorHandler = (fun a b e -> Errors.Error((struct (a, b)).ToString() + e.Message)))
        |> should equal (Result.Error(Errors.Error(testTuple.ToString() + message)))

    [<Fact>]
    let ``TryCatch2<TFst, TSnd, TResult> returns Error<TResult> when previous result is Error<(TFst, TSnd)>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<struct (string * int)>(error)
        TupleResult.TryCatch2(result,
                         tryFunc = (fun _ _ -> "This should be omitted."),
                         errorHandler = (fun _ _ _ -> Errors.Error())) |> should equal (Result.Error<string>(error))

module TryCatch3 =

    let testTuple = struct ("Some stringo", 101, 2.0)
    let errorMessage = "Some error message."

    [<Fact>]
    let ``TryCatch3<TFst, TSnd, TResult> returns Ok<TResult> when previous result is Ok<(TFst, TSnd)> and function call doesn't throw`` () =
        let result = Result.Ok(testTuple)
        TupleResult.TryCatch3(result, (fun a b c -> struct (a, b, c)), (fun _ _ _ _ -> Errors.Error())) |> should equal (Result.Ok(testTuple))

    [<Fact>]
    let ``TryCatch3<TFst, TSnd, TResult> returns Error<TResult> when previous result is Ok<(TFst, TSnd)> and function call throws`` () =
        let message = errorMessage
        let result = Result.Ok(testTuple)
        TupleResult.TryCatch3(result,
                         tryFunc = (fun _ _ _ -> raise(Exception(message))),
                         errorHandler = (fun a b c e -> Errors.Error((struct (a, b, c)).ToString() + e.Message)))
        |> should equal (Result.Error(Errors.Error(testTuple.ToString() + message)))

    [<Fact>]
    let ``TryCatch3<TFst, TSnd, TResult> returns Error<TResult> when previous result is Error<TFst, TSnd>`` () =
        let error = Errors.Error(errorMessage)
        let result = Result.Error<struct (string * int)>(error)
        TupleResult.TryCatch2(result,
                         tryFunc = (fun _ _ -> "This should be omitted."),
                         errorHandler = (fun _ _ _ -> Errors.Error())) |> should equal (Result.Error<string>(error))
