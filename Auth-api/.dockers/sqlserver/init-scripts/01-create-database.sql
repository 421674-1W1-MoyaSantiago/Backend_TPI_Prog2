-- Script para crear la base de datos AuthDB
USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'AuthDB')
BEGIN
    CREATE DATABASE AuthDB;
    PRINT 'Base de datos AuthDB creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Base de datos AuthDB ya existe';
END
GO