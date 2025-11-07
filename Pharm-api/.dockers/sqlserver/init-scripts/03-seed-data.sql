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
('Cardiovascular'),
('Respiratorio');

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

-- Laboratorios de ejemplo - EXPANDIDO para simular farmacia real
INSERT INTO Laboratorios (descripcion, cod_Proveedor) VALUES
('Laboratorio1 Ejemplo', 1),
('Laboratorio2 Ejemplo', 2),
('Laboratorio3 Ejemplo', 3),
('Bayer Argentina', 1),
('Roemmers', 2),
('Bagó', 3),
('Gador', 1),
('Pfizer Argentina', 2),
('Novartis Argentina', 3),
('Abbott Laboratorios', 1),
('Roche Argentina', 2),
('Sanofi Argentina', 3),
('GlaxoSmithKline', 1),
('Merck Sharp & Dohme', 2),
('Laboratorios Phoenix', 3),
('Laboratorios Elea', 1),
('Laboratorios Richmond', 2),
('Laboratorios Andrómaco', 3),
('Laboratorios Casasco', 1),
('Laboratorios Bernabó', 2),
('Laboratorios Montpellier', 3),
('Laboratorios Raffo', 1),
('Laboratorios Rontag', 2);

-- Lotes de medicamentos de ejemplo - EXPANDIDO con fechas variadas
INSERT INTO Lotes_Medicamentos (fecha_Elaboracion, fecha_Vencimiento, cantidad) VALUES
('2024-01-15', '2026-01-15', 1000),
('2024-02-01', '2025-12-01', 500),
('2024-03-10', '2026-03-10', 750),
('2024-01-20', '2026-06-30', 150),
('2024-02-15', '2025-08-15', 90),
('2024-03-10', '2026-03-10', 200),
('2024-01-25', '2025-07-25', 75),
('2024-02-20', '2026-04-20', 300),
('2024-03-15', '2025-09-15', 125),
('2024-01-30', '2026-01-30', 175),
('2024-02-25', '2025-11-25', 110),
('2024-03-20', '2026-05-20', 220),
('2024-01-10', '2025-10-10', 85),
('2024-02-05', '2026-02-05', 160),
('2024-03-25', '2025-12-25', 95),
('2024-01-05', '2026-07-05', 280),
('2024-02-28', '2025-06-28', 65),
('2024-03-30', '2026-08-30', 190),
('2024-01-12', '2025-05-12', 140),
('2024-02-18', '2026-09-18', 230),
('2024-03-08', '2025-11-08', 105),
('2024-01-22', '2026-03-22', 185),
('2024-02-12', '2025-04-12', 70),
('2024-03-18', '2026-10-18', 210),
('2024-01-28', '2025-08-28', 120),
('2024-02-22', '2026-11-22', 165),
('2024-03-12', '2025-07-12', 80),
('2024-01-08', '2026-12-08', 245),
('2024-02-14', '2025-03-14', 55),
('2024-03-28', '2026-06-28', 195),
-- Lotes adicionales para medicamentos de testing
('2024-01-15', '2026-01-15', 150),
('2024-02-20', '2025-11-20', 180),
('2024-03-10', '2026-04-10', 120),
('2024-01-25', '2025-09-25', 200),
('2024-02-28', '2026-07-28', 90),
('2024-03-15', '2025-12-15', 160),
('2024-01-30', '2026-05-30', 130),
('2024-02-10', '2025-08-10', 175),
('2024-03-20', '2026-09-20', 110),
('2024-01-12', '2025-06-12', 145),
('2024-02-25', '2026-10-25', 125),
('2024-03-05', '2025-07-05', 185),
('2024-01-18', '2026-11-18', 95),
('2024-02-15', '2025-04-15', 155),
('2024-03-22', '2026-12-22', 140),
('2024-01-08', '2025-05-08', 115),
('2024-02-12', '2026-08-12', 170),
('2024-03-25', '2025-10-25', 100),
('2024-01-20', '2026-06-20', 190),
('2024-02-18', '2025-09-18', 135);

