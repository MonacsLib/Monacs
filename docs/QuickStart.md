# Quick Start

Using Monacs library is pretty easy. First, you have to install it into the project. You can do it from the UI in Visual Studio, using Package Manager Console:

    Install-Package Monacs.Core

If you're using `dotnet` CLI you can do it as well:

    dotnet add package Monacs.Core

## Creating optional values

Now, let's define some data structure which will have an optional field. If you were creating expense tracking system you may have a class like this:

    using Monacs.Core;

    public class Expense
    {
        public ExpenseCategory Category { get; set; }
        public decimal Amount { get; set; }
        public Option<string> Notes { get; set; }
    }

[`Option<T>`](Option.md) is a generic struct that wraps any value and it annotates that given property, function parameter or result can be empty. Let's create object of `Expense` class and see it in action:

    using Monacs.Core;
    ...
    public Expense CreateExpense(ExpenseCategory category, decimal amount, string notes) => new Expense
    {
        Category = category,
        Amount = amount,
        Notes = string.IsNullOrEmpty(notes)
            ? Option.Some(notes)
            : Option.None<string>()
    }

First thing you will notice is that there are no publicly available constructors for `Option<T>` type. You create them using `Some` and `None` factory methods. This is because the type can only have two states (again `Some` and `None`), and Monacs is doing everything to prevent you from having any other possibility here.

OK, so now Notes field will be `Some` when `notes` parameter was null or empty. The code to do it is quite verbose though. We can make it a bit shorter by leveraging `using static` feature of C#. The code could look like this:

    using static Monacs.Core.Option;
    ...
    public Expense CreateExpense(ExpenseCategory category, decimal amount, string notes) => new Expense
    {
        Category = category,
        Amount = amount,
        Notes = string.IsNullOrEmpty(notes) ? Some(notes) : None<string>()
    }

Things get nicer now. By statically importing `Option` class we can use methods defined there as they were defined in the same class we're in. `Option` class is a static class containing set of factory and extension methods that allow you to create and work with `Option<T>` struct.

Actually the code we've written there is so common that Monacs contains extension that does exact same thing:

    using Monacs.Core;
    ...
    public Expense CreateExpense(ExpenseCategory category, decimal amount, string notes) => new Expense
    {
        Category = category,
        Amount = amount,
        Notes = notes.ToOption()
    }

There are `ToOption()` overloads that allow you to convert any reference type (None when null), nullable (None when null) and string (None when null or empty). You should use them instead of `Some` and `None` factory methods whenever possible, as they prevent you from doing unwise things like this:

    public static Option<T> ToEvilSome<T>(T value = null) where T : class =>
        Option.Some(value);

As you may guess, this code will create `Some` state with potentially `null` value - this isn't a very safe code, is it?

## Working with optional values

Once you've created optional values you'll want to actually use it. Let's say you want to display notes when they are actually present, and display alternate text when it's empty. You can start by creating function like this:

    using Monacs.Core;
    ...
    public string GetNotesText(Option<string> notes) =>
        notes.IsSome
        ? notes.Value
        : "There are no notes, sorry!";

Using `Value` property is convenient, but you shouldn't actually do it unless necessary. In most functional languages you could use pattern matching as an alternative, but the one in C# (as of version 7.0) isn't powerful enough. But no worries, Monacs is here to help:

    using Monacs.Core;
    ...
    public string GetNotesText(Option<string> notes) =>
        notes.Match(
            some: n => n.Value
            none: () => "There are no notes, sorry!");

Now you won't accidentally use `Value` when it's not present. `Match` function is quite powerful, but it's also very very verbose. That's why for many common operations you can find helper methods in Monacs. Shorter version of the code above can look like this:

    using Monacs.Core;
    ...
    public string GetNotesText(Option<string> notes) =>
        notes.GetOrDefault(whenNone: "There are no notes, sorry!");


## Combining calls

