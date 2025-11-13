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
        -- Validaciones
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
            fv.fecha AS FechaVenta,
            c.nomCliente + SPACE(1) + c.apeCliente AS Cliente,
            m.descripcion AS Medicamento,
            dfvm.cantidad AS CantidadVendida,
            r.nomMedico + SPACE(1) + r.apeMedico AS Medico,
            r.matricula AS Matricula,
            ISNULL(os.razonSocial, 'Sin obra social') AS ObraSocial,            
            ISNULL(d.porcentaje_descuento, 0) AS MontoDescuento,
            ISNULL(sm.cantidad, 0) AS StockActual,
            
            -- Indicador de tipo de venta
            CASE 
                WHEN dfvm.codCobertura IS NOT NULL THEN 'Con Cobertura'
                ELSE 'Venta Libre'
            END AS TipoVenta
            
        FROM FacturasVenta fv
        INNER JOIN DetallesFacturaVentasMedicamento dfvm 
            ON fv.cod_FacturaVenta = dfvm.codFacturaVenta
        INNER JOIN Medicamentos m 
            ON dfvm.codMedicamento = m.cod_medicamento
        INNER JOIN Clientes c 
            ON fv.codCliente = c.cod_Cliente
            
        -- Cobertura es OPCIONAL (puede ser NULL para ventas libres)
        LEFT JOIN Coberturas cob 
            ON dfvm.codCobertura = cob.cod_Cobertura
        LEFT JOIN Obras_Sociales os 
            ON cob.cod_Obra_Social = os.cod_Obra_Social
        LEFT JOIN Descuentos d 
            ON cob.cod_descuento = d.cod_descuento
            
        -- Stock actual del medicamento en la sucursal
        LEFT JOIN Stock_Medicamentos sm 
            ON fv.codSucursal = sm.cod_Sucursal 
            AND m.cod_medicamento = sm.cod_Medicamento
            
        -- Receta relacionada con el medicamento vendido (si existe)
        LEFT JOIN Detalles_Receta dr 
            ON dr.codMedicamento = m.cod_medicamento
        LEFT JOIN Recetas r 
            ON r.cod_Receta = dr.cod_Receta 
            AND r.codCliente = c.cod_Cliente 
            AND r.fecha <= fv.fecha
            AND r.estado = 'Activa'
            -- Solo relacionar receta si la venta tiene cobertura
            AND (dfvm.codCobertura IS NULL OR r.codObraSocial = cob.cod_Obra_Social)
            
        WHERE fv.fecha BETWEEN @FechaInicio AND @FechaFin
          -- Filtro por obra social (si se especifica)
          AND (
                @ObraSocialNombre IS NULL  -- Todas las ventas
                OR os.razonSocial = @ObraSocialNombre  -- Obra social especifica
                OR (@ObraSocialNombre = 'SIN_OBRA_SOCIAL' AND dfvm.codCobertura IS NULL)  -- Solo ventas libres
          )
          
        ORDER BY fv.fecha, c.apeCliente, c.nomCliente
        
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
        @Estacion AS Estacion,
        m.descripcion AS MedicamentoMasVendido,
        CONCAT(e.nom_Empleado, ' ', e.ape_Empleado) AS Vendedor,
        SUM(d.cantidad) AS TotalVendido
    FROM FacturasVenta fv
    JOIN DetallesFacturaVentasMedicamento d ON fv.cod_FacturaVenta = d.codFacturaVenta
    JOIN Medicamentos m ON d.codMedicamento = m.cod_medicamento
    JOIN Empleados e ON fv.codEmpleado = e.cod_Empleado
    WHERE MONTH(fv.fecha) BETWEEN @MesInicio AND @MesFin
    GROUP BY m.descripcion, e.nom_Empleado, e.ape_Empleado
    ORDER BY TotalVendido DESC;
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
                    c.nomCliente + ' ' + c.apeCliente AS NombreCliente,
                    os.razonSocial AS ObraSocial
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
    ORDER BY ObraSocial
END
GO

------------

IF OBJECT_ID('dbo.SP_RECETAS_OBRA_SOCIAL_ESTADO','P') IS NOT NULL
    DROP PROCEDURE dbo.SP_RECETAS_OBRA_SOCIAL_ESTADO;
GO

