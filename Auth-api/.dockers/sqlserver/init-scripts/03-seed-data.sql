-- Script para insertar datos iniciales
USE AuthDB;
GO

-- *** USUARIOS SINCRONIZADOS CON PHARM-API ***
-- Estos usuarios deben coincidir exactamente con los de Pharm-API
-- Passwords hasheadas con BCrypt (12 rounds):
-- admin: "admin123" 
-- usuario1: "usuario123"

-- Usuario Admin
INSERT INTO Users (Username, PasswordHash, Email, IsActive, CreatedAt)
VALUES ('admin', '$2b$12$ygCuOb68C1Ong7aT1qzs6ecPqTcLf790sirIvQiYJZq.0Hkg6JT9G', 'admin@farmacia.ejemplo.com', 1, GETDATE());

-- Usuario de Ejemplo
INSERT INTO Users (Username, PasswordHash, Email, IsActive, CreatedAt)
VALUES ('usuario1', '$2b$12$nGzTZ3IUfMFNd9axOvm7peQIxWjoyip.IFZBV2b0y66hDVFdPCw4K', 'usuario1@farmacia.ejemplo.com', 1, GETDATE());

PRINT 'Usuarios sincronizados con Pharm-API creados:';
PRINT '- admin (admin@farmacia.ejemplo.com) - Password: admin123';
PRINT '- usuario1 (usuario1@farmacia.ejemplo.com) - Password: usuario123';

-- Insertar log inicial del sistema
INSERT INTO AuthLogs (Action, Success, CreatedAt)
VALUES ('SYSTEM_INIT', 1, GETDATE());

-- Log de creación de usuarios
INSERT INTO AuthLogs (UserId, Action, Success, CreatedAt)
SELECT Id, 'USER_CREATED', 1, GETDATE() FROM Users WHERE Username IN ('admin', 'usuario1');

PRINT 'Sistema inicializado correctamente - Base de datos lista para usar';
PRINT 'Usuarios sincronizados entre Auth-API y Pharm-API';
GO