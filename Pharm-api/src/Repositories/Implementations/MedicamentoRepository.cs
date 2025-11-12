using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public class MedicamentoRepository : IMedicamentoRepository
    {
        private readonly PharmDbContext _context;

        public MedicamentoRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicamentoDto>> GetAllAsync()
        {
            return await _context.Medicamentos
                .Include(m => m.CodLaboratorioNavigation)
                .Include(m => m.CodLoteMedicamentoNavigation)
                .Include(m => m.CodTipoMedicamentoNavigation)
                .Include(m => m.CodTipoPresentacionNavigation)
                .Include(m => m.CodUnidadMedidaNavigation)
                .Select(m => new MedicamentoDto
                {
                    CodMedicamento = m.CodMedicamento,
                    CodBarra = m.CodBarra,
                    Descripcion = m.Descripcion,
                    RequiereReceta = m.RequiereReceta,
                    VentaLibre = m.VentaLibre,
                    PrecioUnitario = m.PrecioUnitario,
                    Dosis = m.Dosis,
                    Posologia = m.Posologia,
                    CodLoteMedicamento = m.CodLoteMedicamento,
                    LoteDescripcion = $"Lote {m.CodLoteMedicamento} - Vence: {m.CodLoteMedicamentoNavigation.FechaVencimiento:dd/MM/yyyy}",
                    StockDisponible = m.CodLoteMedicamentoNavigation.Cantidad,
                    FechaVencimiento = m.CodLoteMedicamentoNavigation.FechaVencimiento,
                    CodLaboratorio = m.CodLaboratorio,
                    LaboratorioDescripcion = m.CodLaboratorioNavigation.Descripcion,
                    CodTipoPresentacion = m.CodTipoPresentacion,
                    TipoPresentacionDescripcion = m.CodTipoPresentacionNavigation.Descripcion,
                    CodUnidadMedida = m.CodUnidadMedida,
                    UnidadMedidaDescripcion = m.CodUnidadMedidaNavigation.UnidadMedida,
                    CodTipoMedicamento = m.CodTipoMedicamento,
                    TipoMedicamentoDescripcion = m.CodTipoMedicamentoNavigation.Descripcion
                })
                .ToListAsync();
        }

        public async Task<MedicamentoDto?> GetByIdAsync(int id)
        {
            return await _context.Medicamentos
                .Include(m => m.CodLaboratorioNavigation)
                .Include(m => m.CodLoteMedicamentoNavigation)
                .Include(m => m.CodTipoMedicamentoNavigation)
                .Include(m => m.CodTipoPresentacionNavigation)
                .Include(m => m.CodUnidadMedidaNavigation)
                .Where(m => m.CodMedicamento == id)
                .Select(m => new MedicamentoDto
                {
                    CodMedicamento = m.CodMedicamento,
                    CodBarra = m.CodBarra,
                    Descripcion = m.Descripcion,
                    RequiereReceta = m.RequiereReceta,
                    VentaLibre = m.VentaLibre,
                    PrecioUnitario = m.PrecioUnitario,
                    Dosis = m.Dosis,
                    Posologia = m.Posologia,
                    CodLoteMedicamento = m.CodLoteMedicamento,
                    LoteDescripcion = $"Lote {m.CodLoteMedicamento} - Vence: {m.CodLoteMedicamentoNavigation.FechaVencimiento:dd/MM/yyyy}",
                    StockDisponible = m.CodLoteMedicamentoNavigation.Cantidad,
                    FechaVencimiento = m.CodLoteMedicamentoNavigation.FechaVencimiento,
                    CodLaboratorio = m.CodLaboratorio,
                    LaboratorioDescripcion = m.CodLaboratorioNavigation.Descripcion,
                    CodTipoPresentacion = m.CodTipoPresentacion,
                    TipoPresentacionDescripcion = m.CodTipoPresentacionNavigation.Descripcion,
                    CodUnidadMedida = m.CodUnidadMedida,
                    UnidadMedidaDescripcion = m.CodUnidadMedidaNavigation.UnidadMedida,
                    CodTipoMedicamento = m.CodTipoMedicamento,
                    TipoMedicamentoDescripcion = m.CodTipoMedicamentoNavigation.Descripcion
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MedicamentoDto> CreateAsync(CreateMedicamentoDto createDto)
        {
            var medicamento = new Medicamento
            {
                CodBarra = createDto.CodBarra,
                Descripcion = createDto.Descripcion,
                RequiereReceta = createDto.RequiereReceta,
                VentaLibre = createDto.VentaLibre,
                PrecioUnitario = createDto.PrecioUnitario,
                Dosis = createDto.Dosis,
                Posologia = createDto.Posologia,
                CodLoteMedicamento = createDto.CodLoteMedicamento,
                CodLaboratorio = createDto.CodLaboratorio,
                CodTipoPresentacion = createDto.CodTipoPresentacion,
                CodUnidadMedida = createDto.CodUnidadMedida,
                CodTipoMedicamento = createDto.CodTipoMedicamento
            };

            _context.Medicamentos.Add(medicamento);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(medicamento.CodMedicamento) ?? throw new InvalidOperationException("Error al crear el medicamento");
        }

        public async Task<MedicamentoDto?> UpdateAsync(int id, UpdateMedicamentoDto updateDto)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
                return null;

            medicamento.CodBarra = updateDto.CodBarra;
            medicamento.Descripcion = updateDto.Descripcion;
            medicamento.RequiereReceta = updateDto.RequiereReceta;
            medicamento.VentaLibre = updateDto.VentaLibre;
            medicamento.PrecioUnitario = updateDto.PrecioUnitario;
            medicamento.Dosis = updateDto.Dosis;
            medicamento.Posologia = updateDto.Posologia;
            medicamento.CodLoteMedicamento = updateDto.CodLoteMedicamento;
            medicamento.CodLaboratorio = updateDto.CodLaboratorio;
            medicamento.CodTipoPresentacion = updateDto.CodTipoPresentacion;
            medicamento.CodUnidadMedida = updateDto.CodUnidadMedida;
            medicamento.CodTipoMedicamento = updateDto.CodTipoMedicamento;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
                return false;

            _context.Medicamentos.Remove(medicamento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Medicamentos.AnyAsync(m => m.CodMedicamento == id);
        }

        public async Task<IEnumerable<MedicamentoDto>> GetByLaboratorioAsync(int laboratorioId)
        {
            return await _context.Medicamentos
                .Include(m => m.CodLaboratorioNavigation)
                .Include(m => m.CodLoteMedicamentoNavigation)
                .Include(m => m.CodTipoMedicamentoNavigation)
                .Include(m => m.CodTipoPresentacionNavigation)
                .Include(m => m.CodUnidadMedidaNavigation)
                .Where(m => m.CodLaboratorio == laboratorioId)
                .Select(m => new MedicamentoDto
                {
                    CodMedicamento = m.CodMedicamento,
                    CodBarra = m.CodBarra,
                    Descripcion = m.Descripcion,
                    RequiereReceta = m.RequiereReceta,
                    VentaLibre = m.VentaLibre,
                    PrecioUnitario = m.PrecioUnitario,
                    Dosis = m.Dosis,
                    Posologia = m.Posologia,
                    CodLoteMedicamento = m.CodLoteMedicamento,
                    LoteDescripcion = $"Lote {m.CodLoteMedicamento} - Vence: {m.CodLoteMedicamentoNavigation.FechaVencimiento:dd/MM/yyyy}",
                    StockDisponible = m.CodLoteMedicamentoNavigation.Cantidad,
                    FechaVencimiento = m.CodLoteMedicamentoNavigation.FechaVencimiento,
                    CodLaboratorio = m.CodLaboratorio,
                    LaboratorioDescripcion = m.CodLaboratorioNavigation.Descripcion,
                    CodTipoPresentacion = m.CodTipoPresentacion,
                    TipoPresentacionDescripcion = m.CodTipoPresentacionNavigation.Descripcion,
                    CodUnidadMedida = m.CodUnidadMedida,
                    UnidadMedidaDescripcion = m.CodUnidadMedidaNavigation.UnidadMedida,
                    CodTipoMedicamento = m.CodTipoMedicamento,
                    TipoMedicamentoDescripcion = m.CodTipoMedicamentoNavigation.Descripcion
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicamentoDto>> GetByTipoMedicamentoAsync(int tipoMedicamentoId)
        {
            return await _context.Medicamentos
                .Include(m => m.CodLaboratorioNavigation)
                .Include(m => m.CodLoteMedicamentoNavigation)
                .Include(m => m.CodTipoMedicamentoNavigation)
                .Include(m => m.CodTipoPresentacionNavigation)
                .Include(m => m.CodUnidadMedidaNavigation)
                .Where(m => m.CodTipoMedicamento == tipoMedicamentoId)
                .Select(m => new MedicamentoDto
                {
                    CodMedicamento = m.CodMedicamento,
                    CodBarra = m.CodBarra,
                    Descripcion = m.Descripcion,
                    RequiereReceta = m.RequiereReceta,
                    VentaLibre = m.VentaLibre,
                    PrecioUnitario = m.PrecioUnitario,
                    Dosis = m.Dosis,
                    Posologia = m.Posologia,
                    CodLoteMedicamento = m.CodLoteMedicamento,
                    LoteDescripcion = $"Lote {m.CodLoteMedicamento} - Vence: {m.CodLoteMedicamentoNavigation.FechaVencimiento:dd/MM/yyyy}",
                    StockDisponible = m.CodLoteMedicamentoNavigation.Cantidad,
                    FechaVencimiento = m.CodLoteMedicamentoNavigation.FechaVencimiento,
                    CodLaboratorio = m.CodLaboratorio,
                    LaboratorioDescripcion = m.CodLaboratorioNavigation.Descripcion,
                    CodTipoPresentacion = m.CodTipoPresentacion,
                    TipoPresentacionDescripcion = m.CodTipoPresentacionNavigation.Descripcion,
                    CodUnidadMedida = m.CodUnidadMedida,
                    UnidadMedidaDescripcion = m.CodUnidadMedidaNavigation.UnidadMedida,
                    CodTipoMedicamento = m.CodTipoMedicamento,
                    TipoMedicamentoDescripcion = m.CodTipoMedicamentoNavigation.Descripcion
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicamentoDto>> SearchByDescripcionAsync(string descripcion)
        {
            return await _context.Medicamentos
                .Include(m => m.CodLaboratorioNavigation)
                .Include(m => m.CodLoteMedicamentoNavigation)
                .Include(m => m.CodTipoMedicamentoNavigation)
                .Include(m => m.CodTipoPresentacionNavigation)
                .Include(m => m.CodUnidadMedidaNavigation)
                .Where(m => m.Descripcion.Contains(descripcion))
                .Select(m => new MedicamentoDto
                {
                    CodMedicamento = m.CodMedicamento,
                    CodBarra = m.CodBarra,
                    Descripcion = m.Descripcion,
                    RequiereReceta = m.RequiereReceta,
                    VentaLibre = m.VentaLibre,
                    PrecioUnitario = m.PrecioUnitario,
                    Dosis = m.Dosis,
                    Posologia = m.Posologia,
                    CodLoteMedicamento = m.CodLoteMedicamento,
                    LoteDescripcion = $"Lote {m.CodLoteMedicamento} - Vence: {m.CodLoteMedicamentoNavigation.FechaVencimiento:dd/MM/yyyy}",
                    StockDisponible = m.CodLoteMedicamentoNavigation.Cantidad,
                    FechaVencimiento = m.CodLoteMedicamentoNavigation.FechaVencimiento,
                    CodLaboratorio = m.CodLaboratorio,
                    LaboratorioDescripcion = m.CodLaboratorioNavigation.Descripcion,
                    CodTipoPresentacion = m.CodTipoPresentacion,
                    TipoPresentacionDescripcion = m.CodTipoPresentacionNavigation.Descripcion,
                    CodUnidadMedida = m.CodUnidadMedida,
                    UnidadMedidaDescripcion = m.CodUnidadMedidaNavigation.UnidadMedida,
                    CodTipoMedicamento = m.CodTipoMedicamento,
                    TipoMedicamentoDescripcion = m.CodTipoMedicamentoNavigation.Descripcion
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicamentoDto>> GetMedicamentosConStockBajoAsync(int cantidadMinima = 10)
        {
            return await _context.Medicamentos
                .Include(m => m.CodLaboratorioNavigation)
                .Include(m => m.CodLoteMedicamentoNavigation)
                .Include(m => m.CodTipoMedicamentoNavigation)
                .Include(m => m.CodTipoPresentacionNavigation)
                .Include(m => m.CodUnidadMedidaNavigation)
                .Join(_context.StockMedicamentos,
                    medicamento => medicamento.CodMedicamento,
                    stock => stock.CodMedicamento,
                    (medicamento, stock) => new { medicamento, stock })
                .Where(ms => ms.stock.Cantidad < cantidadMinima)
                .Select(ms => new MedicamentoDto
                {
                    CodMedicamento = ms.medicamento.CodMedicamento,
                    CodBarra = ms.medicamento.CodBarra,
                    Descripcion = ms.medicamento.Descripcion,
                    RequiereReceta = ms.medicamento.RequiereReceta,
                    VentaLibre = ms.medicamento.VentaLibre,
                    PrecioUnitario = ms.medicamento.PrecioUnitario,
                    Dosis = ms.medicamento.Dosis,
                    Posologia = ms.medicamento.Posologia,
                    CodLoteMedicamento = ms.medicamento.CodLoteMedicamento,
                    LoteDescripcion = $"Lote {ms.medicamento.CodLoteMedicamento} - Vence: {ms.medicamento.CodLoteMedicamentoNavigation.FechaVencimiento:dd/MM/yyyy}",
                    StockDisponible = ms.stock.Cantidad,
                    FechaVencimiento = ms.medicamento.CodLoteMedicamentoNavigation.FechaVencimiento,
                    CodLaboratorio = ms.medicamento.CodLaboratorio,
                    LaboratorioDescripcion = ms.medicamento.CodLaboratorioNavigation.Descripcion,
                    CodTipoPresentacion = ms.medicamento.CodTipoPresentacion,
                    TipoPresentacionDescripcion = ms.medicamento.CodTipoPresentacionNavigation.Descripcion,
                    CodUnidadMedida = ms.medicamento.CodUnidadMedida,
                    UnidadMedidaDescripcion = ms.medicamento.CodUnidadMedidaNavigation.UnidadMedida,
                    CodTipoMedicamento = ms.medicamento.CodTipoMedicamento,
                    TipoMedicamentoDescripcion = ms.medicamento.CodTipoMedicamentoNavigation.Descripcion
                })
                .ToListAsync();
        }

        public async Task<int> GetCountMedicamentosConStockBajoAsync(int cantidadMinima = 10)
        {
            return await _context.Medicamentos
                .Join(_context.StockMedicamentos,
                    medicamento => medicamento.CodMedicamento,
                    stock => stock.CodMedicamento,
                    (medicamento, stock) => new { medicamento, stock })
                .Where(ms => ms.stock.Cantidad < cantidadMinima)
                .CountAsync();
        }
    }
}