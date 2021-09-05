#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ApolloTest/ApolloTest.csproj", "ApolloTest/"]
RUN dotnet restore "ApolloTest/ApolloTest.csproj"
COPY . .
WORKDIR "/src/ApolloTest"
RUN dotnet build "ApolloTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApolloTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApolloTest.dll"]