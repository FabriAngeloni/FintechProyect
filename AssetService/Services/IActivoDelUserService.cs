using AssetService.DTOs;
using AssetService.Models;

namespace AssetService.Services
{
    public interface IActivoDelUserService
    {
        Task<ActivoDeUserDtoResponse> Agregar(ActivoDeUserDtoRequest activo);
        Task<ActivoDeUserDtoResponse?> BuscarPorId(Guid id);
        Task<IEnumerable<ActivoDeUserDtoResponse>> ObtenerTodos();
    }
}
