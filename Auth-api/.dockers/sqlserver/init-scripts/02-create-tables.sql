-- Script para crear las tablas necesarias
USE AuthDB;
GO

-- Crear tabla Users
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
    
    -- Crear índice único para Username
    CREATE UNIQUE INDEX IX_Users_Username ON Users(Username);
    
    PRINT 'Tabla Users creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla Users ya existe';
END
GO

-- Crear tabla para logs de autenticación (opcional)
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
ELSE
BEGIN
    PRINT 'Tabla AuthLogs ya existe';
END
GO