#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Apollo.Netcore3.0.Demo/Apollo.Netcore3.0.Demo.csproj", "Apollo.Netcore3.0.Demo/"]
RUN dotnet restore "Apollo.Netcore3.0.Demo/Apollo.Netcore3.0.Demo.csproj"
COPY . .
WORKDIR "/src/Apollo.Netcore3.0.Demo"
RUN dotnet build "Apollo.Netcore3.0.Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Apollo.Netcore3.0.Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Apollo.Netcore3.0.Demo.dll"]