using Pharm_api.DTOs;
using Pharm_api.Repositories;
using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;

namespace Pharm_api.Services
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly IMedicamentoRepository _repository;
        private readonly PharmDbContext _context;

        public MedicamentoService(IMedicamentoRepository repository, PharmDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<MedicamentoDto>> GetAllMedicamentosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<MedicamentoDto?> GetMedicamentoByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<MedicamentoDto> CreateMedicamentoAsync(CreateMedicamentoDto createDto)
        {
            // Validaciones de negocio
            var isValid = await ValidateMedicamentoDataAsync(createDto);
            if (!isValid)
            {
                throw new InvalidOperationException("Los datos del medicamento no son válidos");
            }

            // Verificar que el código de barra sea único (si se proporciona)
            if (!string.IsNullOrEmpty(createDto.CodBarra))
            {
                var existsWithSameBarcode = await _context.Medicamentos
                    .AnyAsync(m => m.CodBarra == createDto.CodBarra);
                if (existsWithSameBarcode)
                {
                    throw new InvalidOperationException("Ya existe un medicamento con este código de barra");
                }
            }

            return await _repository.CreateAsync(createDto);
        }

        public async Task<MedicamentoDto?> UpdateMedicamentoAsync(int id, UpdateMedicamentoDto updateDto)
        {
            // Verificar que el medicamento existe
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return null;
            }

            // Validaciones de negocio
            var isValid = await ValidateMedicamentoDataAsync(updateDto);
            if (!isValid)
            {
                throw new InvalidOperationException("Los datos del medicamento no son válidos");
            }

            // Verificar que el código de barra sea único (si se proporciona)
            if (!string.IsNullOrEmpty(updateDto.CodBarra))
            {
                var existsWithSameBarcode = await _context.Medicamentos
                    .AnyAsync(m => m.CodBarra == updateDto.CodBarra && m.CodMedicamento != id);
                if (existsWithSameBarcode)
                {
                    throw new InvalidOperationException("Ya existe otro medicamento con este código de barra");
                }
            }

            return await _repository.UpdateAsync(id, updateDto);
        }

        public async Task<bool> DeleteMedicamentoAsync(int id)
        {
            // Verificar que no hay facturas asociadas al medicamento
            var hasRelatedRecords = await _context.DetallesFacturaVentasMedicamento
                .AnyAsync(d => d.codMedicamento == id);
            
            if (hasRelatedRecords)
            {
                throw new InvalidOperationException("No se puede eliminar el medicamento porque tiene registros asociados en facturas");
            }

            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MedicamentoDto>> GetMedicamentosByLaboratorioAsync(int laboratorioId)
        {
            return await _repository.GetByLaboratorioAsync(laboratorioId);
        }

        public async Task<IEnumerable<MedicamentoDto>> GetMedicamentosByTipoAsync(int tipoMedicamentoId)
        {
            return await _repository.GetByTipoMedicamentoAsync(tipoMedicamentoId);
        }

        public async Task<IEnumerable<MedicamentoDto>> SearchMedicamentosByDescripcionAsync(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                return new List<MedicamentoDto>();
            }

            return await _repository.SearchByDescripcionAsync(descripcion.Trim());
        }

        public async Task<IEnumerable<MedicamentoDto>> GetMedicamentosConStockBajoAsync(int cantidadMinima = 10)
        {
            return await _repository.GetMedicamentosConStockBajoAsync(cantidadMinima);
        }

        public async Task<int> GetCountMedicamentosConStockBajoAsync(int cantidadMinima = 10)
        {
            return await _repository.GetCountMedicamentosConStockBajoAsync(cantidadMinima);
        }

        public async Task<bool> ValidateMedicamentoDataAsync(CreateMedicamentoDto createDto)
        {
            // Validar que el laboratorio existe
            var laboratorioExists = await _context.Laboratorios
                .AnyAsync(l => l.CodLaboratorio == createDto.CodLaboratorio);
            if (!laboratorioExists)
            {
                throw new InvalidOperationException("El laboratorio especificado no existe");
            }

            // Validar que el tipo de medicamento existe
            var tipoMedicamentoExists = await _context.TiposMedicamentos
                .AnyAsync(t => t.CodTipoMedicamento == createDto.CodTipoMedicamento);
            if (!tipoMedicamentoExists)
            {
                throw new InvalidOperationException("El tipo de medicamento especificado no existe");
            }

            // Validar que el tipo de presentación existe
            var tipoPresentacionExists = await _context.TiposPresentacions
                .AnyAsync(t => t.CodTipoPresentacion == createDto.CodTipoPresentacion);
            if (!tipoPresentacionExists)
            {
                throw new InvalidOperationException("El tipo de presentación especificado no existe");
            }

            // Validar que la unidad de medida existe
            var unidadMedidaExists = await _context.UnidadesMedida
                .AnyAsync(u => u.CodUnidadMedida == createDto.CodUnidadMedida);
            if (!unidadMedidaExists)
            {
                throw new InvalidOperationException("La unidad de medida especificada no existe");
            }

            // Validar que el lote de medicamento existe
            var loteExists = await _context.LotesMedicamentos
                .AnyAsync(l => l.CodLoteMedicamento == createDto.CodLoteMedicamento);
            if (!loteExists)
            {
                throw new InvalidOperationException("El lote de medicamento especificado no existe");
            }

            // Validar lógica de negocio: un medicamento no puede requerir receta Y ser de venta libre al mismo tiempo
            if (createDto.RequiereReceta && createDto.VentaLibre)
            {
                throw new InvalidOperationException("Un medicamento no puede requerir receta y ser de venta libre al mismo tiempo");
            }

            // Validar que al menos una de las dos opciones esté seleccionada
            if (!createDto.RequiereReceta && !createDto.VentaLibre)
            {
                throw new InvalidOperationException("El medicamento debe ser de venta libre o requerir receta");
            }

            return true;
        }

        public async Task<bool> ValidateMedicamentoDataAsync(UpdateMedicamentoDto updateDto)
        {
            // Validar que el laboratorio existe
            var laboratorioExists = await _context.Laboratorios
                .AnyAsync(l => l.CodLaboratorio == updateDto.CodLaboratorio);
            if (!laboratorioExists)
            {
                throw new InvalidOperationException("El laboratorio especificado no existe");
            }

            // Validar que el tipo de medicamento existe
            var tipoMedicamentoExists = await _context.TiposMedicamentos
                .AnyAsync(t => t.CodTipoMedicamento == updateDto.CodTipoMedicamento);
            if (!tipoMedicamentoExists)
            {
                throw new InvalidOperationException("El tipo de medicamento especificado no existe");
            }

            // Validar que el tipo de presentación existe
            var tipoPresentacionExists = await _context.TiposPresentacions
                .AnyAsync(t => t.CodTipoPresentacion == updateDto.CodTipoPresentacion);
            if (!tipoPresentacionExists)
            {
                throw new InvalidOperationException("El tipo de presentación especificado no existe");
            }

            // Validar que la unidad de medida existe
            var unidadMedidaExists = await _context.UnidadesMedida
                .AnyAsync(u => u.CodUnidadMedida == updateDto.CodUnidadMedida);
            if (!unidadMedidaExists)
            {
                throw new InvalidOperationException("La unidad de medida especificada no existe");
            }

            // Validar que el lote de medicamento existe
            var loteExists = await _context.LotesMedicamentos
                .AnyAsync(l => l.CodLoteMedicamento == updateDto.CodLoteMedicamento);
            if (!loteExists)
            {
                throw new InvalidOperationException("El lote de medicamento especificado no existe");
            }

            // Validar lógica de negocio: un medicamento no puede requerir receta Y ser de venta libre al mismo tiempo
            if (updateDto.RequiereReceta && updateDto.VentaLibre)
            {
                throw new InvalidOperationException("Un medicamento no puede requerir receta y ser de venta libre al mismo tiempo");
            }

            // Validar que al menos una de las dos opciones esté seleccionada
            if (!updateDto.RequiereReceta && !updateDto.VentaLibre)
            {
                throw new InvalidOperationException("El medicamento debe ser de venta libre o requerir receta");
            }

            return true;
        }

        public async Task<IEnumerable<MedicamentoDto>> GetBySucursalAsync(int codSucursal)
        {
            return await _repository.GetBySucursalAsync(codSucursal);
        }
    }
}