#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SkywalkingWebApi_2/SkywalkingWebApi_2.csproj", "SkywalkingWebApi_2/"]
RUN dotnet restore "SkywalkingWebApi_2/SkywalkingWebApi_2.csproj"
COPY . .
WORKDIR "/src/SkywalkingWebApi_2"
RUN dotnet build "SkywalkingWebApi_2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SkywalkingWebApi_2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkywalkingWebApi_2.dll"]