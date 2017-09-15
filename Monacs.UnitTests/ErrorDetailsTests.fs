namespace Monacs.UnitTests.ErrorDetailsTests

open System
open Xunit
open FsUnit.Xunit

open Monacs.Core

module Constructors =

    [<Fact>]
    let ``Trace sets ErrorLevel.Trace and error details`` () =
        let message = "Message"
        let key = "key"
        let ex = Exception()
        let details = Errors.Trace(message, key, ex)
        details.Level |> should equal ErrorLevel.Trace
        details.Message |> should equal (Option.Some(message))
        details.Key |> should equal (Option.Some(key))
        details.Exception |> should equal (Option.Some(ex))

    [<Fact>]
    let ``Debug sets ErrorLevel.Debug and error details`` () =
        let message = "Message"
        let key = "key"
        let ex = Exception()
        let details = Errors.Debug(message, key, ex)
        details.Level |> should equal ErrorLevel.Debug
        details.Message |> should equal (Option.Some(message))
        details.Key |> should equal (Option.Some(key))
        details.Exception |> should equal (Option.Some(ex))

    [<Fact>]
    let ``Info sets ErrorLevel.Info and error details`` () =
        let message = "Message"
        let key = "key"
        let ex = Exception()
        let details = Errors.Info(message, key, ex)
        details.Level |> should equal ErrorLevel.Info
        details.Message |> should equal (Option.Some(message))
        details.Key |> should equal (Option.Some(key))
        details.Exception |> should equal (Option.Some(ex))

    [<Fact>]
    let ``Warn sets ErrorLevel.Warn and error details`` () =
        let message = "Message"
        let key = "key"
        let ex = Exception()
        let details = Errors.Warn(message, key, ex)
        details.Level |> should equal ErrorLevel.Warn
        details.Message |> should equal (Option.Some(message))
        details.Key |> should equal (Option.Some(key))
        details.Exception |> should equal (Option.Some(ex))

    [<Fact>]
    let ``Error sets ErrorLevel.Error and error details`` () =
        let message = "Message"
        let key = "key"
        let ex = Exception()
        let details = Errors.Error(message, key, ex)
        details.Level |> should equal ErrorLevel.Error
        details.Message |> should equal (Option.Some(message))
        details.Key |> should equal (Option.Some(key))
        details.Exception |> should equal (Option.Some(ex))

    [<Fact>]
    let ``Fatal sets ErrorLevel.Fatal and error details`` () =
        let message = "Message"
        let key = "key"
        let ex = Exception()
        let details = Errors.Fatal(message, key, ex)
        details.Level |> should equal ErrorLevel.Fatal
        details.Message |> should equal (Option.Some(message))
        details.Key |> should equal (Option.Some(key))
        details.Exception |> should equal (Option.Some(ex))