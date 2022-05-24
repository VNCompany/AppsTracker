#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AppsTracker/AppsTracker.csproj", "AppsTracker/"]
COPY ["DataLayer/DataLayer.csproj", "DataLayer/"]
RUN dotnet restore "AppsTracker/AppsTracker.csproj"
COPY . .
WORKDIR "/src/AppsTracker"
RUN dotnet build "AppsTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppsTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppsTracker.dll"]