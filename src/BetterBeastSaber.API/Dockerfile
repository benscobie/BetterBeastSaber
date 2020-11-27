#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/BetterBeastSaber.API/BetterBeastSaber.API.csproj", "src/BetterBeastSaber.API/"]
COPY ["src/BetterBeastSaber.Mapping/BetterBeastSaber.Mapping.csproj", "src/BetterBeastSaber.Mapping/"]
COPY ["src/BetterBeastSaber.Data/BetterBeastSaber.Data.csproj", "src/BetterBeastSaber.Data/"]
COPY ["src/BetterBeastSaber.Domain/BetterBeastSaber.Domain.csproj", "src/BetterBeastSaber.Domain/"]
RUN dotnet restore "src/BetterBeastSaber.API/BetterBeastSaber.API.csproj"
COPY . .
WORKDIR "/src/src/BetterBeastSaber.API"
RUN dotnet build "BetterBeastSaber.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetterBeastSaber.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BetterBeastSaber.API.dll"]