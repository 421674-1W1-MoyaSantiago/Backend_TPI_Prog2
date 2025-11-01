-- ======================================
-- SCRIPTS DE TESTING PARA NUEVAS TABLAS
-- ======================================

-- 1. VERIFICAR QUE TODAS LAS TABLAS SE CREARON
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
ORDER BY TABLE_NAME;

-- 2. VERIFICAR DATOS BÁSICOS DE CATÁLOGOS
SELECT 'Tipos_Empleados' as Tabla, COUNT(*) as Registros FROM Tipos_Empleados
UNION ALL
SELECT 'Provincias', COUNT(*) FROM Provincias
UNION ALL
SELECT 'Localidades', COUNT(*) FROM Localidades
UNION ALL
SELECT 'Formas_Pago', COUNT(*) FROM Formas_Pago
UNION ALL
SELECT 'Sucursales', COUNT(*) FROM Sucursales
UNION ALL
SELECT 'Empleados', COUNT(*) FROM Empleados
UNION ALL
SELECT 'Medicamentos', COUNT(*) FROM Medicamentos
UNION ALL
SELECT 'Articulos', COUNT(*) FROM Articulos
UNION ALL
SELECT 'FacturasVenta', COUNT(*) FROM FacturasVenta;

-- 3. TESTEAR EMPLEADOS CON INFORMACIÓN COMPLETA
SELECT 
    e.cod_Empleado,
    e.nom_Empleado + ' ' + e.ape_Empleado as NombreCompleto,
    te.tipo as TipoEmpleado,
    s.nom_Sucursal as Sucursal,
    e.fechaIngreso,
    e.email
FROM Empleados e
    INNER JOIN Tipos_Empleados te ON e.codTipoEmpleado = te.cod_tipo_empleado
    INNER JOIN Sucursales s ON e.codSucursal = s.cod_Sucursal
ORDER BY e.nom_Empleado;

-- 4. TESTEAR MEDICAMENTOS CON DETALLES
SELECT 
    m.cod_medicamento,
    m.descripcion,
    m.precio_unitario,
    lab.descripcion as Laboratorio,
    tp.descripcion as Presentacion,
    tm.descripcion as TipoMedicamento,
    CASE WHEN m.requiere_receta = 1 THEN 'Sí' ELSE 'No' END as RequiereReceta
FROM Medicamentos m
    INNER JOIN Laboratorios lab ON m.codLaboratorio = lab.cod_Laboratorio
    INNER JOIN Tipos_Presentacion tp ON m.cod_tipo_presentacion = tp.cod_Tipo_Presentacion
    INNER JOIN Tipos_Medicamento tm ON m.cod_tipo_medicamento = tm.cod_tipo_medicamento
ORDER BY m.descripcion;

-- 5. TESTEAR ARTÍCULOS CON CATEGORÍAS
SELECT 
    a.cod_Articulo,
    a.descripcion,
    a.precioUnitario,
    ca.categoria,
    p.razon_Social as Proveedor
FROM Articulos a
    INNER JOIN Categorias_Articulos ca ON a.cod_Categoria_Articulo = ca.cod_Categoria_Articulo
    INNER JOIN Proveedores p ON a.cod_Proveedor = p.cod_Proveedor
ORDER BY a.descripcion;

-- 6. TESTEAR FACTURAS CON DETALLES COMPLETOS
SELECT 
    fv.cod_FacturaVenta,
    fv.fecha,
    e.nom_Empleado + ' ' + e.ape_Empleado as Empleado,
    c.nomCliente + ' ' + c.apeCliente as Cliente,
    s.nom_Sucursal as Sucursal,
    fp.metodo as FormaPago,
    fv.total
FROM FacturasVenta fv
    INNER JOIN Empleados e ON fv.codEmpleado = e.cod_Empleado
    INNER JOIN Clientes c ON fv.codCliente = c.cod_Cliente
    INNER JOIN Sucursales s ON fv.codSucursal = s.cod_Sucursal
    INNER JOIN Formas_Pago fp ON fv.codFormaPago = fp.cod_Forma_Pago
ORDER BY fv.fecha DESC;

-- 7. TESTEAR DETALLES DE FACTURAS - MEDICAMENTOS
SELECT 
    dfm.cod_DetFacVentaM,
    fv.cod_FacturaVenta,
    m.descripcion as Medicamento,
    dfm.cantidad,
    dfm.precioUnitario,
    (dfm.cantidad * dfm.precioUnitario) as Subtotal,
    CASE WHEN dfm.codCobertura IS NOT NULL THEN 'Con Cobertura' ELSE 'Sin Cobertura' END as Cobertura
FROM DetallesFacturaVentasMedicamento dfm
    INNER JOIN FacturasVenta fv ON dfm.codFacturaVenta = fv.cod_FacturaVenta
    INNER JOIN Medicamentos m ON dfm.codMedicamento = m.cod_medicamento
ORDER BY fv.cod_FacturaVenta, dfm.cod_DetFacVentaM;

-- 8. TESTEAR DETALLES DE FACTURAS - ARTÍCULOS
SELECT 
    dfa.cod_DetFacVentaA,
    fv.cod_FacturaVenta,
    a.descripcion as Articulo,
    dfa.cantidad,
    dfa.precioUnitario,
    (dfa.cantidad * dfa.precioUnitario) as Subtotal
