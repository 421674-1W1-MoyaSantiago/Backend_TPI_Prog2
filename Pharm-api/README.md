# 🏥 Pharm-API - Sistema de Gestión de Farmacia

API REST para gestión de farmacia desarrollada con .NET 8, Entity Framework Core y SQL Server en Docker.

## 🚀 Características

- ✅ **CRUD de Empleados** - Gestión completa de empleados con validaciones
- ✅ **CRUD de Medicamentos** - Gestión completa de medicamentos con validaciones y búsquedas
- ✅ **Gestión de Facturas** - Consultar facturas con detalles de medicamentos y artículos
- ✅ **Autenticación JWT** - Integración con Auth-API para autenticación centralizada
- ✅ **Control de Acceso** - Usuarios con acceso por sucursales
- ✅ **Auto-creación de Usuarios** - Sistema de triggers y middleware para sync automática
- ✅ **Base de datos** - SQL Server en Docker con datos de ejemplo
- ✅ **Patrón Repository/Service** - Arquitectura limpia y mantenible
- ✅ **Validaciones** - Data Annotations y validaciones de negocio

## 📁 Estructura del Proyecto

```
Pharm-api/
├── .dockers/                   # Configuración Docker
│   ├── sqlserver/
│   │   └── init-scripts/       # Scripts de inicialización de BD
│   │       ├── 01-create-database.sql
│   │       ├── 02-create-tables.sql
│   │       ├── 03-seed-data.sql
│   │       ├── 04-test-data.sql
│   │       └── 05-triggers.sql
├── docker-compose.yml          # Configuración de servicios Docker
├── Dockerfile                  # Imagen de la aplicación
└── src/                        # Código fuente
    ├── Controllers/            # Controladores REST API
    │   ├── EmpleadosController.cs
    │   ├── FacturasController.cs
    │   ├── MedicamentosController.cs
    │   ├── PingController.cs
    │   └── UsuariosController.cs
    ├── Data/                   # Contexto de Entity Framework
    │   └── PharmDbContext.cs
    ├── DTOs/                   # Data Transfer Objects
    │   ├── EmpleadoDtos.cs
    │   ├── FacturaVentaDto.cs
    │   ├── LoginDto.cs
    │   ├── MedicamentoDtos.cs
    │   ├── PingDtos.cs
    │   ├── SucursalDto.cs
    │   └── UsuarioDto.cs
    ├── Extensions/             # Extensiones
    │   └── MiddlewareExtensions.cs
    ├── Middleware/             # Middleware personalizado
    │   └── AutoCreateUserMiddleware.cs
    ├── Models/                 # Entidades del dominio
    │   ├── Articulo.cs
    │   ├── Cliente.cs
    │   ├── Empleado.cs
    │   ├── FacturasVentum.cs
    │   ├── Medicamento.cs
    │   ├── Sucursale.cs
    │   ├── Usuario.cs
    │   └── ... (otros modelos)
    ├── Repositories/           # Capa de acceso a datos
    │   ├── IEmpleadoRepository.cs
    │   ├── EmpleadoRepository.cs
    │   ├── IFacturaRepository.cs
    │   ├── FacturaRepository.cs
    │   ├── IMedicamentoRepository.cs
    │   ├── MedicamentoRepository.cs
    │   ├── IUsuarioRepository.cs
    │   └── UsuarioRepository.cs
    ├── Services/               # Lógica de negocio
    │   ├── IEmpleadoService.cs
    │   ├── EmpleadoService.cs
    │   ├── IFacturaService.cs
    │   ├── FacturaService.cs
    │   ├── IMedicamentoService.cs
    │   ├── MedicamentoService.cs
    │   ├── IUsuarioService.cs
    │   ├── UsuarioService.cs
    │   ├── IJwtService.cs
    │   └── JwtService.cs
    ├── appsettings.json        # Configuración
    ├── appsettings.Development.json
    ├── appsettings.Production.json
    ├── Program.cs              # Punto de entrada
    └── Pharm-api.csproj       # Archivo del proyecto
```

## 🔗 API Endpoints

### 🏠 **Health Check**
```http
GET /api/ping
```
**Descripción:** Verificar estado de la API  
**Autenticación:** ❌ No requerida  
**Respuesta:** `{ "message": "Pong from Pharm-API", "timestamp": "..." }`

