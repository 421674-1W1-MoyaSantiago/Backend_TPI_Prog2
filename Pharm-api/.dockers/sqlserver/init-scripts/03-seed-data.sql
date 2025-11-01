-- 03-insert-initial-data.sql
-- Insertar datos iniciales para el funcionamiento básico
USE PharmDB;
GO

PRINT 'Insertando datos iniciales...';

-- *** IMPORTANTE: Usuarios PRIMERO (antes que sucursales para que funcione el trigger) ***
INSERT INTO Usuario (Username, Email) VALUES 
('admin', 'admin@farmacia.ejemplo.com'),
('usuario1', 'usuario1@farmacia.ejemplo.com');

-- Datos iniciales para catálogos
INSERT INTO Provincias (nom_Provincia) VALUES 
('Buenos Aires'), 
('Córdoba'), 
('Santa Fe'), 
('Mendoza'),
('Tucumán'),
('Entre Ríos');

INSERT INTO Localidades (nom_Localidad, cod_Provincia) VALUES 
('CABA', 1), 
('La Plata', 1), 
('Mar del Plata', 1),
('Córdoba Capital', 2), 
('Villa Carlos Paz', 2),
('Rosario', 3),
('Santa Fe Capital', 3),
('Mendoza Capital', 4),
('San Rafael', 4);

INSERT INTO Tipos_Documento (tipo) VALUES 
('DNI'), 
('Pasaporte'), 
('CUIT'),
('LC'),
('LE');

INSERT INTO Tipos_Empleados (tipo) VALUES 
('Farmacéutico'), 
('Técnico en Farmacia'), 
('Administrativo'), 
('Gerente'),
('Cajero'),
('Repositor');

INSERT INTO Formas_Pago (metodo) VALUES 
('Efectivo'), 
('Tarjeta de Débito'), 
('Tarjeta de Crédito'), 
('Transferencia'),
('Cheque'),
('Mercado Pago');

INSERT INTO Tipos_Presentacion (descripcion) VALUES
('Comprimidos'),
('Cápsulas'),
('Jarabe'),
('Crema'),
('Gotas'),
('Ampolla'),
('Spray');

INSERT INTO Unidades_Medida (unidadMedida) VALUES
('mg'),
('ml'),
('g'),
('l'),
('unidad'),
('cc');

INSERT INTO Tipos_Medicamento (descripcion) VALUES
('Analgésico'),
('Antibiótico'),
('Antiinflamatorio'),
('Antialérgico'),
('Vitaminas'),
('Cardiovascular');

INSERT INTO Categorias_Articulos (categoria) VALUES
('Higiene Personal'),
('Cosmética'),
('Perfumería'),
('Accesorios'),
('Ortopedia'),
('Nutrición');

INSERT INTO Tipos_Descuentos (descripcion) VALUES
('Descuento por Obra Social'),
('Descuento por Promoción'),
('Descuento por Volumen'),
('Descuento por Cliente Frecuente');

INSERT INTO Tipos_Receta (tipo) VALUES
('Receta Simple'),
('Receta Especial'),
('Receta de Estupefacientes');

-- Obras sociales de ejemplo
INSERT INTO Obras_Sociales (razonSocial, nroTel, email) VALUES
('ObraSocial1 Ejemplo', '011-5555-0001', 'contacto@obrasocial1.ejemplo.com'),
('ObraSocial2 Ejemplo', '011-5555-0002', 'info@obrasocial2.ejemplo.com'),
('ObraSocial3 Ejemplo', '011-5555-0003', 'atencion@obrasocial3.ejemplo.com'),
('ObraSocial4 Ejemplo', '011-5555-0004', 'consultas@obrasocial4.ejemplo.com');

