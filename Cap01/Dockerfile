#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Cap01/Cap01.csproj", "Cap01/"]
RUN dotnet restore "Cap01/Cap01.csproj"
COPY . .
WORKDIR "/src/Cap01"
RUN dotnet build "Cap01.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cap01.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cap01.dll"]