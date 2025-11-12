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
('MET Medicina Privada', '011-4321-0000', 'contacto@met.medicinaprivada.com.ar'),
('Medife', '011-4332-0001', 'info@medife.com.ar'),
('OSDE', '011-4343-0002', 'atencion@osde.com.ar'),
('Swiss Medical', '011-4354-0003', 'consultas@swissmedical.com.ar');

-- Sucursales de ejemplo
INSERT INTO Sucursales (nom_Sucursal, nro_Tel, calle, altura, email, horarioApertura, horarioCierre, cod_Localidad)
VALUES 
('Sucursal Nueva Cordoba', '011-1111-0001', 'Av. Hipólito Yrigoyen', 312, 'nvacba@farmacia.com', '08:00:00', '22:00:00', 1),
('Sucursal Cerro De Las Rosas', '0221-2222-0002', 'Av. Rafael Núñez', 4235, 'cerro@farmacia.com', '08:30:00', '21:30:00', 2),
('Sucursal Jardin', '0351-3333-0003', 'Bv Elias Yofre', 756, 'jardin@farmacia.com', '09:00:00', '21:00:00', 4);

-- Proveedores de ejemplo
INSERT INTO Proveedores (razon_Social, cuit, nro_Tel) VALUES
('Farmacéutica Del Plata SA', '30-11111111-1', '011-4000-1001'),
('Distribuidora Medicinal Argentina SRL', '30-22222222-2', '011-4000-2002'),
('Grupo Farmacológico Central SA', '30-33333333-3', '011-4000-3003');

-- Laboratorios de ejemplo - EXPANDIDO para simular farmacia real
INSERT INTO Laboratorios (descripcion, cod_Proveedor) VALUES
('Leti Argentina', 1),
('Sidus', 2),
('Bristol‑Myers Squibb Argentina', 3),
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
('Phoenix', 3),
('Elea', 1),
('Richmond', 2),
('Andrómaco', 3),
('Casasco', 1),
('Bernabó', 2),
('Montpellier', 3),
('Raffo', 1),
('Rontag', 2);

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
('7790500000001', 'Meloxicam 15mg', 1, 0, 1200.00, 15, 'Tomar 1 comprimido por día', 41, 21, 1, 1, 3),
('7790500000002', 'Celecoxib 200mg', 1, 0, 3500.00, 200, 'Tomar 1 cápsula por día', 42, 22, 1, 1, 3),
('7790500000003', 'Metronidazol 500mg', 1, 0, 900.00, 500, 'Tomar 1 comprimido cada 8 horas por 7 días', 43, 23, 1, 1, 2),
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
('7801111110001', 'Head & Shoulders Anticaspa 400ml', 850.00, 1, 1),
('7801111110002', 'Pantene Reparación Intensa Acondicionador 400ml', 780.00, 2, 1),
('7801111110003', 'Colgate Total Protección 90g', 420.00, 3, 1),
('7801111110004', 'Nivea Crema Hidratante Facial 50ml', 1250.00, 1, 2),
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
('Miguel', 'Rojas', '011-2222-2222', 'Calle Ejemplo', 200, 'rmiguel@farmacia.ejemplo.com', '2024-02-01', 2, 1, 1),
('Claudio', 'Eleuterio', '0221-3333-3333', 'Av. Muestra', 300, 'eclaudio@farmacia.ejemplo.com', '2024-03-01', 1, 1, 2),
('Sofia', 'Martinez', '0221-4444-4444', 'Calle Córdoba', 456, 'smartinez@farmacia.ejemplo.com', '2024-04-10', 5, 1, 2),
('Ricardo', 'Fernandez', '0351-5555-5555', 'Av. Colón', 789, 'rfernandez@farmacia.ejemplo.com', '2024-05-20', 1, 1, 3),
('Lucia', 'Garcia', '0351-6666-6666', 'Bv. San Juan', 1234, 'lgarcia@farmacia.ejemplo.com', '2024-06-15', 3, 1, 3);

-- Clientes de ejemplo (NECESARIOS ANTES que las coberturas)
INSERT INTO Clientes (nomCliente, apeCliente, nroDoc, nroTel, calle, altura, email, cod_Tipo_Documento, cod_Obra_Social)
VALUES
('Tomas', 'Carrizo', '3514347346', '011-9999-1111', 'Av. Bv San Juan', 356, 'cliente1@email.ejemplo.com', 1, 1),
('Roberto', 'Gomez', '114567379', '011-9999-2222', 'Francisco de Quevedo', 2036, 'cliente2@email.ejemplo.com', 1, 2),
('Laura', 'Martinez', '3541395715', '0221-9999-3333', 'Av. Lopez y Planes', 3038, 'cliente3@email.ejemplo.com', 1, 3),
-- Clientes adicionales para pruebas
('María', 'González', '25789456', '011-5555-1234', 'Av. Rivadavia', 1250, 'maria.gonzalez@email.com', 1, 1),
('Juan', 'Pérez', '33456789', '0351-4444-5678', 'San Martín', 789, 'juan.perez@email.com', 1, 2),
('Carolina', 'Fernández', '28654321', '0221-3333-9876', 'Belgrano', 456, 'carolina.fernandez@email.com', 1, 3),
('Diego', 'López', '31234567', '011-6666-4321', 'Av. Corrientes', 2100, 'diego.lopez@email.com', 1, 4),
('Valeria', 'Rodríguez', '29876543', '0351-2222-8765', 'Independencia', 567, 'valeria.rodriguez@email.com', 1, 1),
('Sebastián', 'Torres', 'AB123456', '011-7777-5432', 'Mitre', 890, 'sebastian.torres@email.com', 2, 2),
('Lucía', 'Ramírez', '27543210', '0221-1111-6543', 'Av. Libertador', 1500, 'lucia.ramirez@email.com', 1, 3),
('Matías', 'Silva', '30987654', '011-8888-7654', '9 de Julio', 345, 'matias.silva@email.com', 1, 4),
('Florencia', 'Morales', '26321098', '0351-5555-2345', 'Colón', 678, 'florencia.morales@email.com', 1, 1),
('Agustín', 'Castro', '32109876', '0221-6666-3456', 'Sarmiento', 234, 'agustin.castro@email.com', 1, 2),
('Gabriela', 'Benítez', 'CD789012', '011-9999-8765', 'Av. Belgrano', 1800, 'gabriela.benitez@email.com', 2, 3),
('Facundo', 'Vega', '28765432', '0351-7777-4567', 'Tucumán', 456, 'facundo.vega@email.com', 1, 4),
('Natalia', 'Romero', '27890123', '0221-8888-5678', 'Moreno', 123, 'natalia.romero@email.com', 1, 1),
('Nicolás', 'Díaz', '31456789', '011-2222-9876', 'Av. Santa Fe', 2500, 'nicolas.diaz@email.com', 1, 2),
('Camila', 'Herrera', '29234567', '0351-3333-6789', 'Entre Ríos', 789, 'camila.herrera@email.com', 1, 3),
('Federico', 'Medina', '30678901', '0221-4444-7890', 'Córdoba', 567, 'federico.medina@email.com', 1, 4),
('Victoria', 'Acosta', 'EF456789', '011-5555-8901', 'Av. Cabildo', 3200, 'victoria.acosta@email.com', 2, 1);

