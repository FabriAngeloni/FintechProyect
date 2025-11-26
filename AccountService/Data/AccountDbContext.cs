using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions options) : base(options)
        {
        }
        protected AccountDbContext()
        {
        }

        public DbSet<Cuenta> Cuentas{ get; set; }
    }
}
