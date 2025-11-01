-- Trigger para auto-crear usuarios cuando se necesiten en operaciones
-- Este trigger se ejecuta en varias tablas que referencian Usuario

USE PharmDB;
GO

-- Trigger principal para auto-crear usuarios
CREATE OR ALTER TRIGGER trg_AutoCreateUser_Grupsucursales
ON Grupsucursales
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @codUsuario INT, @codSucursal INT, @activo BIT;
    DECLARE @newUsersCreated TABLE (cod_usuario INT);
    
    -- Crear usuarios que no existen y registrarlos
    INSERT INTO Usuario (CodUsuario, Username, Email)
    OUTPUT inserted.CodUsuario INTO @newUsersCreated
    SELECT DISTINCT 
        i.cod_usuario,
        'user_' + CAST(i.cod_usuario AS NVARCHAR(50)),
        'user_' + CAST(i.cod_usuario AS NVARCHAR(50)) + '@pharm.temp'
    FROM inserted i
    WHERE NOT EXISTS (SELECT 1 FROM Usuario WHERE CodUsuario = i.cod_usuario);
    
    -- Log de usuarios creados
    IF @@ROWCOUNT > 0
    BEGIN
        PRINT 'Usuarios creados automáticamente: ' + CAST(@@ROWCOUNT AS NVARCHAR(10));
        
        -- Para cada usuario nuevo creado, asignar sucursales por defecto
        INSERT INTO Grupsucursales (cod_sucursal, cod_usuario, activo)
        SELECT ds.cod_sucursal, nu.cod_usuario, 1
        FROM @newUsersCreated nu
        CROSS JOIN (VALUES (1), (2)) AS ds(cod_sucursal)
        WHERE EXISTS (SELECT 1 FROM Sucursales WHERE cod_Sucursal = ds.cod_sucursal)
          AND NOT EXISTS (
              SELECT 1 FROM inserted 
              WHERE cod_usuario = nu.cod_usuario AND cod_sucursal = ds.cod_sucursal
          );
        
        PRINT 'Sucursales por defecto asignadas a usuarios nuevos (Sucursales 1 y 2)';
    END
    
    -- Realizar la operación original
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- Es un UPDATE
        UPDATE g SET 
            cod_sucursal = i.cod_sucursal,
            cod_usuario = i.cod_usuario,
            activo = i.activo
        FROM Grupsucursales g
        INNER JOIN inserted i ON g.cod_sucursal = i.cod_sucursal AND g.cod_usuario = i.cod_usuario;
    END
    ELSE
    BEGIN
        -- Es un INSERT
        INSERT INTO Grupsucursales (cod_sucursal, cod_usuario, activo)
        SELECT cod_sucursal, cod_usuario, activo 
        FROM inserted;
    END
END;
GO

-- Trigger para FacturasVenta (empleados)
CREATE OR ALTER TRIGGER trg_AutoCreateUser_FacturasVenta
ON FacturasVenta
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Auto-crear usuarios que no existen basado en empleados
    INSERT INTO Usuario (CodUsuario, Username, Email)
    SELECT DISTINCT 
        emp.CodEmpleado as CodUsuario,
        'emp_' + CAST(emp.CodEmpleado AS NVARCHAR(50)),
        'empleado_' + CAST(emp.CodEmpleado AS NVARCHAR(50)) + '@pharm.temp'
    FROM inserted i
    INNER JOIN Empleados emp ON i.codEmpleado = emp.CodEmpleado
    WHERE NOT EXISTS (SELECT 1 FROM Usuario WHERE CodUsuario = emp.CodEmpleado);
    
    -- Realizar la operación original
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- Es un UPDATE
        UPDATE f SET 
            nroFactura = i.nroFactura,
            fechaHora = i.fechaHora,
            codEmpleado = i.codEmpleado,
            codCliente = i.codCliente,
            codFormaPago = i.codFormaPago,
            total = i.total
        FROM FacturasVenta f
        INNER JOIN inserted i ON f.codFactura = i.codFactura;
    END
    ELSE
    BEGIN
        -- Es un INSERT
        INSERT INTO FacturasVenta (nroFactura, fechaHora, codEmpleado, codCliente, codFormaPago, total)
        SELECT nroFactura, fechaHora, codEmpleado, codCliente, codFormaPago, total
        FROM inserted;
    END
