module Farmtrace.DataAccess

open FSharp.Data
open System.IO

[<Literal>]
let ResolutionFolder = __SOURCE_DIRECTORY__

type FarmData = JsonProvider<"../data/sample.json", SampleIsList=false, ResolutionFolder=ResolutionFolder>

let getFarmData path =
    async {
        return! FarmData.AsyncLoad(Path.Combine(ResolutionFolder, path))
    }