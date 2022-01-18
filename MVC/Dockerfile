#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 58502
EXPOSE 44388

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HealthCheckMVCDemo/MVC.csproj", "HealthCheckMVCDemo/"]
RUN dotnet restore "HealthCheckMVCDemo/MVC.csproj"
COPY . .
WORKDIR "/src/HealthCheckMVCDemo"
RUN dotnet build "MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MVC.dll"]