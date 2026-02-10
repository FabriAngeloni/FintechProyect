using AuditService.Data;
using AuditService.DTOs;
using AuditService.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;

namespace AuditService.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AuditDbContext _dbContext;
        public AuditRepository(AuditDbContext context)
        {
            _dbContext = context;
            
        }

        public async Task<Audit> BuscarPorId(Guid auditId) => await _dbContext.Audit_Logs.FindAsync(auditId);

        public async Task CrearLog(Audit audit)
        {
            await _dbContext.Audit_Logs.AddAsync(audit);
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Audit>> RetornarParaReporte(DateTime desde, DateTime hasta)
        {
            var desdeInicio = desde.Date;
            var hastaFinal = hasta.Date.AddDays(1).AddTicks(-1);
            var todos = _dbContext.Audit_Logs;
            return todos
                .Where(a => a.Created_At >= desdeInicio && a.Created_At <= hastaFinal)
                .ToListAsync();
        }

        public async Task<IEnumerable<Audit>> RetornarTodos() => await _dbContext.Audit_Logs.ToListAsync();
        
    }
}
