using AccountService.Data;
using AccountService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AccountService.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDbContext _context;
        public AccountRepository(AccountDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cuenta>> BuscarCuentasPorUserId(Guid userId) => await _context.Cuentas.Where(c => c.UserId == userId).ToListAsync();

        public async Task<Cuenta> BuscarPorIdDeCuenta(Guid accountId) => await _context.Cuentas.FindAsync(accountId);

        public async Task ActualizarCuenta(Cuenta account)
        {
            _context.Cuentas.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task CrearCuenta(Cuenta account)
        {
            await _context.Cuentas.AddAsync(account);
            await _context.SaveChangesAsync();
        }
    }
}
