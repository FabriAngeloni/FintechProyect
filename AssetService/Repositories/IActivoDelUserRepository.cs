using AssetService.DTOs;
using AssetService.Models;

namespace AssetService.Repositories
{
    public interface IActivoDelUserRepository
    {
        Task Agregar(ActivoDeUser historia);
        Task<ActivoDeUser?> BuscarPorId(Guid id);
        Task<IEnumerable<ActivoDeUser>> ObtenerTodos();
    }
}
