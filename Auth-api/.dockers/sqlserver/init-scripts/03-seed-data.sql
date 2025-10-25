-- Script para insertar datos iniciales
USE AuthDB;
GO

-- Insertar log inicial del sistema
INSERT INTO AuthLogs (Action, Success, CreatedAt)
VALUES ('SYSTEM_INIT', 1, GETDATE());

PRINT 'Sistema inicializado correctamente - Base de datos lista para usar';
PRINT 'Para crear usuarios utiliza el endpoint POST /api/users/register';
GO