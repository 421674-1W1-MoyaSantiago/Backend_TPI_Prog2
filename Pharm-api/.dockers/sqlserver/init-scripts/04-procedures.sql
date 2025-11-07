IF OBJECT_ID('dbo.sp_Ventas_Medicamentos_ObraSocial','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Ventas_Medicamentos_ObraSocial;
GO

CREATE PROCEDURE sp_Ventas_Medicamentos_ObraSocial
    @ObraSocialNombre NVARCHAR(255) = NULL,
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    BEGIN TRY
        IF (@FechaInicio IS NULL OR @FechaFin IS NULL)
        BEGIN
        RAISERROR('Las fechas de inicio y fin son obligatorias.', 16, 1);
        RETURN;
    END
        IF (@FechaInicio > @FechaFin)
        BEGIN
        RAISERROR('La fecha de inicio no puede ser mayor que la fecha de fin.', 16, 1);
        RETURN;
    END

        SELECT
        fv.fecha AS 'Fecha Venta',
        c.nomCliente + SPACE(2) + c.apeCliente AS Cliente,
        m.descripcion AS Medicamento,
        dr.cantidad AS 'Cantidad Vendida',
        r.nomMedico + SPACE(2) + r.apeMedico AS Médico,
        r.matricula AS Matrícula,
        os.razonSocial AS 'Obra Social',
        ISNULL(d.porcentaje_descuento, 0) AS 'Monto Descuento',
        sm.cantidad AS StockActual
    FROM FacturasVenta fv
        LEFT JOIN DetallesFacturaVentasMedicamento dr ON fv.cod_FacturaVenta = dr.codFacturaVenta
        LEFT JOIN Medicamentos m ON dr.codMedicamento = m.cod_medicamento
        LEFT JOIN Clientes c ON fv.codCliente = c.cod_Cliente
        LEFT JOIN Coberturas cob ON dr.codCobertura = cob.cod_Cobertura
        LEFT JOIN Obras_Sociales os ON cob.cod_Obra_Social = os.cod_Obra_Social
        LEFT JOIN Descuentos d ON cob.cod_descuento = d.cod_descuento
        LEFT JOIN Recetas r ON r.codCliente = c.cod_Cliente AND r.codObraSocial = cob.cod_Obra_Social
        LEFT JOIN Stock_Medicamentos sm ON fv.codSucursal = sm.cod_Sucursal
            AND m.cod_medicamento = sm.cod_Medicamento
    WHERE fv.fecha BETWEEN @FechaInicio AND @FechaFin
        AND (
               @ObraSocialNombre IS NULL
        OR os.razonSocial = @ObraSocialNombre
          )
    ORDER BY fv.fecha, Cliente
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000), @ErrSeverity INT, @ErrState INT;
        SELECT @ErrMsg = ERROR_MESSAGE(),
        @ErrSeverity = ERROR_SEVERITY(),
        @ErrState = ERROR_STATE();
        RAISERROR(@ErrMsg, @ErrSeverity, @ErrState);
    END CATCH
END
GO
------------