-- Medicamentos de ejemplo - EXPANDIDO con medicamentos argentinos comunes
INSERT INTO Medicamentos (cod_barra, descripcion, requiere_receta, venta_libre, precio_unitario, dosis, posologia, cod_lote_medicamento, codLaboratorio, cod_tipo_presentacion, cod_unidad_medida, cod_tipo_medicamento) VALUES
('7801111111111', 'Medicamento1 Ejemplo 400mg', 0, 1, 150.00, 400, 'Tomar 1 cada 8 horas', 1, 1, 1, 1, 3),
('7801111111112', 'Medicamento2 Ejemplo 500mg', 1, 0, 200.00, 500, 'Tomar 1 cada 12 horas por 7 días', 2, 2, 2, 1, 2),
('7801111111113', 'Medicamento3 Ejemplo 500mg', 0, 1, 120.00, 500, 'Tomar 1 cada 6 horas', 3, 3, 1, 1, 1),
-- Analgésicos comunes
('7790011234567', 'Tafirol 500mg', 0, 1, 450.00, 500, 'Tomar 1 comprimido cada 6-8 horas', 4, 7, 1, 1, 1),
('7790022345678', 'Actron 400mg', 0, 1, 620.00, 400, 'Tomar 1 comprimido cada 8 horas', 5, 4, 1, 1, 3),
('7790033456789', 'Bayaspirina 500mg', 0, 1, 380.00, 500, 'Tomar 1 comprimido cada 6 horas', 6, 4, 1, 1, 1),
('7790044567890', 'Mejoral 500mg', 0, 1, 520.00, 500, 'Tomar 1-2 comprimidos cada 6 horas', 7, 4, 1, 1, 1),
('7790055678901', 'Ibupirac 600mg', 0, 1, 680.00, 600, 'Tomar 1 comprimido cada 8 horas', 8, 7, 1, 1, 3),
-- Antibióticos
('7790066789012', 'Amoxidal 500mg', 1, 0, 890.00, 500, 'Tomar 1 cápsula cada 8 horas por 7 días', 9, 5, 2, 1, 2),
('7790077890123', 'Clavulin 875mg', 1, 0, 1250.00, 875, 'Tomar 1 comprimido cada 12 horas', 10, 7, 1, 1, 2),
('7790088901234', 'Eritromicina 500mg', 1, 0, 1150.00, 500, 'Tomar 1 cápsula cada 6 horas', 11, 10, 2, 1, 2),
('7790099012345', 'Cefalexina 500mg', 1, 0, 980.00, 500, 'Tomar 1 cápsula cada 6 horas', 12, 11, 2, 1, 2),
-- Digestivos
('7790144567890', 'Buscapina 10mg', 0, 1, 680.00, 10, 'Tomar 1 comprimido cada 8 horas', 13, 4, 1, 1, 5),
('7790155678901', 'Mylanta Plus', 0, 1, 520.00, 10, 'Tomar 1-2 cucharadas después de comidas', 14, 4, 3, 3, 5),
('7790166789012', 'Omeprazol 20mg', 0, 1, 890.00, 20, 'Tomar 1 cápsula en ayunas', 15, 5, 2, 1, 5),
('7790177890123', 'Ranitidina 150mg', 0, 1, 450.00, 150, 'Tomar 1 comprimido cada 12 horas', 16, 6, 1, 1, 5),
-- Cardiovasculares
('7790188901234', 'Enalapril 10mg', 1, 0, 680.00, 10, 'Tomar 1 comprimido cada 12 horas', 17, 8, 1, 1, 6),
('7790199012345', 'Losartan 50mg', 1, 0, 850.00, 50, 'Tomar 1 comprimido por día', 18, 10, 1, 1, 6),
('7790200123456', 'Amlodipina 5mg', 1, 0, 720.00, 5, 'Tomar 1 comprimido por día', 19, 11, 1, 1, 6),
('7790211234567', 'Atenolol 50mg', 1, 0, 620.00, 50, 'Tomar 1 comprimido por día', 20, 12, 1, 1, 6),
-- Respiratorios
('7790222345678', 'Salbutamol 100mcg', 1, 0, 1200.00, 100, '2 puff cada 6 horas según necesidad', 21, 13, 7, 1, 7),
('7790233456789', 'Loratadina 10mg', 0, 1, 480.00, 10, 'Tomar 1 comprimido por día', 22, 14, 1, 1, 7),
('7790244567890', 'Cetirizina 10mg', 0, 1, 520.00, 10, 'Tomar 1 comprimido por día', 23, 15, 1, 1, 7),
('7790255678901', 'Ambroxol 30mg', 0, 1, 380.00, 30, 'Tomar 1 comprimido cada 8 horas', 24, 16, 1, 1, 7),
-- Vitaminas
('7790266789012', 'Vitamina C 1g', 0, 1, 680.00, 1000, 'Tomar 1 comprimido por día', 25, 4, 1, 1, 4),
('7790277890123', 'Complejo B', 0, 1, 850.00, 1, 'Tomar 1 cápsula por día', 26, 5, 2, 5, 4),
('7790288901234', 'Hierro + Ácido Fólico', 0, 1, 750.00, 1, 'Tomar 1 comprimido por día', 27, 6, 1, 5, 4),
('7790299012345', 'Calcio + Vitamina D', 0, 1, 920.00, 1, 'Tomar 1 comprimido por día', 28, 7, 1, 5, 4),
-- Antidiabéticos
('7790333456789', 'Metformina 850mg', 1, 0, 420.00, 850, 'Tomar 1 comprimido cada 12 horas', 29, 12, 1, 1, 2),
('7790344567890', 'Glibenclamida 5mg', 1, 0, 380.00, 5, 'Tomar 1 comprimido antes del desayuno', 30, 13, 1, 1, 2),
-- Medicamentos adicionales para testing de endpoints
('7790400111111', 'Clonazepam 2mg', 1, 0, 680.00, 2, 'Tomar 1 comprimido por noche', 4, 4, 1, 1, 3),
('7790400222222', 'Diclofenac 75mg', 0, 1, 520.00, 75, 'Tomar 1 comprimido cada 8 horas', 5, 5, 1, 1, 3),
('7790400333333', 'Ketorolac 10mg', 1, 0, 780.00, 10, 'Tomar 1 comprimido cada 6 horas', 6, 6, 1, 1, 3),
('7790400444444', 'Nimesulida 100mg', 0, 1, 420.00, 100, 'Tomar 1 comprimido cada 12 horas', 7, 7, 1, 1, 3),
('7790400555555', 'Azitromicina 500mg', 1, 0, 950.00, 500, 'Tomar 1 comprimido por día por 3 días', 8, 8, 2, 1, 2),
('7790400666666', 'Claritromicina 500mg', 1, 0, 1150.00, 500, 'Tomar 1 comprimido cada 12 horas', 9, 9, 2, 1, 2),
('7790400777777', 'Ciprofloxacina 500mg', 1, 0, 880.00, 500, 'Tomar 1 comprimido cada 12 horas', 10, 10, 2, 1, 2),
('7790400888888', 'Fluconazol 150mg', 1, 0, 320.00, 150, 'Tomar 1 cápsula dosis única', 11, 11, 2, 1, 2),
('7790400999999', 'Montelukast 10mg', 1, 0, 1250.00, 10, 'Tomar 1 comprimido por noche', 12, 12, 1, 1, 7),
('7790401000000', 'Budesonida 200mcg', 1, 0, 1450.00, 200, '2 inhalaciones cada 12 horas', 13, 13, 7, 1, 7),
('7790401111111', 'Prednisona 20mg', 1, 0, 380.00, 20, 'Tomar según indicación médica', 14, 14, 1, 1, 3),
('7790401222222', 'Dexametasona 4mg', 1, 0, 280.00, 4, 'Tomar según indicación médica', 15, 15, 1, 1, 3),
('7790401333333', 'Furosemida 40mg', 1, 0, 320.00, 40, 'Tomar 1 comprimido en ayunas', 16, 16, 1, 1, 6),
('7790401444444', 'Espironolactona 25mg', 1, 0, 450.00, 25, 'Tomar 1 comprimido por día', 17, 17, 1, 1, 6),
('7790401555555', 'Captopril 25mg', 1, 0, 290.00, 25, 'Tomar 1 comprimido cada 8 horas', 18, 18, 1, 1, 6),
('7790401666666', 'Carvedilol 6.25mg', 1, 0, 520.00, 6.25, 'Tomar 1 comprimido cada 12 horas', 19, 19, 1, 1, 6),
('7790401777777', 'Simvastatina 20mg', 1, 0, 380.00, 20, 'Tomar 1 comprimido por noche', 20, 20, 1, 1, 6),
('7790401888888', 'Atorvastatina 40mg', 1, 0, 680.00, 40, 'Tomar 1 comprimido por noche', 21, 21, 1, 1, 6),
('7790401999999', 'Fenobarbital 100mg', 1, 0, 180.00, 100, 'Tomar según indicación médica', 22, 22, 1, 1, 3),
('7790402000000', 'Fenitoína 100mg', 1, 0, 240.00, 100, 'Tomar según indicación médica', 23, 23, 2, 1, 3);

