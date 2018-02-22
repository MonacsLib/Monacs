# Monacs.Core.Option\<T>

`Option<T>` type is an implementation of Maybe monad. It has two possible states - default one representing absence of value (`None`) and the other one representing presence of value (`Some`). `Option<T>` wraps value that can be absent, in similar way as `Nullable<T>` wraps value types. The are core differences though. While `Nullable<T>` only wraps structs (value types), `Option<T>` works with all kinds of types, making things more consistent. The value representing absence of data in `Nullable<T>` is the same as for reference types - both use `null` for that purpose. With `Option<T>` you don't really care what is the value representing absence of data, because it's the property of the type itself.

## When to use it

The main use case of `Option` type is modeling properties of data structures which are not required to create given structure. Let's say we want to build expense tracking system and need to model Expense enity. To make things simple, we want it to contain 3 properties: category, amount and optional notes. We may start with creating it like this:

    public class Expense
    {
        public ExpenseCategory Category { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }

This design has quite a few flaws, one of them is the fact that we don't state explicitly which properties are required and which are optional. Let's make a simple change:

    public class Expense
    {
        public ExpenseCategory Category { get; set; }
        public decimal Amount { get; set; }
        public Option<string> Notes { get; set; }
    }

Now we explicitly say that Notes can be present or not. That not only applies to the structure of the class, but also to the usage.

    public string GetExpenseNotesUppercase(Expense expense) =>
        expense.Notes.ToUpper();

The problem with the code above is that it will crash badly with `NullReferenceException` if the Notes field is null. Of course we can fix that:

    public string GetExpenseNotesUppercase(Expense expense) =>
        expense.Notes?.ToUpper();

Not much more complicated, but we only postponed the the issue to the caller - we'll return `null` if the value was `null` in the first place. Of course we could make it return some proper value each time:

    public string GetExpenseNotesUppercase(Expense expense) =>
        expense.Notes?.ToUpper() ?? string.Empty;

But we get to the same issue that we had with our data structure - we don't state if the return value will always be present. Consumers of this method still need to consider `null` case - just in case you decide to change the internal implementation details.

Now let's see how it can look with `Option<T>` type:

    public Option<string> GetExpenseNotesUppercase(Expense expense) =>
        expense.Notes.Map(s => s.ToUpper());

Note that signature of the function has changed, and now we say explicitly that you may not get the value. Consumers will need to handle it directly, making it harder to get unexpected errors.

One other benefit is that you can have now consistent way of checking if the value is there - just by checking `IsSome` or `IsNone` property of the option. If you, for example, would like to hide the Notes field in the UI, you've got it for free. But it doesn't stop there.

## How to use it

Having one type to wrap all the optional values gives you the power to extend things easily. If you've ever used LINQ, you should already know how convenient it can be. Actually the `Option<T>` type has the same superpowers as `IEnumerable<T>`, allowing you to chain calls as long as your inputs and outputs are options (fluent APIs, anyone?). Take a look at this example. Let's say that we want to extract the list of hashtags present in the `Notes` field mentioned above, and also make all of them lowercase.

    public Option<IEnumerable<string>> GetHashtags(string s) {...}

    public Option<List<string>> GetNotesHashtags(Expense expense) =>
        expense.Notes
            .Map(s => s.ToLower())
            .Bind(GetHashtags);
            .Map(x => x.ToList());

Above code is actually equivalent to this:

    public Option<List<string>> GetNotesHashtags(Expense expense)
    {
        if (expense.Notes.IsNone)
            return Option.None<List<string>>();
        var lowercaseNotes = expense.Notes.Value.ToLower();
        var hashtags = lowercaseNotes.GetHashtags(s);
        if (hashtags.IsNone)
            return Option.None<List<string>>();
        return hashtags.ToList();
    }

Not only there is less code to write and read in the first example, but it also reduces the need for naming things.

You can also reimplement it without option at all, for example like this:

    public List<string> GetNotesHashtags(Expense expense)
    {
        if (string.IsNullOrEmpty(expense.Notes))
            return null;
        var lowercaseNotes = expense.Notes.ToLower();
        var hashtags = lowercaseNotes.GetHashtags(s);
        return hashtags?.ToList();
    }

This seems like a good option, especially with null propagation operator in C# being very convenient, but one thing you have to remember is that it doesn't solve the problem of null. It only moves the responsibility of handling it from one piece of code to another.

## Summary

As you can see, option type has quite a few benefits:

- explicity - you state in a very obvious way that something may be not present
- brevity - with just a few helpers you can make code much more concise
- extensibility - the same simple helpers build powerful fluent API
- safety - you substantialy reduce the risk of having `NullReferenceException`
- versality - the same type represents the lack of value for structs and classes

When you start using it, you realize that it is a sweet spot between convenience of simple null propagation and complexity of multi-level null handling.