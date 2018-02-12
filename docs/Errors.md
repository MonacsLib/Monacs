# Monacs.Core.ErrorDetails

`ErrorDetails` type contains the description of the error that occured within the application. It is required by the [`Result<T>` type] in the `Error` case. To construct the instance of this type you need to use static factory methods from the `Errors` class.

`ErrorDetails` contains several fields that help to describe the error to handle or log it properly in the later stage of the application execution. All fields apart from `Level` are optional.