using AuditService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Data
{
    public class AuditDbContext : DbContext
    {
        public DbSet<Audit> Audit_Logs{ get; set; }
        public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
        {
            
        }

    }
}