-- Descuentos de ejemplo
INSERT INTO Descuentos (Fecha_Descuento, cod_localidad, cod_medicamento, porcentaje_descuento, cod_tipo_descuento) VALUES
('2025-02-15', 1, 1, 15.00, 1), -- Descuento OSDE para Medicamento cod. 1 en CABA
('2025-03-20', 2, 2, 20.00, 1), -- Descuento OSDE para Medicamento cod. 2 en La Plata
('2025-01-10', 1, 3, 10.00, 2), -- Descuento por promoción para Medicamento cod. 3
('2025-04-05', 1, 5, 25.00, 2), -- Descuento por Promoción para Actron en CABA
('2025-05-12', 2, 10, 18.00, 1), -- Descuento por Obra Social para Clavulin en La Plata
('2025-06-18', 2, 15, 22.00, 4), -- Descuento Cliente Frecuente para Omeprazol en La Plata
('2025-07-22', 4, 4, 35.00, 2), -- Descuento por Promoción para Tafirol en Córdoba Capital
('2025-08-30', 4, 12, 40.00, 3), -- Descuento por Volumen para Cefalexina en Córdoba Capital
('2025-09-14', 4, 18, 28.00, 1); -- Descuento por Obra Social para Losartan en Córdoba Capital

-- Coberturas de ejemplo (DESPUÉS de clientes, obras sociales y descuentos)
INSERT INTO Coberturas (fechaInicio, fechaFin, cod_Localidad, cod_cliente, cod_Obra_Social, cod_descuento) VALUES
('2024-01-01', '2024-12-31', 1, 1, 1, 1), -- Ana con OSDE en CABA
('2024-01-01', '2024-12-31', 2, 2, 2, 2), -- Roberto con Swiss Medical en La Plata
('2024-01-01', '2024-12-31', 1, 3, 3, 1), -- Laura con Galeno en CABA (usando descuento 1)
('2025-01-01', '2025-12-31', 1, 4, 1, 4), -- María González con MET en CABA
('2025-01-01', '2025-12-31', 4, 5, 2, 7), -- Juan Pérez con Medife en Córdoba Capital
('2025-02-01', '2025-12-31', 2, 6, 3, 5), -- Carolina Fernández con OSDE en La Plata
('2025-01-15', '2025-12-31', 1, 7, 4, 4), -- Diego López con Swiss Medical en CABA
('2025-03-01', '2025-12-31', 4, 8, 1, 8), -- Valeria Rodríguez con MET en Córdoba Capital
('2025-01-01', '2025-06-30', 2, 10, 3, 6), -- Lucía Ramírez con OSDE en La Plata
('2025-02-15', '2025-12-31', 4, 12, 1, 9); -- Florencia Morales con MET en Córdoba Capital