---

### 👥 **Empleados**

#### Obtener todos los empleados
```http
GET /api/empleados
Authorization: Bearer {token}
```
**Descripción:** Lista empleados filtrados por sucursales del usuario  
**Respuesta:**
```json
[
  {
    "codEmpleado": 1,
    "nomEmpleado": "Admin",
    "apeEmpleado": "Administrador",
    "nroTel": "011-1111-1111",
    "calle": "Av. Admin",
    "altura": 100,
    "email": "admin@farmacia.ejemplo.com",
    "fechaIngreso": "2024-01-15T00:00:00",
    "codTipoEmpleado": 4,
    "tipoEmpleado": "Gerente",
    "codTipoDocumento": 1,
    "tipoDocumento": "DNI",
    "codSucursal": 1,
    "nomSucursal": "Sucursal Centro Ejemplo"
  }
]
```

#### Obtener empleado por ID
```http
GET /api/empleados/{id}
Authorization: Bearer {token}
```
**Descripción:** Obtiene un empleado específico si pertenece a sucursales del usuario

#### Crear empleado
```http
POST /api/empleados
Authorization: Bearer {token}
Content-Type: application/json

{
  "nomEmpleado": "Nuevo",
  "apeEmpleado": "Empleado",
  "nroTel": "011-5555-5555",
  "calle": "Calle Nueva",
  "altura": 123,
  "email": "nuevo@farmacia.ejemplo.com",
  "fechaIngreso": "2024-11-01T00:00:00",
  "codTipoEmpleado": 2,
  "codTipoDocumento": 1,
  "codSucursal": 1
}
```
**Validaciones:**
- Nombres y apellidos requeridos (máx 100 caracteres)
- Email único y formato válido
- Teléfono formato válido
- Sucursal debe pertenecer al usuario
- Tipos de empleado y documento deben existir

#### Actualizar empleado
```http
PUT /api/empleados/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "nomEmpleado": "Empleado",
  "apeEmpleado": "Actualizado",
  "nroTel": "011-6666-6666",
  "calle": "Calle Actualizada",
  "altura": 456,
  "email": "actualizado@farmacia.ejemplo.com",
  "codTipoEmpleado": 1,
  "codTipoDocumento": 1,
  "codSucursal": 1
}
```

#### Eliminar empleado
```http
DELETE /api/empleados/{id}
Authorization: Bearer {token}
```

#### Obtener tipos de empleado
```http
GET /api/empleados/tipos-empleado
Authorization: Bearer {token}
```
**Respuesta:**
```json
[
  { "codTipoEmpleado": 1, "tipo": "Farmacéutico" },
  { "codTipoEmpleado": 2, "tipo": "Técnico en Farmacia" },
  { "codTipoEmpleado": 3, "tipo": "Administrativo" },
  { "codTipoEmpleado": 4, "tipo": "Gerente" }
]
```

#### Obtener tipos de documento
```http
GET /api/empleados/tipos-documento
Authorization: Bearer {token}
```

#### Obtener sucursales del usuario
```http
GET /api/empleados/mis-sucursales
Authorization: Bearer {token}
```
**Descripción:** Lista sucursales asignadas al usuario autenticado

---

### 💊 **Medicamentos**

#### Obtener todos los medicamentos
```http
GET /api/medicamentos
Authorization: Bearer {token}
```
**Descripción:** Lista todos los medicamentos con información completa  
**Respuesta:**
```json
[
  {
    "codMedicamento": 1,
    "codBarra": "7801111111111",
    "descripcion": "Paracetamol 500mg",
    "requiereReceta": false,
    "ventaLibre": true,
    "precioUnitario": 150.50,
    "dosis": 500,
    "posologia": "Tomar 1 comprimido cada 8 horas",
    "codLoteMedicamento": 1,
    "loteDescripcion": "Lote A-2024",
    "codLaboratorio": 1,
    "laboratorioDescripcion": "Laboratorio Ejemplo SA",
    "codTipoPresentacion": 1,
    "tipoPresentacionDescripcion": "Comprimidos",
    "codUnidadMedida": 1,
    "unidadMedidaDescripcion": "mg",
    "codTipoMedicamento": 1,
    "tipoMedicamentoDescripcion": "Analgésico"
  }
]
```

