using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data
{
    public class CuentaDbContext : DbContext
    {
        public CuentaDbContext(DbContextOptions<CuentaDbContext> options) : base(options)
        {
        }
   

        public DbSet<Cuenta> Cuentas{ get; set; }
    }
}
