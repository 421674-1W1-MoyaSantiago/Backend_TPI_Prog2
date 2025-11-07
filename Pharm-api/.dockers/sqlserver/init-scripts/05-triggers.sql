-- Trigger para auto-crear usuarios cuando se necesiten en operaciones
-- Este trigger se ejecuta en varias tablas que referencian Usuario

USE PharmDB;
GO

-- Trigger para asignar automáticamente la nueva sucursal al usuario admin
-- Reemplaza '1' por el CodUsuario real del admin si es diferente
CREATE OR ALTER TRIGGER trg_AddAdminToNewSucursal
ON Sucursales
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @adminCodUsuario INT = 1; -- Cambia este valor si el admin tiene otro CodUsuario
    INSERT INTO Grupsucursales (cod_sucursal, cod_usuario, activo)
    SELECT i.cod_sucursal, @adminCodUsuario, 1
    FROM inserted i
    WHERE NOT EXISTS (
        SELECT 1 FROM Grupsucursales gs
        WHERE gs.cod_sucursal = i.cod_sucursal AND gs.cod_usuario = @adminCodUsuario
    );
    PRINT 'Admin asignado automáticamente a la nueva sucursal';
END;
GO

