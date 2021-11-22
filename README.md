[![Docker Image CI](https://github.com/dnovhorodov/cyberfarm/actions/workflows/docker-image.yml/badge.svg)](https://github.com/dnovhorodov/cyberfarm/actions/workflows/docker-image.yml)

# Cyber Farm

Sample of import, process and analysis of farm data

Demonstrates reading and pasing the raw json data, input validation against business rules and running queries for reports

## Prerequisites

* .NET 5.0 runtime or higher

## Getting Started

0. Change *startDate* and *endDate* in **Program.fs** to get some reports
   ```fsharp
   let startDate = DateTime(2021, 10, 01)
   let endDate = DateTime(2021, 11, 01)
   ```

1. Build console application:

   ```bash
   dotnet build
   ```

2. Run:

   ```bash
   dotnet run
   ```

OR Build & Run in docker:
   ```bash
   docker build -t cyberfarm .
   docker run --rm cyberfarm
   ```
