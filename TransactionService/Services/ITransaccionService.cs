using TransactionService.DTOs;
using TransactionService.Models;

namespace TransactionService.Services
{
    public interface ITransaccionService
    {
        public Task<TransaccionDtoResponse> CrearTransaccion(Guid desdeCuenta, decimal monto, Guid paraCuenta);
        public Task<TransaccionDtoResponse?> BuscarTransaccionPorId(Guid transaccionId);
        public Task<IEnumerable<TransaccionDtoResponse>?> RetornarTransacciones();
        public Task CancelarTransaccion(Guid transaccionId);

    }
}
