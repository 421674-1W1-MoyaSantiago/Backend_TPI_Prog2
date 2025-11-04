-- PostgreSQL script para Pharm-API
-- Script unificado para crear toda la base de datos de farmacia

-- 1. Tablas de catálogos base
CREATE TABLE IF NOT EXISTS Tipos_Empleados (
    cod_tipo_empleado SERIAL PRIMARY KEY,
    tipo VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Provincias (
    cod_Provincia SERIAL PRIMARY KEY,
    nom_Provincia VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Localidades (
    cod_Localidad SERIAL PRIMARY KEY,
    nom_Localidad VARCHAR(255) NOT NULL,
    cod_Provincia INTEGER NOT NULL,
    FOREIGN KEY (cod_Provincia) REFERENCES Provincias(cod_Provincia)
);

CREATE TABLE IF NOT EXISTS Tipos_Documento (
    cod_Tipo_Documento SERIAL PRIMARY KEY,
    tipo VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Tipos_Presentacion (
    cod_Tipo_Presentacion SERIAL PRIMARY KEY,
    descripcion VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Unidades_Medida (
    cod_Unidad_Medida SERIAL PRIMARY KEY,
    unidadMedida VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Tipos_Medicamento (
    cod_tipo_medicamento SERIAL PRIMARY KEY,
    descripcion VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Formas_Pago (
    cod_Forma_Pago SERIAL PRIMARY KEY,
    metodo VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Tipos_Descuentos (
    cod_tipo_descuento SERIAL PRIMARY KEY,
    tipo VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Tipos_Receta (
    cod_tipo_receta SERIAL PRIMARY KEY,
    tipo VARCHAR(255) NOT NULL
);

-- 2. Tablas principales
CREATE TABLE IF NOT EXISTS Laboratorios (
    cod_Laboratorio SERIAL PRIMARY KEY,
    nombreLaboratorio VARCHAR(255) NOT NULL,
    direccion VARCHAR(500),
    telefono VARCHAR(20),
    contacto VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS Categorias_Articulos (
    cod_categoria SERIAL PRIMARY KEY,
    categoria VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS Obras_Sociales (
    cod_obra_social SERIAL PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    direccion VARCHAR(500),
    telefono VARCHAR(20),
    email VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS Coberturas (
    cod_cobertura SERIAL PRIMARY KEY,
    cobertura VARCHAR(255) NOT NULL,
    porcentaje_cobertura DECIMAL(5,2) NOT NULL
);

CREATE TABLE IF NOT EXISTS Proveedores (
    cod_Proveedor SERIAL PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    direccion VARCHAR(500),
    telefono VARCHAR(20),
    email VARCHAR(255),
    cuit VARCHAR(20)
);

-- 3. Tabla de Sucursales y Grupos
CREATE TABLE IF NOT EXISTS Grup_Sucursales (
    cod_grupo_sucursal SERIAL PRIMARY KEY,
    nombre_grupo VARCHAR(255) NOT NULL,
    descripcion VARCHAR(500)
);

CREATE TABLE IF NOT EXISTS Sucursales (
    cod_Sucursal SERIAL PRIMARY KEY,
    direccion VARCHAR(500) NOT NULL,
    telefono VARCHAR(20),
    cod_Localidad INTEGER NOT NULL,
    cod_grupo_sucursal INTEGER,
    FOREIGN KEY (cod_Localidad) REFERENCES Localidades(cod_Localidad),
    FOREIGN KEY (cod_grupo_sucursal) REFERENCES Grup_Sucursales(cod_grupo_sucursal)
);

-- 4. Tabla de Usuarios (sincronizada con Auth-API)
CREATE TABLE IF NOT EXISTS Usuarios (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100),
    nombre VARCHAR(255),
    apellido VARCHAR(255),
    cod_sucursal INTEGER,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT true,
    FOREIGN KEY (cod_sucursal) REFERENCES Sucursales(cod_Sucursal)
);

-- 5. Tabla de Empleados
CREATE TABLE IF NOT EXISTS Empleados (
    cod_empleado SERIAL PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    apellido VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE,
    telefono VARCHAR(20),
    direccion VARCHAR(500),
    fecha_nacimiento DATE,
    fecha_ingreso DATE NOT NULL DEFAULT CURRENT_DATE,
    salario DECIMAL(10,2),
    cod_tipo_empleado INTEGER NOT NULL,
    cod_sucursal INTEGER NOT NULL,
    cod_tipo_documento INTEGER NOT NULL,
    nro_documento VARCHAR(20) NOT NULL,
    usuario_id INTEGER,
    FOREIGN KEY (cod_tipo_empleado) REFERENCES Tipos_Empleados(cod_tipo_empleado),
    FOREIGN KEY (cod_sucursal) REFERENCES Sucursales(cod_Sucursal),
    FOREIGN KEY (cod_tipo_documento) REFERENCES Tipos_Documento(cod_Tipo_Documento),
    FOREIGN KEY (usuario_id) REFERENCES Usuarios(id)
);

-- 6. Tabla de Clientes
CREATE TABLE IF NOT EXISTS Clientes (
    cod_cliente SERIAL PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    apellido VARCHAR(255) NOT NULL,
    email VARCHAR(255),
    telefono VARCHAR(20),
    direccion VARCHAR(500),
    fecha_nacimiento DATE,
    cod_tipo_documento INTEGER NOT NULL,
    nro_documento VARCHAR(20) NOT NULL,
    cod_obra_social INTEGER,
    FOREIGN KEY (cod_tipo_documento) REFERENCES Tipos_Documento(cod_Tipo_Documento),
    FOREIGN KEY (cod_obra_social) REFERENCES Obras_Sociales(cod_obra_social)
);

-- 7. Tablas de Productos
CREATE TABLE IF NOT EXISTS Articulos (
    cod_articulo SERIAL PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    descripcion TEXT,
    precio DECIMAL(10,2) NOT NULL,
    stock INTEGER NOT NULL DEFAULT 0,
    cod_categoria INTEGER NOT NULL,
    cod_proveedor INTEGER,
    fecha_vencimiento DATE,
    FOREIGN KEY (cod_categoria) REFERENCES Categorias_Articulos(cod_categoria),
    FOREIGN KEY (cod_proveedor) REFERENCES Proveedores(cod_Proveedor)
);

CREATE TABLE IF NOT EXISTS Medicamentos (
    cod_medicamento SERIAL PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    descripcion TEXT,
    precio DECIMAL(10,2) NOT NULL,
    stock INTEGER NOT NULL DEFAULT 0,
    cod_tipo_medicamento INTEGER NOT NULL,
    cod_laboratorio INTEGER NOT NULL,
    cod_tipo_presentacion INTEGER NOT NULL,
    cod_unidad_medida INTEGER NOT NULL,
    requiere_receta BOOLEAN DEFAULT false,
    FOREIGN KEY (cod_tipo_medicamento) REFERENCES Tipos_Medicamento(cod_tipo_medicamento),
    FOREIGN KEY (cod_laboratorio) REFERENCES Laboratorios(cod_Laboratorio),
    FOREIGN KEY (cod_tipo_presentacion) REFERENCES Tipos_Presentacion(cod_Tipo_Presentacion),
    FOREIGN KEY (cod_unidad_medida) REFERENCES Unidades_Medida(cod_Unidad_Medida)
);

CREATE TABLE IF NOT EXISTS Lotes_Medicamentos (
    cod_lote SERIAL PRIMARY KEY,
    numero_lote VARCHAR(50) NOT NULL,
    cod_medicamento INTEGER NOT NULL,
    fecha_fabricacion DATE NOT NULL,
    fecha_vencimiento DATE NOT NULL,
    cantidad INTEGER NOT NULL,
    precio_compra DECIMAL(10,2),
    FOREIGN KEY (cod_medicamento) REFERENCES Medicamentos(cod_medicamento)
);

-- 8. Tablas de Descuentos
CREATE TABLE IF NOT EXISTS Descuentos (
    cod_descuento SERIAL PRIMARY KEY,
    descripcion VARCHAR(255) NOT NULL,
    porcentaje DECIMAL(5,2) NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    cod_tipo_descuento INTEGER NOT NULL,
    FOREIGN KEY (cod_tipo_descuento) REFERENCES Tipos_Descuentos(cod_tipo_descuento)
);

-- 9. Tablas de Facturas
CREATE TABLE IF NOT EXISTS Facturas_Venta (
    cod_factura SERIAL PRIMARY KEY,
    fecha DATE NOT NULL DEFAULT CURRENT_DATE,
    hora TIME NOT NULL DEFAULT CURRENT_TIME,
    total DECIMAL(10,2) NOT NULL,
    cod_cliente INTEGER,
    cod_empleado INTEGER NOT NULL,
    cod_sucursal INTEGER NOT NULL,
    cod_forma_pago INTEGER NOT NULL,
    FOREIGN KEY (cod_cliente) REFERENCES Clientes(cod_cliente),
    FOREIGN KEY (cod_empleado) REFERENCES Empleados(cod_empleado),
    FOREIGN KEY (cod_sucursal) REFERENCES Sucursales(cod_Sucursal),
    FOREIGN KEY (cod_forma_pago) REFERENCES Formas_Pago(cod_Forma_Pago)
);

CREATE TABLE IF NOT EXISTS Detalles_Factura_Venta_Articulos (
    cod_detalle SERIAL PRIMARY KEY,
    cod_factura INTEGER NOT NULL,
    cod_articulo INTEGER NOT NULL,
    cantidad INTEGER NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    cod_descuento INTEGER,
    FOREIGN KEY (cod_factura) REFERENCES Facturas_Venta(cod_factura),
    FOREIGN KEY (cod_articulo) REFERENCES Articulos(cod_articulo),
    FOREIGN KEY (cod_descuento) REFERENCES Descuentos(cod_descuento)
);

CREATE TABLE IF NOT EXISTS Detalles_Factura_Venta_Medicamentos (
    cod_detalle SERIAL PRIMARY KEY,
    cod_factura INTEGER NOT NULL,
    cod_medicamento INTEGER NOT NULL,
    cantidad INTEGER NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    cod_descuento INTEGER,
    cod_tipo_receta INTEGER,
    FOREIGN KEY (cod_factura) REFERENCES Facturas_Venta(cod_factura),
    FOREIGN KEY (cod_medicamento) REFERENCES Medicamentos(cod_medicamento),
    FOREIGN KEY (cod_descuento) REFERENCES Descuentos(cod_descuento),
    FOREIGN KEY (cod_tipo_receta) REFERENCES Tipos_Receta(cod_tipo_receta)
);

-- Función para actualizar updated_at
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Trigger para Usuarios
DROP TRIGGER IF EXISTS update_usuarios_updated_at ON Usuarios;
CREATE TRIGGER update_usuarios_updated_at
    BEFORE UPDATE ON Usuarios
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- Índices para mejorar rendimiento
CREATE INDEX IF NOT EXISTS idx_empleados_sucursal ON Empleados(cod_sucursal);
CREATE INDEX IF NOT EXISTS idx_empleados_usuario ON Empleados(usuario_id);
CREATE INDEX IF NOT EXISTS idx_facturas_fecha ON Facturas_Venta(fecha);
CREATE INDEX IF NOT EXISTS idx_facturas_empleado ON Facturas_Venta(cod_empleado);
CREATE INDEX IF NOT EXISTS idx_medicamentos_tipo ON Medicamentos(cod_tipo_medicamento);

-- Comentarios para documentación
COMMENT ON TABLE Usuarios IS 'Usuarios sincronizados desde Auth-API';
COMMENT ON TABLE Empleados IS 'Empleados de la farmacia';
COMMENT ON TABLE Medicamentos IS 'Catálogo de medicamentos';
COMMENT ON TABLE Facturas_Venta IS 'Facturas de ventas realizadas';