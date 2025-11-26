using System.Transactions;
using TransactionService.Models;

namespace TransactionService.Repositories
{
    public interface ITransaccionRepository
    {
        public Task CrearTransaccion(Transaccion transaccion);
        public Task<Transaccion> BuscarTransaccionPorId(Guid transaccionId);
        public Task<IEnumerable<Transaccion>> RetornarTransacciones();
        public Task CancelarTransaccion(Guid transaccionId);
    }
}
