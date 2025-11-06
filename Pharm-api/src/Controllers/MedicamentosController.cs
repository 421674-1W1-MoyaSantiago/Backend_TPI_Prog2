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
        private readonly IUsuarioService _usuarioService;

        public MedicamentosController(IMedicamentoService medicamentoService, IUsuarioService usuarioService)
        {
            _medicamentoService = medicamentoService;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> GetMedicamentos()
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                var medicamentos = await _medicamentoService.GetAllMedicamentosByUsuarioAsync(userId.Value);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MedicamentoDto>> GetMedicamento(int id)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                var medicamento = await _medicamentoService.GetMedicamentoByIdAsync(id, userId.Value);
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> GetMedicamentosByLaboratorio(int laboratorioId)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                var medicamentos = await _medicamentoService.GetMedicamentosByLaboratorioAsync(laboratorioId, userId.Value);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos por laboratorio: {ex.Message}");
            }
        }

        [HttpGet("tipo/{tipoMedicamentoId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> GetMedicamentosByTipo(int tipoMedicamentoId)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                var medicamentos = await _medicamentoService.GetMedicamentosByTipoAsync(tipoMedicamentoId, userId.Value);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos por tipo: {ex.Message}");
            }
        }

        [HttpGet("buscar")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> SearchMedicamentos([FromQuery] string descripcion)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                if (string.IsNullOrWhiteSpace(descripcion))
                {
                    return BadRequest("La descripción de búsqueda es requerida");
                }

                var medicamentos = await _medicamentoService.SearchMedicamentosByDescripcionAsync(descripcion, userId.Value);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al buscar medicamentos: {ex.Message}");
            }
        }

        [HttpGet("stock-bajo")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MedicamentoDto>>> GetMedicamentosConStockBajo([FromQuery] int cantidadMinima = 10)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                var medicamentos = await _medicamentoService.GetMedicamentosConStockBajoAsync(cantidadMinima, userId.Value);
                return Ok(medicamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos con stock bajo: {ex.Message}");
            }
        }

        [HttpGet("stock-bajo/count")]
        [Authorize]
        public async Task<ActionResult<object>> GetCountMedicamentosConStockBajo([FromQuery] int cantidadMinima = 10)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                var count = await _medicamentoService.GetCountMedicamentosConStockBajoAsync(cantidadMinima, userId.Value);
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
        [Authorize]
        public async Task<ActionResult<MedicamentoDto>> CreateMedicamento(CreateMedicamentoDto createDto)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var medicamento = await _medicamentoService.CreateMedicamentoAsync(createDto, userId.Value);
                if (medicamento == null)
                    return BadRequest("No se pudo crear el medicamento o no tienes acceso a la sucursal especificada");

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
        [Authorize]
        public async Task<ActionResult<MedicamentoDto>> UpdateMedicamento(int id, UpdateMedicamentoDto updateDto)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var medicamento = await _medicamentoService.UpdateMedicamentoAsync(id, updateDto, userId.Value);
                if (medicamento == null)
                    return NotFound($"Medicamento con ID {id} no encontrado o no tienes acceso");

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
        [Authorize]
        public async Task<IActionResult> DeleteMedicamento(int id)
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromToken(HttpContext);
                if (userId == null)
                    return Unauthorized();

                var deleted = await _medicamentoService.DeleteMedicamentoAsync(id, userId.Value);
                if (!deleted)
                    return NotFound($"Medicamento con ID {id} no encontrado o no tienes acceso");

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