-- Artículos de ejemplo - EXPANDIDO con productos de farmacia realistas
INSERT INTO Articulos (cod_barra, descripcion, precioUnitario, cod_Proveedor, cod_Categoria_Articulo) VALUES
('7801111110001', 'Producto1 Ejemplo 400ml', 350.00, 1, 1),
('7801111110002', 'Producto2 Ejemplo 200ml', 280.00, 2, 2),
('7801111110003', 'Producto3 Ejemplo 100ml', 1200.00, 3, 3),
('7801111110004', 'Producto4 Ejemplo Digital', 450.00, 1, 4),
-- Higiene Personal
('7790500123456', 'Shampoo Anticaspa Head & Shoulders 400ml', 750.00, 1, 1),
('7790511234567', 'Acondicionador Pantene Reparación 400ml', 680.00, 1, 1),
('7790522345678', 'Jabón Líquido Dove 250ml', 520.00, 1, 1),
('7790533456789', 'Desodorante Rexona Aerosol 150ml', 480.00, 1, 1),
('7790544567890', 'Crema Dental Colgate Total 90g', 320.00, 1, 1),
('7790555678901', 'Enjuague Bucal Listerine 500ml', 650.00, 1, 1),
('7790566789012', 'Cepillo de Dientes Oral-B', 280.00, 2, 1),
('7790577890123', 'Papel Higiénico Elite x4', 450.00, 2, 1),
('7790588901234', 'Toallas Femeninas Always x16', 580.00, 1, 1),
-- Cosmética
('7790599012345', 'Crema Hidratante Nivea 200ml', 950.00, 1, 2),
('7790600123456', 'Protector Solar Eucerin FPS 60', 1850.00, 1, 2),
('7790611234567', 'Base de Maquillaje Maybelline', 1250.00, 1, 2),
('7790622345678', 'Labial Revlon Color Rosa', 850.00, 1, 2),
('7790633456789', 'Rimmel Máscara de Pestañas', 920.00, 1, 2),
('7790644567890', 'Crema Antiarrugas Olay 50ml', 2200.00, 1, 2),
-- Perfumería
('7790655678901', 'Perfume Antonio Banderas 100ml', 3500.00, 1, 3),
('7790666789012', 'Perfume Paco Rabanne 80ml', 4200.00, 1, 3),
('7790677890123', 'Colonia Agua de Rosas 500ml', 1200.00, 1, 3),
('7790688901234', 'Desodorante Roll-On Nivea 50ml', 380.00, 1, 3),
-- Cuidado Infantil
('7790699012345', 'Pañales Pampers M x30', 2800.00, 1, 1),
('7790700123456', 'Shampoo Johnson Baby 400ml', 650.00, 1, 1),
('7790711234567', 'Crema Pañalitis Bepanthen 100g', 980.00, 1, 1),
('7790722345678', 'Toallitas Húmedas Huggies x80', 450.00, 1, 1),
('7790733456789', 'Aceite Johnson Baby 200ml', 520.00, 1, 1),
('7790744567890', 'Mamaderas Chicco 260ml', 1200.00, 2, 1),
('7790755678901', 'Chupetes MAM x2', 680.00, 2, 1),
-- Accesorios Médicos
('7790811234567', 'Termómetro Digital', 1200.00, 2, 4),
('7790822345678', 'Tensiómetro Digital Brazo', 4500.00, 2, 4),
('7790833456789', 'Glucómetro One Touch + 25 tiras', 2800.00, 2, 4),
('7790844567890', 'Nebulizador Ultrasonico', 3200.00, 2, 4),
('7790855678901', 'Jeringas Descartables 5ml x10', 450.00, 2, 4),
('7790866789012', 'Gasas Estériles 10x10 x25', 380.00, 2, 4),
('7790877890123', 'Alcohol en Gel 250ml', 320.00, 2, 4),
('7790888901234', 'Barbijos Quirúrgicos x50', 1200.00, 2, 4),
('7790899012345', 'Vendas Elásticas 10cm', 650.00, 2, 4),
('7790900123456', 'Curitas Adhesivas x40', 280.00, 2, 4),
-- Productos adicionales
('7790911234567', 'Algodón Hidrófilo 100g', 320.00, 2, 1),
('7790922345678', 'Agua Oxigenada 10vol 250ml', 180.00, 2, 1),
('7790933456789', 'Alcohol Etílico 70° 250ml', 220.00, 2, 1),
('7790944567890', 'Iodopovidona Solución 120ml', 450.00, 2, 1),
-- Suplementos
('7790955678901', 'Omega 3 en Cápsulas x60', 1850.00, 1, 1),
('7790966789012', 'Magnesio + B6 x30', 920.00, 1, 1),
('7790977890123', 'Probióticos en Cápsulas x30', 1650.00, 1, 1),
('7790988901234', 'Coenzima Q10 x60', 2200.00, 1, 1);

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
(GETDATE(), 1, 1, 15.00, 1), -- Descuento OSDE para Medicamento1 en CABA
(GETDATE(), 2, 2, 20.00, 1), -- Descuento OSDE para Medicamento2 en La Plata
(GETDATE(), 1, 3, 10.00, 2); -- Descuento por promoción para Medicamento3

