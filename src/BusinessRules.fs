module Farmtrace.BusinessRules

open System

[<AutoOpen>]
module ActivePatterns = 

    let (|InvariantEqual|_|) (str: string) arg = 
        if String.Compare(str, arg, StringComparison.OrdinalIgnoreCase) = 0
            then Some() else None

[<AutoOpen>]
module Validation = 
    
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