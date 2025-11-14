-- 02-create-tables.sql
-- Crear todas las tablas del esquema de farmacia
USE PharmDB;
GO

-- Tablas de catálogos base
PRINT 'Creando tablas de catálogos...';

CREATE TABLE Tipos_Empleados (
    cod_tipo_empleado INT PRIMARY KEY IDENTITY(1,1),
    tipo VARCHAR(255) NOT NULL
);

CREATE TABLE Provincias (
    cod_Provincia INT PRIMARY KEY IDENTITY(1,1),
    nom_Provincia VARCHAR(255) NOT NULL
);

CREATE TABLE Localidades (
    cod_Localidad INT PRIMARY KEY IDENTITY(1,1),
    nom_Localidad VARCHAR(255) NOT NULL,
    cod_Provincia INT NOT NULL,
    FOREIGN KEY (cod_Provincia) REFERENCES Provincias(cod_Provincia)
);

CREATE TABLE Tipos_Documento (
    cod_Tipo_Documento INT PRIMARY KEY IDENTITY(1,1),
    tipo VARCHAR(255) NOT NULL
);

CREATE TABLE Tipos_Presentacion (
    cod_Tipo_Presentacion INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(255) NOT NULL
);

CREATE TABLE Unidades_Medida (
    cod_Unidad_Medida INT PRIMARY KEY IDENTITY(1,1),
    unidadMedida VARCHAR(255) NOT NULL
);

CREATE TABLE Tipos_Medicamento (
    cod_tipo_medicamento INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(255) NOT NULL
);

CREATE TABLE Formas_Pago (
    cod_Forma_Pago INT PRIMARY KEY IDENTITY(1,1),
    metodo VARCHAR(255) NOT NULL
);

CREATE TABLE Tipos_Receta (
    cod_Tipo_Receta INT PRIMARY KEY IDENTITY(1,1),
    tipo VARCHAR(255) NOT NULL
);

CREATE TABLE Categorias_Articulos (
    cod_Categoria_Articulo INT PRIMARY KEY IDENTITY(1,1),
    categoria VARCHAR(255) NOT NULL
);

CREATE TABLE Tipos_Descuentos (
    cod_tipo_descuento INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(500) NOT NULL
);

PRINT 'Tablas de catálogos creadas exitosamente';

-- Tablas principales
PRINT 'Creando tablas principales...';

CREATE TABLE Sucursales (
    cod_Sucursal INT PRIMARY KEY IDENTITY(1,1),
    nom_Sucursal VARCHAR(255) NOT NULL,
    nro_Tel VARCHAR(255),
    calle VARCHAR(255),
    altura INT,
    email VARCHAR(255),
    horarioApertura DATETIME,
    horarioCierre DATETIME,
    cod_Localidad INT NOT NULL,
    FOREIGN KEY (cod_Localidad) REFERENCES Localidades(cod_Localidad)
);

CREATE TABLE Empleados (
    cod_Empleado INT PRIMARY KEY IDENTITY(1,1),
    nom_Empleado VARCHAR(255) NOT NULL,
    ape_Empleado VARCHAR(255) NOT NULL,
    nro_Tel VARCHAR(255),
    calle VARCHAR(255),
    altura INT,
    email VARCHAR(255),
    fechaIngreso DATETIME NOT NULL,
    codTipoEmpleado INT NOT NULL,
    codTipoDocumento INT NOT NULL,
    codSucursal INT NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (codTipoEmpleado) REFERENCES Tipos_Empleados(cod_tipo_empleado),
    FOREIGN KEY (codTipoDocumento) REFERENCES Tipos_Documento(cod_Tipo_Documento),
    FOREIGN KEY (codSucursal) REFERENCES Sucursales(cod_Sucursal)
);

CREATE TABLE Proveedores (
    cod_Proveedor INT PRIMARY KEY IDENTITY(1,1),
    razon_Social VARCHAR(255) NOT NULL,
    cuit VARCHAR(255),
    nro_Tel VARCHAR(255)
);

CREATE TABLE Laboratorios (
    cod_Laboratorio INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(255) NOT NULL,
    cod_Proveedor INT NOT NULL,
    FOREIGN KEY (cod_Proveedor) REFERENCES Proveedores(cod_Proveedor)
);