FROM DetallesFacturaVentasArticulo dfa
    INNER JOIN FacturasVenta fv ON dfa.codFacturaVenta = fv.cod_FacturaVenta
    INNER JOIN Articulos a ON dfa.codArticulo = a.cod_Articulo
ORDER BY fv.cod_FacturaVenta, dfa.cod_DetFacVentaA;

-- 9. TESTEAR FACTURA COMPLETA (ID = 1) CON TODOS SUS DETALLES
-- Datos de la factura
SELECT 
    fv.cod_FacturaVenta,
    fv.fecha,
    e.nom_Empleado + ' ' + e.ape_Empleado as Empleado,
    c.nomCliente + ' ' + c.apeCliente as Cliente,
    s.nom_Sucursal as Sucursal,
    fp.metodo as FormaPago,
    fv.total
FROM FacturasVenta fv
    INNER JOIN Empleados e ON fv.codEmpleado = e.cod_Empleado
    INNER JOIN Clientes c ON fv.codCliente = c.cod_Cliente
    INNER JOIN Sucursales s ON fv.codSucursal = s.cod_Sucursal
    INNER JOIN Formas_Pago fp ON fv.codFormaPago = fp.cod_Forma_Pago
WHERE fv.cod_FacturaVenta = 1;

-- Detalles de medicamentos de la factura 1
SELECT 
    'Medicamento' as Tipo,
    m.descripcion as Producto,
    dfm.cantidad,
    dfm.precioUnitario,
    (dfm.cantidad * dfm.precioUnitario) as Subtotal
FROM DetallesFacturaVentasMedicamento dfm
    INNER JOIN Medicamentos m ON dfm.codMedicamento = m.cod_medicamento
WHERE dfm.codFacturaVenta = 1;

-- Detalles de artículos de la factura 1
SELECT 
    'Artículo' as Tipo,
    a.descripcion as Producto,
    dfa.cantidad,
    dfa.precioUnitario,
    (dfa.cantidad * dfa.precioUnitario) as Subtotal
FROM DetallesFacturaVentasArticulo dfa
    INNER JOIN Articulos a ON dfa.codArticulo = a.cod_Articulo
WHERE dfa.codFacturaVenta = 1;

-- 10. TESTEAR USUARIOS Y SUS SUCURSALES (Funcionalidad por usuario)
SELECT 
    u.cod_usuario,
    u.Username,
    s.nom_Sucursal,
    gs.fecha_asignacion,
    CASE WHEN gs.activo = 1 THEN 'Activo' ELSE 'Inactivo' END as Estado
FROM Usuario u
    INNER JOIN Grupsucursales gs ON u.cod_usuario = gs.cod_usuario
    INNER JOIN Sucursales s ON gs.cod_sucursal = s.cod_Sucursal
ORDER BY u.Username, s.nom_Sucursal;

-- 11. TESTEAR STOCK DE MEDICAMENTOS Y ARTÍCULOS
SELECT 
    'Medicamento' as TipoProducto,
    s.nom_Sucursal as Sucursal,
    m.descripcion as Producto,
    sm.cantidad as Stock
FROM Stock_Medicamentos sm
    INNER JOIN Sucursales s ON sm.cod_Sucursal = s.cod_Sucursal
    INNER JOIN Medicamentos m ON sm.cod_Medicamento = m.cod_medicamento
UNION ALL
SELECT 
    'Artículo' as TipoProducto,
    s.nom_Sucursal as Sucursal,
    a.descripcion as Producto,
    sa.cantidad as Stock
FROM Stock_Articulos sa
    INNER JOIN Sucursales s ON sa.codSucursal = s.cod_Sucursal
    INNER JOIN Articulos a ON sa.codArticulo = a.cod_Articulo
ORDER BY Sucursal, TipoProducto, Producto;

-- 12. TESTEAR TRIGGER - VERIFICAR QUE ADMIN TIENE ACCESO A TODAS LAS SUCURSALES
SELECT 
    'Usuario Admin tiene acceso a estas sucursales:' as Info,
    s.nom_Sucursal,
    gs.fecha_asignacion
FROM Grupsucursales gs
    INNER JOIN Sucursales s ON gs.cod_sucursal = s.cod_Sucursal
WHERE gs.cod_usuario = 1 AND gs.activo = 1
ORDER BY s.nom_Sucursal;

-- 13. INSERTAR UNA NUEVA SUCURSAL PARA TESTEAR EL TRIGGER
INSERT INTO Sucursales (nom_Sucursal, nro_Tel, calle, altura, email, horarioApertura, horarioCierre, cod_Localidad)
VALUES ('Farmacia Test Trigger', '011-TEST-123', 'Calle Test', 999, 'test@farmacia.com', '09:00:00', '21:00:00', 1);

-- Verificar que el trigger asignó automáticamente el admin a la nueva sucursal
SELECT 
    'Trigger funcionó - Admin asignado a nueva sucursal:' as Resultado,
    s.nom_Sucursal,
    gs.fecha_asignacion
FROM Grupsucursales gs
    INNER JOIN Sucursales s ON gs.cod_sucursal = s.cod_Sucursal
WHERE gs.cod_usuario = 1 AND s.nom_Sucursal = 'Farmacia Test Trigger';