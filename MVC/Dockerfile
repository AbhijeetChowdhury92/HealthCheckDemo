#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
COPY ca-cert.crt /usr/local/share/ca-certificates/ca-cert.crt
RUN chmod 644 /usr/local/share/ca-certificates/ca-cert.crt && update-ca-certificates
WORKDIR /app
EXPOSE 58502
EXPOSE 44388

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MVC/MVC.csproj", "MVC/"]
RUN dotnet restore "MVC/MVC.csproj"
COPY . .
WORKDIR "/src/MVC"
RUN dotnet build "MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MVC.dll"]