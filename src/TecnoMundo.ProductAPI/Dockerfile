FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
COPY ../Core/TecnoMundo.Domain.csproj ./src/Core/TecnoMundo.Domain
COPY ../Core/TecnoMundo.Infra.Data.csproj ./src/Core/TecnoMundo.Infra.Data
COPY ../Core/TecnoMundo.Infra.Ioc.csproj ./src/Core/TecnoMundo.Infra.Ioc
COPY ../Core/TecnoMundo.Application.csproj ./src/Core/TecnoMundo.Application
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "TecnoMundo.ProductAPI.dll"]