-- Stock de artículos
INSERT INTO Stock_Articulos (cantidad, codSucursal, codArticulo) VALUES
-- Stock para todos los artículos distribuidos entre las 3 sucursales
(80, 1, 1), (70, 2, 1), -- Head & Shoulders Anticaspa 400ml
(90, 1, 2), (60, 3, 2), -- Pantene Reparación Intensa Acondicionador 400ml
(100, 2, 3), (50, 1, 3), -- Colgate Total Protección 90g
(120, 3, 4), -- Nivea Crema Hidratante Facial 50ml
(150, 1, 5), (80, 2, 5), -- Shampoo Anticaspa Head & Shoulders 400ml
(110, 2, 6), (90, 3, 6), -- Acondicionador Pantene Reparación 400ml
(140, 1, 7), (70, 3, 7), -- Jabón Líquido Dove 250ml
(130, 2, 8), (60, 1, 8), -- Desodorante Rexona Aerosol 150ml
(200, 3, 9), (100, 1, 9), -- Crema Dental Colgate Total 90g
(90, 1, 10), (80, 2, 10), -- Enjuague Bucal Listerine 500ml
(70, 2, 11), (60, 3, 11), -- Cepillo de Dientes Oral-B
(160, 1, 12), (90, 3, 12), -- Papel Higiénico Elite x4
(110, 2, 13), (70, 1, 13), -- Toallas Femeninas Always x16
(80, 3, 14), (50, 2, 14), -- Crema Hidratante Nivea 200ml
(60, 1, 15), (40, 3, 15), -- Protector Solar Eucerin FPS 60
(90, 2, 16), (50, 1, 16), -- Base de Maquillaje Maybelline
(70, 1, 17), (40, 3, 17), -- Labial Revlon Color Rosa
(80, 2, 18), (50, 1, 18), -- Rimmel Máscara de Pestañas
(60, 3, 19), (30, 2, 19), -- Crema Antiarrugas Olay 50ml
(50, 1, 20), (40, 2, 20), -- Perfume Antonio Banderas 100ml
(40, 2, 21), (30, 3, 21), -- Perfume Paco Rabanne 80ml
(100, 1, 22), (70, 2, 22), -- Colonia Agua de Rosas 500ml
(120, 2, 23), (80, 3, 23), -- Desodorante Roll-On Nivea 50ml
(150, 1, 24), (100, 2, 24), -- Pañales Pampers M x30
(110, 3, 25), (80, 1, 25), -- Shampoo Johnson Baby 400ml
(90, 2, 26), (60, 3, 26), -- Crema Pañalitis Bepanthen 100g
(130, 1, 27), (90, 2, 27), -- Toallitas Húmedas Huggies x80
(100, 2, 28), (70, 3, 28), -- Aceite Johnson Baby 200ml
(80, 1, 29), (50, 2, 29), -- Mamaderas Chicco 260ml
(90, 3, 30), (60, 1, 30), -- Chupetes MAM x2
(50, 1, 31), (40, 2, 31), -- Termómetro Digital
(30, 2, 32), (20, 3, 32), -- Tensiómetro Digital Brazo
(40, 3, 33), (30, 1, 33), -- Glucómetro One Touch + 25 tiras
(50, 1, 34), (30, 2, 34), -- Nebulizador Ultrasonico
(200, 2, 35), (150, 3, 35), -- Jeringas Descartables 5ml x10
(180, 1, 36), (120, 2, 36), -- Gasas Estériles 10x10 x25
(250, 2, 37), (200, 3, 37), -- Alcohol en Gel 250ml
(300, 1, 38), (200, 2, 38), -- Barbijos Quirúrgicos x50
(140, 3, 39), (100, 1, 39), -- Vendas Elásticas 10cm
(220, 2, 40), (180, 3, 40), -- Curitas Adhesivas x40
(190, 1, 41), (140, 2, 41), -- Algodón Hidrófilo 100g
(150, 2, 42), (110, 3, 42), -- Agua Oxigenada 10vol 250ml
(170, 3, 43), (130, 1, 43), -- Alcohol Etílico 70° 250ml
(120, 1, 44), (90, 2, 44), -- Iodopovidona Solución 120ml
(70, 2, 45), (50, 3, 45), -- Omega 3 en Cápsulas x60
(80, 1, 46), (60, 2, 46), -- Magnesio + B6 x30
(90, 3, 47), (60, 1, 47), -- Probióticos en Cápsulas x30
(70, 2, 48), (50, 3, 48), -- Coenzima Q10 x60
(100, 1, 49), (80, 3, 49), -- Shampoo en sucursal 1 y 3 adicional
(110, 2, 50), (90, 1, 50), -- Crema en sucursal 2 y 1 adicional
(130, 3, 51), (100, 2, 51), -- Perfume en sucursal 3 y 2 adicional
(140, 1, 52), (110, 3, 52); -- Termómetro en sucursal 1 y 3 adicional

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
('Dr. Carlos', 'Menem', 'MP12345', '2025-01-05', 'Dolor de cabeza', 12345, 'Activa', 1, 1, 1),
('Dra. María', 'González', 'MP67890', '2025-01-08', 'Infección respiratoria', 67890, 'Activa', 2, 2, 1),
('Dr. Roberto', 'Silva', 'MP23456', '2025-01-15', 'Hipertensión arterial', 23456, 'Activa', 1, 3, 1),
('Dra. Laura', 'Fernandez', 'MP34567', '2025-02-10', 'Dolor muscular', 34567, 'Pendiente', 3, 4, 1),
('Dr. Martín', 'Rodríguez', 'MP45678', '2025-02-28', 'Alergia estacional', 45678, 'Activa', 2, 5, 1),
('Dra. Patricia', 'Lopez', 'MP56789', '2025-03-15', 'Infección urinaria', 56789, 'Cancelada', 4, 6, 1),
('Dr. Diego', 'Martinez', 'MP67891', '2025-04-05', 'Gastritis', 67891, 'Activa', 1, 7, 2),
('Dra. Ana', 'Torres', 'MP78912', '2025-05-12', 'Diabetes tipo 2', 78912, 'Activa', 3, 8, 1),
('Dr. Javier', 'Gomez', 'MP89123', '2025-06-20', 'Bronquitis', 89123, 'Pendiente', 2, 9, 1),
('Dra. Claudia', 'Ramirez', 'MP91234', '2025-07-08', 'Dolor de garganta', 91234, 'Activa', 1, 10, 1),
('Dr. Fernando', 'Castro', 'MP12347', '2025-08-14', 'Migraña crónica', 12347, 'Activa', 4, 11, 1),
('Dra. Silvia', 'Morales', 'MP23458', '2025-09-25', 'Artritis', 23458, 'Pendiente', 2, 12, 2),
('Dr. Pablo', 'Herrera', 'MP34569', '2025-10-10', 'Rinitis alérgica', 34569, 'Activa', 3, 13, 1),
('Dra. Monica', 'Vega', 'MP45670', '2025-11-01', 'Faringitis', 45670, 'Cancelada', 1, 14, 1),
('Dr. Gustavo', 'Diaz', 'MP56781', '2025-11-08', 'Asma bronquial', 56781, 'Activa', 2, 15, 2);

-- Detalles de recetas
INSERT INTO Detalles_Receta (cantidad, codMedicamento, cod_Receta) VALUES
(1, 2, 1), -- Medicamento2 para receta 1
(2, 1, 2), -- Medicamento1 para receta 2
(1, 18, 3), -- Losartan para receta 3 (Hipertensión)
(2, 8, 4), -- Ibupirac para receta 4 (Dolor muscular)
(1, 22, 5), -- Loratadina para receta 5 (Alergia)
(2, 10, 6), -- Clavulin para receta 6 (Infección urinaria)
(1, 15, 7), -- Omeprazol para receta 7 (Gastritis)
(2, 29, 8), -- Metformina para receta 8 (Diabetes)
(1, 24, 9), -- Ambroxol para receta 9 (Bronquitis)
(2, 11, 10), -- Eritromicina para receta 10 (Dolor de garganta)
(1, 5, 11), -- Actron para receta 11 (Migraña)
(2, 32, 12), -- Diclofenac para receta 12 (Artritis)
(1, 23, 13), -- Cetirizina para receta 13 (Rinitis alérgica)
(2, 12, 14), -- Cefalexina para receta 14 (Faringitis)
(1, 21, 15); -- Salbutamol para receta 15 (Asma)

