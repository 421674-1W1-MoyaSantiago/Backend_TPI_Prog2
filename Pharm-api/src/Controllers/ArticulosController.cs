using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pharm_api.DTOs;
using Pharm_api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ArticulosController : ControllerBase
    {
        private readonly IArticuloService _articuloService;
        public ArticulosController(IArticuloService articuloService)
        {
            _articuloService = articuloService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticuloDto>>> GetArticulos()
        {
            var articulos = await _articuloService.GetAllArticulosAsync();
            return Ok(articulos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticuloDto>> GetArticulo(int id)
        {
            var articulo = await _articuloService.GetArticuloByIdAsync(id);
            if (articulo == null) return NotFound();
            return Ok(articulo);
        }

        [HttpGet("sucursal/{codSucursal}")]
        public async Task<ActionResult<IEnumerable<ArticuloDto>>> GetArticulosPorSucursal(int codSucursal)
        {
            var articulos = await _articuloService.GetBySucursalAsync(codSucursal);
            return Ok(articulos);
        }
    }
}
