# Auth API - Sistema de Autenticación JWT

API de autenticación con JWT tokens desarrollada en .NET 8 y SQL Server.

## 📁 Estructura del Proyecto

```
Auth-api/
├── src/                              # Código fuente de la aplicación
│   ├── Controllers/                  # Controladores de la API
│   │   └── UserController.cs        # Endpoints de autenticación y usuarios
│   ├── Models/                       # Modelos de datos
│   │   └── User.cs                  # Modelo de usuario con propiedades
│   ├── Services/                     # Servicios de lógica de negocio
│   │   ├── UserService.cs           # Lógica de usuarios (CRUD, validaciones)
│   │   ├── IUserService.cs          # Interfaz del servicio de usuarios
│   │   ├── JwtService.cs            # Generación y validación de tokens JWT
│   │   └── IJwtService.cs           # Interfaz del servicio JWT
│   ├── Repositories/                 # Acceso a datos
│   │   ├── UserRepository.cs        # Repositorio de usuarios con Entity Framework
│   │   └── IUserRepository.cs       # Interfaz del repositorio
│   ├── Data/                         # Contexto de base de datos
│   │   └── AuthDbContext.cs         # Configuración de Entity Framework
│   ├── DTOs/                         # Objetos de transferencia de datos
│   │   ├── UserDtos.cs              # DTOs para usuarios (Login, Register, etc.)
│   │   └── PingResponseDtos.cs      # DTOs para respuestas de ping
│   ├── Properties/                   # Propiedades del proyecto
│   │   └── launchSettings.json      # Configuración de lanzamiento
│   ├── Auth-api.csproj              # Archivo de proyecto .NET
│   ├── Program.cs                   # Punto de entrada de la aplicación
│   ├── appsettings.json             # Configuración general
│   ├── appsettings.Development.json # Configuración para desarrollo
│   └── appsettings.Production.json  # Configuración para producción
├── .dockers/                         # Configuración de Docker
│   └── sqlserver/                   # SQL Server personalizado
│       ├── Dockerfile               # Imagen personalizada de SQL Server
│       ├── configure-db.sh          # Script de configuración de BD
│       └── init-scripts/            # Scripts de inicialización
│           ├── 01-create-database.sql # Crear base de datos AuthDB
│           ├── 02-create-tables.sql   # Crear tablas de usuarios
│           ├── 03-seed-data.sql       # Datos iniciales (opcional)
│           └── 04-test-data.sql       # Datos de prueba
├── Dockerfile                        # Imagen Docker de la API
├── docker-compose.yml               # Orquestación de servicios
├── docker-compose.dev.yml           # Configuración para desarrollo
├── .dockerignore                    # Archivos ignorados por Docker
└── README.md                        # Documentación del proyecto
```

### 🏗️ Descripción de Componentes