CREATE TABLE Lotes_Medicamentos (
    cod_lote_medicamento INT PRIMARY KEY IDENTITY(1,1),
    fecha_Elaboracion DATETIME NOT NULL,
    fecha_Vencimiento DATETIME NOT NULL,
    cantidad INT NOT NULL
);

CREATE TABLE Medicamentos (
    cod_medicamento INT PRIMARY KEY IDENTITY(1,1),
    cod_barra VARCHAR(255),
    descripcion VARCHAR(255) NOT NULL,
    requiere_receta BIT NOT NULL,
    venta_libre BIT NOT NULL,
    precio_unitario DECIMAL(18,2) NOT NULL,
    dosis INT,
    posologia VARCHAR(255),
    cod_lote_medicamento INT NOT NULL,
    codLaboratorio INT NOT NULL,
    cod_tipo_presentacion INT NOT NULL,
    cod_unidad_medida INT NOT NULL,
    cod_tipo_medicamento INT NOT NULL,
    FOREIGN KEY (cod_lote_medicamento) REFERENCES Lotes_Medicamentos(cod_lote_medicamento),
    FOREIGN KEY (codLaboratorio) REFERENCES Laboratorios(cod_Laboratorio),
    FOREIGN KEY (cod_tipo_presentacion) REFERENCES Tipos_Presentacion(cod_Tipo_Presentacion),
    FOREIGN KEY (cod_unidad_medida) REFERENCES Unidades_Medida(cod_Unidad_Medida),
    FOREIGN KEY (cod_tipo_medicamento) REFERENCES Tipos_Medicamento(cod_tipo_medicamento)
);

CREATE TABLE Articulos (
    cod_Articulo INT PRIMARY KEY IDENTITY(1,1),
    cod_barra VARCHAR(255),
    descripcion VARCHAR(255) NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    cod_Proveedor INT NOT NULL,
    cod_Categoria_Articulo INT NOT NULL,
    FOREIGN KEY (cod_Proveedor) REFERENCES Proveedores(cod_Proveedor),
    FOREIGN KEY (cod_Categoria_Articulo) REFERENCES Categorias_Articulos(cod_Categoria_Articulo)
);

CREATE TABLE Obras_Sociales (
    cod_Obra_Social INT PRIMARY KEY IDENTITY(1,1),
    razonSocial VARCHAR(255) NOT NULL,
    nroTel VARCHAR(255),
    email VARCHAR(255)
);

CREATE TABLE Clientes (
    cod_Cliente INT PRIMARY KEY IDENTITY(1,1),
    nomCliente VARCHAR(255) NOT NULL,
    apeCliente VARCHAR(255) NOT NULL,
    nroDoc VARCHAR(255),
    nroTel VARCHAR(255),
    calle VARCHAR(255),
    altura INT,
    email VARCHAR(255),
    cod_Tipo_Documento INT NOT NULL,
    cod_Obra_Social INT NOT NULL,
    FOREIGN KEY (cod_Tipo_Documento) REFERENCES Tipos_Documento(cod_Tipo_Documento),
    FOREIGN KEY (cod_Obra_Social) REFERENCES Obras_Sociales(cod_Obra_Social)
);

-- Tabla de descuentos (debe crearse antes de Coberturas)
CREATE TABLE Descuentos (
    cod_descuento INT PRIMARY KEY IDENTITY(1,1),
    Fecha_Descuento DATETIME NOT NULL,
    cod_localidad INT NOT NULL,
    cod_medicamento INT NOT NULL,
    porcentaje_descuento DECIMAL(12,2) NOT NULL,
    cod_tipo_descuento INT NOT NULL,
    FOREIGN KEY (cod_localidad) REFERENCES Localidades(cod_Localidad),
    FOREIGN KEY (cod_medicamento) REFERENCES Medicamentos(cod_medicamento),
    FOREIGN KEY (cod_tipo_descuento) REFERENCES Tipos_Descuentos(cod_tipo_descuento)
);

-- Actualizar tabla Coberturas con estructura completa
CREATE TABLE Coberturas (
    cod_Cobertura INT PRIMARY KEY IDENTITY(1,1),
    fechaInicio DATETIME NOT NULL,
    fechaFin DATETIME NOT NULL,
    cod_Localidad INT NOT NULL,
    cod_cliente INT NOT NULL,
    cod_Obra_Social INT NOT NULL,
    cod_descuento INT NOT NULL,
    FOREIGN KEY (cod_Localidad) REFERENCES Localidades(cod_Localidad),
    FOREIGN KEY (cod_cliente) REFERENCES Clientes(cod_Cliente),
    FOREIGN KEY (cod_Obra_Social) REFERENCES Obras_Sociales(cod_Obra_Social),
    FOREIGN KEY (cod_descuento) REFERENCES Descuentos(cod_descuento)
);

