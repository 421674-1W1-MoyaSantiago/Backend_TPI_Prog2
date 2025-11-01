-- Script para crear la base de datos PharmDB
USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'PharmDB')
BEGIN
    CREATE DATABASE PharmDB;
    PRINT 'Base de datos PharmDB creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Base de datos PharmDB ya existe';
END
GO