#### Obtener medicamento por ID
```http
GET /api/medicamentos/{id}
Authorization: Bearer {token}
```
**Descripción:** Obtiene un medicamento específico por su ID  
**Ejemplo:** `GET /api/medicamentos/1`

#### Buscar medicamentos por descripción
```http
GET /api/medicamentos/buscar?descripcion={descripcion}
Authorization: Bearer {token}
```
**Descripción:** Busca medicamentos que contengan la descripción especificada  
**Ejemplos:**
- `GET /api/medicamentos/buscar?descripcion=paracetamol`
- `GET /api/medicamentos/buscar?descripcion=aspirina`

#### Obtener medicamentos por laboratorio
```http
GET /api/medicamentos/laboratorio/{laboratorioId}
Authorization: Bearer {token}
```
**Descripción:** Lista medicamentos de un laboratorio específico  
**Ejemplo:** `GET /api/medicamentos/laboratorio/1`

#### Obtener medicamentos por tipo
```http
GET /api/medicamentos/tipo/{tipoMedicamentoId}
Authorization: Bearer {token}
```
**Descripción:** Lista medicamentos de un tipo específico  
**Ejemplo:** `GET /api/medicamentos/tipo/1`

#### Crear medicamento
```http
POST /api/medicamentos
Authorization: Bearer {token}
Content-Type: application/json

{
  "codBarra": "123456789012",
  "descripcion": "Ibuprofeno 400mg",
  "requiereReceta": false,
  "ventaLibre": true,
  "precioUnitario": 180.75,
  "dosis": 400,
  "posologia": "Tomar 1 comprimido cada 6-8 horas con alimentos",
  "codLoteMedicamento": 1,
  "codLaboratorio": 2,
  "codTipoPresentacion": 1,
  "codUnidadMedida": 1,
  "codTipoMedicamento": 2
}
```
**Validaciones:**
- Descripción requerida (máx 200 caracteres)
- Precio unitario mayor a 0
- Código de barra único (si se proporciona)
- No puede requerir receta Y ser de venta libre simultáneamente
- Debe ser de venta libre O requerir receta
- Todos los códigos de referencia deben existir en BD

#### Actualizar medicamento
```http
PUT /api/medicamentos/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "codBarra": "123456789012",
  "descripcion": "Ibuprofeno 400mg - Actualizado",
  "requiereReceta": false,
  "ventaLibre": true,
  "precioUnitario": 195.00,
  "dosis": 400,
  "posologia": "Tomar 1 comprimido cada 6-8 horas con alimentos",
  "codLoteMedicamento": 1,
  "codLaboratorio": 2,
  "codTipoPresentacion": 1,
  "codUnidadMedida": 1,
  "codTipoMedicamento": 2
}
```

#### Eliminar medicamento
```http
DELETE /api/medicamentos/{id}
Authorization: Bearer {token}
```
**Restricciones:** No se puede eliminar si tiene registros asociados en facturas

---

### 🧾 **Facturas**

#### Obtener facturas del usuario
```http
GET /api/facturas/mis-facturas
Authorization: Bearer {token}
```
**Descripción:** Lista facturas de sucursales del usuario con detalles unificados  
**Respuesta:**
```json
[
  {
    "codFactura": 1,
    "nroFactura": "F001-00000001",
    "fechaHora": "2024-10-01T10:30:00",
    "total": 1650.00,
    "empleado": "Admin Administrador",
    "cliente": "Cliente1 Apellido1",
    "formaPago": "Efectivo",
    "detalles": [
      {
        "tipo": "Medicamento",
        "codigo": "7801111111111",
        "descripcion": "Medicamento1 Ejemplo 400mg",
        "precio": 150.00,
        "cantidad": 1,
        "subtotal": 150.00,
        "laboratorio": "Laboratorio1 Ejemplo",
        "requiereReceta": false
      },
      {
        "tipo": "Articulo", 
        "codigo": "7801111110001",
        "descripcion": "Producto1 Ejemplo 400ml",
        "precio": 350.00,
        "cantidad": 2,
        "subtotal": 700.00,
        "proveedor": "Proveedor1 Ejemplo SA"
      }
    ]
  }
]
```

