# Nito.LINQ

Nito.LINQ provides several LINQ-like operators and algorithms, particularly for lists (IList<T>), sorted sequences, and sorted lists.

Much of the inspiration for Nito.LINQ operators comes from Python sequences and C++ STL algorithms.

## Why Nito.Linq?

There are several motivational forces behind the development of Nito.Linq, but they all come down to *Promote*, *Optimize*, *Preserve*, and *Expand*:

- *Promote* useful operators now defined in `List<T>`/`Array`, making them available for any `IList<T>` (or `IEnumerable<T>`, if possible).
  - Example: `List<T>.AsReadOnly` may be applied to any `IList<T>`: `IList<T> AsReadOnly<T>(this IList<T> list)`
  - Example: `List<T>.IndexOf` may be applied to any `IEnumerable<T>`: `int IndexOf<T>(this IEnumerable<T> source, T value)`
- *Optimize* some existing LINQ sequence operators when applied to `IList<T>`.
  - Example: `Reverse` may be applied to an `IList<T>` without copying the sequence: `IList<T> Reverse<T>(this IList<T> list)`
- *Preserve* indexed element access if it doesn't need to be lost.
  - Example: `Select` may be applied to an `IList<T>` and still permit indexed element access: `IList<TResult> Select<TSource, TResult>(this IList<TSource> list, Func<TSource, TResult> selector)`
- *Expand* the available operators, taking inspiration from C++ STL algorithms, Python sequences, and jQuery result sets. (Note that although the _concepts_ are similar, Nito.Linq operators almost always create "views" of the unchanged source rather than modifying the source directly.)
  - Example: Python has a `slice` syntax that efficiently references a subset of the original sequence: `IList<T> Slice<T>(this IList<T> list, int offset, int count)`
  - Example: The C++ STL has a `rotate` algorithm that rotates the elements in a container: `IList<T> Rotate<T>(this IList<T> list, int offset)`
- *Expand* the LINQ namespace by providing _sorted sequence_ and _sorted list_ interfaces and operators.
  - *Promote* sort-related operators now defined in `List<T>`/`Array`.
    - Example: `List<T>.BinarySearch` may be applied on any `ISortedList<T>`: `int BinarySearch<T>(this ISortedList<T> list, T item)`
  - *Optimize* some existing LINQ operators when applied to sorted sequences and lists.
    - Example: `Union` may be applied to sorted sequences without buffering: `ISortedEnumerable<T> Union<T>(this ISortedEnumerable<T> source, params ISortedEnumerable<T>[] others)`
  - *Preserve* "sortedness" if it doesn't need to be lost.
    - Example: `Skip` may be applied to an `ISortedList<T>`, and the result is still sorted: `ISortedList<T> Skip<T>(this ISortedList<T> list, int offset)`
  - *Expand* the available operators, taking inspiration from C++ STL operators.
    - Example: `EqualRange` may be applied to any sorted sequence: `void EqualRange<T>(this ISortedList<T> list, T item, out int begin, out int end)`

## Current State
Nito.Linq is currently in a pre-release state. See the note at the end of this page if you need to compile the source. The binaries are included in source control for ease of linking to other projects (using svn:externals).

It will optionally use Rx ([http://msdn.microsoft.com/en-us/devlabs/ee794896.aspx]); however, if Rx is not used, then Rx-like operators are provided (e.g., `Zip`).

## Building the Sources

The library projects contain references to a "Source\Sources\Properties\AssemblyVersion.cs" file that is not included in source code control. This file is expected to contain just an AssemblyVersion attribute, like this:

````C#
using System.Reflection;
[assembly:AssemblyVersion("0.0.0.1")]
````
