# Usar la imagen base de .NET 8 para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Usar la imagen SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos del proyecto desde src/
COPY src/*.csproj .
RUN dotnet restore "Auth-api.csproj"

# Copiar todo el código fuente
COPY src/ .
RUN dotnet build "Auth-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar la aplicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Auth-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Crear la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Crear un usuario no-root para seguridad
RUN adduser --disabled-password --home /app --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Auth-api.dll"]