#### Obtener factura con detalles por ID
```http
GET /api/facturas/{codFacturaVenta}/detalles
Authorization: Bearer {token}
```
**Descripción:** Obtiene una factura específica con todos sus detalles  
**Ejemplo:** `GET /api/facturas/1/detalles`

#### Crear nueva factura
```http
POST /api/facturas
Authorization: Bearer {token}
Content-Type: application/json

{
  "codEmpleado": 1,
  "codCliente": 1,
  "codSucursal": 1,
  "codFormaPago": 1,
  "total": 1500.00
}
```
**Validaciones:**
- Usuario debe tener acceso a la sucursal especificada
- Empleado, cliente, sucursal y forma de pago deben existir

#### Endpoints de Debug (desarrollo)
```http
GET /api/facturas/debug/medicamentos/{facturaId}
GET /api/facturas/debug/detalles/{facturaId}
```
**Descripción:** Endpoints para verificar medicamentos y detalles unificados de facturas

---

### 👤 **Usuarios**

#### Obtener usuario por username
```http
GET /api/usuarios/by-username/{username}
Authorization: Bearer {token}
```
**Descripción:** Obtiene información de un usuario por su username  
**Ejemplo:** `GET /api/usuarios/by-username/admin`

#### Crear usuario desde Auth-API
```http
POST /api/usuarios
Authorization: Bearer {token}
Content-Type: application/json

{
  "username": "nuevo_usuario",
  "email": "usuario@farmacia.com"
}
```
**Descripción:** Crear usuario en Pharm-API (usado por Auth-API)

#### Asignar sucursales a usuario
```http
POST /api/usuarios/{userId}/sucursales
Authorization: Bearer {token}
Content-Type: application/json

{
  "sucursales": [1, 2, 3]
}
```
**Descripción:** Asigna sucursales específicas a un usuario  
**Ejemplo:** `POST /api/usuarios/1/sucursales`

#### Obtener sucursales de usuario
```http
GET /api/usuarios/{userId}/sucursales
Authorization: Bearer {token}
```
**Descripción:** Lista las sucursales asignadas a un usuario específico  
**Ejemplo:** `GET /api/usuarios/1/sucursales`

#### Generar token para usuario
```http
GET /api/usuarios/generate-token/{username}
Authorization: Bearer {token}
```
**Descripción:** Genera un token JWT para un usuario (desarrollo)  
**Ejemplo:** `GET /api/usuarios/generate-token/admin`

#### Obtener todos los usuarios
```http
GET /api/usuarios
Authorization: Bearer {token}
```
**Descripción:** Lista todos los usuarios del sistema

---

## 🔐 Autenticación

### Flujo de Autenticación Cross-Service

1. **Login en Auth-API:**
```bash
POST http://localhost:5001/api/user/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password"
}
```

2. **Respuesta con Token:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": { "id": 1, "username": "admin" }
}
```

3. **Usar Token en Pharm-API:**
```bash
GET http://localhost:5002/api/empleados
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Sistema de Auto-creación de Usuarios

**Pharm-API** incluye un sistema automático que:
- 🔍 **Intercepta tokens** de Auth-API via middleware
- ➕ **Auto-crea usuarios** si no existen en Pharm-API
- 🏢 **Asigna sucursales por defecto** (1 y 2) automáticamente
- 🔄 **Sincroniza datos** del token (username, email) con datos reales

---

## 🎯 Usuarios de Ejemplo

### 🔑 **Admin**
- **Username:** `admin`
- **Acceso:** Todas las sucursales (1, 2, 3)
- **Uso:** Super usuario para testing completo

### 👤 **Usuario1**
- **Username:** `usuario1`  
- **Acceso:** Sucursales limitadas (1, 2)
- **Uso:** Usuario regular para testing de restricciones

### 🆕 **Usuarios Nuevos**
- **Creación:** Automática al hacer login en Auth-API
- **Acceso:** Sucursales por defecto (1, 2)
- **Personalización:** Se pueden agregar más sucursales manualmente

---

## 🗃️ Base de Datos

