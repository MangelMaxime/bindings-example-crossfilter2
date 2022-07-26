module Main

open Feliz
open Browser.Dom
open Fable.Core
open Fable.Import.Crossfilter

open type Fable.Import.Crossfilter.Exports

type Payment =
    {
        Date : string
        Quantity : int
        Total : int
        Tip : int
        Type : string
        ProductIDs : ResizeArray<string>
    }

let initialValues =
    [
        {
            Date = "2011-11-14T16:17:54Z"
            Quantity = 2
            Total = 190
            Tip = 100
            Type = "tab"
            ProductIDs = ResizeArray ["001"]
        }
        {
            Date = "2011-11-14T16:28:54Z"
            Quantity = 1
            Total = 300
            Tip = 200
            Type = "visa"
            ProductIDs = ResizeArray ["004" ; "005"]
        }
    ]

let payments : Crossfilter.Crossfilter<Payment> =
    crossfilter(ResizeArray initialValues)

JS.console.log(payments.size())

payments.add(
    ResizeArray([
        {
            Date = "2011-11-14T16:30:43Z"
            Quantity = 2
            Total = 90
            Tip = 0
            Type = "tab"
            ProductIDs = ResizeArray ["001"; "002"]
        }
    ])
) |> ignore

JS.console.log(payments.size())
