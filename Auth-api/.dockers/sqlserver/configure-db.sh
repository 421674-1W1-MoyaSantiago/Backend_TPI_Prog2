#!/bin/bash

# Script para configurar SQL Server con base de datos inicial

echo "Iniciando configuración de SQL Server..."

# Iniciar SQL Server en background
echo "Iniciando SQL Server con configuración optimizada..."
echo "Variables de entorno:"
echo "ACCEPT_EULA: $ACCEPT_EULA"
echo "SA_PASSWORD: [REDACTED]"
echo "MSSQL_PID: $MSSQL_PID"

# Verificar que las variables estén configuradas
if [ -z "$ACCEPT_EULA" ] || [ "$ACCEPT_EULA" != "Y" ]; then
    echo "ERROR: ACCEPT_EULA no está configurado correctamente"
    exit 1
fi

if [ -z "$SA_PASSWORD" ]; then
    echo "ERROR: SA_PASSWORD no está configurado"
    exit 1
fi

# Iniciar SQL Server y capturar errores
echo "Iniciando SQL Server..."
/opt/mssql/bin/sqlservr --accept-eula &
SQL_PID=$!

echo "SQL Server PID: $SQL_PID"

# Esperar a que SQL Server esté listo
echo "Esperando a que SQL Server esté listo..."
sleep 90s

# Verificar que el proceso siga corriendo
if ! kill -0 $SQL_PID 2>/dev/null; then
    echo "ERROR: SQL Server se cerró inesperadamente"
    exit 1
fi

echo "SQL Server proceso aún corriendo, verificando conectividad..."

# Verificar que SQL Server esté realmente listo
echo "Verificando conectividad..."
for i in {1..45}; do
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -Q "SELECT 1" &>/dev/null; then
        echo "SQL Server está listo!"
        break
    fi
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -Q "SELECT 1" -C &>/dev/null; then
        echo "SQL Server está listo!"
        break
    fi
    echo "Intento $i/45 - Esperando..."
    sleep 3
done

# Ejecutar scripts de inicialización solo si SQL Server está respondiendo
echo "Ejecutando scripts de inicialización..."

# Intentar crear base de datos (con manejo de errores)
echo "Creando base de datos AuthDB..."
if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i /opt/mssql-scripts/01-create-database.sql; then
    echo "Base de datos creada exitosamente"
    
    # Solo continuar si la base se creó correctamente
    echo "Creando tablas..."
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d AuthDB -i /opt/mssql-scripts/02-create-tables.sql

    echo "Insertando datos iniciales..."
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d AuthDB -i /opt/mssql-scripts/03-seed-data.sql
    
    echo "¡Base de datos AuthDB configurada correctamente!"
else
    echo "ADVERTENCIA: No se pudo crear la base de datos"
    echo "Intentando sin certificado..."
    
    # Intentar sin verificación de certificado
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i /opt/mssql-scripts/01-create-database.sql -C; then
        echo "Base de datos creada con -C flag"
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d AuthDB -i /opt/mssql-scripts/02-create-tables.sql -C
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d AuthDB -i /opt/mssql-scripts/03-seed-data.sql -C
        echo "¡Base de datos AuthDB configurada correctamente!"
    else
        echo "ERROR: No se pudo crear la base de datos"
    fi
fi

echo "Configuración completada. SQL Server listo para recibir conexiones."

echo "Contenedor listo - SQL Server en puerto 1433"

# Mantener el contenedor corriendo indefinidamente
while true; do
    sleep 3600  # Dormir 1 hora y repetir
done