-- Tablas de stock
CREATE TABLE Stock_Articulos (
    cod_StockArticulo INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT NOT NULL,
    codSucursal INT NOT NULL,
    codArticulo INT NOT NULL,
    FOREIGN KEY (codSucursal) REFERENCES Sucursales(cod_Sucursal),
    FOREIGN KEY (codArticulo) REFERENCES Articulos(cod_Articulo)
);

CREATE TABLE Stock_Medicamentos (
    cod_Stock_Medicamento INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT NOT NULL,
    cod_Sucursal INT NOT NULL,
    cod_Medicamento INT NOT NULL,
    FOREIGN KEY (cod_Sucursal) REFERENCES Sucursales(cod_Sucursal),
    FOREIGN KEY (cod_Medicamento) REFERENCES Medicamentos(cod_medicamento)
);

-- Tablas de recetas
CREATE TABLE Recetas (
    cod_Receta INT PRIMARY KEY IDENTITY(1,1),
    nomMedico VARCHAR(255) NOT NULL,
    apeMedico VARCHAR(255) NOT NULL,
    matricula VARCHAR(255) NOT NULL,
    fecha DATETIME NOT NULL,
    diagnostico VARCHAR(255),
    codigo INT,
    estado VARCHAR(255) NOT NULL,
    codObraSocial INT NOT NULL,
    codCliente INT NOT NULL,
    codTipoReceta INT NOT NULL,
    FOREIGN KEY (codObraSocial) REFERENCES Obras_Sociales(cod_Obra_Social),
    FOREIGN KEY (codCliente) REFERENCES Clientes(cod_Cliente),
    FOREIGN KEY (codTipoReceta) REFERENCES Tipos_Receta(cod_Tipo_Receta)
);

CREATE TABLE Autorizaciones (
    cod_Autorizacion INT PRIMARY KEY IDENTITY(1,1),
    codigo INT NOT NULL,
    estado VARCHAR(255) NOT NULL,
    fechaAutorizacion DATETIME NOT NULL,
    codObraSocial INT NOT NULL,
    codReceta INT NOT NULL,
    FOREIGN KEY (codObraSocial) REFERENCES Obras_Sociales(cod_Obra_Social),
    FOREIGN KEY (codReceta) REFERENCES Recetas(cod_Receta)
);

CREATE TABLE Detalles_Receta (
    cod_DetalleReceta INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT NOT NULL,
    codMedicamento INT NOT NULL,
    cod_Receta INT NOT NULL,
    FOREIGN KEY (codMedicamento) REFERENCES Medicamentos(cod_medicamento),
    FOREIGN KEY (cod_Receta) REFERENCES Recetas(cod_Receta)
);

-- Tablas de facturas de compra
CREATE TABLE Facturas_Compra (
    cod_FacturaCompra INT PRIMARY KEY IDENTITY(1,1),
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    cod_Empleado INT NOT NULL,
    cod_Sucursal INT NOT NULL,
    cod_Proveedor INT NOT NULL,
    FOREIGN KEY (cod_Empleado) REFERENCES Empleados(cod_Empleado),
    FOREIGN KEY (cod_Sucursal) REFERENCES Sucursales(cod_Sucursal),
    FOREIGN KEY (cod_Proveedor) REFERENCES Proveedores(cod_Proveedor)
);

CREATE TABLE DetallesFacturaCompraMedicamento (
    cod_DetFacCompraM INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    codFacturaCompra INT NOT NULL,
    codMedicamento INT NOT NULL,
    FOREIGN KEY (codFacturaCompra) REFERENCES Facturas_Compra(cod_FacturaCompra),
    FOREIGN KEY (codMedicamento) REFERENCES Medicamentos(cod_medicamento)
);

CREATE TABLE DetallesFacturaCompraArticulo (
    cod_DetFacCompraA INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    codFacturaCompra INT NOT NULL,
    codArticulo INT NOT NULL,
    FOREIGN KEY (codFacturaCompra) REFERENCES Facturas_Compra(cod_FacturaCompra),
    FOREIGN KEY (codArticulo) REFERENCES Articulos(cod_Articulo)
);

