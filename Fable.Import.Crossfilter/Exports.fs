namespace Fable.Import.Crossfilter

open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type Exports =

    [<Import("default", "crossfilter2")>]
    static member crossfilter(?records: ResizeArray<'T>) : Crossfilter.Crossfilter<'T> =
        jsNative