### Datos de Ejemplo Incluidos:
- ✅ **3 Sucursales** de ejemplo
- ✅ **3 Empleados** con diferentes roles
- ✅ **3 Clientes** con obras sociales
- ✅ **4 Obras Sociales** de ejemplo
- ✅ **3 Medicamentos** con diferentes propiedades
- ✅ **4 Artículos** de farmacia
- ✅ **Facturas de ejemplo** con detalles completos
- ✅ **Tipos de datos** (empleados, documentos, medicamentos, etc.)

### Triggers Automáticos:
- 🔧 **Auto-creación de usuarios** cuando se referencian
- 🏢 **Asignación automática de sucursales** por defecto
- 📝 **Logging** de operaciones automáticas

---

## 🚀 Ejecución

### Desarrollo
```bash
cd Pharm-api
docker-compose up -d
dotnet run
```

### Producción
```bash
docker-compose -f docker-compose.yml up -d
```

**URLs:**
- 🌐 API: `http://localhost:5002`
- 🔍 Health Check: `http://localhost:5002/api/ping`
- 🗄️ SQL Server: `localhost:1434`

---

## 🔧 Configuración

### Variables de Entorno
```bash
# Base de datos
ConnectionStrings__DefaultConnection="Server=localhost,1434;Database=PharmDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=true"

# JWT (debe coincidir con Auth-API)
Jwt__Key="your-super-secret-key-that-should-be-very-long-and-secure"
Jwt__Issuer="auth-api"
Jwt__Audience="pharm-api"
```

### Docker Compose
- 🗄️ **SQL Server:** Puerto 1434
- 🔄 **Auto-restart:** Habilitado
- 💾 **Persistencia:** Volumen Docker para datos
- 📝 **Inicialización:** Scripts automáticos

---

## 🧪 Testing

### Colección Postman
```json
{
  "info": { "name": "Pharm-API Tests" },
  "auth": {
    "type": "bearer",
    "bearer": [{ "key": "token", "value": "{{jwt_token}}" }]
  },
  "variable": [
    { "key": "base_url", "value": "http://localhost:5002" },
    { "key": "auth_url", "value": "http://localhost:5001" }
  ]
}
```

### Casos de Prueba Sugeridos:
1. ✅ **Login y obtención de token**
2. ✅ **CRUD completo de empleados**
3. ✅ **Validaciones de campos requeridos**
4. ✅ **Restricciones por sucursal**
5. ✅ **Auto-creación de usuarios nuevos**
6. ✅ **Consulta de facturas con detalles**

---

## 📊 Arquitectura

### Tecnologías:
- 🔧 **.NET 8** - Framework principal
- 🗄️ **Entity Framework Core** - ORM
- 🏗️ **SQL Server** - Base de datos
- 🐳 **Docker** - Containerización
- 🔐 **JWT** - Autenticación
- 📝 **Data Annotations** - Validaciones

### Patrones:
- 🏛️ **Repository Pattern** - Acceso a datos
- ⚙️ **Service Layer** - Lógica de negocio  
- 📦 **DTO Pattern** - Transferencia de datos
- 🔌 **Dependency Injection** - Inversión de control
- 🛡️ **Middleware** - Cross-cutting concerns

---

## 🤝 Integración con Auth-API

Este proyecto funciona en conjunto con **Auth-API** para proporcionar:
- 🔐 **Autenticación centralizada**
- 👥 **Gestión de usuarios**
- 🔄 **Sincronización automática**
- 🎯 **Single Sign-On (SSO)**

Ver documentación de Auth-API para detalles de configuración.
│   ├── 03-insert-initial-data.sql
│   └── init-db.sh
├── src/                        # Código fuente
│   ├── Controllers/            # Controladores API
│   ├── Models/                 # Entidades de BD
│   ├── DTOs/                   # Objetos de transferencia
│   ├── Repositories/           # Acceso a datos
│   ├── Services/               # Lógica de negocio
│   └── Data/                   # DbContext
├── docker-compose.yml          # Configuración Docker
├── Dockerfile                  # Imagen de la API
└── README.md
```

## 🛠️ Tecnologías

- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM
- **SQL Server 2022** - Base de datos
- **Docker & Docker Compose** - Contenedorización
- **JWT Authentication** - Seguridad
- **Swagger/OpenAPI** - Documentación

## 🐳 Instalación y Uso

### Prerrequisitos
- Docker y Docker Compose
- .NET 8 SDK (para desarrollo local)

### 1. Clonar el repositorio
```bash
git clone <repo-url>
cd Pharm-api
```

### 2. Levantar los servicios con Docker
```bash
# Levantar solo la base de datos
docker-compose up -d sqlserver

