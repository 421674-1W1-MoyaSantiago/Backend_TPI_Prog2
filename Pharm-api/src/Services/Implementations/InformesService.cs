using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Repositories;

namespace Pharm_api.Services
{
    public class InformesService : IInformesService
    {
        private readonly IInformesRepository _repository;

        public InformesService(IInformesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ObraSocialDto>> GetObrasSocialesAsync()
        {
            IEnumerable<ObrasSociale> obrasSociales = await _repository.GetObrasSocialesAsync();
            return obrasSociales.Select(os => new ObraSocialDto
            {
                Id = os.CodObraSocial,
                Nombre = os.RazonSocial
            });
        }

        public IEnumerable<string> GetEstadosAutorizaciones()
        {
            return new List<string>() { "Autorizada", "En Revisión", "Pendiente", "Rechazado" };
        }

        public async Task<IEnumerable<TopMedicamentoYVendedorPorEstacionDto>> GetTopMedicamentoYVendedorPorEstacionAsync(string estacion)
        {
            List<string> estacionesValidas = new List<string> { "Verano", "Otoño", "Invierno", "Primavera" };
            if (!estacionesValidas.Contains(estacion))
            {
                throw new ArgumentException("Estación inválida. Las estaciones válidas son: 'Verano', 'Otoño', 'Invierno' y 'Primavera'.");
            }
            return await _repository.GetTopMedicamentoYVendedorPorEstacionAsync(estacion);

        }

        public async Task<IEnumerable<ComprasSuministrosConAutorizacionObraSocialDto>> GetComprasSuministrosConAutorizacionObraSocialAsync()
        {
            return await _repository.GetComprasSuministrosConAutorizacionObraSocialAsync();
        }

        public async Task<IEnumerable<VentasMedicamentosObraSocialDto>> GetVentasMedicamentosObraSocialAsync(DateTime fechaInicio, DateTime fechaFin, string? nombreObraSocial = null)
        {
            // Validar las fechas antes de proceder
            // No usar fechas default
            if (fechaInicio == new DateTime() || fechaFin == new DateTime())
            {
                throw new InvalidOperationException("Debe indicar las fechas de inicio y de fin.");
            }

            if (fechaInicio > fechaFin)
            {
                throw new InvalidOperationException("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            if (fechaFin > DateTime.Now)
            {
                throw new InvalidOperationException("La fecha de fin no puede ser una fecha futura.");
            }

            // Validar si la obra social existe
            if (!string.IsNullOrEmpty(nombreObraSocial))
            {
                if (nombreObraSocial != "Sin Obra Social" && !await _repository.DoObraSocialExists(nombreObraSocial))
                {
                    throw new InvalidOperationException($"La obra social '{nombreObraSocial}' no existe.");
                }
            }

            return await _repository.GetVentasMedicamentosObraSocialAsync(fechaInicio, fechaFin, nombreObraSocial);
        }

        public async Task<IEnumerable<RecetasObraSocialEstadoDto>> GetRecetasObraSocialEstadoAsync(string? nombreObraSocial, string? estado)
        {
            // Validar si la obra social existe
            if (!string.IsNullOrEmpty(nombreObraSocial))
            {
                if (!await _repository.DoObraSocialExists(nombreObraSocial))
                {
                    throw new InvalidOperationException($"La obra social '{nombreObraSocial}' no existe.");
                }
            }

            // Validar estado
            var estadosValidos = GetEstadosAutorizaciones();
            if (!string.IsNullOrEmpty(estado) && !estadosValidos.Contains(estado))
            {
                throw new InvalidOperationException($"El estado '{estado}' no es válido. Los estados válidos son: {string.Join(", ", estadosValidos)}.");
            }

            return await _repository.GetRecetasObraSocialEstadoAsync(nombreObraSocial, estado);
        }

        public async Task<IEnumerable<ConsultarReintegrosObrasSocialesDto>> GetReintegrosObrasSocialesAsync(
            DateTime fechaInicio, DateTime fechaFin, string? nombreObraSocial = null, string? estado = null
        )
        {
            // Validar las fechas antes de proceder
            // No usar fechas default
            validarFechas(fechaInicio, fechaFin);
            

            // Validar si la obra social existe
            if (!string.IsNullOrEmpty(nombreObraSocial))
            {
                if (!await _repository.DoObraSocialExists(nombreObraSocial))
                {
                    throw new InvalidOperationException($"La obra social '{nombreObraSocial}' no existe.");
                }
            }

            return await _repository.GetReintegrosObrasSocialesAsync(fechaInicio, fechaFin, nombreObraSocial, estado);
        }

        public async Task<IEnumerable<ReporteVentasPorObraSocialDto>> GetReporteVentasPorObraSocialAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            // Validar fechas
            validarFechas(fechaInicio, fechaFin);
            return await _repository.GetReporteVentasPorObraSocialAsync(fechaInicio, fechaFin);
        }

        private bool validarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio == new DateTime() || fechaFin == new DateTime())
            {
                throw new InvalidOperationException("Debe indicar las fechas de inicio y de fin.");
            }
            if (fechaInicio > fechaFin)
            {
                throw new InvalidOperationException("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }
            if (fechaFin > DateTime.Now)
            {
                throw new InvalidOperationException("La fecha de fin no puede ser una fecha futura.");
            }
            return true;
        }

    }
}
