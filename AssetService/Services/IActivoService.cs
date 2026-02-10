using AssetService.DTOs;
using AssetService.Models;

namespace AssetService.Services
{
    public interface IActivoService
    {
        Task<ActivoDtoResponse> CrearActivo(ActivoDtoRequest dto);
        Task BorrarActivo(Guid id);
        Task<ActivoDtoResponse?> BuscarActivoPorId(Guid id);
        Task ModificarActivo(Guid id,ModificarActivoDto dto);
        Task<IEnumerable<ActivoDtoResponse>> ObtenerTodosLosActivos();
    }
}
