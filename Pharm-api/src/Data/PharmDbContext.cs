using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pharm_api.Models;

namespace Pharm_api.Data
{
    public partial class PharmDbContext : DbContext
    {
        public virtual DbSet<DetallesFacturaVentasMedicamento> DetallesFacturaVentasMedicamento { get; set; }
        public virtual DbSet<DetallesFacturaVentasArticulo> DetallesFacturaVentasArticulo { get; set; }
        
        // Nuevas entidades agregadas
        public virtual DbSet<Articulo> Articulos { get; set; }
        public virtual DbSet<Cobertura> Coberturas { get; set; }
        public virtual DbSet<Descuento> Descuentos { get; set; }
        
        public PharmDbContext()
        {
        }

        public PharmDbContext(DbContextOptions<PharmDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CategoriasArticulo> CategoriasArticulos { get; set; }

        public virtual DbSet<Cliente> Clientes { get; set; }

        public virtual DbSet<Empleado> Empleados { get; set; }

        public virtual DbSet<FacturasVentum> FacturasVenta { get; set; }

        public virtual DbSet<FormasPago> FormasPagos { get; set; }

        public virtual DbSet<Grupsucursale> Grupsucursales { get; set; }

        public virtual DbSet<Laboratorio> Laboratorios { get; set; }

        public virtual DbSet<Localidade> Localidades { get; set; }

        public virtual DbSet<LotesMedicamento> LotesMedicamentos { get; set; }

        public virtual DbSet<Medicamento> Medicamentos { get; set; }

        public virtual DbSet<ObrasSociale> ObrasSociales { get; set; }

        public virtual DbSet<Proveedore> Proveedores { get; set; }

        public virtual DbSet<Provincia> Provincias { get; set; }

        public virtual DbSet<Sucursale> Sucursales { get; set; }

        public virtual DbSet<StockMedicamento> StockMedicamentos { get; set; }

        public virtual DbSet<TiposDescuento> TiposDescuentos { get; set; }

        public virtual DbSet<TiposDocumento> TiposDocumentos { get; set; }

        public virtual DbSet<TiposEmpleado> TiposEmpleados { get; set; }

        public virtual DbSet<TiposMedicamento> TiposMedicamentos { get; set; }

        public virtual DbSet<TiposPresentacion> TiposPresentacions { get; set; }

        public virtual DbSet<TiposRecetum> TiposReceta { get; set; }

        public virtual DbSet<UnidadesMedidum> UnidadesMedida { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetallesFacturaVentasMedicamento>(entity =>
            {
                entity.HasKey(e => e.cod_DetFacVentaM);
                entity.Property(e => e.cod_DetFacVentaM).HasColumnName("cod_DetFacVentaM");
                entity.Property(e => e.codFacturaVenta).HasColumnName("codFacturaVenta");
                entity.Property(e => e.cantidad).HasColumnName("cantidad");
                entity.Property(e => e.precioUnitario).HasColumnName("precioUnitario").HasPrecision(18, 2);
                entity.Property(e => e.codCobertura).HasColumnName("codCobertura");
                entity.Property(e => e.codMedicamento).HasColumnName("codMedicamento");
                entity.Property(e => e.Anulada)
                    .HasDefaultValue(false)
                    .HasColumnName("anulada");
                entity.HasOne(d => d.FacturaVenta)
                    .WithMany()
                    .HasForeignKey(d => d.codFacturaVenta);
                entity.HasOne(d => d.Medicamento)
                    .WithMany()
                    .HasForeignKey(d => d.codMedicamento);
                entity.HasOne(d => d.Cobertura)
                    .WithMany(p => p.DetallesFacturaVentasMedicamentos)
                    .HasForeignKey(d => d.codCobertura);
            });

            modelBuilder.Entity<DetallesFacturaVentasArticulo>(entity =>
            {
                entity.HasKey(e => e.cod_DetFacVentaA);
                entity.Property(e => e.cod_DetFacVentaA).HasColumnName("cod_DetFacVentaA");
                entity.Property(e => e.codFacturaVenta).HasColumnName("codFacturaVenta");
                entity.Property(e => e.cantidad).HasColumnName("cantidad");
                entity.Property(e => e.precioUnitario).HasColumnName("precioUnitario").HasPrecision(18, 2);
                entity.Property(e => e.codArticulo).HasColumnName("codArticulo");
                entity.Property(e => e.Anulada)
                    .HasDefaultValue(false)
                    .HasColumnName("anulada");
                entity.HasOne(d => d.FacturaVenta)
                    .WithMany()
                    .HasForeignKey(d => d.codFacturaVenta);
                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.DetallesFacturaVentasArticulos)
                    .HasForeignKey(d => d.codArticulo);
            });

            // Configuración para Articulos
            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.CodArticulo);
                entity.Property(e => e.CodArticulo).HasColumnName("cod_Articulo");
                entity.Property(e => e.CodBarra).HasColumnName("cod_barra").HasMaxLength(255);
                entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(255).IsRequired();
                entity.Property(e => e.PrecioUnitario).HasColumnName("precioUnitario").HasColumnType("decimal(18,2)");
                entity.Property(e => e.CodProveedor).HasColumnName("cod_Proveedor");
                entity.Property(e => e.CodCategoriaArticulo).HasColumnName("cod_Categoria_Articulo");
                
