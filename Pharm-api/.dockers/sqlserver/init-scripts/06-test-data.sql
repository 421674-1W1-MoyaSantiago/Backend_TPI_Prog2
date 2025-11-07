GO
USE PharmDB;

GO
select * 
from Empleados;

GO
select *
from Usuario;

GO
select *
from Medicamentos;

SELECT * 
FROM Usuario 
WHERE Username = 'admin'

select *
from DetallesFacturaVentasMedicamento;

-- Verificar qué tablas existen
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
ORDER BY TABLE_NAME;

-- Verificar si hay datos en las tablas de detalles
SELECT COUNT(*) as 'Total_Medicamentos' FROM DetallesFacturaVentasMedicamento;
SELECT COUNT(*) as 'Total_Articulos' FROM DetallesFacturaVentasArticulo;

-- Verificar si hay coberturas (necesarias para medicamentos)
SELECT COUNT(*) as 'Total_Coberturas' FROM Coberturas;

-- Verificar si hay clientes (necesarios para coberturas)
SELECT COUNT(*) as 'Total_Clientes' FROM Clientes;

-- Verificar si hay empleados (necesarios para facturas)
SELECT COUNT(*) as 'Total_Empleados' FROM Empleados;

select * 
from FacturasVenta;

select *
from provincias;

GO
SELECT s.*
FROM Sucursales s
INNER JOIN Grupsucursales gs ON s.cod_Sucursal = gs.cod_sucursal
WHERE gs.cod_usuario = 1;
GO

GO
SELECT e.*
FROM Empleados e
INNER JOIN Sucursales s ON e.codSucursal = s.cod_Sucursal
INNER JOIN Grupsucursales gs ON s.cod_Sucursal = gs.cod_sucursal
WHERE gs.cod_usuario = 1;
GO