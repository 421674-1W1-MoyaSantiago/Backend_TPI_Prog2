-- =============================================
-- Stored Procedure: sp_UpdateUserFromToken
-- Descripción: Crea usuario y asigna las 3 sucursales automáticamente
-- =============================================
USE PharmDB;
GO

CREATE OR ALTER PROCEDURE sp_UpdateUserFromToken
    @UserId INT,
    @Username NVARCHAR(255),
    @Email NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verificar si el usuario ya existe
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Cod_Usuario = @UserId)
        BEGIN
            -- Crear el usuario
            INSERT INTO Usuarios (Cod_Usuario, Username, Email)
            VALUES (@UserId, @Username, @Email);
            
            PRINT 'Usuario creado: ' + @Username;
        END
        ELSE
        BEGIN
            -- Actualizar datos del usuario existente
            UPDATE Usuarios 
            SET Username = @Username, Email = @Email
            WHERE Cod_Usuario = @UserId;
            
            PRINT 'Usuario actualizado: ' + @Username;
        END
        
        -- Asignar las 3 sucursales automáticamente
        -- Verificar y crear asignaciones que no existan
        
        -- Sucursal 1 (Buenos Aires)
        IF NOT EXISTS (SELECT 1 FROM Empleado_Sucursal WHERE Cod_Usuario = @UserId AND Cod_Sucursal = 1)
        BEGIN
            INSERT INTO Empleado_Sucursal (Cod_Usuario, Cod_Sucursal)
            VALUES (@UserId, 1);
            PRINT 'Asignada sucursal 1 (Buenos Aires)';
        END
        
        -- Sucursal 2 (Mendoza)
        IF NOT EXISTS (SELECT 1 FROM Empleado_Sucursal WHERE Cod_Usuario = @UserId AND Cod_Sucursal = 2)
        BEGIN
            INSERT INTO Empleado_Sucursal (Cod_Usuario, Cod_Sucursal)
            VALUES (@UserId, 2);
            PRINT 'Asignada sucursal 2 (Mendoza)';
        END
        
        -- Sucursal 3 (Córdoba)
        IF NOT EXISTS (SELECT 1 FROM Empleado_Sucursal WHERE Cod_Usuario = @UserId AND Cod_Sucursal = 3)
        BEGIN
            INSERT INTO Empleado_Sucursal (Cod_Usuario, Cod_Sucursal)
            VALUES (@UserId, 3);
            PRINT 'Asignada sucursal 3 (Córdoba)';
        END
        
        COMMIT TRANSACTION;
        
        PRINT 'Usuario ' + @Username + ' configurado exitosamente con 3 sucursales';
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        PRINT 'Error en sp_UpdateUserFromToken: ' + @ErrorMessage;
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

PRINT 'Stored procedure sp_UpdateUserFromToken creado exitosamente';