-- Autorizaciones
INSERT INTO Autorizaciones (codigo, estado, fechaAutorizacion, codObraSocial, codReceta) VALUES
(100001, 'Autorizada', '2025-01-06', 1, 1),
(100002, 'Autorizada', '2025-01-09', 2, 2),
(100003, 'Autorizada', '2025-01-16', 1, 3),
(100004, 'Pendiente', '2025-02-11', 3, 4),
(100005, 'Autorizada', '2025-03-01', 2, 5),
(100006, 'Rechazada', '2025-03-16', 4, 6),
(100007, 'Autorizada', '2025-04-06', 1, 7),
(100008, 'Autorizada', '2025-05-13', 3, 8),
(100009, 'Pendiente', '2025-06-21', 2, 9),
(100010, 'Autorizada', '2025-07-09', 1, 10),
(100011, 'Autorizada', '2025-08-15', 4, 11),
(100012, 'Pendiente', '2025-09-26', 2, 12),
(100013, 'Autorizada', '2025-10-11', 3, 13),
(100014, 'Rechazada', '2025-11-02', 1, 14),
(100015, 'Autorizada', '2025-11-09', 2, 15);

-- ====================================================================================================
-- PASO 1: Facturas de compra basadas en Stock_Medicamentos y Stock_Articulos
-- ====================================================================================================
-- Cada fila de stock tiene una factura de compra con la misma sucursal
-- Empleado aleatorio con codTipoEmpleado entre 1 y 4 (Farmacéutico, Técnico, Administrativo, Gerente)
-- Fecha aleatoria entre 01/01/2025 y 11/11/2025

-- Facturas de compra para Stock_Medicamentos (60 facturas)
INSERT INTO Facturas_Compra (fecha, cod_Empleado, cod_Sucursal, cod_Proveedor) VALUES
-- Sucursal 1 - Stock_Medicamentos
('2025-01-05', 1, 1, 1), ('2025-01-08', 2, 1, 2), ('2025-01-12', 1, 1, 3), ('2025-01-18', 2, 1, 1),
('2025-01-22', 1, 1, 2), ('2025-01-25', 2, 1, 3), ('2025-02-03', 1, 1, 1), ('2025-02-08', 2, 1, 2),
('2025-02-15', 1, 1, 3), ('2025-02-20', 2, 1, 1), ('2025-02-25', 1, 1, 2), ('2025-03-02', 2, 1, 3),
('2025-03-08', 1, 1, 1), ('2025-03-12', 2, 1, 2), ('2025-03-18', 1, 1, 3), ('2025-03-25', 2, 1, 1),
('2025-04-02', 1, 1, 2), ('2025-04-08', 2, 1, 3), ('2025-04-15', 1, 1, 1), ('2025-04-22', 2, 1, 2),
('2025-05-05', 1, 1, 3), ('2025-05-10', 2, 1, 1), ('2025-05-18', 1, 1, 2), ('2025-05-25', 2, 1, 3),
('2025-06-03', 1, 1, 1), ('2025-06-10', 2, 1, 2), ('2025-06-18', 1, 1, 3), ('2025-06-25', 2, 1, 1),
('2025-07-05', 1, 1, 2), ('2025-07-12', 2, 1, 3), ('2025-07-20', 1, 1, 1), ('2025-08-05', 2, 1, 2),
('2025-08-15', 1, 1, 3), ('2025-08-25', 2, 1, 1), ('2025-09-05', 1, 1, 2), ('2025-09-15', 2, 1, 3),
('2025-09-25', 1, 1, 1), ('2025-10-05', 2, 1, 2), ('2025-10-15', 1, 1, 3), ('2025-10-25', 2, 1, 1),
-- Sucursal 2 - Stock_Medicamentos  
('2025-01-07', 3, 2, 1), ('2025-01-14', 3, 2, 2), ('2025-01-20', 3, 2, 3), ('2025-01-28', 3, 2, 1),
('2025-02-05', 3, 2, 2), ('2025-02-12', 3, 2, 3), ('2025-02-18', 3, 2, 1), ('2025-02-24', 3, 2, 2),
('2025-03-05', 3, 2, 3), ('2025-03-14', 3, 2, 1), ('2025-03-22', 3, 2, 2), ('2025-04-01', 3, 2, 3),
('2025-04-10', 3, 2, 1), ('2025-04-18', 3, 2, 2), ('2025-05-02', 3, 2, 3), ('2025-05-12', 3, 2, 1),
('2025-05-22', 3, 2, 2), ('2025-06-05', 3, 2, 3), ('2025-06-15', 3, 2, 1), ('2025-06-28', 3, 2, 2),
-- Sucursal 3 - Stock_Medicamentos
('2025-01-10', 5, 3, 1), ('2025-01-16', 6, 3, 2), ('2025-02-10', 5, 3, 3), ('2025-03-10', 6, 3, 1),
('2025-04-12', 5, 3, 2), ('2025-05-15', 6, 3, 3), ('2025-06-20', 5, 3, 1), ('2025-07-18', 6, 3, 2),
('2025-08-20', 5, 3, 3), ('2025-09-22', 6, 3, 1);

