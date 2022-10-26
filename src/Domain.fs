module Farmtrace.Domain

open System

type Feeding = Feeding of datetime : DateTime * amount : int
type Milking = Milking of datetime : DateTime * amount : int

type AnimalKind = | Cow | Goat | Other

type Animal = {
    Id: int
    Kind: AnimalKind
    Feedings: Feeding list
    Milkings: Milking list
}

type Farm = {
    Name: string
    Animals: Animal list
}