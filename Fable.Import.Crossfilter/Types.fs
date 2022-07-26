namespace rec Fable.Import.Crossfilter

open Fable.Core

module Crossfilter =

    let [<Import("heap","module/crossfilter")>] heap: Heap.IExports = jsNative
    let [<Import("heapselect","module/crossfilter")>] heapselect: Heapselect.IExports = jsNative
    let [<Import("bisect","module/crossfilter")>] bisect: Bisect.IExports = jsNative

    type [<AllowNullLiteral>] IExports =
        abstract version: string
        abstract heap: records: ResizeArray<'T> * lo: float * hi: float -> ResizeArray<'T>
        abstract heapselect: records: ResizeArray<'T> * lo: float * hi: float * k: float -> ResizeArray<'T>
        abstract bisect: records: ResizeArray<'T> * record: 'T * lo: float * hi: float -> float
        abstract permute: records: ResizeArray<'T> * index: ResizeArray<float> * deep: float -> ResizeArray<'T>

    type ComparableValue =
        U3<string, float, bool>

    type [<AllowNullLiteral>] ComparableObject =
        abstract valueOf: unit -> ComparableValue

    type NaturallyOrderedValue =
        U2<ComparableValue, ComparableObject>

    type [<AllowNullLiteral>] Predicate<'T> =
        [<Emit("$0($1...)")>] abstract Invoke: record: 'T -> bool

    type TSelectorValue =
        U2<NaturallyOrderedValue, ResizeArray<NaturallyOrderedValue>>

    type OrderedValueSelector<'TRecord> =
        OrderedValueSelector<'TRecord, NaturallyOrderedValue>

    // type [<AllowNullLiteral>] OrderedValueSelector<'TRecord, 'TValue when 'TValue :> TSelectorValue> =
    type [<AllowNullLiteral>] OrderedValueSelector<'TRecord, 'TValue> =
        [<Emit("$0($1...)")>] abstract Invoke: record: 'TRecord -> 'TValue

    type FilterValue =
        U3<NaturallyOrderedValue, NaturallyOrderedValue * NaturallyOrderedValue, Predicate<NaturallyOrderedValue>>

    // type [<AllowNullLiteral>] Grouping<'TKey, 'TValue when 'TKey :> NaturallyOrderedValue> =
    type [<AllowNullLiteral>] Grouping<'TKey, 'TValue> =
        abstract key: 'TKey with get, set
        abstract value: 'TValue with get, set

    // type [<AllowNullLiteral>] Group<'TRecord, 'TKey, 'TValue when 'TKey :> NaturallyOrderedValue> =
    type [<AllowNullLiteral>] Group<'TRecord, 'TKey, 'TValue> =
        abstract top: k: float -> ResizeArray<Grouping<'TKey, 'TValue>>
        abstract all: unit -> ResizeArray<Grouping<'TKey, 'TValue>>
        abstract reduce: add: ('TValue -> 'TRecord -> bool -> 'TValue) * remove: ('TValue -> 'TRecord -> bool -> 'TValue) * initial: (unit -> 'TValue) -> Group<'TRecord, 'TKey, 'TValue>
        abstract reduceCount: unit -> Group<'TRecord, 'TKey, 'TValue>
        abstract reduceSum: selector: ('TRecord -> float) -> Group<'TRecord, 'TKey, 'TValue>
        abstract order: selector: ('TValue -> NaturallyOrderedValue) -> Group<'TRecord, 'TKey, 'TValue>
        abstract orderNatural: unit -> Group<'TRecord, 'TKey, 'TValue>
        abstract size: unit -> float
        abstract dispose: unit -> Group<'TRecord, 'TKey, 'TValue>

    type [<AllowNullLiteral>] GroupAll<'TRecord, 'TValue> =
        abstract reduce: add: ('TValue -> 'TRecord -> bool -> 'TValue) * remove: ('TValue -> 'TRecord -> bool -> 'TValue) * initial: (unit -> 'TValue) -> GroupAll<'TRecord, 'TValue>
        abstract reduceCount: unit -> GroupAll<'TRecord, 'TValue>
        abstract reduceSum: selector: ('TRecord -> float) -> GroupAll<'TRecord, 'TValue>
        abstract dispose: unit -> GroupAll<'TRecord, 'TValue>
        abstract value: unit -> 'TValue

    // type [<AllowNullLiteral>] Dimension<'TRecord, 'TValue when 'TValue :> NaturallyOrderedValue> =
    type [<AllowNullLiteral>] Dimension<'TRecord, 'TValue> =
        abstract filter: filterValue: FilterValue -> Dimension<'TRecord, 'TValue>
        abstract filterExact: value: 'TValue -> Dimension<'TRecord, 'TValue>
        abstract filterRange: range: 'TValue * 'TValue -> Dimension<'TRecord, 'TValue>
        abstract filterFunction: predicate: Predicate<'TValue> -> Dimension<'TRecord, 'TValue>
        abstract filterAll: unit -> Dimension<'TRecord, 'TValue>
        abstract currentFilter: unit -> FilterValue option
        abstract hasCurrentFilter: unit -> bool
        abstract top: k: float * ?offset: float -> ResizeArray<'TRecord>
        abstract bottom: k: float * ?offset: float -> ResizeArray<'TRecord>
        abstract group: ?groupValue: ('TValue -> 'TKey) -> Group<'TRecord, 'TKey, 'TGroupValue> when 'TKey :> NaturallyOrderedValue
        abstract groupAll: unit -> GroupAll<'TRecord, 'TGroupValue>
        abstract dispose: unit -> Dimension<'TRecord, 'TValue>
        abstract accessor: record: 'TRecord -> NaturallyOrderedValue
        abstract id: unit -> float

    type [<StringEnum>] [<RequireQualifiedAccess>] EventType =
        | [<CompiledName("dataAdded")>] DATA_ADDED
        | [<CompiledName("dataRemoved")>] DATA_REMOVED
        | [<CompiledName("filtered")>] FILTERED

    type [<AllowNullLiteral>] Crossfilter<'T> =
        abstract add: records: ResizeArray<'T> -> Crossfilter<'T>
        abstract remove: ?predicate: Predicate<'T> -> unit
        abstract dimension: selector: OrderedValueSelector<'T, U2<'TValue, ResizeArray<'TValue>>> * ?isArray: bool -> Dimension<'T, 'TValue> when 'TValue :> NaturallyOrderedValue
        abstract groupAll: unit -> GroupAll<'T, 'TGroupValue>
        abstract size: unit -> float
        abstract all: unit -> ResizeArray<'T>
        abstract allFiltered: unit -> ResizeArray<'T>
        abstract onChange: callback: (EventType -> unit) -> (unit -> unit)
        abstract isElementFiltered: index: float * ?ignoreDimensions: ResizeArray<float> -> bool

    type [<AllowNullLiteral>] HeapSelector<'T> =
        [<Emit("$0($1...)")>] abstract Invoke: records: ResizeArray<'T> * lo: float * hi: float * k: float -> ResizeArray<'T>

    type [<AllowNullLiteral>] Heap<'T> =
        [<Emit("$0($1...)")>] abstract Invoke: records: ResizeArray<'T> * lo: float * hi: float -> ResizeArray<'T>
        abstract sort: records: ResizeArray<'T> * lo: float * hi: float -> ResizeArray<'T>

    type [<AllowNullLiteral>] Sorter<'T> =
        [<Emit("$0($1...)")>] abstract Invoke: records: ResizeArray<'T> * lo: float * hi: float -> ResizeArray<'T>

    type [<AllowNullLiteral>] Bisection<'T> =
        [<Emit("$0($1...)")>] abstract Invoke: records: ResizeArray<'T> * record: 'T * lo: float * hi: float -> float

    type [<AllowNullLiteral>] Bisector<'T> =
        inherit Bisection<'T>
        abstract left: Bisection<'T> with get, set
        abstract right: Bisection<'T> with get, set

    module Heap =

        type [<AllowNullLiteral>] IExports =
            abstract by: selector: OrderedValueSelector<'T> -> Heap<'T>

    module Heapselect =

        type [<AllowNullLiteral>] IExports =
            abstract by: selector: OrderedValueSelector<'T> -> HeapSelector<'T>

    module Bisect =

        type [<AllowNullLiteral>] IExports =
            abstract by: selector: OrderedValueSelector<'T> -> Bisector<'T>
