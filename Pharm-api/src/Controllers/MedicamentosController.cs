using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pharm_api.DTOs;
using Pharm_api.Services;

namespace Pharm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación para todos los endpoints
    public class MedicamentosController : ControllerBase
    {
        private readonly IMedicamentoService _medicamentoService;

        public MedicamentosController(IMedicamentoService medicamentoService)
        {
            _medicamentoService = medicamentoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> GetMedicamentos()
        {
            try
            {
                var medicamentos = await _medicamentoService.GetAllMedicamentosAsync();
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicamentoDto>> GetMedicamento(int id)
        {
            try
            {
                var medicamento = await _medicamentoService.GetMedicamentoByIdAsync(id);
                if (medicamento == null)
                    return NotFound($"Medicamento con ID {id} no encontrado");

                return Ok(medicamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamento: {ex.Message}");
            }
        }

        [HttpGet("laboratorio/{laboratorioId}")]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> GetMedicamentosByLaboratorio(int laboratorioId)
        {
            try
            {
                var medicamentos = await _medicamentoService.GetMedicamentosByLaboratorioAsync(laboratorioId);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos por laboratorio: {ex.Message}");
            }
        }

        [HttpGet("tipo/{tipoMedicamentoId}")]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> GetMedicamentosByTipo(int tipoMedicamentoId)
        {
            try
            {
                var medicamentos = await _medicamentoService.GetMedicamentosByTipoAsync(tipoMedicamentoId);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos por tipo: {ex.Message}");
            }
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> SearchMedicamentos([FromQuery] string descripcion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descripcion))
                {
                    return BadRequest("La descripción de búsqueda es requerida");
                }

                var medicamentos = await _medicamentoService.SearchMedicamentosByDescripcionAsync(descripcion);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al buscar medicamentos: {ex.Message}");
            }
        }

        [HttpGet("stock-bajo")]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> GetMedicamentosConStockBajo([FromQuery] int cantidadMinima = 10)
        {
            try
            {
                var medicamentos = await _medicamentoService.GetMedicamentosConStockBajoAsync(cantidadMinima);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos con stock bajo: {ex.Message}");
            }
        }

        [HttpGet("stock-bajo/count")]
        public async Task<ActionResult<object>> GetCountMedicamentosConStockBajo([FromQuery] int cantidadMinima = 10)
        {
            try
            {
                var count = await _medicamentoService.GetCountMedicamentosConStockBajoAsync(cantidadMinima);
                return Ok(new { 
                    CantidadMedicamentos = count, 
                    CantidadMinima = cantidadMinima,
                    Mensaje = $"Hay {count} medicamentos con stock menor a {cantidadMinima} unidades"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener conteo de medicamentos con stock bajo: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MedicamentoDto>> CreateMedicamento(CreateMedicamentoDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var medicamento = await _medicamentoService.CreateMedicamentoAsync(createDto);
                return CreatedAtAction(nameof(GetMedicamento), new { id = medicamento.CodMedicamento }, medicamento);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear medicamento: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MedicamentoDto>> UpdateMedicamento(int id, UpdateMedicamentoDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var medicamento = await _medicamentoService.UpdateMedicamentoAsync(id, updateDto);
                if (medicamento == null)
                    return NotFound($"Medicamento con ID {id} no encontrado");

                return Ok(medicamento);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar medicamento: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicamento(int id)
        {
            try
            {
                var deleted = await _medicamentoService.DeleteMedicamentoAsync(id);
                if (!deleted)
                    return NotFound($"Medicamento con ID {id} no encontrado");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar medicamento: {ex.Message}");
            }
        }
    }
}