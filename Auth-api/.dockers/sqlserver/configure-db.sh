#!/bin/bash

# Script para configurar SQL Server con base de datos inicial

echo "Iniciando configuración de SQL Server..."

# Iniciar SQL Server en background
/opt/mssql/bin/sqlservr &

# Esperar a que SQL Server esté listo
echo "Esperando a que SQL Server esté listo..."
sleep 60s

# Verificar que SQL Server esté realmente listo
echo "Verificando conectividad..."
for i in {1..30}; do
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -Q "SELECT 1" -C &>/dev/null; then
        echo "SQL Server está listo!"
        break
    fi
    echo "Intento $i/30 - Esperando..."
    sleep 2
done

# Ejecutar scripts de inicialización
echo "Ejecutando scripts de inicialización..."

# Crear base de datos AuthDB
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i /opt/mssql-scripts/01-create-database.sql -C

# Crear tablas
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d AuthDB -i /opt/mssql-scripts/02-create-tables.sql -C

# Insertar datos iniciales (opcional)
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d AuthDB -i /opt/mssql-scripts/03-seed-data.sql -C

echo "Configuración completada. SQL Server listo para recibir conexiones."

# Crear un simple health check HTTP en puerto 8080
echo "Iniciando health check HTTP en puerto 8080..."
(
  while true; do
    echo -e "HTTP/1.1 200 OK\r\nContent-Length: 19\r\n\r\nSQL Server Ready!" | nc -l -p 8080
  done
) &

# Mantener el contenedor corriendo
wait