-- Coberturas de ejemplo (DESPUÉS de clientes, obras sociales y descuentos)
INSERT INTO Coberturas (fechaInicio, fechaFin, cod_Localidad, cod_cliente, cod_Obra_Social, cod_descuento) VALUES
('2024-01-01', '2024-12-31', 1, 1, 1, 1), -- Ana con OSDE en CABA
('2024-01-01', '2024-12-31', 2, 2, 2, 2), -- Roberto con Swiss Medical en La Plata
('2024-01-01', '2024-12-31', 1, 3, 3, 1); -- Laura con Galeno en CABA (usando descuento 1)

-- Stock de artículos
INSERT INTO Stock_Articulos (cantidad, codSucursal, codArticulo) VALUES
(50, 1, 1), -- Shampoo en Central CABA
(30, 1, 2), -- Crema en Central CABA
(20, 2, 3), -- Perfume en Norte La Plata
(25, 1, 4); -- Termómetro en Central CABA

-- Stock de medicamentos - EXPANDIDO para testing de endpoints
INSERT INTO Stock_Medicamentos (cantidad, cod_Sucursal, cod_Medicamento) VALUES
-- Stock normal (mayor a 10)
(150, 1, 1), -- Medicamento1 en Central CABA
(120, 1, 2), -- Medicamento2 en Central CABA  
(200, 2, 3), -- Medicamento3 en Norte La Plata
(180, 1, 4), -- Tafirol en Central CABA
(90, 2, 5),  -- Actron en Norte La Plata
(110, 1, 6), -- Bayaspirina en Central CABA
(95, 2, 7),  -- Mejoral en Norte La Plata
(160, 1, 8), -- Ibupirac en Central CABA
(75, 2, 9),  -- Amoxidal en Norte La Plata
(135, 1, 10), -- Clavulin en Central CABA
(85, 2, 11), -- Eritromicina en Norte La Plata
(100, 1, 12), -- Cefalexina en Central CABA
(45, 2, 13), -- Buscapina en Norte La Plata
(65, 1, 14), -- Mylanta en Central CABA
(80, 2, 15), -- Omeprazol en Norte La Plata
(120, 1, 16), -- Ranitidina en Central CABA
(90, 2, 17), -- Enalapril en Norte La Plata
(110, 1, 18), -- Losartan en Central CABA
(85, 2, 19), -- Amlodipina en Norte La Plata
(95, 1, 20), -- Atenolol en Central CABA

