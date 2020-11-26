#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/BetterBeastSaber.Scraper/BetterBeastSaber.Scraper.csproj", "src/BetterBeastSaber.Scraper/"]
COPY ["src/BetterBeastSaber.Domain/BetterBeastSaber.Domain/BetterBeastSaber.Domain.csproj", "src/BetterBeastSaber.Domain/BetterBeastSaber.Domain/"]
COPY ["src/BetterBeastSaber.Data/BetterBeastSaber.Data/BetterBeastSaber.Data.csproj", "src/BetterBeastSaber.Data/BetterBeastSaber.Data/"]
RUN dotnet restore "src/BetterBeastSaber.Scraper/BetterBeastSaber.Scraper.csproj"
COPY . .
WORKDIR "/src/src/BetterBeastSaber.Scraper"
RUN dotnet build "BetterBeastSaber.Scraper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetterBeastSaber.Scraper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BetterBeastSaber.Scraper.dll"]