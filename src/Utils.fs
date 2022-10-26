module rec Farmtrace.Utils

open Domain
open DataAccess
open BusinessRules

(*
    Parse JSON data to domain models
*)
let parse (data: FarmData.Root[]) =
    data
    |> Seq.map(fun farm -> 
        {
            Name = farm.Name
            Animals = farm.Animals |> Array.map(fun animal -> create animal) |> List.ofArray
        })

(*
    Public function to create a validated animal from raw input
*)
let create (animal: FarmData.Animal) =
    let animalKind = 
        match animal.AnimalType with
        | InvariantEqual "Cow" -> Cow
        | InvariantEqual "Goat" -> Goat
        | _ -> Other

    let feeds = 
        animal.Feedings |> Array.choose(fun f -> 
            (animal.AnimalType, f.Amount) ||> validateFeeding
            |> function
            | Ok amount -> Some (Feeding (f.DateTime, amount))
            | Error _ -> None
        ) |> List.ofArray

    let milks = 
        animal.Milkings |> Array.choose(fun m -> 
            (animal.AnimalType, m.Amount) ||> validateMilking
            |> function
            | Ok amount -> Some (Milking (m.DateTime, amount))
            | Error _ -> None
        ) |> List.ofArray

    { Id = animal.Id; Kind = animalKind; Feedings = feeds; Milkings = milks }