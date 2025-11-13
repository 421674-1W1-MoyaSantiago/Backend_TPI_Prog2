-- Procedimiento almacenado para recalcular y corregir el campo `total` de FacturasVenta
USE PharmDB;
GO

IF OBJECT_ID('dbo.usp_CorrectInvoiceTotals','P') IS NOT NULL
    DROP PROCEDURE dbo.usp_CorrectInvoiceTotals;
GO

CREATE PROCEDURE dbo.usp_CorrectInvoiceTotals
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @cod INT;
    DECLARE @total MONEY;

    -- Cursor para recorrer todas las facturas
    DECLARE factura_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT cod_FacturaVenta FROM FacturasVenta;

    OPEN factura_cursor;
    FETCH NEXT FROM factura_cursor INTO @cod;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Calcular total sumando detalles de medicamentos y articulos (precioUnitario * cantidad)
        SELECT @total =
            ISNULL((SELECT SUM(CONVERT(MONEY, precioUnitario * cantidad))
                    FROM DetallesFacturaVentasMedicamento
                    WHERE codFacturaVenta = @cod), 0)
            + ISNULL((SELECT SUM(CONVERT(MONEY, precioUnitario * cantidad))
                      FROM DetallesFacturaVentasArticulo
                      WHERE codFacturaVenta = @cod), 0);

        -- Actualizar el total en la factura
        UPDATE FacturasVenta
        SET total = @total
        WHERE cod_FacturaVenta = @cod;

        FETCH NEXT FROM factura_cursor INTO @cod;
    END

    CLOSE factura_cursor;
    DEALLOCATE factura_cursor;
    PRINT 'Totales de facturas corregidos exitosamente.';
END;
GO

EXEC dbo.usp_CorrectInvoiceTotals;

GO
