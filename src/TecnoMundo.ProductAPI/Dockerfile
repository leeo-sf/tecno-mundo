FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY ./src/Core/TecnoMundo.Domain/TecnoMundo.Domain.csproj ./App/Core/TecnoMundo.Domain
COPY ./src/Core/TecnoMundo.Application/TecnoMundo.Application.csproj ./App/Core/TecnoMundo.Application
COPY ./src/Core/TecnoMundo.Infra.Data/TecnoMundo.Infra.Data.csproj ./App/Core/TecnoMundo.Infra.Data
COPY ./src/Core/TecnoMundo.Infra.Ioc/TecnoMundo.Infra.Ioc.csproj ./App/Core/TecnoMundo.Infra.Ioc
# Copy everything
COPY ./src/TecnoMundo.ProductAPI ./App

# Restore as distinct layers
RUN dotnet restore ./App

# Build and publish a release
RUN dotnet publish -c Release -o out ./App/TecnoMundo.ProductAPI

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "TecnoMundo.ProductAPI.dll"]
