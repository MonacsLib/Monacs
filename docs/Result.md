# Monacs.Core.Result\<T>

`Result<T>` type is a monad encapsulating common case when function call may end up with success or error. It has two possible states - one representing successful execution and a value being returned (`Ok`) and the other one representing error in execution (`Error`). You can find some similarities to Either, IO or Task monads, if you're familiar with any of those. It is meant to inform consumer of the function that the execution may not finish successfully (very often due to external side effects) and suggests handling such case on the consumer side. The most common examples would be any functions performing IO tasks. If such operation fails, the author of the function should provide all needed details in the `Error` property, described by the [`ErrorDetails` class](Errors.md).

## When to use it

Whenever you are creating a function which calls other functions that are known to throw exceptions or return errors of other kind, you face a dillema. You can either let the error to bubble up or handle it in-place. With the first approach in most cases you lose the explicitness, as you don't inform consumers that they should handle the error. There is always a problem when is the right moment to stop bubbling up which often leads to unhandled exceptions.

    public Expense GetExpenseById(Guid id) =>
        _httpClient.GetJson<Expense>(RootUrl + $"/expenses/{id}"); // Happy path, no exception handling

The solution for this could be handling the exception at the source, so the second approach. In this case the problem is usually deciding what kind of value you should return to the calling function, or how to stop further execution without breaking whole application.

    public Expense GetExpenseById(Guid id)
    {
        try
        {
            return _httpClient.GetJson<Expense>(RootUrl + $"/expenses/{id}"); // Happy path, no exception handling
        }
        catch (Exception ex)
        {
            _logger.Error($"Could not get the expence with id {id}", ex);
            throw; // bubble up anyway
            // or
            return null; // return evil value
        }
    }

This is where `Result<T>` comes in. You can handle the exception close to the source, provide meaningful error details and inform calling function about the failure.

    public Result<Expense> GetExpenseById(Guid id) =>
        Result.TryCatch(
            _httpClient.GetJson<Expense>(RootUrl + $"/expenses/{id}"),
            ex => Errors.Error($"Could not get the expence with id {id}", exception: ex) // You can add more error details here
        );

With the syntactic sugar provided by the extension methods you can write code that focuses mostly on the domain problem you are trying to solve. It leads to better understanding of the problem and better quality of the software.

## How to use it

TODO

## Summary

Using the `Result<T>` type won't prevent all of the exceptions bubbling up in the application - you may always miss some case. That's why you should have some global exception handler that will catch such cases and help to nail them down (e.g. by logging details and notifying responsible people). Having said that, if used correctly and system-wide, it should help to minimize potential bugs due to non-handled errors. It helps to reason about the code by favoring explicitness in the important aspects (like potential errors) while hiding less important details under the hood (in the related functions).