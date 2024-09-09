using ContractInProcessAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractInProcessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly IContractRepository _contractRepository;

        public ContractsController(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        // Acción para obtener información del colaborador con parametro de entrada "Fecha" - Para accionarlo debe ingresarse las credenciales
        [Authorize]
        [HttpGet("empleados")]
        public async Task<IActionResult> GetEmpleadosPorFecha([FromQuery] string? date)
        {
            var contracts = await _contractRepository.GetEmpleados(date);
            return Ok(contracts);
        }

        // Accion que verifica la ciudad del colaborador dentro del contrato - Utilizando convenciones del DANE incluidas en base de datos de Kactus
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeterminarCiudadesDesdeKactus(string codPais, string codDepartamento, string codMunicipio)
        {
            var cities = await _contractRepository.DeterminarCiudadesKactus(codPais, codDepartamento, codMunicipio);
            return Ok(cities);
        }
    }
}
