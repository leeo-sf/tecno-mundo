FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY ./src/Core/TecnoMundo.Domain ./Core/TecnoMundo.Domain
COPY ./src/Core/TecnoMundo.Application ./Core/TecnoMundo.Application
COPY ./src/Core/TecnoMundo.Infra.Data ./Core/TecnoMundo.Infra.Data
COPY ./src/Core/TecnoMundo.Infra.Ioc ./Core/TecnoMundo.Infra.Ioc
# Copy everything
COPY ./src/TecnoMundo.ProductAPI .

# Restore as distinct layers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "TecnoMundo.ProductAPI.dll"]
