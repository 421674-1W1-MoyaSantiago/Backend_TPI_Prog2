-- Datos de prueba para PostgreSQL - Pharm-API
-- Este archivo contiene datos básicos para probar la aplicación

-- 1. Insertar datos en tablas de catálogos
INSERT INTO Tipos_Empleados (tipo) VALUES 
('Farmacéutico'),
('Técnico'),
('Vendedor'),
('Administrador'),
('Gerente')
ON CONFLICT DO NOTHING;

INSERT INTO Provincias (nom_Provincia) VALUES 
('Buenos Aires'),
('Córdoba'),
('Santa Fe'),
('Mendoza'),
('Tucumán')
ON CONFLICT DO NOTHING;

INSERT INTO Localidades (nom_Localidad, cod_Provincia) VALUES 
('La Plata', 1),
('Mar del Plata', 1),
('Córdoba Capital', 2),
('Rosario', 3),
('Santa Fe Capital', 3),
('Mendoza Capital', 4),
('Tucumán Capital', 5)
ON CONFLICT DO NOTHING;

INSERT INTO Tipos_Documento (tipo) VALUES 
('DNI'),
('Pasaporte'),
('CUIT'),
('CUIL')
ON CONFLICT DO NOTHING;

INSERT INTO Tipos_Presentacion (descripcion) VALUES 
('Comprimidos'),
('Cápsulas'),
('Jarabe'),
('Gotas'),
('Crema'),
('Pomada'),
('Inyectable')
ON CONFLICT DO NOTHING;

INSERT INTO Unidades_Medida (unidadMedida) VALUES 
('mg'),
('g'),
('ml'),
('l'),
('unidades'),
('gotas'),
('cucharadas')
ON CONFLICT DO NOTHING;

INSERT INTO Tipos_Medicamento (descripcion) VALUES 
('Analgésico'),
('Antibiótico'),
('Antiinflamatorio'),
('Vitaminas'),
('Digestivo'),
('Cardiovascular'),
('Respiratorio')
ON CONFLICT DO NOTHING;

INSERT INTO Formas_Pago (metodo) VALUES 
('Efectivo'),
('Tarjeta de Débito'),
('Tarjeta de Crédito'),
('Transferencia'),
('Obra Social')
ON CONFLICT DO NOTHING;

INSERT INTO Tipos_Descuentos (tipo) VALUES 
('Promoción'),
('Obra Social'),
('Tercera Edad'),
('Cliente Frecuente'),
('Liquidación')
ON CONFLICT DO NOTHING;

INSERT INTO Tipos_Receta (tipo) VALUES 
('Sin Receta'),
('Receta Simple'),
('Receta Archivada'),
('Receta Especial')
ON CONFLICT DO NOTHING;

-- 2. Insertar laboratorios
INSERT INTO Laboratorios (nombreLaboratorio, direccion, telefono, contacto) VALUES 
('Bayer Argentina', 'Av. Corrientes 1234, CABA', '011-4567-8901', 'contacto@bayer.com.ar'),
('Roemmers', 'Av. Santa Fe 5678, CABA', '011-4567-8902', 'info@roemmers.com.ar'),
('Bagó', 'Av. Rivadavia 9012, CABA', '011-4567-8903', 'ventas@bago.com.ar')
ON CONFLICT DO NOTHING;

-- 3. Insertar categorías
INSERT INTO Categorias_Articulos (categoria) VALUES 
('Higiene Personal'),
('Cosmética'),
('Perfumería'),
('Cuidado Infantil'),
('Productos Naturales'),
('Accesorios Médicos')
ON CONFLICT DO NOTHING;

-- 4. Insertar obras sociales
INSERT INTO Obras_Sociales (nombre, direccion, telefono, email) VALUES 
('OSDE', 'Av. Córdoba 1000, CABA', '011-5555-0001', 'info@osde.com.ar'),
('Swiss Medical', 'Av. Callao 2000, CABA', '011-5555-0002', 'contacto@swissmedical.com.ar'),
('Galeno', 'Av. Santa Fe 3000, CABA', '011-5555-0003', 'info@galeno.com.ar')
ON CONFLICT DO NOTHING;

-- 5. Insertar coberturas
INSERT INTO Coberturas (cobertura, porcentaje_cobertura) VALUES 
('Cobertura Básica', 40.00),
('Cobertura Intermedia', 60.00),
('Cobertura Premium', 80.00),
('Cobertura Total', 100.00)
ON CONFLICT DO NOTHING;

-- 6. Insertar proveedores
INSERT INTO Proveedores (nombre, direccion, telefono, email, cuit) VALUES 
('Droguería del Sud', 'Av. Belgrano 1500, CABA', '011-4000-0001', 'ventas@drogueriadelsud.com', '30-12345678-9'),
('Farmacity Distribución', 'Av. Cabildo 2500, CABA', '011-4000-0002', 'distribución@farmacity.com', '30-87654321-0')
ON CONFLICT DO NOTHING;

