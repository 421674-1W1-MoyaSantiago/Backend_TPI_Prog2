using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharm_api.DTOs;
using Pharm_api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoberturasController : ControllerBase
    {
        private readonly ICoberturaService _coberturaService;
        public CoberturasController(ICoberturaService coberturaService)
        {
            _coberturaService = coberturaService;
        }


        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<IEnumerable<CoberturaDto>>> GetCoberturasByCliente(int idCliente)
        {
            var coberturas = await _coberturaService.GetCoberturasByClienteAsync(idCliente);
            return Ok(coberturas);
        }
    }
}
