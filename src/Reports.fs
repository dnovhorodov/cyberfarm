module Farmtrace.Reports

open Domain
open System

(*
    Queries for reports on farm animals data
*)

let getTotalProductionOfMilk animalKind startDate endDate animals = 
    animals
        |> Seq.where(fun a -> a.Kind = animalKind)
        |> Seq.collect (fun a -> a.Milkings)
        |> Seq.where (function Milking (dt, _) -> dt >= startDate && dt <= endDate)
        |> Seq.sumBy (function Milking (_, amount) -> amount)

let getTotalAmountOfFood animalKind startDate endDate animals = 
    animals
        |> Seq.where(fun a -> a.Kind = animalKind)
        |> Seq.collect (fun a -> a.Feedings)
        |> Seq.where (function Feeding (dt, _) -> dt >= startDate && dt <= endDate)
        |> Seq.sumBy (function Feeding (_, amount) -> amount)

let getBestProducingAnimals top startDate endDate animals = 
    animals
        |> Seq.groupBy (fun a -> a.Id)
        |> Seq.map (fun (id, animals) ->
            let total = 
                animals 
                |> Seq.collect (fun a -> a.Milkings)
                |> Seq.where (function Milking (dt, _) -> dt >= startDate && dt <= endDate)
                |> Seq.sumBy (function Milking (_, amount) -> amount)

            {| Id = id; TotalProduced = total |}
        ) 
        |> Seq.sortByDescending (fun x -> x.TotalProduced)
        |> Seq.take (top)

let getTop10BestProducingAnimals startDate endDate = getBestProducingAnimals 10 startDate endDate

let getTop10BestProducingAnimalsLastWeek () = 
    let today = DateTime.Now.Date
    getTop10BestProducingAnimals today (today.AddDays(-7))