END;
GO

-- Procedimiento para actualizar datos de usuario desde el token
CREATE OR ALTER PROCEDURE sp_UpdateUserFromToken
    @UserId INT,
    @Username NVARCHAR(100),
    @Email NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @UserCreated BIT = 0;
    
    -- Verificar si el usuario existe
    IF EXISTS (SELECT 1 FROM Usuario WHERE CodUsuario = @UserId)
    BEGIN
        -- Actualizar solo si los datos son diferentes y no son temporales
        UPDATE Usuario 
        SET 
            Username = CASE 
                WHEN Username LIKE 'user_%' OR Username LIKE 'emp_%' THEN @Username 
                ELSE Username 
            END,
            Email = CASE 
                WHEN Email LIKE '%@pharm.temp' THEN @Email 
                ELSE Email 
            END
        WHERE CodUsuario = @UserId;
        
        PRINT 'Usuario actualizado: ' + @Username;
    END
    ELSE
    BEGIN
        -- Crear usuario si no existe
        INSERT INTO Usuario (CodUsuario, Username, Email)
        VALUES (@UserId, @Username, @Email);
        
        SET @UserCreated = 1;
        PRINT 'Usuario creado: ' + @Username;
    END
    
    -- Si se creó un usuario nuevo, asignar sucursales por defecto
    IF @UserCreated = 1
    BEGIN
        -- Asignar sucursales por defecto (1 y 2) al nuevo usuario
        INSERT INTO Grupsucursales (cod_sucursal, cod_usuario, activo)
        SELECT cod_sucursal, @UserId, 1
        FROM (VALUES (1), (2)) AS DefaultSucursales(cod_sucursal)
        WHERE EXISTS (SELECT 1 FROM Sucursales WHERE cod_Sucursal = DefaultSucursales.cod_sucursal)
          AND NOT EXISTS (SELECT 1 FROM Grupsucursales WHERE cod_usuario = @UserId AND cod_sucursal = DefaultSucursales.cod_sucursal);
        
        PRINT 'Sucursales por defecto asignadas al usuario: ' + @Username + ' (Sucursales 1 y 2)';
    END
END;
GO

-- Procedimiento para asignar sucursales por defecto a un usuario existente
CREATE OR ALTER PROCEDURE sp_AssignDefaultSucursales
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar que el usuario existe
    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE CodUsuario = @UserId)
    BEGIN
        RAISERROR('Usuario no existe', 16, 1);
        RETURN;
    END
    
    -- Asignar sucursales por defecto (1 y 2) si no las tiene ya
    INSERT INTO Grupsucursales (cod_sucursal, cod_usuario, activo)
    SELECT cod_sucursal, @UserId, 1
    FROM (VALUES (1), (2)) AS DefaultSucursales(cod_sucursal)
    WHERE EXISTS (SELECT 1 FROM Sucursales WHERE cod_Sucursal = DefaultSucursales.cod_sucursal)
      AND NOT EXISTS (SELECT 1 FROM Grupsucursales WHERE cod_usuario = @UserId AND cod_sucursal = DefaultSucursales.cod_sucursal);
    
    DECLARE @assignedCount INT = @@ROWCOUNT;
    
    IF @assignedCount > 0
        PRINT 'Sucursales por defecto asignadas: ' + CAST(@assignedCount AS NVARCHAR(10));
    ELSE
        PRINT 'Usuario ya tiene asignadas las sucursales por defecto';
END;
GO

-- Procedimiento para configurar qué sucursales son las "por defecto"
CREATE OR ALTER PROCEDURE sp_ConfigureDefaultSucursales
    @SucursalIds NVARCHAR(100) = '1,2' -- IDs separados por coma, por defecto 1,2
AS
BEGIN
    SET NOCOUNT ON;
    
    PRINT 'Sucursales por defecto configuradas: ' + @SucursalIds;
    PRINT 'Nota: Esta configuración se debe aplicar manualmente en los triggers/procedures';
    PRINT 'Para cambiar las sucursales por defecto, modifica los triggers correspondientes';
END;
GO