# Esperar a que se inicialice (30-60 segundos)
# Levantar toda la aplicación
docker-compose up -d
```

### 3. Verificar que está funcionando
```bash
# API disponible en:
http://localhost:5002/swagger

# Base de datos en:
Server: localhost,1433
Database: PharmDB
User: sa
Password: YourPassword123!
```

## 📚 Endpoints Disponibles

### 🔐 Autenticación
Los endpoints protegidos requieren JWT token del Auth-api:
```
Authorization: Bearer <jwt-token>
```

### 👥 Empleados
```
GET    /api/empleados                    # Obtener todos los empleados
GET    /api/empleados/{id}               # Obtener empleado por ID
POST   /api/empleados                    # Crear empleado
PUT    /api/empleados/{id}               # Actualizar empleado
DELETE /api/empleados/{id}               # Eliminar empleado
GET    /api/empleados/sucursal/{id}      # Empleados por sucursal
```

### 🧾 Facturas
```
GET    /api/facturas                     # Consultar facturas (con filtros)
GET    /api/facturas/{id}                # Obtener factura por ID
POST   /api/facturas                     # Crear nueva factura
```

**Filtros disponibles para GET /api/facturas:**
- `codSucursal` - Filtrar por sucursal
- `fechaDesde` - Fecha desde (YYYY-MM-DD)
- `fechaHasta` - Fecha hasta (YYYY-MM-DD)
- `codEmpleado` - Filtrar por empleado
- `codCliente` - Filtrar por cliente

### 👤 Usuarios (Integración con Auth-api)
```
POST   /api/usuarios                     # Crear usuario desde Auth-api
POST   /api/usuarios/{userId}/sucursales # Asignar sucursales
GET    /api/usuarios/{userId}/sucursales # Obtener sucursales del usuario
```

## 🔗 Integración con Auth-api

### Flujo de registro:
1. Usuario se registra en Auth-api
2. Auth-api llama a Pharm-api para crear el usuario
3. Se asignan sucursales al usuario

### Flujo de autenticación:
1. Usuario hace login en Auth-api
2. Recibe JWT token
3. Usa el token para acceder a Pharm-api
4. Pharm-api valida el token y verifica permisos

## 🗄️ Base de Datos

### Esquema principal:
- **Empleados** - Gestión de personal
- **Sucursales** - Múltiples ubicaciones
- **Facturas** - Ventas y transacciones
- **Usuarios/GrupSucursales** - Control de acceso
- **Clientes, Proveedores, Medicamentos** - Catálogos

### Datos de prueba incluidos:
- 3 usuarios de testing
- 2 sucursales de ejemplo
- Empleados, clientes y facturas de muestra
- Catálogos completos (tipos, formas de pago, etc.)

## 🛠️ Desarrollo

### Comandos útiles:
```bash
# Compilar el proyecto
dotnet build src/Pharm-api.csproj

# Ejecutar localmente
dotnet run --project src/Pharm-api.csproj

# Crear migración EF
dotnet ef migrations add <NombreMigracion> --project src/Pharm-api.csproj

# Aplicar migraciones
dotnet ef database update --project src/Pharm-api.csproj

# Ver logs de Docker
docker-compose logs -f

# Restart solo la API
docker-compose restart pharm-api
```

### Tasks de VS Code:
- `Ctrl+Shift+P` → "Tasks: Run Task"
- **build** - Compilar proyecto
- **run** - Ejecutar localmente
- **docker-up** - Levantar contenedores
- **docker-down** - Parar contenedores

## 🔧 Configuración

### Variables de entorno importantes:
```yaml
# docker-compose.yml
ASPNETCORE_ENVIRONMENT: Development
ConnectionStrings__DefaultConnection: "Server=sqlserver,1433;Database=PharmDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;"
```

### Configuración JWT (appsettings.json):
```json
{
  "Jwt": {
    "Secret": "YourSuperSecretKeyThatIsAtLeast32CharactersLong123456789",
    "Issuer": "PharmApi",
    "Audience": "PharmApi"
  }
}
```

## 📖 Ejemplos de Uso

### 🔗 URLs de Producción en Railway:
- **Auth-API:** `https://auth-api-production-1503.up.railway.app`
- **Pharm-API:** `https://pharm-api-production.up.railway.app`

