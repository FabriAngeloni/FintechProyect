using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

namespace TransactionService.Data
{
    public class TransaccionDbContext : DbContext
    {
        public TransaccionDbContext(DbContextOptions<TransaccionDbContext> options) : base(options)
        {
        }

        public DbSet<Transaccion> Transacciones { get; set; }
    }
}
