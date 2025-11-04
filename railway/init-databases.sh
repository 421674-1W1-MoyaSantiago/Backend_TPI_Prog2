#!/bin/bash
# Script de inicialización para Railway con SQL Server

echo "Esperando a que SQL Server esté listo..."

# Esperar a que SQL Server esté disponible
until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT 1" &> /dev/null
do
  echo "SQL Server no está listo todavía..."
  sleep 1
done

echo "SQL Server está listo. Ejecutando scripts de inicialización..."

# Ejecutar scripts de inicialización para Auth-API
if [ -f "/docker-entrypoint-initdb.d/auth-init.sql" ]; then
    echo "Ejecutando script de Auth-API..."
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /docker-entrypoint-initdb.d/auth-init.sql
fi

# Ejecutar scripts de inicialización para Pharm-API
if [ -f "/docker-entrypoint-initdb.d/pharm-init.sql" ]; then
    echo "Ejecutando script de Pharm-API..."
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /docker-entrypoint-initdb.d/pharm-init.sql
fi

echo "Inicialización de base de datos completada."