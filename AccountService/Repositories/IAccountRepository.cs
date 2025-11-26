using AccountService.Models;

namespace AccountService.Repositories
{
    public interface IAccountRepository
    {
        Task<Cuenta> BuscarPorIdDeCuenta(Guid accountId);
        Task ActualizarCuenta(Cuenta account);
        Task<IEnumerable<Cuenta>> BuscarCuentasPorUserId(Guid userId);
        Task CrearCuenta(Cuenta account);
    }
}