IF OBJECT_ID('dbo.sp_TopMedicamentoYVendedorPorEstacion','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_TopMedicamentoYVendedorPorEstacion;
GO

CREATE PROCEDURE sp_TopMedicamentoYVendedorPorEstacion
    @Estacion VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MesInicio INT;
    DECLARE @MesFin INT;

    SET @MesInicio = CASE @Estacion
                        WHEN 'Verano' THEN 1
                        WHEN 'Otoño' THEN 4
                        WHEN 'Invierno' THEN 7
                        WHEN 'Primavera' THEN 10
                     END;

    SET @MesFin = CASE @Estacion
                        WHEN 'Verano' THEN 3
                        WHEN 'Otoño' THEN 6
                        WHEN 'Invierno' THEN 9
                        WHEN 'Primavera' THEN 12
                   END;
    IF @MesInicio IS NULL OR @MesFin IS NULL
    BEGIN
        RAISERROR('Estación no válida. Use: Verano, Otoño, Invierno o Primavera.', 16, 1);
        RETURN;
    END

    SELECT TOP 5
        @Estacion AS Estación,
        m.descripcion AS 'Medicamento más Vendido',
        CONCAT(e.nom_Empleado, ' ', e.ape_Empleado) AS Vendedor,
        SUM(d.cantidad) AS 'Total Vendido'
    FROM FacturasVenta fv
    JOIN DetallesFacturaVentasMedicamento d ON fv.cod_FacturaVenta = d.codFacturaVenta
    JOIN Medicamentos m ON d.codMedicamento = m.cod_medicamento
    JOIN Empleados e ON fv.codEmpleado = e.cod_Empleado
    WHERE MONTH(fv.fecha) BETWEEN @MesInicio AND @MesFin
    GROUP BY m.descripcion, e.nom_Empleado, e.ape_Empleado
    ORDER BY 'Total Vendido' DESC;
END;
GO

------------

IF OBJECT_ID('dbo.sp_ComprasSuministrosConAutorizacionObraSocial','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ComprasSuministrosConAutorizacionObraSocial;
GO

CREATE PROCEDURE sp_ComprasSuministrosConAutorizacionObraSocial
AS
BEGIN
    SELECT DISTINCT
                    c.nomCliente + ' ' + c.apeCliente AS 'Nombre del Cliente',
                    os.razonSocial AS  'Obra Social'
    FROM FacturasVenta fv
    JOIN Clientes c ON fv.codCliente = c.cod_Cliente
    JOIN DetallesFacturaVentasMedicamento dfvm ON fv.cod_FacturaVenta = dfvm.codFacturaVenta
    JOIN Medicamentos m ON dfvm.codMedicamento = m.cod_medicamento
    JOIN Coberturas cob ON dfvm.codCobertura = cob.cod_Cobertura
    JOIN Obras_Sociales os ON cob.cod_Obra_Social = os.cod_Obra_Social
    JOIN Autorizaciones a ON a.codObraSocial = os.cod_Obra_Social
    JOIN Recetas r ON a.codReceta = r.cod_Receta 
    AND r.codCliente = c.cod_Cliente
    WHERE m.requiere_receta = 1
        AND YEAR(fv.fecha) = YEAR(GETDATE())
        AND a.estado = 'AUTORIZADA'
    GROUP BY c.nomCliente, c.apeCliente, os.razonSocial
    ORDER BY 'Obra Social'
END
GO

------------

IF OBJECT_ID('dbo.SP_RECETAS_OBRA_SOCIAL_ESTADO','P') IS NOT NULL
    DROP PROCEDURE dbo.SP_RECETAS_OBRA_SOCIAL_ESTADO;
GO

CREATE PROCEDURE SP_RECETAS_OBRA_SOCIAL_ESTADO
@obra_social VARCHAR(100),
@estado VARCHAR(30)
AS
BEGIN
SELECT apeMedico + ', '+ nomMedico 'Nombre del médico',
       matricula 'Nro de Matricula',
       CONVERT(VARCHAR,r.fecha,103) 'Fecha de la receta',
       diagnostico 'Diagnostico',
       tr.tipo 'Tipo de Receta',
       r.estado 'Estado de la Receta',
       c.apeCliente + ', ' + c.nomCliente 'Nombre del cliente',
       c.nroDoc 'Nro de Documento del Cliente',
       os.razonSocial 'Obra Social',
       a.estado 'Estado de la Autorización'
FROM Recetas r
JOIN Tipos_Receta tr ON r.codTipoReceta = tr.cod_Tipo_Receta
JOIN Clientes c ON c.cod_Cliente = r.codCliente
JOIN Autorizaciones a ON a.codReceta = r.cod_Receta
JOIN Obras_Sociales os ON os.cod_Obra_Social = a.codObraSocial
WHERE os.razonSocial = @obra_social
AND a.estado = @estado
ORDER BY [Nro de Documento del Cliente]
END
GO
------------

IF OBJECT_ID('dbo.sp_Consultar_Reintegros_Obras_Sociales','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Consultar_Reintegros_Obras_Sociales;
GO

CREATE PROCEDURE sp_Consultar_Reintegros_Obras_Sociales
    @FechaInicio DATE,
    @FechaFin DATE,
    @Estado NVARCHAR(50) = NULL,
    @ObraSocialNombre NVARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        IF (@FechaInicio IS NULL OR @FechaFin IS NULL)
        BEGIN
            RAISERROR('Las fechas de inicio y fin son obligatorias.', 16, 1);
            RETURN;
        END

        IF (@FechaInicio > @FechaFin)
        BEGIN
            RAISERROR('La fecha de inicio no puede ser mayor que la fecha de fin.', 16, 1);
            RETURN;
        END

        SELECT 
            rei.fechaEmision AS 'Fecha Emisión',
            rei.fechaReembolso AS 'Fecha Reembolso',
            rei.estado AS 'Estado Reintegro',
            DATEDIFF(DAY, rei.fechaEmision, ISNULL(rei.fechaReembolso, GETDATE())) AS 'Días Pendiente',
            os.razonSocial AS 'Obra Social',
            c.nomCliente + SPACE(2) + c.apeCliente AS 'Cliente',
            m.descripcion AS 'Medicamento',
            ISNULL(d.porcentaje_descuento, 0) AS 'Porcentaje Descuento',
            (dfvm.cantidad * dfvm.precioUnitario) * (ISNULL(d.porcentaje_descuento, 0) / 100) AS 'Monto Descuento',
            td.descripcion AS 'Tipo Descuento',
            fv.fecha AS 'Fecha Venta',
            fv.cod_FacturaVenta AS 'Nro Factura',
            fp.metodo AS 'Forma de Pago',
            s.nom_Sucursal AS 'Sucursal',
            s.calle + SPACE(2) + CAST(s.altura AS VARCHAR) AS 'Dirección Sucursal',
            e.nom_Empleado + SPACE(2) + e.ape_Empleado AS 'Empleado',
            l.nom_Localidad AS 'Localidad',
            p.nom_Provincia AS 'Provincia'
            
        FROM Reintegros rei
        JOIN Obras_Sociales os ON rei.cod_ObraSocial = os.cod_Obra_Social
        JOIN Coberturas cob ON rei.cod_Cobertura = cob.cod_Cobertura
        JOIN Clientes c ON cob.cod_cliente = c.cod_Cliente
        JOIN DetallesFacturaVentasMedicamento dfvm ON rei.cod_DetFacVentaM = dfvm.cod_DetFacVentaM
        JOIN Medicamentos m ON dfvm.codMedicamento = m.cod_medicamento
        JOIN FacturasVenta fv ON dfvm.codFacturaVenta = fv.cod_FacturaVenta
        
        LEFT JOIN Descuentos d ON cob.cod_descuento = d.cod_descuento
        LEFT JOIN Tipos_Descuentos td ON d.cod_tipo_descuento = td.cod_tipo_descuento
        LEFT JOIN Formas_Pago fp ON fv.codFormaPago = fp.cod_Forma_Pago
        LEFT JOIN Sucursales s ON fv.codSucursal = s.cod_Sucursal
        LEFT JOIN Empleados e ON fv.codEmpleado = e.cod_Empleado
        LEFT JOIN Localidades l ON cob.cod_Localidad = l.cod_Localidad
        LEFT JOIN Provincias p ON l.cod_Provincia = p.cod_Provincia
        WHERE rei.fechaEmision BETWEEN @FechaInicio AND @FechaFin
            AND (@Estado IS NULL OR rei.estado = @Estado)
            AND (@ObraSocialNombre IS NULL OR os.razonSocial = @ObraSocialNombre)
    
        ORDER BY 
            rei.fechaEmision DESC,
            os.razonSocial,
            c.apeCliente,
            c.nomCliente
    END TRY

    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000), 
                @ErrSeverity INT, 
                @ErrState INT;
        
        SELECT @ErrMsg = ERROR_MESSAGE(),
               @ErrSeverity = ERROR_SEVERITY(),
               @ErrState = ERROR_STATE();
        
        RAISERROR(@ErrMsg, @ErrSeverity, @ErrState);
    END CATCH
END
GO

------------

IF OBJECT_ID('dbo.sp_ReporteVentasPorObraSocial','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ReporteVentasPorObraSocial;
GO

CREATE PROCEDURE sp_ReporteVentasPorObraSocial
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaFin IS NULL
        SET @FechaFin = GETDATE();

    IF @FechaInicio IS NULL
        SET @FechaInicio = DATEADD(MONTH, -6, @FechaFin);

    DECLARE @TotalGeneral DECIMAL(18,2);

    SELECT @TotalGeneral = SUM(TotalMedicamentos)
    FROM (
        SELECT fv.cod_FacturaVenta, ISNULL(SUM(dvm.cantidad * dvm.precioUnitario), 0) AS TotalMedicamentos
        FROM FacturasVenta fv
        LEFT JOIN DetallesFacturaVentasMedicamento dvm 
               ON fv.cod_FacturaVenta = dvm.codFacturaVenta
        WHERE fv.fecha BETWEEN @FechaInicio AND @FechaFin
        GROUP BY fv.cod_FacturaVenta
    ) AS TotalGeneral;

    SELECT 
        os.razonSocial AS 'Obra Social',
        COUNT( fv.cod_FacturaVenta) AS 'Cantidad de Ventas',
        COUNT( DISTINCT c.cod_Cliente) AS 'Clientes Distintos',
        SUM( ISNULL(dvm.cantidad * dvm.precioUnitario, 0)) AS 'Total Recaudado',
        ROUND( AVG( ISNULL(dvm.cantidad * dvm.precioUnitario, 0)), 2) AS 'Promedio por Venta',
        CAST( ROUND( SUM( ISNULL(dvm.cantidad * dvm.precioUnitario, 0)) * 100.0 / @TotalGeneral, 2) AS VARCHAR(10)) + ' %' AS 'Porcentaje del Total Recaudado',
        ISNULL( COUNT( CASE WHEN r.cod_Reintegros IS NULL THEN 1 END), 0) AS 'Reintegros Pendientes'
    FROM FacturasVenta fv
    JOIN Clientes c ON fv.codCliente = c.cod_Cliente
    JOIN Obras_Sociales os ON c.cod_Obra_Social = os.cod_Obra_Social
    LEFT JOIN DetallesFacturaVentasMedicamento dvm 
           ON fv.cod_FacturaVenta = dvm.codFacturaVenta
    LEFT JOIN Reintegros r 
           ON dvm.cod_DetFacVentaM = r.cod_DetFacVentaM
    WHERE fv.fecha BETWEEN @FechaInicio AND @FechaFin
    GROUP BY os.razonSocial
    ORDER BY 'Total Recaudado' DESC;
END;
GO

------------

IF OBJECT_ID('dbo.sp_PorcentajeVentasXObraSocial','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_PorcentajeVentasXObraSocial;
GO

CREATE PROCEDURE sp_PorcentajeVentasXObraSocial
AS
BEGIN
    SELECT 
        OS.razonSocial as NomObraSocial,
        sum(FV.total) / (SELECT sum(total) FROM FacturasVenta FV WHERE year(fecha) = year(getdate())) as Porcentaje 
    FROM FacturasVenta FV
    JOIN Clientes C ON C.cod_Cliente = FV.codCliente
    JOIN Obras_Sociales OS ON OS.cod_Obra_Social = C.cod_Obra_Social
    WHERE year(FV.fecha) = year(getdate())
    GROUP BY OS.razonSocial
    ORDER BY Porcentaje DESC
END
GO

------------

IF OBJECT_ID('dbo.sp_IngresosPorMesAnioActual','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_IngresosPorMesAnioActual;
GO

CREATE PROCEDURE sp_IngresosPorMesAnioActual
AS
BEGIN
    SELECT 
        month(fecha) as NroMes,
        sum(total) as TotalMes
    FROM FacturasVenta FV
    WHERE YEAR(fecha) = YEAR(GETDATE())
    GROUP BY month(fecha)
END
GO