-- Stock BAJO (menor a 10) para testing del endpoint stock-bajo
(8, 1, 21),  -- Salbutamol - STOCK BAJO
(5, 2, 22),  -- Loratadina - STOCK BAJO
(3, 1, 23),  -- Cetirizina - STOCK BAJO
(9, 2, 24),  -- Ambroxol - STOCK BAJO
(2, 1, 25),  -- Vitamina C - STOCK BAJO
(7, 2, 26),  -- Complejo B - STOCK BAJO
(4, 1, 27),  -- Hierro + Ácido Fólico - STOCK BAJO
(6, 2, 28),  -- Calcio + Vitamina D - STOCK BAJO
(1, 1, 29),  -- Metformina - STOCK CRÍTICO
(9, 2, 30),  -- Glibenclamida - STOCK BAJO

-- Medicamentos nuevos - algunos con stock normal, otros bajo
(8, 1, 31),  -- Clonazepam - STOCK BAJO
(45, 2, 32), -- Diclofenac - STOCK NORMAL
(5, 1, 33),  -- Ketorolac - STOCK BAJO
(35, 2, 34), -- Nimesulida - STOCK NORMAL
(7, 1, 35),  -- Azitromicina - STOCK BAJO
(55, 2, 36), -- Claritromicina - STOCK NORMAL
(3, 1, 37),  -- Ciprofloxacina - STOCK BAJO
(25, 2, 38), -- Fluconazol - STOCK NORMAL
(6, 1, 39),  -- Montelukast - STOCK BAJO
(40, 2, 40), -- Budesonida - STOCK NORMAL
(9, 1, 41),  -- Prednisona - STOCK BAJO
(30, 2, 42), -- Dexametasona - STOCK NORMAL
(4, 1, 43),  -- Furosemida - STOCK BAJO
(50, 2, 44), -- Espironolactona - STOCK NORMAL
(8, 1, 45),  -- Captopril - STOCK BAJO
(35, 2, 46), -- Carvedilol - STOCK NORMAL
(2, 1, 47),  -- Simvastatina - STOCK CRÍTICO
(60, 2, 48), -- Atorvastatina - STOCK NORMAL
(5, 1, 49),  -- Fenobarbital - STOCK BAJO
(25, 2, 50), -- Fenitoína - STOCK NORMAL