-- Facturas de compra para Stock_Articulos (96 facturas)
INSERT INTO Facturas_Compra (fecha, cod_Empleado, cod_Sucursal, cod_Proveedor) VALUES
-- Sucursal 1 - Stock_Articulos
('2025-01-06', 1, 1, 1), ('2025-01-09', 2, 1, 1), ('2025-01-13', 1, 1, 1), ('2025-01-17', 2, 1, 1),
('2025-01-23', 1, 1, 1), ('2025-01-27', 2, 1, 1), ('2025-02-04', 1, 1, 1), ('2025-02-09', 2, 1, 1),
('2025-02-16', 1, 1, 1), ('2025-02-21', 2, 1, 1), ('2025-02-26', 1, 1, 1), ('2025-03-04', 2, 1, 1),
('2025-03-10', 1, 1, 1), ('2025-03-15', 2, 1, 1), ('2025-03-20', 1, 1, 1), ('2025-03-26', 2, 1, 1),
('2025-04-04', 1, 1, 1), ('2025-04-10', 2, 1, 1), ('2025-04-16', 1, 1, 1), ('2025-04-24', 2, 1, 1),
('2025-05-06', 1, 1, 1), ('2025-05-13', 2, 1, 1), ('2025-05-20', 1, 1, 1), ('2025-05-27', 2, 1, 1),
('2025-06-04', 1, 1, 1), ('2025-06-12', 2, 1, 1), ('2025-06-20', 1, 1, 1), ('2025-06-27', 2, 1, 1),
('2025-07-06', 1, 1, 1), ('2025-07-14', 2, 1, 1), ('2025-07-22', 1, 1, 1), ('2025-08-06', 2, 1, 1),
('2025-08-16', 1, 1, 1), ('2025-08-26', 2, 1, 1), ('2025-09-06', 1, 1, 1), ('2025-09-16', 2, 1, 1),
('2025-09-26', 1, 1, 1), ('2025-10-06', 2, 1, 1), ('2025-10-16', 1, 1, 1), ('2025-10-26', 2, 1, 1),
-- Sucursal 2 - Stock_Articulos
('2025-01-11', 3, 2, 1), ('2025-01-15', 3, 2, 1), ('2025-01-21', 3, 2, 1), ('2025-02-01', 3, 2, 1),
('2025-02-06', 3, 2, 1), ('2025-02-13', 3, 2, 1), ('2025-02-19', 3, 2, 1), ('2025-02-27', 3, 2, 1),
('2025-03-06', 3, 2, 1), ('2025-03-16', 3, 2, 1), ('2025-03-24', 3, 2, 1), ('2025-04-03', 3, 2, 1),
('2025-04-12', 3, 2, 1), ('2025-04-20', 3, 2, 1), ('2025-05-04', 3, 2, 1), ('2025-05-14', 3, 2, 1),
('2025-05-24', 3, 2, 1), ('2025-06-06', 3, 2, 1), ('2025-06-17', 3, 2, 1), ('2025-07-01', 3, 2, 1),
('2025-07-10', 3, 2, 1), ('2025-07-24', 3, 2, 1), ('2025-08-08', 3, 2, 1), ('2025-08-18', 3, 2, 1),
('2025-08-28', 3, 2, 1), ('2025-09-10', 3, 2, 1), ('2025-09-20', 3, 2, 1), ('2025-10-02', 3, 2, 1),
('2025-10-12', 3, 2, 1), ('2025-10-22', 3, 2, 1), ('2025-11-01', 3, 2, 1), ('2025-11-08', 3, 2, 1),
-- Sucursal 3 - Stock_Articulos
('2025-01-19', 5, 3, 1), ('2025-02-02', 6, 3, 1), ('2025-02-11', 5, 3, 1), ('2025-02-22', 6, 3, 1),
('2025-03-03', 5, 3, 1), ('2025-03-13', 6, 3, 1), ('2025-03-23', 5, 3, 1), ('2025-04-05', 6, 3, 1),
('2025-04-14', 5, 3, 1), ('2025-04-26', 6, 3, 1), ('2025-05-08', 5, 3, 1), ('2025-05-19', 6, 3, 1),
('2025-05-29', 5, 3, 1), ('2025-06-09', 6, 3, 1), ('2025-06-22', 5, 3, 1), ('2025-07-03', 6, 3, 1),
('2025-07-16', 5, 3, 1), ('2025-07-26', 6, 3, 1), ('2025-08-10', 5, 3, 1), ('2025-08-22', 6, 3, 1),
('2025-09-02', 5, 3, 1), ('2025-09-14', 6, 3, 1), ('2025-09-28', 5, 3, 1), ('2025-10-10', 6, 3, 1);

-- Detalles facturas compra medicamentos (60 detalles)
INSERT INTO DetallesFacturaCompraMedicamento (cantidad, precioUnitario, codFacturaCompra, codMedicamento) VALUES
-- Sucursal 1 medicamentos (40 entries)
(150, 1200.00, 1, 1), (120, 3500.00, 2, 2), (180, 450.00, 3, 4), (110, 380.00, 4, 6),
(160, 680.00, 5, 8), (135, 1250.00, 6, 10), (100, 980.00, 7, 12), (65, 520.00, 8, 14),
(120, 450.00, 9, 16), (110, 850.00, 10, 18), (95, 620.00, 11, 20), (8, 1200.00, 12, 21),
(3, 520.00, 13, 23), (2, 680.00, 14, 25), (4, 750.00, 15, 27), (1, 420.00, 16, 29),
(8, 680.00, 17, 31), (5, 780.00, 18, 33), (7, 950.00, 19, 35), (3, 880.00, 20, 37),
(6, 1250.00, 21, 39), (9, 380.00, 22, 41), (4, 320.00, 23, 43), (8, 290.00, 24, 45),
(2, 380.00, 25, 47), (5, 180.00, 26, 49), (15, 1200.00, 27, 1), (120, 3500.00, 28, 2),
(180, 450.00, 29, 4), (110, 380.00, 30, 6), (160, 680.00, 31, 8), (135, 1250.00, 32, 10),
(100, 980.00, 33, 12), (65, 520.00, 34, 14), (120, 450.00, 35, 16), (110, 850.00, 36, 18),
(95, 620.00, 37, 20), (8, 1200.00, 38, 21), (3, 520.00, 39, 23), (2, 680.00, 40, 25),
-- Sucursal 2 medicamentos (20 entries)
(200, 900.00, 41, 3), (90, 620.00, 42, 5), (95, 520.00, 43, 7), (75, 890.00, 44, 9),
(85, 1150.00, 45, 11), (45, 680.00, 46, 13), (80, 890.00, 47, 15), (90, 680.00, 48, 17),
(85, 720.00, 49, 19), (5, 480.00, 50, 22), (9, 380.00, 51, 24), (7, 850.00, 52, 26),
(6, 920.00, 53, 28), (9, 380.00, 54, 30), (45, 520.00, 55, 32), (35, 420.00, 56, 34),
(55, 1150.00, 57, 36), (25, 320.00, 58, 38), (40, 1450.00, 59, 40), (30, 280.00, 60, 42);