-- Sucursales de ejemplo
INSERT INTO Sucursales (nom_Sucursal, nro_Tel, calle, altura, email, horarioApertura, horarioCierre, cod_Localidad)
VALUES 
('Sucursal Centro Ejemplo', '011-1111-0001', 'Av. Principal', 1000, 'centro@farmacia.ejemplo.com', '08:00:00', '22:00:00', 1),
('Sucursal Norte Ejemplo', '0221-2222-0002', 'Calle Norte', 500, 'norte@farmacia.ejemplo.com', '08:30:00', '21:30:00', 2),
('Sucursal Sur Ejemplo', '0351-3333-0003', 'Av. Sur', 750, 'sur@farmacia.ejemplo.com', '09:00:00', '21:00:00', 4);

-- Proveedores de ejemplo
INSERT INTO Proveedores (razon_Social, cuit, nro_Tel) VALUES
('Proveedor1 Ejemplo SA', '30-11111111-1', '011-4000-1001'),
('Proveedor2 Ejemplo SA', '30-22222222-2', '011-4000-2002'),
('Proveedor3 Ejemplo SA', '30-33333333-3', '011-4000-3003');

-- Laboratorios de ejemplo
INSERT INTO Laboratorios (descripcion, cod_Proveedor) VALUES
('Laboratorio1 Ejemplo', 1),
('Laboratorio2 Ejemplo', 2),
('Laboratorio3 Ejemplo', 3);

-- Lotes de medicamentos de ejemplo
INSERT INTO Lotes_Medicamentos (fecha_Elaboracion, fecha_Vencimiento, cantidad) VALUES
('2024-01-15', '2026-01-15', 1000),
('2024-02-01', '2025-12-01', 500),
('2024-03-10', '2026-03-10', 750);

-- Medicamentos de ejemplo
INSERT INTO Medicamentos (cod_barra, descripcion, requiere_receta, venta_libre, precio_unitario, dosis, posologia, cod_lote_medicamento, codLaboratorio, cod_tipo_presentacion, cod_unidad_medida, cod_tipo_medicamento) VALUES
('7801111111111', 'Medicamento1 Ejemplo 400mg', 0, 1, 150.00, 400, 'Tomar 1 cada 8 horas', 1, 1, 1, 1, 3),
('7801111111112', 'Medicamento2 Ejemplo 500mg', 1, 0, 200.00, 500, 'Tomar 1 cada 12 horas por 7 días', 2, 2, 2, 1, 2),
('7801111111113', 'Medicamento3 Ejemplo 500mg', 0, 1, 120.00, 500, 'Tomar 1 cada 6 horas', 3, 3, 1, 1, 1);

-- Artículos de ejemplo  
INSERT INTO Articulos (cod_barra, descripcion, precioUnitario, cod_Proveedor, cod_Categoria_Articulo) VALUES
('7801111110001', 'Producto1 Ejemplo 400ml', 350.00, 1, 1),
('7801111110002', 'Producto2 Ejemplo 200ml', 280.00, 2, 2),
('7801111110003', 'Producto3 Ejemplo 100ml', 1200.00, 3, 3),
('7801111110004', 'Producto4 Ejemplo Digital', 450.00, 1, 4);

-- Empleados de ejemplo (NECESARIOS ANTES que las facturas)
INSERT INTO Empleados (nom_Empleado, ape_Empleado, nro_Tel, calle, altura, email, fechaIngreso, codTipoEmpleado, codTipoDocumento, codSucursal)
VALUES 
('Admin', 'Administrador', '011-1111-1111', 'Av. Admin', 100, 'admin@farmacia.ejemplo.com', '2024-01-15', 4, 1, 1),
('Empleado1', 'Apellido1', '011-2222-2222', 'Calle Ejemplo', 200, 'empleado1@farmacia.ejemplo.com', '2024-02-01', 2, 1, 1),
('Empleado2', 'Apellido2', '0221-3333-3333', 'Av. Muestra', 300, 'empleado2@farmacia.ejemplo.com', '2024-03-01', 1, 1, 2);