CREATE PROCEDURE SP_RECETAS_OBRA_SOCIAL_ESTADO
@obra_social VARCHAR(100) = null,
@estado VARCHAR(30) = null
AS
BEGIN
    SELECT apeMedico + SPACE(1) + nomMedico as NombreMedico,
           matricula as NroMatricula,
           CONVERT(VARCHAR,r.fecha,103) as FechaReceta,
           diagnostico as Diagnostico,
           tr.tipo as TipoReceta,
           r.estado as EstadoReceta,
           c.apeCliente + SPACE(1) + c.nomCliente as NombreCliente,
           c.nroDoc as NroDocumentoCliente,
           os.razonSocial as ObraSocial,
           a.estado as EstadoAutorizacion
    FROM Recetas r
    JOIN Tipos_Receta tr ON r.codTipoReceta = tr.cod_Tipo_Receta
    JOIN Clientes c ON c.cod_Cliente = r.codCliente
    JOIN Autorizaciones a ON a.codReceta = r.cod_Receta
    JOIN Obras_Sociales os ON os.cod_Obra_Social = a.codObraSocial
    WHERE (@obra_social IS NULL OR os.razonSocial = @obra_social)
        AND (@estado IS NULL OR a.estado = @estado)
    ORDER BY NroDocumentoCliente
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
        -- Validaciones
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
            -- Datos del Reintegro
            rei.cod_Reintegros AS CodigoReintegro,
            rei.fechaEmision AS FechaEmision,
            rei.fechaReembolso AS FechaReembolso,
            rei.estado AS EstadoReintegro,
            DATEDIFF(DAY, rei.fechaEmision, ISNULL(rei.fechaReembolso, GETDATE())) AS DiasTranscurridos,

            -- Datos del Cliente y Obra Social
            os.razonSocial AS ObraSocial,
            c.nomCliente + ' ' + c.apeCliente AS Cliente,
            c.nroDoc AS DniCliente,
            
            -- Datos del Medicamento
            m.descripcion AS Medicamento,
            m.cod_barra AS CodigoDeBarras,
            dfvm.cantidad AS CantidadVendida,
            dfvm.precioUnitario AS PrecioUnitario,
            
            -- Cálculos de Descuentos
            ISNULL(d.porcentaje_descuento, 0) AS PorcentajeDescuento,
            ISNULL(td.descripcion, 'Sin Descuento') AS TipoDescuento,
            
            -- Montos Calculados
            (dfvm.cantidad * dfvm.precioUnitario) AS SubtotalVenta,
            (dfvm.cantidad * dfvm.precioUnitario) * (ISNULL(d.porcentaje_descuento, 0) / 100) AS MontoDescuento,
            (dfvm.cantidad * dfvm.precioUnitario) - 
                ((dfvm.cantidad * dfvm.precioUnitario) * (ISNULL(d.porcentaje_descuento, 0) / 100)) AS TotalAReintegrar,
            fv.cod_FacturaVenta AS NroFactura,
            fv.fecha AS FechaVenta,
            ISNULL(fp.metodo, 'No especificado') AS FormaDePago,
            s.nom_Sucursal AS Sucursal,
            e.nom_Empleado + SPACE(1) + e.ape_Empleado AS Empleado
            
        FROM Reintegros rei
        
        -- Relaciones principales (INNER JOIN - obligatorias)
        INNER JOIN Obras_Sociales os 
            ON rei.cod_ObraSocial = os.cod_Obra_Social
        INNER JOIN Coberturas cob 
            ON rei.cod_Cobertura = cob.cod_Cobertura
        INNER JOIN Clientes c 
            ON cob.cod_cliente = c.cod_Cliente
        INNER JOIN DetallesFacturaVentasMedicamento dfvm 
            ON rei.cod_DetFacVentaM = dfvm.cod_DetFacVentaM
        INNER JOIN Medicamentos m 
            ON dfvm.codMedicamento = m.cod_medicamento
        INNER JOIN FacturasVenta fv 
            ON dfvm.codFacturaVenta = fv.cod_FacturaVenta
        
        -- Relaciones opcionales (LEFT JOIN)
        LEFT JOIN Descuentos d 
            ON cob.cod_descuento = d.cod_descuento
        LEFT JOIN Tipos_Descuentos td 
            ON d.cod_tipo_descuento = td.cod_tipo_descuento
        LEFT JOIN Formas_Pago fp 
            ON fv.codFormaPago = fp.cod_Forma_Pago
        LEFT JOIN Sucursales s 
            ON fv.codSucursal = s.cod_Sucursal
        LEFT JOIN Empleados e 
            ON fv.codEmpleado = e.cod_Empleado
        LEFT JOIN Tipos_Empleados te
            ON e.codTipoEmpleado = te.cod_tipo_empleado
            
        
        WHERE CONVERT(DATE, rei.fechaEmision) BETWEEN @FechaInicio AND @FechaFin
            AND (@Estado IS NULL OR rei.estado = @Estado)
            AND (@ObraSocialNombre IS NULL OR os.razonSocial = @ObraSocialNombre)
    
        ORDER BY 
            rei.fechaEmision DESC,
            os.razonSocial ASC,
            c.apeCliente ASC,
            c.nomCliente ASC
            
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
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SET NOCOUNT ON;

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
        os.razonSocial AS ObraSocial,
        COUNT( fv.cod_FacturaVenta) AS CantidadVentas,
        COUNT( DISTINCT c.cod_Cliente) AS ClientesDistintos,
        SUM( ISNULL(dvm.cantidad * dvm.precioUnitario, 0)) AS TotalRecaudado,
        ROUND( AVG( ISNULL(dvm.cantidad * dvm.precioUnitario, 0)), 2) AS PromedioVenta,
        ROUND( SUM( ISNULL(dvm.cantidad * dvm.precioUnitario, 0)) * 100.0 / @TotalGeneral, 2) AS PorcentajeTotalRecaudado,
        ISNULL( COUNT( CASE WHEN r.cod_Reintegros IS NULL THEN 1 END), 0) AS ReintegrosPendientes
    FROM FacturasVenta fv
    JOIN Clientes c ON fv.codCliente = c.cod_Cliente
    JOIN Obras_Sociales os ON c.cod_Obra_Social = os.cod_Obra_Social
    LEFT JOIN DetallesFacturaVentasMedicamento dvm 
           ON fv.cod_FacturaVenta = dvm.codFacturaVenta
    LEFT JOIN Reintegros r 
           ON dvm.cod_DetFacVentaM = r.cod_DetFacVentaM
    WHERE fv.fecha BETWEEN @FechaInicio AND @FechaFin
    GROUP BY os.razonSocial
    ORDER BY TotalRecaudado DESC;
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