-- 7. Insertar grupos de sucursales
INSERT INTO Grup_Sucursales (nombre_grupo, descripcion) VALUES 
('Centro', 'Sucursales del centro de la ciudad'),
('Norte', 'Sucursales de la zona norte'),
('Sur', 'Sucursales de la zona sur'),
('Oeste', 'Sucursales de la zona oeste')
ON CONFLICT DO NOTHING;

-- 8. Insertar sucursales
INSERT INTO Sucursales (direccion, telefono, cod_Localidad, cod_grupo_sucursal) VALUES 
('Av. Corrientes 1234, Centro', '011-1111-1111', 1, 1),
('Av. Santa Fe 5678, Palermo', '011-2222-2222', 1, 2),
('Av. Rivadavia 9012, Caballito', '011-3333-3333', 1, 3),
('Av. Cabildo 1357, Belgrano', '011-4444-4444', 1, 2)
ON CONFLICT DO NOTHING;

-- 9. Insertar usuarios de ejemplo (se sincronizarán con Auth-API)
INSERT INTO Usuarios (username, email, nombre, apellido, cod_sucursal) VALUES 
('admin', 'admin@farmacia.com', 'Admin', 'Sistema', 1),
('farmaceutico1', 'farm1@farmacia.com', 'Juan', 'Pérez', 1),
('vendedor1', 'vend1@farmacia.com', 'María', 'González', 2),
('gerente1', 'ger1@farmacia.com', 'Carlos', 'Rodríguez', 1)
ON CONFLICT (username) DO NOTHING;

-- 10. Insertar empleados
INSERT INTO Empleados (nombre, apellido, email, telefono, direccion, fecha_nacimiento, fecha_ingreso, salario, cod_tipo_empleado, cod_sucursal, cod_tipo_documento, nro_documento, usuario_id) VALUES 
('Juan', 'Pérez', 'farm1@farmacia.com', '011-1111-1001', 'Av. Corrientes 100', '1985-03-15', '2020-01-15', 150000.00, 1, 1, 1, '12345678', 2),
('María', 'González', 'vend1@farmacia.com', '011-2222-2001', 'Av. Santa Fe 200', '1990-07-22', '2021-03-01', 80000.00, 3, 2, 1, '87654321', 3),
('Carlos', 'Rodríguez', 'ger1@farmacia.com', '011-3333-3001', 'Av. Rivadavia 300', '1978-11-10', '2019-05-20', 200000.00, 5, 1, 1, '11223344', 4)
ON CONFLICT (email) DO NOTHING;

-- 11. Insertar medicamentos de ejemplo
INSERT INTO Medicamentos (nombre, descripcion, precio, stock, cod_tipo_medicamento, cod_laboratorio, cod_tipo_presentacion, cod_unidad_medida, requiere_receta) VALUES 
('Ibuprofeno 400mg', 'Antiinflamatorio y analgésico', 450.00, 100, 3, 1, 1, 1, false),
('Amoxicilina 500mg', 'Antibiótico de amplio espectro', 890.00, 50, 2, 2, 2, 1, true),
('Paracetamol 500mg', 'Analgésico y antipirético', 320.00, 150, 1, 3, 1, 1, false),
('Vitamina C 1g', 'Suplemento vitamínico', 680.00, 80, 4, 1, 1, 1, false)
ON CONFLICT DO NOTHING;

-- 12. Insertar artículos de ejemplo
INSERT INTO Articulos (nombre, descripcion, precio, stock, cod_categoria, cod_proveedor, fecha_vencimiento) VALUES 
('Shampoo Anticaspa', 'Shampoo para tratamiento de caspa', 750.00, 30, 1, 1, '2025-12-31'),
('Crema Hidratante', 'Crema hidratante para piel seca', 950.00, 25, 2, 1, '2025-06-30'),
('Termómetro Digital', 'Termómetro digital para uso clínico', 1200.00, 15, 6, 2, NULL)
ON CONFLICT DO NOTHING;

-- 13. Insertar descuentos
INSERT INTO Descuentos (descripcion, porcentaje, fecha_inicio, fecha_fin, cod_tipo_descuento) VALUES 
('Descuento Tercera Edad', 15.00, '2024-01-01', '2024-12-31', 3),
('Promoción Verano', 20.00, '2024-12-01', '2025-03-31', 1),
('Descuento OSDE', 10.00, '2024-01-01', '2024-12-31', 2)
ON CONFLICT DO NOTHING;

-- Comentario informativo
-- Este script inserta datos de prueba para desarrollo y testing
-- En producción, algunos de estos datos deberán ser reemplazados por datos reales