using AssetService.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetService.Repositories
{
    public interface IActivoRepository
    {
        Task Agregar(Activo activo);
        Task Borrar(Activo activo);
        Task<Activo?> BuscarPorId(Guid id);
        Task Modificar(Activo activo);
        Task<IEnumerable<Activo>> ObtenerTodos();
    }
}

