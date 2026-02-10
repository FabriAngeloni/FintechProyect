using AccountService.DTOs;
using AccountService.Models;


namespace AccountService.Services
{
    public interface ICuentaService
    {
        Task<CuentaDtoResponse> CrearCuentaAsync(CrearCuentaParaUsuarioDto dtoRequest);

        Task<CuentaDtoResponse> BuscarPorIdAsync(Guid accountId);
        Task<IEnumerable<CuentaDtoResponse>> BuscarPorUserIdAsync(Guid userId);

        Task<BalanceDtoResponse> Debito(Guid accountId, decimal monto);
        Task<BalanceDtoResponse> Acreditar(Guid accountId, decimal monto);

    }
}