-- Detalles facturas compra artículos (96 detalles)  
INSERT INTO DetallesFacturaCompraArticulo (cantidad, precioUnitario, codFacturaCompra, codArticulo) VALUES
-- Sucursal 1 artículos (40 entries)
(80, 850.00, 71, 1), (90, 780.00, 72, 2), (50, 420.00, 73, 3), (150, 750.00, 74, 5),
(60, 520.00, 75, 8), (100, 320.00, 76, 9), (90, 650.00, 77, 10), (70, 450.00, 78, 13),
(60, 1850.00, 79, 15), (50, 1250.00, 80, 16), (70, 850.00, 81, 17), (50, 920.00, 82, 18),
(50, 3500.00, 83, 20), (100, 1200.00, 84, 22), (80, 1200.00, 85, 25), (60, 520.00, 86, 28),
(80, 1200.00, 87, 29), (60, 680.00, 88, 30), (50, 1200.00, 89, 31), (30, 2800.00, 90, 33),
(50, 3200.00, 91, 34), (180, 380.00, 92, 36), (300, 1200.00, 93, 38), (100, 650.00, 94, 39),
(190, 320.00, 95, 41), (130, 220.00, 96, 43), (120, 450.00, 97, 44), (80, 920.00, 98, 46),
(60, 1650.00, 99, 47), (100, 750.00, 100, 49), (90, 780.00, 101, 50), (140, 850.00, 102, 52),
(80, 850.00, 103, 1), (90, 780.00, 104, 2), (50, 420.00, 105, 3), (150, 750.00, 106, 5),
(60, 520.00, 107, 8), (100, 320.00, 108, 9), (90, 650.00, 109, 10), (70, 450.00, 110, 13),
-- Sucursal 2 artículos (32 entries)
(70, 850.00, 111, 1), (100, 420.00, 112, 3), (80, 750.00, 113, 5), (110, 780.00, 114, 6),
(130, 480.00, 115, 8), (80, 650.00, 116, 10), (70, 280.00, 117, 11), (110, 580.00, 118, 13),
(50, 950.00, 119, 14), (90, 1250.00, 120, 16), (80, 920.00, 121, 18), (30, 2200.00, 122, 19),
(40, 3500.00, 123, 20), (40, 4200.00, 124, 21), (70, 1200.00, 125, 22), (120, 380.00, 126, 23),
(100, 2800.00, 127, 24), (90, 650.00, 128, 26), (100, 520.00, 129, 28), (50, 1200.00, 130, 29),
(30, 4500.00, 131, 32), (200, 450.00, 132, 35), (120, 380.00, 133, 36), (250, 320.00, 134, 37),
(200, 1200.00, 135, 38), (220, 280.00, 136, 40), (140, 320.00, 137, 41), (150, 180.00, 138, 42),
(90, 450.00, 139, 44), (70, 1850.00, 140, 45), (80, 920.00, 141, 46), (110, 780.00, 142, 50),
-- Sucursal 3 artículos (24 entries)
(60, 780.00, 143, 2), (120, 1250.00, 144, 4), (90, 780.00, 145, 6), (70, 520.00, 146, 7),
(200, 320.00, 147, 9), (60, 280.00, 148, 11), (90, 980.00, 149, 26), (70, 520.00, 150, 28),
(90, 680.00, 151, 30), (20, 4500.00, 152, 32), (40, 2800.00, 153, 33), (150, 450.00, 154, 35),
(200, 320.00, 155, 37), (100, 650.00, 156, 39), (180, 280.00, 157, 40), (110, 180.00, 158, 42),
(170, 220.00, 159, 43), (50, 1850.00, 160, 45), (90, 1650.00, 161, 47), (50, 2200.00, 162, 48),
(80, 750.00, 163, 49), (100, 780.00, 164, 51), (110, 850.00, 165, 52), (60, 780.00, 166, 2);

-- Proveedores de ejemplo (movido aquí para orden lógico)
INSERT INTO Proveedores (razon_Social, cuit, nro_Tel) VALUES
('Laboratorios Bagó', '30-12345678-9', '011-4000-1000'),
('Roemmers SA', '30-87654321-2', '011-4000-2000'),
('Laboratorios Phoenix', '30-11223344-5', '011-4000-3000');

-- *** NOTA: Los usuarios ya fueron insertados al principio del script ***
-- *** El trigger automáticamente asignará el admin (ID=1) a cada nueva sucursal ***
-- *** Empleados y clientes ya fueron insertados arriba para respetar dependencias ***

-- ====================================================================================================
-- PASO 2: Facturas de venta (100 facturas)
-- ====================================================================================================
-- Empleado debe tener acceso a la sucursal (codSucursal del empleado debe coincidir)
-- Cliente aleatorio, forma de pago aleatoria, fecha aleatoria entre 01/01/2025 y 11/11/2025
-- Total se calculará después de cargar los detalles

