module Farmtrace.Domain

open System

type Feeding = Feeding of datetime : DateTime * amount : int
type Milking = Milking of datetime : DateTime * amount : int

type Animal = {
    Id: int
    Type: string
    Feedings: Feeding list
    Milkings: Milking list
}

type FarmAnimal = 
    | Cow of Animal
    | Goat of Animal
    | Other of string

type Farm = {
    Name: string
    Animals: FarmAnimal list
}