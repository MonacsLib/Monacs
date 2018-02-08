# Monacs - Collection of basic monads and functional extensions for C#

This library provides few core types and functions that can help you start writing C# code in more functional way.
It also encourages use of [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/) approach.

Monacs uses type and function names similar to F#. That's intentional and it should make potential transition to F# easier.
Some people may prefer Haskell type names (e.g. Maybe instead of Option) or LINQ-like function naming (e.g. Select instead of Map) - if that's your preference, you can always fork this library and change the names accordingly :)

[![Build Status](https://travis-ci.org/MonacsLib/Monacs.svg?branch=master)](https://travis-ci.org/MonacsLib/Monacs)

## Target platform and language versions

Currently the library is build against .NET 4.6.1 and .NET Standard 1.3. To use the library you need to have .NET 4.6.1+ or .NET Core 1.0+ project with C# language version 6 or higher.

## Documentation

You can find [docs and samples here](docs/Index.md).

## Want more?

If you're into functional programming and you're stuck in the .NET platform (or just like it) then you should definitely go with [F# programming language](http://fsharp.org/)!
Not only most of what is provided by Monacs library is already there, but you also have much better defaults in the language (immutability anyone?).
There are also great libraries to go with Railway Oriented Programming like [Chessie](http://fsprojects.github.io/Chessie/), so be sure to check it out.

If you really need to stick to C# and this library is not enough then you can try much more comprehensive library [Language Extensions](https://github.com/louthy/language-ext) - it probably has everything which this library covers and much, much more.

## Status

It's currently work-in-progress, expect new things to be added, APIs to be changed and docs to be updated more or less often. First publicly available version was released [on NuGet](https://www.nuget.org/packages/Monacs.Core/).

## Contributing

If you've found any errors or missing functionalities in the provided library feel free to report an issue using GitHub.
You can also contribute by sending pull requests with bug fixes or enhancments.

The [`develop` branch](https://github.com/MonacsLib/Monacs/tree/develop) should be used as a base for contributions, and I recommend rebasing on newest version of this branch when creating pull request. If you'd like to help but don't know where to look, start with [issues marked as up-for-grabs](https://github.com/MonacsLib/Monacs/labels/up-for-grabs).

## License

This code is provided as is, without any warranties, as specified in [MIT license](LICENSE).

## Maintainers

* Bart Sokol - [@bartsokol on GitHub](https://github.com/bartsokol/) and [@bartsokol on Twitter](https://twitter.com/bartsokol).