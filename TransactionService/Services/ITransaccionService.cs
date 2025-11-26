using TransactionService.DTOs;
using TransactionService.Models;

namespace TransactionService.Services
{
    public interface ITransaccionService
    {
        public Task<Transaccion> CrearTransaccion(Guid desdeCuenta, decimal monto, Guid paraCuenta);
        public Task<Transaccion?> BuscarTransaccionPorId(Guid transaccionId);
        public Task<IEnumerable<Transaccion>?> RetornarTransacciones();
        public Task CancelarTransaccion(Guid transaccionId);

    }
}