Once you got an optional value, you may want to transform it in one way or another. Getting back to the notes example, let's assume that the editor allows you to use Markdown for formatting and you want to get the word count from your field. To make it clear what are you doing you want to make it explicit, so the code can look like this:

    using Monacs.Core;
    ...
    public int GetWordCount(Option<string> notes)
    {
        if (notes.IsNone)
            return 0;
        var strippedNotes = RemoveFormatting(notes.Value);
        return GetWordCount(strippedNotes);
    }

As with earlier example, you can accidentaly use the Value when it's not set, so you probably want to use a bit different approach. With Monacs you can do it like this:

    using Monacs.Core;
    ...
    public int GetWordCount(Option<string> notes) =>
        notes
            .Map(RemoveFormatting)
            .Map(GetWordCount)
            .GetOrDefault();

Now the code is only using `Value` when it's actually set. Another important change is that the code is now an expression instead of set of instructions, making code briefer and removing unnecessary noise.

So you may ask what is this `Map` function? If you've ever used LINQ then probably you know `Select` function from it. `Map` is exactly the same thing, just operating on `Option<T>` instead of `IEnumerable<T>`. It takes in the option (as an extension of it) and a mapper function that accepts one value and returns other value. The signature of mapper in C# convention is `Func<T1, T2>`. Mapper is executed only when input option is `Some`, and it will return it's result wrapped into `Some`. Otherwise, it will return `None<T2>`. If the function above was to return `Option<int>`, giving `None` when there are no notes, code without `Map` could look like this:

    using Monacs.Core;
    using static Monacs.Core.Option;
    ...
    public Option<int> GetWordCount(Option<string> notes)
    {
        if (notes.IsNone)
            return None<int>();
        var strippedNotes = RemoveFormatting(notes.Value);
        var wordCount = GetWordCount(strippedNotes);
        return Some(wordCount);
    }

Using `Map` it gets much simpler (and safer):

    using Monacs.Core;
    ...
    public Option<int> GetWordCount(Option<string> notes) =>
        notes.Map(RemoveFormatting).Map(GetWordCount);

So `Map` is really nice helper function, but what would happen if the `GetWordCount` function was returning `Option<int>`, giving `None` when there are no words? We would get compile error that the value returned doesn't match function signature. Now it would be `Option<Option<int>>`, which doesn't look good. Fortunatelly there is one more function that can solve this particular problem. It's called `Bind`. Let's see it in action, given the described case:

    using Monacs.Core;
    ...
    public Option<int> GetWordCount(Option<string> notes) =>
        notes.Map(RemoveFormatting).Bind(GetWordCount);

Now the returned value matches the signature again. We're good to go. So what's this `Bind` function? It is very similar to `Map`, the difference being signature of the function it accepts as a parameter. In `Bind` it's `Func<T1, Option<T2>>` and it's called `binder`. If the input to `Bind` is `Some`, it will return result of `binder` without wrapping it into `Some`, so it will actually return `None` when `binder` returns `None`. That allows for even more composability.

This should give you a brief overview of how to work with `Option<T>` type, but what about `Result<T>`? It turns out it works in the same way, so let's explore the differences.

## Working with `Result<T>`

Similar to `Option<T>`, `Result<T>` type is a struct and has two possible states - `Ok` with a data of type `T` in the `Value` property, and `Error` with a data of type `ErrorDetails` in the `Error` property. This makes it perfect candidate for second common case, where you have a function and it may fail to execute properly. Usualy in such case you can expect an exception to be thrown. The problem with exceptions is that you don't have any explicit way to say that the function may fail and which exceptions it may throw (apart from comments). Handling exceptions is also problematic, as you need to decide when to wrap the code into `try...catch` - having it everywhere requires a lot of code, and it's also impacting performance quite substantialy. So just like `Option<T>` mitigates the problem of null, `Result<T>` deals with exceptions.

TODO