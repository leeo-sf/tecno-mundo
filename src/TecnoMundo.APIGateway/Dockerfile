FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./src

# Restore as distinct layers
RUN dotnet restore ./src

# Build and publish a release
RUN dotnet publish -c Release -o out ./src

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "TecnoMundo.APIGateway.dll"]