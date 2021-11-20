module Farmtrace.Reports

open Domain
open System

let getTotalProductionOfMilk animalType startDate endDate farmAnimals = 
    farmAnimals
        |> Seq.choose (fun a -> 
            match a with
            | Cow c when c.Type = animalType -> Some a
            | Goat g when g.Type = animalType -> Some a
            | _ -> None)
        |> Seq.collect (function Cow c -> c.Milkings | Goat g -> g.Milkings | _ -> List.empty<Milking>)
        |> Seq.where (function Milking (dt, _) -> dt >= startDate && dt <= endDate)
        |> Seq.sumBy (function Milking (_, amount) -> amount)

let getTotalAmountOfFood animalType startDate endDate farmAnimals = 
    farmAnimals
        |> Seq.choose (fun a -> 
            match a with
            | Cow c when c.Type = animalType -> Some a
            | Goat g when g.Type = animalType -> Some a
            | _ -> None)
        |> Seq.collect (function Cow c -> c.Feedings | Goat g -> g.Feedings | _ -> List.empty<Feeding>)
        |> Seq.where (function Feeding (dt, _) -> dt >= startDate && dt <= endDate)
        |> Seq.sumBy (function Feeding (_, amount) -> amount)

let getBestProducingAnimals top startDate endDate farmAnimals = 
    farmAnimals
        |> Seq.groupBy (function Cow c -> c.Id | Goat g -> g.Id | _ -> -1)
        |> Seq.map (fun (id, animals) ->
            let total = 
                animals 
                |> Seq.collect (function Cow c -> c.Milkings | Goat g -> g.Milkings | _ -> List.empty<Milking>)
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