# Deployment en Railway - APIs Farmacia

## 📦 Resumen del Proyecto

Este proyecto contiene dos APIs .NET 8 con SQL Server:
- **Auth-API**: Manejo de autenticación y usuarios
- **Pharm-API**: Sistema de gestión de farmacia

**✅ MANTIENE tu configuración actual de SQL Server en Docker**

## 🚀 Deployment en Railway

### ✨ La Buena Noticia

Tu configuración actual de SQL Server funcionará **EXACTAMENTE IGUAL** en Railway. No necesitas cambiar nada de tu base de datos.

### Opción Recomendada: Mantener tu configuración

1. **Conectar repositorio a Railway**
   - Ir a [railway.app](https://railway.app)
   - Conectar con GitHub
   - Seleccionar el repositorio `PracticoProgramacion2`

2. **Railway detectará automáticamente:**
   - Tu `docker-compose.railway.yml`
   - Tus Dockerfiles de SQL Server personalizados
   - Todos tus scripts de inicialización

3. **Se crearán 4 servicios automáticamente:**
   - `auth-db` (Base de datos Auth - SQL Server)
   - `pharm-db` (Base de datos Pharm - SQL Server) 
   - `auth-api` (API de autenticación)
   - `pharm-api` (API de farmacia)

### Opción 2: Servicios Separados

1. **Crear 3 servicios separados en Railway:**

#### Servicio 1: SQL Server
```bash
# Usar imagen oficial de SQL Server
mcr.microsoft.com/mssql/server:2022-latest
```

#### Servicio 2: Auth-API
```bash
# Build desde: /Auth-api
# Dockerfile: Dockerfile
```

#### Servicio 3: Pharm-API
```bash
# Build desde: /Pharm-api  
# Dockerfile: Dockerfile
```

## ⚙️ Variables de Entorno Requeridas

### Variables que necesitas configurar en Railway:

```env
# SQL Server Password (puedes mantener tu actual o cambiarla)
SQL_SERVER_SA_PASSWORD=Root123!

# JWT Configuration (mantén tus valores actuales)
JWT_SECRET_KEY=mi-super-clave-secreta-jwt-para-desarrollo-con-32-caracteres-minimo
JWT_ISSUER=FarmaciaAPI
JWT_AUDIENCE=FarmaciaAPIUsers

# URL de Auth-API (Railway la genera automáticamente)
AUTH_API_URL=https://auth-api-production.up.railway.app
```

### ✅ Variables que Railway configura automáticamente:

```env
# Puerto dinámico (diferente para cada servicio)
PORT=<puerto-asignado-por-railway>

# Connection Strings (se construyen automáticamente)
SQLSERVER_CONNECTION_STRING=Server=auth-db,1433;Database=AuthDB;User Id=sa;Password=${SQL_SERVER_SA_PASSWORD};TrustServerCertificate=true;

# Configuración de entorno
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
```

## 🗄️ Base de Datos

### ✅ **Excelente noticia: CERO cambios necesarios**

Tu configuración actual funcionará perfectamente:

- ✅ SQL Server 2022 Express (como tienes ahora)
- ✅ Tus scripts de inicialización se ejecutarán automáticamente
- ✅ Tus Dockerfiles personalizados se usarán tal como están
- ✅ Mismos nombres de bases de datos: `AuthDB` y `PharmDB`
- ✅ Mismo usuario: `sa` con tu password actual
- ✅ Mismos puertos internos: 1433 y 1434

### Archivos que Railway usará automáticamente:
```
Auth-api/.dockers/sqlserver/
├── Dockerfile
└── init-scripts/
    ├── 01-create-database.sql
    ├── 02-create-tables.sql
    └── 03-seed-data.sql  ← Crea usuarios: admin, usuario1

Pharm-api/.dockers/sqlserver/
├── Dockerfile  
└── init-scripts/
    ├── 01-create-database.sql
    ├── 02-create-tables.sql
    ├── 03-seed-data.sql  ← Crea usuarios: admin, usuario1 (sincronizados)
    └── 05-triggers.sql

railway/  ← Scripts auxiliares organizados
├── auth-init.sql      ← Script unificado Auth-API
├── pharm-init.sql     ← Script unificado Pharm-API  
└── init-databases.sh  ← Script de inicialización
```

### 👥 **Usuarios Sincronizados**

**✅ Usuarios por defecto en ambas APIs:**
- **admin** - admin@farmacia.ejemplo.com - Password: **admin123**
- **usuario1** - usuario1@farmacia.ejemplo.com - Password: **usuario123**

**🔄 Sincronización automática:**
- Auth-API: Maneja autenticación y JWTs
- Pharm-API: Recibe usuarios automáticamente via triggers/middleware
- Ambos sistemas mantienen los mismos usuarios siempre

## � Configuración de Puertos

### ✅ **Configuración automática en Railway:**

```yaml
# Bases de datos (internas, no expuestas públicamente)
auth-db:     puerto 1433 (interno)
pharm-db:    puerto 1433 (interno)

# APIs (Railway asigna puertos automáticamente)
auth-api:    $PORT (Railway lo asigna automáticamente)
pharm-api:   $PORT (Railway lo asigna automáticamente)
```

### 🔗 **Conexiones entre servicios:**

```env
# Auth-API se conecta a su base de datos:
Server=auth-db,1433;Database=AuthDB

# Pharm-API se conecta a su base de datos:
Server=pharm-db,1433;Database=PharmDB

# Pharm-API se conecta a Auth-API:
AUTH_API_URL=${AUTH_API_URL}  # Railway configura esto automáticamente
```

### 🌐 **URLs públicas finales:**
```
Auth-API:  https://auth-api-production.up.railway.app
Pharm-API: https://pharm-api-production.up.railway.app
```

**📝 Nota importante:** Railway maneja automáticamente:
- Los puertos internos entre servicios
- Los puertos públicos (HTTPS automático)
- La comunicación entre contenedores
- Las variables de entorno necesarias

## 📋 Checklist de Deployment

- [ ] Repositorio conectado a Railway
- [ ] Variables de entorno configuradas
- [ ] SQL Server desplegado y funcionando
- [ ] Auth-API desplegado correctamente
- [ ] Pharm-API desplegado correctamente
- [ ] Bases de datos inicializadas
- [ ] APIs pueden comunicarse entre sí
- [ ] Endpoints funcionando correctamente

## 🧪 Testing Post-Deployment

### 1. Verificar Auth-API
```bash
curl https://tu-auth-api.up.railway.app/api/ping
```

### 2. Verificar Pharm-API
```bash
curl https://tu-pharm-api.up.railway.app/api/ping
```

### 3. Test de Autenticación con usuarios por defecto

```bash
# Login con usuario admin
curl -X POST https://tu-auth-api.up.railway.app/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Login con usuario1
curl -X POST https://tu-auth-api.up.railway.app/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"usuario1","password":"usuario123"}'

# Verificar que Pharm-API reconoce el token
curl -X GET https://tu-pharm-api.up.railway.app/api/empleados \
  -H "Authorization: Bearer <token-obtenido-del-login>"
```

### 4. Verificar sincronización de usuarios

```bash
# Listar usuarios en Auth-API
curl https://tu-auth-api.up.railway.app/api/users

# Verificar que Pharm-API tiene los mismos usuarios
curl https://tu-pharm-api.up.railway.app/api/usuarios
```

## 💰 Costos con GitHub Student

- **Railway**: $5 USD/mes gratis
- **SQL Server Express**: Gratis
- **Ancho de banda**: Generoso límite gratuito

## 🔧 Troubleshooting

### Problema: SQL Server no inicia
- Verificar que `SA_PASSWORD` cumpla requisitos de seguridad
- Debe tener al menos 8 caracteres, mayúsculas, minúsculas, números y símbolos

### Problema: APIs no se conectan a SQL Server
- Verificar que la connection string use `sqlserver.railway.internal`
- Confirmar que las variables de entorno están bien configuradas

### Problema: Auth-API y Pharm-API no se comunican
- Verificar que `AUTH_API_URL` apunte a la URL correcta de Railway
- Asegurar que ambos servicios estén en el mismo proyecto Railway

## 📞 Soporte

Si tienes problemas:
1. Revisar logs en Railway Dashboard
2. Verificar variables de entorno
3. Confirmar que todos los servicios estén corriendo
4. Revisar los health checks de SQL Server

## 🎯 Próximos Pasos

1. Configurar dominio personalizado (opcional)
2. Implementar monitoreo y alertas
3. Configurar backups de base de datos
4. Optimizar performance y escalabilidad