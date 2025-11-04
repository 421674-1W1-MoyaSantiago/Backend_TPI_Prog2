# Dockerfile para Pharm-api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Puerto para Railway
EXPOSE 8080

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

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

# Crear un usuario no-root para seguridad
RUN adduser --disabled-password --home /app --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Pharm-api.dll"]