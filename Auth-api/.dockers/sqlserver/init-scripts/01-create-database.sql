-- Script para crear las bases de datos AuthDB y PharmDB
USE master;
GO

-- Crear base de datos AuthDB si no existe
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'AuthDB')
BEGIN
    CREATE DATABASE AuthDB;
    PRINT 'Base de datos AuthDB creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Base de datos AuthDB ya existe';
END

-- Crear base de datos PharmDB si no existe
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'PharmDB')
BEGIN
    CREATE DATABASE PharmDB;
    PRINT 'Base de datos PharmDB creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Base de datos PharmDB ya existe';
END

PRINT 'Bases de datos listas para Auth-API y Pharm-API';