#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app

COPY . .

ENTRYPOINT ["dotnet", "Consul01.dll"]
