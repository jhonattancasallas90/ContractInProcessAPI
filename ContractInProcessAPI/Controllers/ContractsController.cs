using ContractInProcessAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractInProcessAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly IContractRepository _contractRepository;

        public ContractsController(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        // Acción para obtener empleados por fecha
        [Authorize]
        [HttpGet("empleados")]
        public async Task<IActionResult> GetEmpleadosPorFecha([FromQuery] string? date)
        {
            var contracts = await _contractRepository.GetEmpleados(date);
            return Ok(contracts);
        }

        [HttpGet]
        public async Task<IActionResult> DeterminarCiudadesDesdeKactus(string codPais, string codDepartamento, string codMunicipio)
        {
            var cities = await _contractRepository.DeterminarCiudadesKactus(codPais, codDepartamento, codMunicipio);
            return Ok(cities);
        }
    }
}
