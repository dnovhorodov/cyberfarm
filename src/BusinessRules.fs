module Farmtrace.BusinessRules

open System
open Domain
open DataAccess

[<AutoOpen>]
module ActivePatterns = 

    let (|InvariantEqual|_|) (str: string) arg = 
        if String.Compare(str, arg, StringComparison.OrdinalIgnoreCase) = 0
            then Some() else None
            
[<AutoOpen>]
module private FarmAnimal = 
    
    let validateCowMilking amount = 
        if amount >= 0 && amount <= 35
            then Ok amount else Error "A cow can only produce between 0 and 35 kilogram of milk"
    
    let validateCowFeeding amount = 
        if amount >= 0 && amount <= 30
            then Ok amount else Error "A cow can only eat between 0 and 30 kilogram of food"
    
    let validateGoatMilking amount = 
        if amount >= 0 && amount <= 8
            then Ok amount else Error "A goat can only produce between 0 and 8 kilogram of milk"
    
    let validateGoatFeeding amount = 
        if amount >= 0 && amount <= 3
            then Ok amount else Error "A goat can only eat between 0 and 3 kilogram of food"

    let validateFeeding animalType foodAmount =
        match animalType with
        | InvariantEqual "Cow" ->
            validateCowFeeding foodAmount
        | InvariantEqual "Goat" ->
            validateGoatFeeding foodAmount
        | _ -> Error "Animal is not supported on the farm"
    
    let validateMilking animalType milkAmount =
        match animalType with
        | InvariantEqual "Cow" ->
            milkAmount |> validateCowMilking 
        | InvariantEqual "Goat" ->
            milkAmount |> validateGoatMilking 
        | _ -> Error "Animal is not supported on the farm"

    let createAnimal animalType id (feedings: Feeding list) (milkings: Milking list) =
        match animalType with
        | InvariantEqual "Cow" ->
            Cow { Id = id; Type = animalType; Feedings = feedings; Milkings = milkings }
        | InvariantEqual "Goat" ->
            Goat { Id = id; Type = animalType; Feedings = feedings; Milkings = milkings }
        | _ -> Other animalType

let create animalType id (feedings: ActionDto array) (milkings: ActionDto array) =
    let feeds = 
        feedings |> Array.choose(fun f -> 
            (animalType, f.Amount) ||> validateFeeding
            |> function
            | Ok amount -> Some (Feeding (f.DateTime, amount))
            | Error _ -> None
        ) |> List.ofArray

    let milks = 
        milkings |> Array.choose(fun m -> 
            (animalType, m.Amount) ||> validateMilking
            |> function
            | Ok amount -> Some (Milking (m.DateTime, amount))
            | Error _ -> None
        ) |> List.ofArray

    createAnimal animalType id feeds milks

let parse (data: FarmData.Root[]) =
    data
    |> Seq.map(fun farm -> 
        {
            Name = farm.Name
            Animals = farm.Animals |> Array.map(fun animal ->
                let feedings = animal.Feedings |> Array.map (fun f -> { DateTime = f.DateTime; Amount = f.Amount })
                let milkings = animal.Milkings |> Array.map (fun m -> { DateTime = m.DateTime; Amount = m.Amount })
                create animal.AnimalType animal.Id feedings milkings
            ) |> List.ofArray
        })