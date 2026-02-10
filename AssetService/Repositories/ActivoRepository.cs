using AssetService.Data;
using AssetService.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetService.Repositories
{
    public class ActivoRepository : IActivoRepository
    {
        private readonly AssetDbContext _dbContext;
        public ActivoRepository(AssetDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Agregar(Activo activo)
        {
            await _dbContext.Activos.AddAsync(activo);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Borrar(Activo activo)
        {
            var aux = await _dbContext.Activos.FindAsync(activo.Id);
            if (aux != null) _dbContext.Remove(aux);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<Activo?> BuscarPorId(Guid id) => await _dbContext.Activos.FindAsync(id);
        public async Task Modificar(Activo activo)
        {
            var aux = await _dbContext.Activos.FindAsync(activo.Id);
            if (aux == null)
                throw new Exception("No se encontro el activo a modificar.");
            _dbContext.Entry(aux).CurrentValues.SetValues(activo);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Activo>> ObtenerTodos() => await _dbContext.Activos.ToListAsync();

    }
}