-- Clientes de ejemplo (NECESARIOS ANTES que las coberturas)
INSERT INTO Clientes (nomCliente, apeCliente, nroDoc, nroTel, calle, altura, email, cod_Tipo_Documento, cod_Obra_Social)
VALUES
('Cliente1', 'Apellido1', '12345678', '011-9999-1111', 'Av. Cliente', 1000, 'cliente1@email.ejemplo.com', 1, 1),
('Cliente2', 'Apellido2', '87654321', '011-9999-2222', 'Calle Cliente', 2000, 'cliente2@email.ejemplo.com', 1, 2),
('Cliente3', 'Apellido3', '11223344', '0221-9999-3333', 'Av. Muestra', 3000, 'cliente3@email.ejemplo.com', 1, 3);

-- Descuentos de ejemplo
INSERT INTO Descuentos (Fecha_Descuento, cod_localidad, cod_medicamento, porcentaje_descuento, cod_tipo_descuento) VALUES
(GETDATE(), 1, 1, 15.00, 1), -- Descuento OSDE para Ibuprofeno en CABA
(GETDATE(), 2, 2, 20.00, 1), -- Descuento OSDE para Amoxicilina en La Plata
(GETDATE(), 1, 3, 10.00, 2); -- Descuento por promoción para Paracetamol

-- Coberturas de ejemplo (DESPUÉS de clientes, obras sociales y descuentos)
INSERT INTO Coberturas (fechaInicio, fechaFin, cod_Localidad, cod_cliente, cod_Obra_Social, cod_descuento) VALUES
('2024-01-01', '2024-12-31', 1, 1, 1, 1), -- Ana con OSDE en CABA
('2024-01-01', '2024-12-31', 2, 2, 2, 2), -- Roberto con Swiss Medical en La Plata
('2024-01-01', '2024-12-31', 1, 3, 3, 3); -- Laura con Galeno en CABA

-- Stock de artículos
INSERT INTO Stock_Articulos (cantidad, codSucursal, codArticulo) VALUES
(50, 1, 1), -- Shampoo en Central CABA
(30, 1, 2), -- Crema en Central CABA
(20, 2, 3), -- Perfume en Norte La Plata
(25, 1, 4); -- Termómetro en Central CABA

-- Stock de medicamentos
INSERT INTO Stock_Medicamentos (cantidad, cod_Sucursal, cod_Medicamento) VALUES
(100, 1, 1), -- Ibuprofeno en Central CABA
(75, 1, 2),  -- Amoxicilina en Central CABA
(150, 2, 3), -- Paracetamol en Norte La Plata
(80, 1, 3);  -- Paracetamol en Central CABA

-- Recetas de ejemplo
INSERT INTO Recetas (nomMedico, apeMedico, matricula, fecha, diagnostico, codigo, estado, codObraSocial, codCliente, codTipoReceta) VALUES
('Dr. Carlos', 'Médico', 'MP12345', GETDATE(), 'Dolor de cabeza', 12345, 'Activa', 1, 1, 1),
('Dra. María', 'González', 'MP67890', GETDATE(), 'Infección respiratoria', 67890, 'Activa', 2, 2, 1);

-- Detalles de recetas
INSERT INTO Detalles_Receta (cantidad, codMedicamento, cod_Receta) VALUES
(1, 2, 1), -- Amoxicilina para receta 1
(2, 1, 2); -- Ibuprofeno para receta 2

-- Autorizaciones
INSERT INTO Autorizaciones (codigo, estado, fechaAutorizacion, codObraSocial, codReceta) VALUES
(100001, 'Autorizada', GETDATE(), 1, 1),
(100002, 'Autorizada', GETDATE(), 2, 2);

-- Facturas de compra
INSERT INTO Facturas_Compra (fecha, cod_Empleado, cod_Sucursal, cod_Proveedor) VALUES
(GETDATE(), 1, 1, 1), -- Compra a Bagó en Central CABA
(GETDATE(), 2, 1, 2); -- Compra a Roemmers en Central CABA

-- Detalles facturas compra medicamentos
INSERT INTO DetallesFacturaCompraMedicamento (cantidad, precioUnitario, codFacturaCompra, codMedicamento) VALUES
(100, 120.00, 1, 1), -- Compra Ibuprofeno
(50, 180.00, 2, 2);  -- Compra Amoxicilina

