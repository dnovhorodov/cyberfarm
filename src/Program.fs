open System
open Farmtrace.DataAccess
open Farmtrace.BusinessRules
open Farmtrace.Reports

// Change these dates for reports
let startDate = DateTime(2021, 10, 01)
let endDate = DateTime(2021, 11, 01)

// Read the data from raw json file
let data = getFarmData @"../data/farmdata.json" |> Async.RunSynchronously
// Parse raw data in the valid shape
let farmAnimals = data |> parse |> Seq.collect (fun a -> a.Animals)
// Run various reports on the validated data
let cowsMilkings = farmAnimals |> getTotalProductionOfMilk "Cow" startDate endDate
let goatsMilkings = farmAnimals |> getTotalProductionOfMilk "Goat" startDate endDate
let cowsEated = farmAnimals |> getTotalAmountOfFood "Cow" startDate endDate
let goatsEated = farmAnimals |> getTotalAmountOfFood "Goat" startDate endDate
let top10producers = farmAnimals |> getTop10BestProducingAnimals startDate endDate |> List.ofSeq

printfn "               Hello from Farmtrace cyberfarm               "
printfn "============================================================"
printfn "Here the report from the farms... "
printfn $"From: {startDate} to {endDate}"
printfn "============================================================"
printfn $"Milk produced by cows: {cowsMilkings} kg"
printfn $"Milk produced by goats: {goatsMilkings} kg"
printfn $"Cows ate: {cowsEated} kg"
printfn $"Goats ate: {cowsEated} kg"
printfn ""
printfn $"Top 10 best producing animals [by id's]:"
printfn $"%A{top10producers}"
