namespace Monacs.UnitTests.ResultTests.Async

open System
open System.Threading.Tasks
open FSharp.Control.Tasks
open Xunit
open FsUnit.Xunit

open Monacs.Core

[<AutoOpen>]
module Helpers =
    let async v =
        v |> Task.FromResult
    let asyncF f =
        f >> Task.FromResult
    let asyncA f a =
        f(a)
        Task.CompletedTask
    let ok v =
        Result.Ok(v)
    let okAsync v =
        v |> Result.Ok |> Task.FromResult
    let okF f =
        f >> Result.Ok
    let okFAsync f =
        f >> Result.Ok >> Task.FromResult

module BindAsync =

    [<Fact>]
    let ``BindAsync<TIn, TOut> with async binder returns result of binder when value is Ok<TIn>`` () =
        task {
            let value = ok(42)
            let expected = ok("42")
            let! result = AsyncResult.BindAsync(value, okFAsync(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``BindAsync<TIn, TOut> with async binder returns Error<TOut> when value is Error<TIn>`` () =
        task {
            let error = Errors.Error()
            let value = Result.Error<int>(error)
            let expected = Result.Error<string>(error)
            let! result = AsyncResult.BindAsync(value, okFAsync(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``BindAsync<TIn, TOut> with async binder returns result of binder when value is task of Ok<TIn>`` () =
        task {
            let value = okAsync(42)
            let expected = ok("42")
            let! result = AsyncResult.BindAsync(value, okFAsync(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``BindAsync<TIn, TOut> with async binder returns Error<TOut> when value is task of Error<TIn>`` () =
        task {
            let error = Errors.Error()
            let value = Task.FromResult(Result.Error<int>(error))
            let expected = Result.Error<string>(error)
            let! result = AsyncResult.BindAsync(value, okF(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``BindAsync<TIn, TOut> returns result of binder when value is task of Ok<TIn>`` () =
        task {
            let value = okAsync(42)
            let expected = ok("42")
            let! result = AsyncResult.BindAsync(value, okF(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``BindAsync<TIn, TOut> returns Error<TOut> when value is task of Error<TIn>`` () =
        task {
            let error = Errors.Error()
            let value = Task.FromResult(Result.Error<int>(error))
            let expected = Result.Error<string>(error)
            let! result = AsyncResult.BindAsync(value, okF(fun x -> x.ToString()))
            result |> should equal expected
        }

module MapAsync =

    [<Fact>]
    let ``MapAsync<TIn, TOut> with async mapper returns result of mapper when value is Ok<TIn>`` () =
        task {
            let value = ok(42)
            let expected = ok("42")
            let! result = AsyncResult.MapAsync(value, asyncF(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``MapAsync<TIn, TOut> with async mapper returns Error<TOut> when value is Error<TIn>`` () =
        task {
            let error = Errors.Error()
            let value = Result.Error<int>(error)
            let expected = Result.Error<string>(error)
            let! result = AsyncResult.MapAsync(value, asyncF(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``BindAsync<TIn, TOut> with async mapper returns result of mapper when value is task of Ok<TIn>`` () =
        task {
            let value = okAsync(42)
            let expected = ok("42")
            let! result = AsyncResult.MapAsync<int, string>(value, asyncF(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``MapAsync<TIn, TOut> with async mapper returns Error<TOut> when value is task of Error<TIn>`` () =
        task {
            let error = Errors.Error()
            let value = Task.FromResult(Result.Error<int>(error))
            let expected = Result.Error<string>(error)
            let! result = AsyncResult.MapAsync<int, string>(value, asyncF(fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``MapAsync<TIn, TOut> returns result of mapper when value is task of Ok<TIn>`` () =
        task {
            let value = okAsync(42)
            let expected = ok("42")
            let! result = AsyncResult.MapAsync(value, (fun x -> x.ToString()))
            result |> should equal expected
        }

    [<Fact>]
    let ``MapAsync<TIn, TOut> returns Error<TOut> when value is task of Error<TIn>`` () =
        task {
            let error = Errors.Error()
            let value = Task.FromResult(Result.Error<int>(error))
            let expected = Result.Error<string>(error)
            let! result = AsyncResult.MapAsync(value, (fun x -> x.ToString()))
            result |> should equal expected
        }

module MatchAsync =

    let errorMessage (e: ErrorDetails) = e.Message.Value

    [<Fact>]
    let ``MatchAsync<TIn, TOut> returns result of async error when value is Error<TIn>`` () =
        task {
            let expected = "test"
            let error = Errors.Error(expected)
            let value = Result.Error<int>(error)
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = asyncF(fun _ -> ""), error = asyncF(errorMessage))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> returns result of async ok when value is Ok<TIn>`` () =
        task {
            let value = ok(42)
            let expected = "test"
            let! result = AsyncResult.MatchAsync(value, ok = asyncF(fun _ -> expected), error = asyncF(fun _ -> ""))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync error returns result of sync error when value is Error<TIn>`` () =
        task {
            let expected = "test"
            let error = Errors.Error(expected)
            let value = Result.Error<int>(error)
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = asyncF(fun _ -> ""), error = errorMessage)
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync error returns result of async ok when value is Ok<TIn>`` () =
        task {
            let value = ok(42)
            let expected = "test"
            let! result = AsyncResult.MatchAsync(value, ok = asyncF(fun _ -> expected), error = (fun _ -> ""))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync ok returns result of async error when value is Error<TIn>`` () =
        task {
            let expected = "test"
            let error = Errors.Error(expected)
            let value = Result.Error<int>(error)
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = (fun _ -> ""), error = asyncF(errorMessage))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync ok returns result of sync ok when value is Ok<TIn>`` () =
        task {
            let value = ok(42)
            let expected = "test"
            let! result = AsyncResult.MatchAsync(value, ok = (fun _ -> expected), error = asyncF(fun _ -> ""))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync ok and error returns result of sync error when value is Task<Error<TIn>>`` () =
        task {
            let expected = "test"
            let error = Errors.Error(expected)
            let value = async(Result.Error<int>(error))
            let! result = AsyncResult.MatchAsync(value, ok = (fun _ -> ""), error = errorMessage)
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync ok and error returns result of sync ok when value is Task<Ok<TIn>>`` () =
        task {
            let value = async(ok(42))
            let expected = "test"
            let! result = AsyncResult.MatchAsync(value, ok = (fun _ -> expected), error = (fun _ -> ""))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with async ok and error returns result of async error when value is Task<Error<TIn>>`` () =
        task {
            let expected = "test"
            let error = Errors.Error(expected)
            let value = async(Result.Error<int>(error))
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = asyncF(fun _ -> ""), error = asyncF(errorMessage))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with async ok and error returns result of async ok when value is Task<Ok<TIn>>`` () =
        task {
            let value = async(ok(42))
            let expected = "test"
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = asyncF(fun _ -> expected), error = asyncF(fun _ -> ""))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync ok returns result of async error when value is Task<Error<TIn>>`` () =
        task {
            let expected = "test"
            let error = Errors.Error(expected)
            let value = async(Result.Error<int>(error))
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = (fun _ -> ""), error = asyncF(errorMessage))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync ok returns result of sync ok when value is Task<Ok<TIn>>`` () =
        task {
            let value = async(ok(42))
            let expected = "test"
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = (fun _ -> expected), error = asyncF(fun _ -> ""))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync error and error returns result of sync error when value is Task<Error<TIn>>`` () =
        task {
            let expected = "test"
            let error = Errors.Error(expected)
            let value = async(Result.Error<int>(error))
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = asyncF(fun _ -> ""), error = (errorMessage))
            result |> should equal expected
        }

    [<Fact>]
    let ``MatchAsync<TIn, TOut> with sync error and error returns result of async ok when value is Task<Ok<TIn>>`` () =
        task {
            let value = async(ok(42))
            let expected = "test"
            let! result = AsyncResult.MatchAsync<int, string>(value, ok = asyncF(fun _ -> expected), error = (fun _ -> ""))
            result |> should equal expected
        }

module ``Side effects`` =

    [<Fact>]
    let ``DoAsync<T> with sync action returns value and executes action when value is Task<Ok<T>>`` () =
        task {
            let value = Result.Ok(42)
            let expected = "42"
            let mutable result = ""
            let! returnValue = AsyncResult.DoAsync(async(value), (fun _ -> result <- expected.ToString()))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoAsync<T> with sync action returns value and doesn't execute action when value is Task<Error<T>>`` () =
        task {
            let value = Result.Error<int>(Errors.Error())
            let expected = "test"
            let mutable result = expected
            let! returnValue = AsyncResult.DoAsync(async(value), (fun _ -> result <- "fail"))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoAsync<T> with async action returns value and executes action when value is Ok<T>`` () =
        task {
            let value = Result.Ok(42)
            let expected = "42"
            let mutable result = ""
            let! returnValue = AsyncResult.DoAsync(value, asyncA(fun _ -> result <- expected.ToString()))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoAsync<T> with async action returns value and doesn't execute action when value is Error<T>`` () =
        task {
            let value = Result.Error<int>(Errors.Error())
            let expected = "test"
            let mutable result = expected
            let! returnValue = AsyncResult.DoAsync(value, asyncA(fun _ -> result <- "fail"))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoAsync<T> with async action returns value and executes action when value is Task<Ok<T>>`` () =
        task {
            let value = Result.Ok(42)
            let expected = "42"
            let mutable result = ""
            let! returnValue = AsyncResult.DoAsync(async(value), asyncA(fun _ -> result <- expected.ToString()))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoAsync<T> with async action returns value and doesn't execute action when value is Task<Error<T>>`` () =
        task {
            let value = Result.Error<int>(Errors.Error())
            let expected = "test"
            let mutable result = expected
            let! returnValue = AsyncResult.DoAsync(async(value), asyncA(fun _ -> result <- "fail"))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoWhenErrorAsync<T> returns value and doesn't execute action when value is Task<Ok<T>>`` () =
        task {
            let value = Result.Ok(42)
            let expected = "test"
            let mutable result = expected
            let! returnValue = AsyncResult.DoWhenErrorAsync(async(value), (fun _ -> result <- "fail"))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoWhenErrorAsync<T> returns value and executes action when value is Task<Error<T>>`` () =
        task {
            let value = Result.Error<int>(Errors.Error())
            let expected = "42"
            let mutable result = ""
            let! returnValue = AsyncResult.DoWhenErrorAsync(async(value), (fun _ -> result <- expected.ToString()))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoWhenErrorAsync<T> with async action returns value and doesn't execute action when value is Ok<T>`` () =
        task {
            let value = Result.Ok(42)
            let expected = "test"
            let mutable result = expected
            let! returnValue = AsyncResult.DoWhenErrorAsync(value, asyncA(fun _ -> result <- "fail"))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoWhenErrorAsync<T> with async action returns value and executes action when value is Error<T>`` () =
        task {
            let value = Result.Error<int>(Errors.Error())
            let expected = "42"
            let mutable result = ""
            let! returnValue = AsyncResult.DoWhenErrorAsync(value, asyncA(fun _ -> result <- expected.ToString()))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoWhenErrorAsync<T> with async action returns value and doesn't execute action when value is Task<Ok<T>>`` () =
        task {
            let value = Result.Ok(42)
            let expected = "test"
            let mutable result = expected
            let! returnValue = AsyncResult.DoWhenErrorAsync(async(value), asyncA(fun _ -> result <- "fail"))
            returnValue |> should equal value
            result |> should equal expected
        }

    [<Fact>]
    let ``DoWhenErrorAsync<T> with async action returns value and executes action when value is Task<Error<T>>`` () =
        task {
            let value = Result.Error<int>(Errors.Error())
            let expected = "42"
            let mutable result = ""
            let! returnValue = AsyncResult.DoWhenErrorAsync(async(value), asyncA(fun _ -> result <- expected.ToString()))
            returnValue |> should equal value
            result |> should equal expected
        }

module TryCatch =

    [<Fact>]
    let ``TryCatchAsync<T> returns Ok<T> when function call doesn't throw`` () =
        task {
            let value = 42
            let! result = AsyncResult.TryCatchAsync((fun () -> async(value)), (fun _ -> Errors.Error()))
            result |> should equal (Result.Ok(value))
        }

    [<Fact>]
    let ``TryCatchAsync<T> returns Error<T> when function call throws`` () =
        task {
            let message = "This should not happen!"
            let! result = AsyncResult.TryCatchAsync((fun () -> Task.FromException<obj>(Exception(message))), (fun e -> Errors.Error(e.Message)))
            result |> should equal (Result.Error(Errors.Error(message)))
        }

    [<Fact>]
    let ``TryCatchAsync<TIn, TOut> returns Ok<TOut> when previous result is Ok<TIn> and function call doesn't throw`` () =
        task {
            let value = Result.Ok(42)
            let expected = "42"
            let! result = AsyncResult.TryCatchAsync(value, (fun v -> async(v.ToString())), (fun _ _ -> Errors.Error()))
            result |> should equal (Result.Ok(expected))
        }

    [<Fact>]
    let ``TryCatchAsync<TIn, TOut> returns Error<TOut> when previous result is Ok<TIn> and function call throws`` () =
        task {
            let expected = "OK"
            let message = "This should not happen!"
            let value = Result.Ok(expected)
            let! result = AsyncResult.TryCatchAsync(value, (fun _ -> Task.FromException<obj>(Exception(message))), (fun v e -> Errors.Error(v + e.Message)))
            result |> should equal (Result.Error(Errors.Error(expected + message)))
        }

    [<Fact>]
    let ``TryCatchAsync<TIn, TOut> returns Error<TOut> when previous result is Error<TIn>`` () =
        task {
            let error = Errors.Error("Oh no!")
            let value = Result.Error<int>(error)
            let! result = AsyncResult.TryCatchAsync(value, (fun v -> async(v.ToString())), (fun _ _ -> Errors.Error()))
            result |> should equal (Result.Error<string>(error))
        }