-- Tabla de usuarios para conectar con Auth-api
CREATE TABLE Usuario (
    cod_usuario INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL
);

CREATE TABLE Grupsucursales (
    id INT PRIMARY KEY IDENTITY(1,1),
    cod_usuario INT NOT NULL,
    cod_sucursal INT NOT NULL,
    fecha_asignacion DATETIME NOT NULL DEFAULT GETDATE(),
    activo BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (cod_usuario) REFERENCES Usuario(cod_usuario),
    FOREIGN KEY (cod_sucursal) REFERENCES Sucursales(cod_Sucursal)
);

-- Tablas de facturas (simplificadas para el CRUD)
CREATE TABLE FacturasVenta (
    cod_FacturaVenta INT PRIMARY KEY IDENTITY(1,1),
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    codEmpleado INT NOT NULL,
    codCliente INT NOT NULL,
    codSucursal INT NOT NULL,
    codFormaPago INT NOT NULL,
    total DECIMAL(18,2) DEFAULT 0,
    FOREIGN KEY (codEmpleado) REFERENCES Empleados(cod_Empleado),
    FOREIGN KEY (codCliente) REFERENCES Clientes(cod_Cliente),
    FOREIGN KEY (codSucursal) REFERENCES Sucursales(cod_Sucursal),
    FOREIGN KEY (codFormaPago) REFERENCES Formas_Pago(cod_Forma_Pago)
);


-- Tabla DetallesFacturaVentasMedicamento
CREATE TABLE DetallesFacturaVentasMedicamento (
    cod_DetFacVentaM INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    codCobertura INT NULL,
    codMedicamento INT NOT NULL,
    codFacturaVenta INT NOT NULL,
    FOREIGN KEY (codFacturaVenta) REFERENCES FacturasVenta(cod_FacturaVenta),
    FOREIGN KEY (codCobertura) REFERENCES Coberturas(cod_Cobertura),
    FOREIGN KEY (codMedicamento) REFERENCES Medicamentos(cod_medicamento)
);

-- Tabla DetallesFacturaVentasArticulo
CREATE TABLE DetallesFacturaVentasArticulo (
    cod_DetFacVentaA INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    codFacturaVenta INT NOT NULL,
    codArticulo INT NOT NULL,
    FOREIGN KEY (codFacturaVenta) REFERENCES FacturasVenta(cod_FacturaVenta),
    FOREIGN KEY (codArticulo) REFERENCES Articulos(cod_Articulo)
);

-- Tabla de reintegros
CREATE TABLE Reintegros (
    cod_Reintegros INT PRIMARY KEY IDENTITY(1,1),
    fechaEmision DATETIME NOT NULL,
    fechaReembolso DATETIME NULL,
    estado VARCHAR(255) NOT NULL,
    cod_Cobertura INT NOT NULL,
    cod_ObraSocial INT NOT NULL,
    cod_Receta INT NOT NULL,
    cod_DetFacVentaM INT NOT NULL,
    FOREIGN KEY (cod_Cobertura) REFERENCES Coberturas(cod_Cobertura),
    FOREIGN KEY (cod_DetFacVentaM) REFERENCES DetallesFacturaVentasMedicamento(cod_DetFacVentaM),
    FOREIGN KEY (cod_ObraSocial) REFERENCES Obras_Sociales(cod_Obra_Social),
    FOREIGN KEY (cod_Receta) REFERENCES Recetas(cod_Receta)
);



PRINT 'Todas las tablas creadas exitosamente';
GO

-- Trigger para asignar automáticamente el usuario admin (ID=1) a nuevas sucursales
CREATE TRIGGER tr_AsignarAdminASucursal
ON Sucursales
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Insertar automáticamente el usuario admin (ID=1) en Grupsucursales para cada nueva sucursal
    INSERT INTO Grupsucursales (cod_usuario, cod_sucursal, fecha_asignacion, activo)
    SELECT 1, inserted.cod_Sucursal, GETDATE(), 1
    FROM inserted
    WHERE NOT EXISTS (
        SELECT 1 FROM Grupsucursales 
        WHERE cod_usuario = 1 AND cod_sucursal = inserted.cod_Sucursal
    );
END;
GO

PRINT 'Trigger para asignar admin a nuevas sucursales creado';
GO