-- Empleados por sucursal: Sucursal 1: emp 1,2 | Sucursal 2: emp 3,4 | Sucursal 3: emp 5,6
INSERT INTO FacturasVenta (fecha, codEmpleado, codCliente, codSucursal, codFormaPago, total)
VALUES 
-- Sucursal 1 (40 facturas)
('2025-01-02', 1, 1, 1, 1, 1200.00), ('2025-01-05', 2, 2, 1, 2, 850.00), ('2025-01-08', 1, 3, 1, 3, 2200.00),
('2025-01-11', 2, 4, 1, 1, 1500.00), ('2025-01-14', 1, 5, 1, 2, 950.00), ('2025-01-17', 2, 6, 1, 3, 1750.00),
('2025-01-20', 1, 7, 1, 4, 1100.00), ('2025-01-23', 2, 8, 1, 1, 1350.00), ('2025-01-26', 1, 9, 1, 2, 2000.00),
('2025-01-29', 2, 10, 1, 3, 1650.00), ('2025-02-01', 1, 11, 1, 4, 1800.00), ('2025-02-04', 2, 12, 1, 1, 1250.00),
('2025-02-07', 1, 13, 1, 2, 900.00), ('2025-02-10', 2, 14, 1, 3, 1450.00), ('2025-02-13', 1, 15, 1, 4, 1900.00),
('2025-02-16', 2, 16, 1, 1, 1050.00), ('2025-02-19', 1, 17, 1, 2, 1300.00), ('2025-02-22', 2, 18, 1, 3, 2100.00),
('2025-02-25', 1, 19, 1, 4, 1400.00), ('2025-02-28', 2, 20, 1, 1, 1600.00), ('2025-03-03', 1, 1, 1, 2, 1150.00),
('2025-03-06', 2, 2, 1, 3, 950.00), ('2025-03-09', 1, 3, 1, 4, 1550.00), ('2025-03-12', 2, 4, 1, 1, 1700.00),
('2025-03-15', 1, 5, 1, 2, 1250.00), ('2025-03-18', 2, 6, 1, 3, 1850.00), ('2025-03-21', 1, 7, 1, 4, 1450.00),
('2025-03-24', 2, 8, 1, 1, 1200.00), ('2025-03-27', 1, 9, 1, 2, 1650.00), ('2025-03-30', 2, 10, 1, 3, 1950.00),
('2025-04-02', 1, 11, 1, 4, 1350.00), ('2025-04-05', 2, 12, 1, 1, 1100.00), ('2025-04-08', 1, 13, 1, 2, 1500.00),
('2025-04-11', 2, 14, 1, 3, 1800.00), ('2025-04-14', 1, 15, 1, 4, 1400.00), ('2025-04-17', 2, 16, 1, 1, 1750.00),
('2025-04-20', 1, 17, 1, 2, 1250.00), ('2025-04-23', 2, 18, 1, 3, 1600.00), ('2025-04-26', 1, 19, 1, 4, 2000.00),
('2025-04-29', 2, 20, 1, 1, 1450.00),
-- Sucursal 2 (40 facturas)
('2025-05-02', 3, 1, 2, 2, 1300.00), ('2025-05-05', 4, 2, 2, 3, 1150.00), ('2025-05-08', 3, 3, 2, 4, 1700.00),
('2025-05-11', 4, 4, 2, 1, 1900.00), ('2025-05-14', 3, 5, 2, 2, 1050.00), ('2025-05-17', 4, 6, 2, 3, 1550.00),
('2025-05-20', 3, 7, 2, 4, 1800.00), ('2025-05-23', 4, 8, 2, 1, 1200.00), ('2025-05-26', 3, 9, 2, 2, 1650.00),
('2025-05-29', 4, 10, 2, 3, 1950.00), ('2025-06-01', 3, 11, 2, 4, 1400.00), ('2025-06-04', 4, 12, 2, 1, 1100.00),
('2025-06-07', 3, 13, 2, 2, 1500.00), ('2025-06-10', 4, 14, 2, 3, 1750.00), ('2025-06-13', 3, 15, 2, 4, 1250.00),
('2025-06-16', 4, 16, 2, 1, 1600.00), ('2025-06-19', 3, 17, 2, 2, 2000.00), ('2025-06-22', 4, 18, 2, 3, 1450.00),
('2025-06-25', 3, 19, 2, 4, 1300.00), ('2025-06-28', 4, 20, 2, 1, 1850.00), ('2025-07-01', 3, 1, 2, 2, 1150.00),
('2025-07-04', 4, 2, 2, 3, 950.00), ('2025-07-07', 3, 3, 2, 4, 1550.00), ('2025-07-10', 4, 4, 2, 1, 1700.00),
('2025-07-13', 3, 5, 2, 2, 1250.00), ('2025-07-16', 4, 6, 2, 3, 1950.00), ('2025-07-19', 3, 7, 2, 4, 1400.00),
('2025-07-22', 4, 8, 2, 1, 1100.00), ('2025-07-25', 3, 9, 2, 2, 1650.00), ('2025-07-28', 4, 10, 2, 3, 2000.00),
('2025-07-31', 3, 11, 2, 4, 1350.00), ('2025-08-03', 4, 12, 2, 1, 1500.00), ('2025-08-06', 3, 13, 2, 2, 1800.00),
('2025-08-09', 4, 14, 2, 3, 1400.00), ('2025-08-12', 3, 15, 2, 4, 1750.00), ('2025-08-15', 4, 16, 2, 1, 1250.00),
('2025-08-18', 3, 17, 2, 2, 1600.00), ('2025-08-21', 4, 18, 2, 3, 2100.00), ('2025-08-24', 3, 19, 2, 4, 1450.00),
('2025-08-27', 4, 20, 2, 1, 1300.00),
-- Sucursal 3 (20 facturas)
('2025-08-30', 5, 1, 3, 2, 1200.00), ('2025-09-02', 6, 2, 3, 3, 1550.00), ('2025-09-05', 5, 3, 3, 4, 1800.00),
('2025-09-08', 6, 4, 3, 1, 1350.00), ('2025-09-11', 5, 5, 3, 2, 950.00), ('2025-09-14', 6, 6, 3, 3, 1700.00),
('2025-09-17', 5, 7, 3, 4, 1900.00), ('2025-09-20', 6, 8, 3, 1, 1050.00), ('2025-09-23', 5, 9, 3, 2, 1650.00),
('2025-09-26', 6, 10, 3, 3, 2000.00), ('2025-09-29', 5, 11, 3, 4, 1400.00), ('2025-10-02', 6, 12, 3, 1, 1150.00),
('2025-10-05', 5, 13, 3, 2, 1500.00), ('2025-10-08', 6, 14, 3, 3, 1750.00), ('2025-10-11', 5, 15, 3, 4, 1250.00),
('2025-10-14', 6, 16, 3, 1, 1850.00), ('2025-10-17', 5, 17, 3, 2, 1450.00), ('2025-10-20', 6, 18, 3, 3, 1300.00),
('2025-10-23', 5, 19, 3, 4, 1600.00), ('2025-10-26', 6, 20, 3, 1, 2200.00);

