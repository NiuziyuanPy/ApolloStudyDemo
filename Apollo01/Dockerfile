#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Apollo01/Apollo01.csproj", "Apollo01/"]
RUN dotnet restore "Apollo01/Apollo01.csproj"
COPY . .
WORKDIR "/src/Apollo01"
RUN dotnet build "Apollo01.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Apollo01.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Apollo01.dll"]