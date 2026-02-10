using AuditService.DTOs;
using AuditService.Models;

namespace AuditService.Services
{
    public interface IAuditServices
    {
        Task<AuditDtoResponse> BuscarPorId(Guid id);
        Task CrearLog(AuditDtoRequest request);
        Task<byte[]> GenerarReporte(DateTime desde, DateTime hasta);
    }
}
