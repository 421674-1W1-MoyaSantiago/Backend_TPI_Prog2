# Usar la imagen oficial de SQL Server 2022
FROM mcr.microsoft.com/mssql/server:2022-latest

# Cambiar a usuario root para configuración
USER root

# Instalar dependencias y herramientas de SQL Server
RUN apt-get update && apt-get install -y \
    curl \
    gnupg2 \
    software-properties-common \
    && curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg \
    && echo "deb [arch=amd64,armhf,arm64 signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/22.04/prod jammy main" > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Agregar sqlcmd al PATH
ENV PATH="$PATH:/opt/mssql-tools18/bin"

# Crear directorio para scripts
RUN mkdir -p /opt/mssql-scripts

# Copiar scripts de inicialización
COPY init-scripts/ /opt/mssql-scripts/

# Script de configuración personalizado
COPY configure-db.sh /opt/mssql-scripts/
RUN chmod +x /opt/mssql-scripts/configure-db.sh

# Variables de entorno
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Express

# Exponer puerto
EXPOSE 1433

# Volver al usuario mssql
USER mssql

# Comando personalizado para iniciar SQL Server y ejecutar scripts
CMD /bin/bash /opt/mssql-scripts/configure-db.sh