-- Stock en sucursal 3 (Córdoba) para algunos medicamentos
(15, 3, 1),  -- Medicamento1 en Córdoba
(8, 3, 2),   -- Medicamento2 - STOCK BAJO en Córdoba
(25, 3, 4),  -- Tafirol en Córdoba
(6, 3, 5),   -- Actron - STOCK BAJO en Córdoba
(40, 3, 10), -- Clavulin en Córdoba
(3, 3, 21),  -- Salbutamol - STOCK CRÍTICO en Córdoba
(7, 3, 25),  -- Vitamina C - STOCK BAJO en Córdoba
(1, 3, 29),  -- Metformina - STOCK CRÍTICO en Córdoba
(9, 3, 31),  -- Clonazepam - STOCK BAJO en Córdoba
(4, 3, 37); -- Ciprofloxacina - STOCK BAJO en Córdoba

-- Recetas de ejemplo
INSERT INTO Recetas (nomMedico, apeMedico, matricula, fecha, diagnostico, codigo, estado, codObraSocial, codCliente, codTipoReceta) VALUES
('Dr. Carlos', 'Médico', 'MP12345', GETDATE(), 'Dolor de cabeza', 12345, 'Activa', 1, 1, 1),
('Dra. María', 'González', 'MP67890', GETDATE(), 'Infección respiratoria', 67890, 'Activa', 2, 2, 1);

