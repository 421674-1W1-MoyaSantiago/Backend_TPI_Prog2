# Dockerfile para Pharm-api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Pharm-api.csproj", "src/"]
RUN dotnet restore "src/Pharm-api.csproj"
COPY . .
WORKDIR "/src/src"
RUN dotnet build "Pharm-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pharm-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pharm-api.dll"]