-- DetallesFacturaVentasMedicamento (60 ventas de medicamentos)
-- codCobertura solo si el cliente tiene cobertura
-- Solo medicamentos con stock en esa sucursal y cantidad <= stock disponible
INSERT INTO DetallesFacturaVentasMedicamento (cantidad, precioUnitario, codCobertura, codMedicamento, codFacturaVenta) VALUES
-- Sucursal 1 medicamentos (stock disponible: 1,2,4,6,8,10,12,14,16,18,20,21,23,25,27,29,31,33,35,37,39,41,43,45,47,49)
(2, 1200.00, 1, 1, 1), (1, 3500.00, NULL, 2, 2), (2, 450.00, 1, 4, 3), (1, 450.00, 4, 4, 4),
(1, 620.00, NULL, 4, 5), (1, 890.00, NULL, 4, 6), (2, 680.00, NULL, 8, 7), (1, 1250.00, NULL, 10, 8),
(2, 980.00, NULL, 12, 9), (1, 520.00, NULL, 14, 10), (1, 450.00, 4, 16, 11), (2, 850.00, 1, 18, 12),
(1, 620.00, NULL, 20, 13), (2, 1200.00, NULL, 1, 14), (1, 3500.00, 4, 2, 15), (1, 450.00, 1, 4, 16),
(1, 380.00, NULL, 6, 17), (2, 680.00, 1, 8, 18), (1, 1250.00, 4, 10, 19), (2, 980.00, 1, 12, 20),
(1, 520.00, NULL, 14, 21), (1, 450.00, NULL, 16, 22), (2, 850.00, 1, 18, 23), (1, 620.00, 4, 20, 24),
(1, 1200.00, 1, 1, 25), (2, 3500.00, NULL, 2, 26), (1, 450.00, 4, 4, 27), (1, 380.00, 1, 6, 28),
(2, 680.00, NULL, 8, 29), (1, 1250.00, 1, 10, 30), (1, 980.00, 4, 12, 31), (2, 520.00, 1, 14, 32),
(1, 450.00, NULL, 16, 33), (1, 850.00, NULL, 18, 34), (2, 620.00, 4, 20, 35), (1, 1200.00, 1, 1, 36),
(1, 3500.00, NULL, 2, 37), (2, 450.00, 1, 4, 38), (1, 380.00, 4, 6, 39), (1, 680.00, 1, 8, 40),
-- Sucursal 2 medicamentos (stock disponible: 3,5,7,9,11,13,15,17,19,22,24,26,28,30,32,34,36,38,40,42,44,46,48,50)
(2, 900.00, NULL, 3, 41), (1, 620.00, NULL, 5, 42), (1, 520.00, 1, 7, 43), (2, 890.00, NULL, 9, 44),
(1, 1150.00, NULL, 11, 45), (1, 680.00, 1, 13, 46), (2, 890.00, 4, 15, 47), (1, 680.00, 1, 17, 48),
(1, 720.00, NULL, 19, 49), (2, 480.00, 1, 22, 50), (1, 380.00, 4, 24, 51), (1, 850.00, 1, 26, 52),
(2, 920.00, NULL, 28, 53), (1, 380.00, NULL, 30, 54), (1, 520.00, 4, 32, 55), (2, 420.00, 1, 34, 56),
(1, 1150.00, NULL, 36, 57), (1, 320.00, NULL, 38, 58), (2, 1450.00, 4, 40, 59), (1, 280.00, 1, 42, 60);

-- DetallesFacturaVentasArticulo (40 ventas de artículos)
-- Solo artículos con stock en esa sucursal y cantidad <= stock disponible
INSERT INTO DetallesFacturaVentasArticulo (cantidad, precioUnitario, codFacturaVenta, codArticulo) VALUES
-- Sucursal 1 artículos (stock disponible: 1,2,3,5,7,8,9,10,13,15,16,17,18,20,22,24,25,27,29,30,31,33,34,36,38,39,41,43,44,46,47,49,50,52)
(1, 850.00, 1, 1), (2, 780.00, 2, 2), (1, 420.00, 3, 3), (2, 750.00, 4, 5),
(1, 520.00, 5, 8), (2, 320.00, 6, 9), (1, 650.00, 7, 10), (1, 450.00, 8, 13),
(2, 1850.00, 9, 15), (1, 1250.00, 10, 16), (1, 850.00, 11, 17), (2, 920.00, 12, 18),
(1, 3500.00, 13, 20), (1, 1200.00, 14, 22), (2, 2800.00, 15, 24), (1, 1200.00, 16, 25),
(1, 450.00, 17, 27), (2, 1200.00, 18, 29), (1, 680.00, 19, 30), (1, 1200.00, 20, 31),
(2, 2800.00, 21, 33), (1, 3200.00, 22, 34), (1, 380.00, 23, 36), (2, 1200.00, 24, 38),
(1, 650.00, 25, 39), (1, 320.00, 26, 41), (2, 220.00, 27, 43), (1, 450.00, 28, 44),
-- Sucursal 2 artículos (stock disponible: 1,3,5,6,8,10,11,13,14,16,18,19,20,21,22,23,24,26,28,29,32,35,36,37,38,40,41,42,44,45,46,50,51)
(1, 920.00, 41, 46), (1, 1650.00, 42, 47), (2, 780.00, 61, 50), (1, 850.00, 62, 51),
(1, 850.00, 63, 1), (2, 420.00, 64, 3), (1, 750.00, 65, 5), (1, 780.00, 66, 6),
(2, 480.00, 67, 8), (1, 650.00, 68, 10), (1, 280.00, 69, 11), (2, 580.00, 70, 13),
-- Sucursal 3 artículos (stock disponible: 2,4,6,7,9,11,14,15,17,19,21,23,25,26,30,32,33,35,37,39,40,42,43,45,47,48,49,51,52)
(1, 950.00, 81, 14), (1, 1250.00, 82, 16), (2, 920.00, 83, 18), (1, 2200.00, 84, 19);

-- Reintegros con fechas aleatorias entre 01/01/2025 y 11/11/2025
INSERT INTO Reintegros (fechaEmision, fechaReembolso, estado, cod_Cobertura, cod_ObraSocial, cod_Receta, cod_DetFacVentaM) VALUES
('2025-01-10', NULL, 'Pendiente', 1, 1, 1, 1), -- Reintegro pendiente
('2025-02-15', '2025-03-01', 'Aprobado', 1, 1, 1, 3); -- Reintegro aprobado

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