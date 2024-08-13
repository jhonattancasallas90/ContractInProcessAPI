using ContractInProcessAPI.Models;

namespace ContractInProcessAPI.DataAccess
{
    public interface IContractRepository
    {
        Task<List<EmpleadoME>> GetEmpleados(string? date);
        Task<string?> DeterminarCiudadesKactus(string codPais, string codDepartamento, string codMunicipio);
    }
}