-- Detalles facturas compra artículos
INSERT INTO DetallesFacturaCompraArticulo (cantidad, precioUnitario, codFacturaCompra, codArticulo) VALUES
(30, 280.00, 1, 1), -- Compra Shampoo
(20, 220.00, 2, 2); -- Compra Crema

-- Proveedores de ejemplo (movido aquí para orden lógico)
INSERT INTO Proveedores (razon_Social, cuit, nro_Tel) VALUES
('Laboratorios Bagó', '30-12345678-9', '011-4000-1000'),
('Roemmers SA', '30-87654321-2', '011-4000-2000'),
('Laboratorios Phoenix', '30-11223344-5', '011-4000-3000');

-- Laboratorios de ejemplo
INSERT INTO Laboratorios (descripcion, cod_Proveedor) VALUES
('Bagó Argentina', 1),
('Roemmers Labs', 2),
('Phoenix Pharma', 3);

-- *** NOTA: Los usuarios ya fueron insertados al principio del script ***
-- *** El trigger automáticamente asignará el admin (ID=1) a cada nueva sucursal ***
-- *** Empleados y clientes ya fueron insertados arriba para respetar dependencias ***

-- Facturas de ejemplo para testing
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total)
VALUES 
(GETDATE(), 1, 1, 1, 1, 1500.00),
(GETDATE(), 2, 2, 1, 2, 850.00),
(GETDATE(), 3, 3, 2, 3, 2200.00);

-- AHORA SÍ: Detalles de facturas (después de crear las facturas)
-- Detalles de medicamentos para las facturas
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta)
VALUES 
(2, 150.00, 1, 1, 1), -- Ibuprofeno con cobertura en factura 1
(1, 200.00, 2, 2, 1), -- Amoxicilina con cobertura en factura 1
(3, 120.00, NULL, 3, 2), -- Paracetamol sin cobertura en factura 2
(1, 150.00, 3, 1, 3); -- Ibuprofeno con cobertura en factura 3

-- Detalles de artículos para las facturas
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo)
VALUES 
(1, 350.00, 1, 1), -- Shampoo en factura 1
(2, 280.00, 2, 2), -- Crema en factura 2
(1, 1200.00, 3, 3), -- Perfume en factura 3
(1, 450.00, 3, 4); -- Termómetro en factura 3

-- Reintegros de ejemplo
INSERT INTO Reintegros (fechaEmision, fechaReembolso, estado, cod_Cobertura, cod_ObraSocial, cod_Receta, cod_DetFacVentaM) VALUES
(GETDATE(), NULL, 'Pendiente', 1, 1, 1, 1), -- Reintegro pendiente para Cliente1
(GETDATE(), GETDATE(), 'Aprobado', 2, 2, 2, 2); -- Reintegro aprobado para Cliente2

-- *** ASIGNACIONES DE SUCURSALES A USUARIOS ***
-- Admin tiene acceso a TODAS las sucursales
INSERT INTO Grupsucursales (cod_sucursal, cod_usuario, activo) VALUES
(1, 1, 1), -- Admin -> Sucursal Centro Ejemplo
(2, 1, 1), -- Admin -> Sucursal Norte Ejemplo  
(3, 1, 1); -- Admin -> Sucursal Sur Ejemplo

-- Usuario1 tiene acceso limitado (solo sucursales 1 y 2)
INSERT INTO Grupsucursales (cod_sucursal, cod_usuario, activo) VALUES
(1, 2, 1), -- Usuario1 -> Sucursal Centro Ejemplo
(2, 2, 1); -- Usuario1 -> Sucursal Norte Ejemplo

PRINT 'Datos iniciales insertados exitosamente';
PRINT 'Base de datos PharmDB lista para usar';
PRINT 'Usuario admin tiene acceso a todas las sucursales (1, 2, 3)';
PRINT 'Usuario usuario1 tiene acceso limitado a sucursales (1, 2)';
GO