-- Detalles de recetas
INSERT INTO Detalles_Receta (cantidad, codMedicamento, cod_Receta) VALUES
(1, 2, 1), -- Medicamento2 para receta 1
(2, 1, 2); -- Medicamento1 para receta 2

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
(100, 120.00, 1, 1), -- Compra Medicamento1
(50, 180.00, 2, 2);  -- Compra Medicamento2

-- Detalles facturas compra artículos
INSERT INTO DetallesFacturaCompraArticulo (cantidad, precioUnitario, codFacturaCompra, codArticulo) VALUES
(30, 280.00, 1, 1), -- Compra Shampoo
(20, 220.00, 2, 2); -- Compra Crema

-- Proveedores de ejemplo (movido aquí para orden lógico)
INSERT INTO Proveedores (razon_Social, cuit, nro_Tel) VALUES
('Laboratorios Bagó', '30-12345678-9', '011-4000-1000'),
('Roemmers SA', '30-87654321-2', '011-4000-2000'),
('Laboratorios Phoenix', '30-11223344-5', '011-4000-3000');

-- *** NOTA: Los usuarios ya fueron insertados al principio del script ***
-- *** El trigger automáticamente asignará el admin (ID=1) a cada nueva sucursal ***
-- *** Empleados y clientes ya fueron insertados arriba para respetar dependencias ***

-- Facturas de ejemplo para testing
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total)
VALUES 
(DATEADD(DAY, -7, GETDATE()), 1, 1, 1, 1, 1500.00), -- hace una semana
(GETDATE(), 2, 2, 1, 2, 850.00), -- hoy
(DATEADD(DAY, 7, GETDATE()), 3, 3, 2, 3, 2200.00); -- dentro de una semana

-- Facturas adicionales para testeo de fechas entre hace una semana y dentro de una semana
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total) VALUES (DATEADD(DAY, -5, GETDATE()), 1, 2, 1, 1, 1200.00); -- hace 5 días
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total) VALUES (DATEADD(DAY, -3, GETDATE()), 2, 3, 2, 2, 950.00); -- hace 3 días
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total) VALUES (DATEADD(DAY, -1, GETDATE()), 3, 1, 3, 3, 1750.00); -- ayer
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total) VALUES (DATEADD(DAY, 1, GETDATE()), 1, 3, 1, 2, 1100.00); -- mañana
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total) VALUES (DATEADD(DAY, 3, GETDATE()), 2, 1, 2, 1, 1350.00); -- en 3 días
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total) VALUES (DATEADD(DAY, 5, GETDATE()), 3, 2, 3, 2, 2000.00); -- en 5 días

-- AHORA SÍ: Detalles de facturas (después de crear las facturas)
-- Detalles de medicamentos para las facturas
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta)
VALUES 
(2, 150.00, 1, 1, 1), -- Medicamento1 con cobertura en factura 1
(1, 200.00, 2, 2, 1), -- Medicamento2 con cobertura en factura 1
(3, 120.00, NULL, 3, 2), -- Medicamento3 sin cobertura en factura 2
(1, 150.00, 1, 1, 3); -- Medicamento1 con cobertura en factura 3

