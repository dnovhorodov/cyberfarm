ARG SDK_VERSION=6.0-alpine
ARG RUNTIME_VERSION=6.0-alpine

# Stage 1 - Build SDK image
FROM mcr.microsoft.com/dotnet/sdk:$SDK_VERSION AS build

WORKDIR /app
COPY ./ /app

RUN dotnet publish "./Farmtrace.fsproj" -c Release -r alpine-x64 --self-contained true /p:PublishTrimmed=true -o out

# Stage 2 - Build runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:$RUNTIME_VERSION AS base
WORKDIR /app

COPY --from=build /app/out .
COPY ./data/ ./data
ENTRYPOINT ["./Farmtrace"]