                entity.HasOne(d => d.CodProveedorNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodProveedor);
                entity.HasOne(d => d.CodCategoriaArticuloNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodCategoriaArticulo);
            });

            // Configuración para Descuentos
            modelBuilder.Entity<Descuento>(entity =>
            {
                entity.HasKey(e => e.CodDescuento);
                entity.Property(e => e.CodDescuento).HasColumnName("cod_descuento");
                entity.Property(e => e.FechaDescuento).HasColumnName("Fecha_Descuento").HasColumnType("datetime");
                entity.Property(e => e.CodLocalidad).HasColumnName("cod_localidad");
                entity.Property(e => e.CodMedicamento).HasColumnName("cod_medicamento");
                entity.Property(e => e.PorcentajeDescuento).HasColumnName("porcentaje_descuento").HasColumnType("decimal(12,2)");
                entity.Property(e => e.CodTipoDescuento).HasColumnName("cod_tipo_descuento");
                
                entity.HasOne(d => d.CodLocalidadNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodLocalidad);
                entity.HasOne(d => d.CodMedicamentoNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodMedicamento);
                entity.HasOne(d => d.CodTipoDescuentoNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodTipoDescuento);
            });

            // Configuración para Coberturas
            modelBuilder.Entity<Cobertura>(entity =>
            {
                entity.HasKey(e => e.CodCobertura);
                entity.Property(e => e.CodCobertura).HasColumnName("cod_Cobertura");
                entity.Property(e => e.FechaInicio).HasColumnName("fechaInicio").HasColumnType("datetime");
                entity.Property(e => e.FechaFin).HasColumnName("fechaFin").HasColumnType("datetime");
                entity.Property(e => e.CodLocalidad).HasColumnName("cod_Localidad");
                entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
                entity.Property(e => e.CodObraSocial).HasColumnName("cod_Obra_Social");
                entity.Property(e => e.CodDescuento).HasColumnName("cod_descuento");
                
                entity.HasOne(d => d.CodLocalidadNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodLocalidad);
                entity.HasOne(d => d.CodClienteNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodCliente);
                entity.HasOne(d => d.CodObraSocialNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodObraSocial);
                entity.HasOne(d => d.CodDescuentoNavigation)
                    .WithMany(p => p.Coberturas)
                    .HasForeignKey(d => d.CodDescuento);
            });
            modelBuilder.Entity<CategoriasArticulo>(entity =>
            {
                entity.HasKey(e => e.CodCategoriaArticulo).HasName("PK__Categori__CD6548446D9C1E6F");

                entity.ToTable("Categorias_Articulos");

                entity.Property(e => e.CodCategoriaArticulo).HasColumnName("cod_Categoria_Articulo");
                entity.Property(e => e.Categoria)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("categoria");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.CodCliente).HasName("PK__Clientes__7A537CA82E42EA10");

                entity.Property(e => e.CodCliente).HasColumnName("cod_Cliente");
                entity.Property(e => e.Altura).HasColumnName("altura");
                entity.Property(e => e.ApeCliente)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("apeCliente");
                entity.Property(e => e.Calle)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("calle");
                entity.Property(e => e.CodObraSocial).HasColumnName("cod_Obra_Social");
                entity.Property(e => e.CodTipoDocumento).HasColumnName("cod_Tipo_Documento");
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.NomCliente)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nomCliente");
                entity.Property(e => e.NroDoc)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nroDoc");
                entity.Property(e => e.NroTel)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nroTel");

                entity.HasOne(d => d.CodObraSocialNavigation).WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.CodObraSocial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Clientes__cod_Ob__778AC167");

                entity.HasOne(d => d.CodTipoDocumentoNavigation).WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.CodTipoDocumento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Clientes__cod_Ti__76969D2E");
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.CodEmpleado).HasName("PK__Empleado__6AA308D1C7793BF5");

                entity.Property(e => e.CodEmpleado).HasColumnName("cod_Empleado");
                entity.Property(e => e.Altura).HasColumnName("altura");
                entity.Property(e => e.ApeEmpleado)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ape_Empleado");
                entity.Property(e => e.Calle)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("calle");
                entity.Property(e => e.CodSucursal).HasColumnName("codSucursal");
                entity.Property(e => e.CodTipoDocumento).HasColumnName("codTipoDocumento");
                entity.Property(e => e.CodTipoEmpleado).HasColumnName("codTipoEmpleado");
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaIngreso");
                entity.Property(e => e.NomEmpleado)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom_Empleado");
                entity.Property(e => e.NroTel)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nro_Tel");
                entity.Property(e => e.Activo)
                    .HasDefaultValue(true)
                    .HasColumnName("activo");

                entity.HasOne(d => d.CodSucursalNavigation).WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.CodSucursal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empleados__codSu__6477ECF3");

                entity.HasOne(d => d.CodTipoDocumentoNavigation).WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.CodTipoDocumento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empleados__codTi__6383C8BA");

                entity.HasOne(d => d.CodTipoEmpleadoNavigation).WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.CodTipoEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empleados__codTi__628FA481");
            });

            modelBuilder.Entity<FacturasVentum>(entity =>
            {
                entity.HasKey(e => e.CodFacturaVenta).HasName("PK__Facturas__982E4AD8ED72C347");

                entity.Property(e => e.CodFacturaVenta).HasColumnName("cod_FacturaVenta");
                entity.Property(e => e.CodCliente).HasColumnName("codCliente");
                entity.Property(e => e.CodEmpleado).HasColumnName("codEmpleado");
                entity.Property(e => e.CodFormaPago).HasColumnName("codFormaPago");
                entity.Property(e => e.CodSucursal).HasColumnName("codSucursal");
                entity.Property(e => e.Fecha)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");
                entity.Property(e => e.Total)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("total");
                entity.Property(e => e.Anulada)
                    .HasDefaultValue(false)
                    .HasColumnName("anulada");

                entity.HasOne(d => d.CodClienteNavigation).WithMany(p => p.FacturasVenta)
                    .HasForeignKey(d => d.CodCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FacturasV__codCl__04E4BC85");

                entity.HasOne(d => d.CodEmpleadoNavigation).WithMany(p => p.FacturasVenta)
                    .HasForeignKey(d => d.CodEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FacturasV__codEm__03F0984C");

                entity.HasOne(d => d.CodFormaPagoNavigation).WithMany(p => p.FacturasVenta)
                    .HasForeignKey(d => d.CodFormaPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FacturasV__codFo__06CD04F7");

                entity.HasOne(d => d.CodSucursalNavigation).WithMany(p => p.FacturasVenta)
                    .HasForeignKey(d => d.CodSucursal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FacturasV__codSu__05D8E0BE");
            });

            modelBuilder.Entity<FormasPago>(entity =>
            {
                entity.HasKey(e => e.CodFormaPago).HasName("PK__Formas_P__372D87F98BCDA1C8");

                entity.ToTable("Formas_Pago");

                entity.Property(e => e.CodFormaPago).HasColumnName("cod_Forma_Pago");
                entity.Property(e => e.Metodo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("metodo");
            });

            modelBuilder.Entity<Grupsucursale>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Grupsucu__3213E83F52874DC2");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Activo)
                    .HasDefaultValue(true)
                    .HasColumnName("activo");
                entity.Property(e => e.CodSucursal).HasColumnName("cod_sucursal");
                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");
                entity.Property(e => e.FechaAsignacion)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_asignacion");

                entity.HasOne(d => d.CodSucursalNavigation).WithMany(p => p.Grupsucursales)
                    .HasForeignKey(d => d.CodSucursal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grupsucur__cod_s__7F2BE32F");

                entity.HasOne(d => d.CodUsuarioNavigation).WithMany(p => p.Grupsucursales)
                    .HasForeignKey(d => d.CodUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grupsucur__cod_u__7E37BEF6");
            });

            modelBuilder.Entity<Laboratorio>(entity =>
            {
                entity.HasKey(e => e.CodLaboratorio).HasName("PK__Laborato__247A2FD1B79BE801");

                entity.Property(e => e.CodLaboratorio).HasColumnName("cod_Laboratorio");
                entity.Property(e => e.CodProveedor).HasColumnName("cod_Proveedor");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.HasOne(d => d.CodProveedorNavigation).WithMany(p => p.Laboratorios)
                    .HasForeignKey(d => d.CodProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Laborator__cod_P__693CA210");
            });

            modelBuilder.Entity<Localidade>(entity =>
            {
                entity.HasKey(e => e.CodLocalidad).HasName("PK__Localida__717F46AF44FAAD2E");

                entity.Property(e => e.CodLocalidad).HasColumnName("cod_Localidad");
                entity.Property(e => e.CodProvincia).HasColumnName("cod_Provincia");
                entity.Property(e => e.NomLocalidad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom_Localidad");

                entity.HasOne(d => d.CodProvinciaNavigation).WithMany(p => p.Localidades)
                    .HasForeignKey(d => d.CodProvincia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Localidad__cod_P__4D94879B");
            });

            modelBuilder.Entity<LotesMedicamento>(entity =>
            {
                entity.HasKey(e => e.CodLoteMedicamento).HasName("PK__Lotes_Me__534D1ADCCDFE23AD");

                entity.ToTable("Lotes_Medicamentos");

                entity.Property(e => e.CodLoteMedicamento).HasColumnName("cod_lote_medicamento");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.FechaElaboracion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_Elaboracion");
                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_Vencimiento");
            });

            modelBuilder.Entity<Medicamento>(entity =>
            {
                entity.HasKey(e => e.CodMedicamento).HasName("PK__Medicame__4D38A6871582DBBA");

                entity.Property(e => e.CodMedicamento).HasColumnName("cod_medicamento");
                entity.Property(e => e.CodBarra)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("cod_barra");
                entity.Property(e => e.CodLaboratorio).HasColumnName("codLaboratorio");
                entity.Property(e => e.CodLoteMedicamento).HasColumnName("cod_lote_medicamento");
                entity.Property(e => e.CodTipoMedicamento).HasColumnName("cod_tipo_medicamento");
                entity.Property(e => e.CodTipoPresentacion).HasColumnName("cod_tipo_presentacion");
                entity.Property(e => e.CodUnidadMedida).HasColumnName("cod_unidad_medida");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
                entity.Property(e => e.Dosis).HasColumnName("dosis");
                entity.Property(e => e.Posologia)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("posologia");
                entity.Property(e => e.PrecioUnitario)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("precio_unitario");
                entity.Property(e => e.RequiereReceta).HasColumnName("requiere_receta");
                entity.Property(e => e.VentaLibre).HasColumnName("venta_libre");

                entity.HasOne(d => d.CodLaboratorioNavigation).WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.CodLaboratorio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicamen__codLa__6EF57B66");

                entity.HasOne(d => d.CodLoteMedicamentoNavigation).WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.CodLoteMedicamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicamen__cod_l__6E01572D");

                entity.HasOne(d => d.CodTipoMedicamentoNavigation).WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.CodTipoMedicamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicamen__cod_t__71D1E811");

                entity.HasOne(d => d.CodTipoPresentacionNavigation).WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.CodTipoPresentacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicamen__cod_t__6FE99F9F");

                entity.HasOne(d => d.CodUnidadMedidaNavigation).WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.CodUnidadMedida)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicamen__cod_u__70DDC3D8");
            });

            modelBuilder.Entity<ObrasSociale>(entity =>
            {
                entity.HasKey(e => e.CodObraSocial).HasName("PK__Obras_So__D7D09FCD0E3B450B");

                entity.ToTable("Obras_Sociales");

                entity.Property(e => e.CodObraSocial).HasColumnName("cod_Obra_Social");
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.NroTel)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nroTel");
                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("razonSocial");
            });

            modelBuilder.Entity<Proveedore>(entity =>
            {
                entity.HasKey(e => e.CodProveedor).HasName("PK__Proveedo__0B43AEB1D00048A5");

                entity.Property(e => e.CodProveedor).HasColumnName("cod_Proveedor");
                entity.Property(e => e.Cuit)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("cuit");
                entity.Property(e => e.NroTel)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nro_Tel");
                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("razon_Social");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => e.CodProvincia).HasName("PK__Provinci__2097A308CF6C7B42");

                entity.Property(e => e.CodProvincia).HasColumnName("cod_Provincia");
                entity.Property(e => e.NomProvincia)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom_Provincia");
            });

            modelBuilder.Entity<Sucursale>(entity =>
            {
                entity.HasKey(e => e.CodSucursal).HasName("PK__Sucursal__1A4BE98E15980601");

                entity.Property(e => e.CodSucursal).HasColumnName("cod_Sucursal");
                entity.Property(e => e.Altura).HasColumnName("altura");
                entity.Property(e => e.Calle)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("calle");
                entity.Property(e => e.CodLocalidad).HasColumnName("cod_Localidad");
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.HorarioApertura)
                    .HasColumnType("datetime")
                    .HasColumnName("horarioApertura");
                entity.Property(e => e.HorarioCierre)
                    .HasColumnType("datetime")
                    .HasColumnName("horarioCierre");
                entity.Property(e => e.NomSucursal)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom_Sucursal");
                entity.Property(e => e.NroTel)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nro_Tel");

                entity.HasOne(d => d.CodLocalidadNavigation).WithMany(p => p.Sucursales)
                    .HasForeignKey(d => d.CodLocalidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sucursale__cod_L__5FB337D6");
            });

            modelBuilder.Entity<TiposDescuento>(entity =>
            {
                entity.HasKey(e => e.CodTipoDescuento).HasName("PK__Tipos_De__E4689E47AA664BE9");

                entity.ToTable("Tipos_Descuentos");

                entity.Property(e => e.CodTipoDescuento).HasColumnName("cod_tipo_descuento");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<TiposDocumento>(entity =>
            {
                entity.HasKey(e => e.CodTipoDocumento).HasName("PK__Tipos_Do__F2417FCF4844615D");

                entity.ToTable("Tipos_Documento");

                entity.Property(e => e.CodTipoDocumento).HasColumnName("cod_Tipo_Documento");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<TiposEmpleado>(entity =>
            {
                entity.HasKey(e => e.CodTipoEmpleado).HasName("PK__Tipos_Em__5923F3157DA4C695");

                entity.ToTable("Tipos_Empleados");

                entity.Property(e => e.CodTipoEmpleado).HasColumnName("cod_tipo_empleado");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<TiposMedicamento>(entity =>
            {
                entity.HasKey(e => e.CodTipoMedicamento).HasName("PK__Tipos_Me__BC4A1A7B1C756A7C");

                entity.ToTable("Tipos_Medicamento");

                entity.Property(e => e.CodTipoMedicamento).HasColumnName("cod_tipo_medicamento");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<TiposPresentacion>(entity =>
            {
                entity.HasKey(e => e.CodTipoPresentacion).HasName("PK__Tipos_Pr__17353F63E45AF077");

                entity.ToTable("Tipos_Presentacion");

                entity.Property(e => e.CodTipoPresentacion).HasColumnName("cod_Tipo_Presentacion");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<TiposRecetum>(entity =>
            {
                entity.HasKey(e => e.CodTipoReceta).HasName("PK__Tipos_Re__5E598504AC46AB81");

                entity.ToTable("Tipos_Receta");

                entity.Property(e => e.CodTipoReceta).HasColumnName("cod_Tipo_Receta");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<UnidadesMedidum>(entity =>
            {
                entity.HasKey(e => e.CodUnidadMedida).HasName("PK__Unidades__4D1C36C4EB1A998C");

                entity.ToTable("Unidades_Medida");

                entity.Property(e => e.CodUnidadMedida).HasColumnName("cod_Unidad_Medida");
                entity.Property(e => e.UnidadMedida)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("unidadMedida");
            });

            modelBuilder.Entity<StockMedicamento>(entity =>
            {
                entity.HasKey(e => e.CodStockMedicamento).HasName("PK__Stock_Me__A8DCFD02C2F47F8E");

                entity.ToTable("Stock_Medicamentos");

                entity.Property(e => e.CodStockMedicamento).HasColumnName("cod_Stock_Medicamento");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.CodSucursal).HasColumnName("cod_Sucursal");
                entity.Property(e => e.CodMedicamento).HasColumnName("cod_Medicamento");

                entity.HasOne(d => d.CodSucursalNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodSucursal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock_Me__cod_Su__7B5B524B");

                entity.HasOne(d => d.CodMedicamentoNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CodMedicamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock_Me__cod_Me__7C4F7684");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.CodUsuario).HasName("PK__Usuario__EA3C9B1AC732D1C8");

                entity.ToTable("Usuario");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