**src/Controllers/**: Contiene los controladores de la API que manejan las peticiones HTTP y definen los endpoints.

**src/Models/**: Define las entidades del dominio (User) que representan las tablas de la base de datos.

**src/Services/**: Implementa la lógica de negocio, validaciones y reglas de la aplicación.

**src/Repositories/**: Maneja el acceso a datos y las operaciones CRUD con Entity Framework.

**src/Data/**: Configura el contexto de Entity Framework y la conexión a la base de datos.

**src/DTOs/**: Objetos para transferencia de datos entre capas, validación de entrada y respuestas.

**.dockers/sqlserver/**: Configuración personalizada de SQL Server con scripts de inicialización automática.

**docker-compose.yml**: Define los servicios (API y Base de datos) y su orquestación.

## 🚀 Cómo ejecutar el proyectoh API - Sistema de Autenticación JWT

API de autenticación con JWT tokens desarrollada en .NET 8 y SQL Server.

## � Cómo ejecutar el proyecto

### Prerrequisitos
- Docker y Docker Compose instalados
- Puerto 5004 disponible (API)
- Puerto 1434 disponible (Base de datos)

### Ejecutar todo el stack
```bash
# Clonar y navegar al proyecto
cd Auth-api

# Levantar todos los servicios
docker compose up --build -d

# Verificar que todo está funcionando
docker compose ps
```

### Comandos útiles
```bash
# Ver logs de todos los servicios
docker compose logs

# Ver logs solo de la API
docker compose logs auth-api

# Ver logs en tiempo real
docker compose logs -f

# Detener todo
docker compose down

# Detener y eliminar volúmenes (reset completo)
docker compose down -v
```

## 🌐 URLs disponibles

- **API**: http://localhost:5004
- **Health Check**: http://localhost:5004/api/user/health

## 📋 Endpoints de la API

### 🔍 Health Check
Verifica que la API esté funcionando correctamente.

http
GET
http://localhost:5004/api/user/health

**Respuesta exitosa (200 OK):**

{
  "status": "healthy",
  "timestamp": "2025-10-25T16:30:00Z"
}

---

### 👤 Registrar Usuario
Crea un nuevo usuario en el sistema.

http
POST
http://localhost:5004/api/user/register
Content-Type: application/json

**Body requerido:**

{
  "username": "miusuario",
  "password": "mipassword123",
  "email": "usuario@email.com"
}

**Respuesta exitosa (200 OK):**

{
  "message": "Usuario registrado exitosamente"
}

**Errores posibles:**
- **400 Bad Request**: Datos inválidos o username ya existe

{
  "message": "El username ya está en uso"
}

---

### 🔐 Login
Inicia sesión y obtiene un token JWT válido por 60 minutos.

http
POST
http://localhost:5004/api/user/login
Content-Type: application/json

**Body requerido:**

{
  "username": "miusuario",
  "password": "mipassword123"
}

**Respuesta exitosa (200 OK):**

{
  "success": true,
  "message": "Login successful", 
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwibmFtZSI6Im1pdXN1YXJpbyIsImVtYWlsIjoidXN1YXJpb0BlbWFpbC5jb20iLCJleHAiOjE3Mjk4ODQwMjR9..."
}

**Error de credenciales (401 Unauthorized):**

{
  "success": false,
  "message": "Credenciales inválidas"
}

> ⚠️ **Importante**: Guarda el token, lo necesitarás para los endpoints protegidos.

---

### 👤 Obtener Usuario por Username
Busca un usuario específico por su nombre de usuario.

http
GET
http://localhost:5004/api/user/{username}

**Ejemplo:**
http://localhost:5004/api/user/miusuario

**Respuesta exitosa (200 OK):**

{
  "id": 1,
  "username": "miusuario",
  "email": "usuario@email.com", 
  "createdAt": "2025-10-25T16:30:00Z",
  "updatedAt": "2025-10-25T16:30:00Z",
  "status": "Active",
  "deletedAt": null
}


**Usuario no encontrado (404 Not Found):**

{
  "message": "Usuario no encontrado"
}



**Respuesta exitosa (200 OK):**
```json
{
  "id": 1,
  "username": "miusuario",
  "email": "usuario@email.com", 
  "createdAt": "2025-10-25T16:30:00Z",
  "updatedAt": "2025-10-25T16:30:00Z",
  "status": "Active",
  "deletedAt": null
}
```

**Usuario no encontrado (404 Not Found):**
```json
{
  "message": "Usuario no encontrado"
}
```


---

### 🗑️ Eliminar Usuario (Soft Delete)
Elimina lógicamente un usuario (no lo borra físicamente de la base de datos).

http
DELETE
http://localhost:5004/api/user/{id}

**Ejemplo:**
http://localhost:5004/api/user/1

**Respuesta exitosa (200 OK):**

{
  "message": "Usuario eliminado exitosamente"
}

**Usuario no encontrado (404 Not Found):**

{
  "message": "Usuario no encontrado"
}

---

### ♻️ Restaurar Usuario
Restaura un usuario previamente eliminado.

http
PATCH
http://localhost:5004/api/user/{id}/restore

**Ejemplo:**
http://localhost:5004/api/user/1/restore

**Respuesta exitosa (200 OK):**

{
  "message": "Usuario restaurado exitosamente"
}

**Errores posibles:**
- **404 Not Found**: Usuario no encontrado
- **400 Bad Request**: Usuario ya está activo

{
  "message": "El usuario ya está activo"
}

---

### 🔒 Perfil del Usuario (Protegido)
Obtiene información del usuario autenticado actual.

http
GET
http://localhost:5004/api/user/profile
Authorization: Bearer {token}

**Headers requeridos:**
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

**Respuesta exitosa (200 OK):**

{
  "message": "Acceso autorizado",
  "user": {
    "id": "1",
    "username": "miusuario", 
    "email": "usuario@email.com"
  },
  "timestamp": "2025-10-25T16:30:00Z"
}

**Token inválido o expirado (401 Unauthorized):**

{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}

---

## 🔒 Autenticación JWT

### Cómo usar el token
1. Haz login para obtener el token
2. Incluye el token en el header `Authorization` de las peticiones protegidas:
   ```
   Authorization: Bearer tu_token_aqui
   ```

### Token contiene
- **ID del usuario**
- **Email**
- **Username** 
- **Fecha de expiración** (60 minutos)

### Endpoints protegidos
Actualmente solo `/api/user/profile` requiere autenticación. Los demás endpoints son públicos para facilitar las pruebas.

## 💾 Base de Datos

### Información de conexión
- **Host**: localhost
- **Puerto**: 1434
- **Usuario**: sa
- **Password**: Root123!
- **Base de datos**: AuthDB

## 🛠️ Desarrollo

### Estructura del código
```
src/
├── Controllers/UserController.cs    # Endpoints de la API
├── Models/User.cs                   # Modelo de usuario
├── Services/
│   ├── UserService.cs              # Lógica de negocio
│   └── JwtService.cs               # Generación de tokens
├── Data/AuthDbContext.cs           # Contexto de Entity Framework
└── DTOs/UserDtos.cs               # Objetos de transferencia
```

### Tecnologías utilizadas
- **.NET 8** - Framework web
- **Entity Framework Core** - ORM
- **SQL Server 2022** - Base de datos
- **JWT Bearer** - Autenticación
- **BCrypt** - Hash de contraseñas
- **Docker & Docker Compose** - Containerización

## 📝 Notas importantes

- La base de datos se inicializa automáticamente al arrancar
- Los tokens JWT expiran en 60 minutos
- Las contraseñas se hashean con BCrypt
- Se implementa soft delete (eliminación lógica)
- Los logs de Docker ayudan a debuggear problemas

## 🧪 Ejemplo de prueba completa

```bash
# 1. Verificar que funciona
curl http://localhost:5004/api/user/health

# 2. Registrar usuario
curl -X POST http://localhost:5004/api/user/register \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"test123","email":"test@email.com"}'

# 3. Hacer login
curl -X POST http://localhost:5004/api/user/login \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"test123"}'

# 4. Usar el token (reemplazar TOKEN_AQUI)
curl http://localhost:5004/api/user/profile \
  -H "Authorization: Bearer TOKEN_AQUI"
```

---

¡Listo para usar! 🚀

