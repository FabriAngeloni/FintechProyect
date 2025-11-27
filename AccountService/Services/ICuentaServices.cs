using AccountService.Models;


namespace AccountService.Services
{
    public interface ICuentaServices
    {
        Task<Cuenta> CrearCuentaAsync(string nombreUsuario, decimal balance);
        Task<Cuenta> BuscarPorIdAsync(Guid accountId);
        Task<IEnumerable<Cuenta>> BuscarPorUserIdAsync(Guid userId);
        Task DepositarAsync(Guid accountId, decimal monto);
        Task ExtraerAsync(Guid accountId, decimal monto);

    }
}