### 🚀 Endpoints de Prueba Rápidos:

```bash
# 1. Health Check (sin autenticación)
GET https://pharm-api-production.up.railway.app/api/ping

# 2. Obtener todos los medicamentos ⭐ NUEVO
GET https://pharm-api-production.up.railway.app/api/medicamentos
Authorization: Bearer {tu_token}

# 3. Buscar medicamento específico ⭐ NUEVO  
GET https://pharm-api-production.up.railway.app/api/medicamentos/1
Authorization: Bearer {tu_token}

# 4. Buscar medicamentos por descripción ⭐ NUEVO
GET https://pharm-api-production.up.railway.app/api/medicamentos/buscar?descripcion=paracetamol
Authorization: Bearer {tu_token}

# 5. Obtener empleados
GET https://pharm-api-production.up.railway.app/api/empleados
Authorization: Bearer {tu_token}

# 6. Obtener facturas
GET https://pharm-api-production.up.railway.app/api/facturas/mis-facturas
Authorization: Bearer {tu_token}
```

### 📝 Ejemplos de Creación:

#### Crear medicamento ⭐ NUEVO:
```bash
POST https://pharm-api-production.up.railway.app/api/medicamentos
Authorization: Bearer {token}
Content-Type: application/json

{
  "descripcion": "Ibuprofeno 400mg",
  "requiereReceta": false,
  "ventaLibre": true,
  "precioUnitario": 180.75,
  "dosis": 400,
  "posologia": "Tomar 1 comprimido cada 6-8 horas con alimentos",
  "codLoteMedicamento": 1,
  "codLaboratorio": 1,
  "codTipoPresentacion": 1,
  "codUnidadMedida": 1,
  "codTipoMedicamento": 1
}
```

#### Crear empleado:
```bash
POST https://pharm-api-production.up.railway.app/api/empleados
Authorization: Bearer {token}
Content-Type: application/json

{
  "nomEmpleado": "Juan Carlos",
  "apeEmpleado": "Pérez García",
  "nroTel": "011-5555-5555",
  "email": "juan.perez@farmacia.com",
  "fechaIngreso": "2024-11-06T00:00:00",
  "codTipoEmpleado": 2,
  "codTipoDocumento": 1,
  "codSucursal": 1
}
```

#### Crear factura:
```bash
POST https://pharm-api-production.up.railway.app/api/facturas
Authorization: Bearer {token}
Content-Type: application/json

{
  "codEmpleado": 1,
  "codCliente": 1,
  "codSucursal": 1,
  "codFormaPago": 1,
  "total": 1500.00
}
```

### Consultar facturas con filtros:
```bash
GET /api/facturas?codSucursal=1&fechaDesde=2024-01-01&fechaHasta=2024-12-31
```

## 🚨 Solución de Problemas

### La API no se conecta a la BD:
1. Verificar que SQL Server esté corriendo: `docker ps`
2. Revisar logs: `docker-compose logs sqlserver`
3. Verificar connection string en appsettings.json

### Errores de JWT:
1. Verificar que Auth-api esté funcionando
2. Validar configuración JWT en ambas APIs
3. Verificar que el token no haya expirado

### La BD no tiene datos:
1. Verificar que se ejecutó el script de inicialización
2. Revisar logs: `docker-compose logs db-init`
3. Ejecutar manualmente: `docker-compose up db-init`

## 📄 Licencia

Este proyecto es para fines educativos - UTN Programación 2.

## 👥 Contribuir

1. Fork el proyecto
2. Crear feature branch
3. Commit cambios
4. Push al branch
5. Crear Pull Request

---

**🎯 Estado del proyecto:** ✅ Funcional - Listo para testing y desarrollo

**📞 Soporte:** Documentar issues en el repositorio de GitHub