-- Detalles para facturas nuevas (IDs: 4 a 9)
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (1, 200.00, 2, 2, 4); -- Amoxidal en factura 4
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (2, 120.00, NULL, 3, 4); -- Medicamento3 sin cobertura en factura 4
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (1, 450.00, 1, 4, 5); -- Tafirol en factura 5
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (2, 620.00, NULL, 5, 5); -- Actron sin cobertura en factura 5
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (1, 890.00, 2, 6, 6); -- Amoxidal en factura 6
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (1, 1150.00, NULL, 7, 6); -- Clavulin sin cobertura en factura 6
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (2, 480.00, 1, 8, 7); -- Eritromicina en factura 7
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (1, 520.00, NULL, 9, 7); -- Cefalexina sin cobertura en factura 7
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (1, 680.00, 2, 10, 8); -- Buscapina en factura 8
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (2, 850.00, NULL, 11, 8); -- Mylanta sin cobertura en factura 8
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (1, 1200.00, 1, 12, 9); -- Omeprazol en factura 9
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES (1, 1750.00, NULL, 13, 9); -- Ranitidina sin cobertura en factura 9

-- Detalles de artículos para las facturas
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo)
VALUES 
(1, 350.00, 1, 1), -- Shampoo en factura 1
(2, 280.00, 2, 2), -- Crema en factura 2
(1, 1200.00, 3, 3), -- Perfume en factura 3
(1, 450.00, 3, 4); -- Termómetro en factura 3

-- Detalles de artículos para facturas nuevas (IDs: 4 a 9)
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo) VALUES (1, 350.00, 4, 1); -- Shampoo en factura 4
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo) VALUES (2, 280.00, 5, 2); -- Crema en factura 5
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo) VALUES (1, 1200.00, 6, 3); -- Perfume en factura 6
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo) VALUES (1, 450.00, 7, 4); -- Termómetro en factura 7
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo) VALUES (2, 950.00, 8, 5); -- Jabón en factura 8
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo) VALUES (1, 1850.00, 9, 6); -- Protector solar en factura 9

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

-- =============================================
-- Stored Procedure: sp_UpdateUserFromToken
-- Descripción: Crea usuario y asigna las 3 sucursales automáticamente
-- =============================================

CREATE OR ALTER PROCEDURE sp_UpdateUserFromToken
    @UserId INT,
    @Username NVARCHAR(255),
    @Email NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        PRINT 'Iniciando sp_UpdateUserFromToken para usuario: ' + @Username;
        
        -- Verificar si el usuario ya existe
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Cod_Usuario = @UserId)
        BEGIN
            -- Crear el usuario
            INSERT INTO Usuario (Cod_Usuario, Username, Email)
            VALUES (@UserId, @Username, @Email);
            
            PRINT 'Usuario creado: ' + @Username;
        END
        ELSE
        BEGIN
            -- Actualizar datos del usuario existente
            UPDATE Usuario 
            SET Username = @Username, Email = @Email
            WHERE Cod_Usuario = @UserId;
            
            PRINT 'Usuario actualizado: ' + @Username;
        END
        
        -- Asignar las 3 sucursales automáticamente
        -- Verificar y crear asignaciones que no existan
        
        -- Sucursal 1 (Buenos Aires)
        IF NOT EXISTS (SELECT 1 FROM Grupsucursales WHERE cod_usuario = @UserId AND cod_sucursal = 1)
        BEGIN
            INSERT INTO Grupsucursales (cod_usuario, cod_sucursal, activo)
            VALUES (@UserId, 1, 1);
            PRINT 'Asignada sucursal 1 (Buenos Aires)';
        END
        
        -- Sucursal 2 (Mendoza) 
        IF NOT EXISTS (SELECT 1 FROM Grupsucursales WHERE cod_usuario = @UserId AND cod_sucursal = 2)
        BEGIN
            INSERT INTO Grupsucursales (cod_usuario, cod_sucursal, activo)
            VALUES (@UserId, 2, 1);
            PRINT 'Asignada sucursal 2 (Mendoza)';
        END
        
        -- Sucursal 3 (Córdoba)
        IF NOT EXISTS (SELECT 1 FROM Grupsucursales WHERE cod_usuario = @UserId AND cod_sucursal = 3)
        BEGIN
            INSERT INTO Grupsucursales (cod_usuario, cod_sucursal, activo)
            VALUES (@UserId, 3, 1);
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
GO