using AuditService.Models;

namespace AuditService.Repositories
{
    public interface IAuditRepository
    {
        Task CrearLog(Audit audit);
        Task<Audit> BuscarPorId(Guid auditId);
        Task<IEnumerable<Audit>> RetornarTodos();
        Task<List<Audit>> RetornarParaReporte(DateTime desde, DateTime hasta);
    }
}
