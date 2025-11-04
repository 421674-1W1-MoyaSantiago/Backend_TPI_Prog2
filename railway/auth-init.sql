-- Script unificado de inicialización para Auth-API en SQL Server
-- Este script crea la base de datos y tablas necesarias

-- Crear base de datos AuthDB si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'AuthDB')
BEGIN
    CREATE DATABASE AuthDB;
    PRINT 'Base de datos AuthDB creada';
END
ELSE
BEGIN
    PRINT 'Base de datos AuthDB ya existe';
END
GO

USE AuthDB;
GO

-- Crear tabla Users si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Username nvarchar(50) NOT NULL,
        PasswordHash nvarchar(255) NOT NULL,
        Email nvarchar(100) NULL,
        CreatedAt datetime2 DEFAULT GETDATE(),
        UpdatedAt datetime2 DEFAULT GETDATE(),
        IsActive bit DEFAULT 1,
        DeletedAt datetime2 NULL
    );
    
    CREATE UNIQUE INDEX IX_Users_Username ON Users(Username);
    PRINT 'Tabla Users creada exitosamente';
END
GO

-- Crear tabla AuthLogs si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AuthLogs' AND xtype='U')
BEGIN
    CREATE TABLE AuthLogs (
        Id bigint IDENTITY(1,1) PRIMARY KEY,
        UserId int NULL,
        Action nvarchar(50) NOT NULL,
        IpAddress nvarchar(45) NULL,
        UserAgent nvarchar(500) NULL,
        Success bit NOT NULL DEFAULT 0,
        CreatedAt datetime2 DEFAULT GETDATE(),
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
    
    PRINT 'Tabla AuthLogs creada exitosamente';
END
GO

-- Insertar usuarios sincronizados con Pharm-API
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, PasswordHash, Email, IsActive)
    VALUES ('admin', '$2b$12$ygCuOb68C1Ong7aT1qzs6ecPqTcLf790sirIvQiYJZq.0Hkg6JT9G', 'admin@farmacia.ejemplo.com', 1);
    PRINT 'Usuario admin creado - Password: admin123';
END

IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'usuario1')
BEGIN
    INSERT INTO Users (Username, PasswordHash, Email, IsActive)
    VALUES ('usuario1', '$2b$12$nGzTZ3IUfMFNd9axOvm7peQIxWjoyip.IFZBV2b0y66hDVFdPCw4K', 'usuario1@farmacia.ejemplo.com', 1);
    PRINT 'Usuario usuario1 creado - Password: usuario123';
END

-- Log de inicialización
INSERT INTO AuthLogs (Action, Success, CreatedAt)
VALUES ('SYSTEM_INIT', 1, GETDATE());

PRINT 'Usuarios sincronizados con Pharm-API:';
PRINT '- admin (admin@farmacia.ejemplo.com) - Password: admin123';
PRINT '- usuario1 (usuario1@farmacia.ejemplo.com) - Password: usuario123';
GO

PRINT 'Inicialización de AuthDB completada';
GO