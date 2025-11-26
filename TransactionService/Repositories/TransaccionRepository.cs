using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.Models;

namespace TransactionService.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly TransaccionDbContext _context;
        public TransaccionRepository(TransaccionDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Transaccion?> BuscarTransaccionPorId(Guid transaccionId) =>await _context.Transacciones.FindAsync(transaccionId);

        public async Task CancelarTransaccion(Guid transaccionId)
        {
            var transaccion = await _context.Transacciones.FindAsync(transaccionId);
            if (transaccion != null) _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();
        }

        public async Task CrearTransaccion(Transaccion transaccion)
        {
            await _context.Transacciones.AddAsync(transaccion);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaccion>> RetornarTransacciones() => await _context.Transacciones.ToListAsync();
    }
}
