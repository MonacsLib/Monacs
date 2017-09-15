# Getting started

Monacs is a library that provides a set of types and functions that can be used to substantialy change the approach you use to write your C# code. And while it won't change object-oriented language into fully-featured functional language, it gives you oportunity to use some of the FP concepts in your C# code today.

To fully leverage the potential of this library, you'll need to get familiar with a few simple concepts. Once you get through them, everything about this library should be pretty obvious.

## Don't fear the monad
The M Word. It's been a topic of countless discussions, jokes and even flamewars. You can look at the monad from many perspectives, but the perspective this library encourages is pretty simple: Monad is a combination of a type (e.g. Option) and a collection of functions around this type (like Map and Bind).

Actually, as a .NET developer you've probably used at least a couple of monads. `IEnumerable<T>` with LINQ is actually a monad. TPL and async/await workflow is a monad as well. And if you had chance to use Reactive Extensions and `IObservable<T>` type, then yes, it's a proper monad too.

You may find quite a few similarities between LINQ and what you'll find in this library, although the naming is a bit different. For example, both LINQ and Rx use `Select()` name for the function that makes a projection of an encapsulated value to the value of (potentially) other type. In Monacs you'll find it under the name `Map()`, which is quite common across FP languages.

## Higher order functions
This is another complicated name for a pretty simple concept. Higher order function is just a function which takes another function as a parameter. And again, if you've used LINQ, TPL or Rx you're probably quite familiar with this idea and I think it doesn't require more explanation.

## Value types
As you may know, there are two kinds of types in C# - reference types, such as `System.Object` or any class you write, and value types, such as `System.Int32` or enums. One particular thing that is very often underestimated by developers is the ability to create your own value types - structs. Apart from memory management differences, the key distinction between struct and class is the default value - the problematic null in reference type is substituted by the default value you create when designing a struct. And while you don't want to go and replace all your classes with structs, there are certain places where it can make a huge difference to not have to deal with (implicit) null.

## The power of extension methods
One of the most important features of C# that allowed to build this library is extension methods. Having the possibility to extend any type with additional methods from virtually any place gives us the flexibility to build modular fluent APIs around simple types.

## Leveraging `using static` imports
There is one particular feature of C# language that can significantly reduce the amount of code you have to write when you use the same static class multiple times. Take a look at an example:

    namespace Monacs.Samples
    {
        using System;
        
        public class Sample1
        {
            TODO
        }
    }

TODO