using AssetService.Data;
using AssetService.DTOs;
using AssetService.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetService.Repositories
{
    public class ActivoDelUserRepository : IActivoDelUserRepository
    {
        private readonly AssetDbContext _assetDbContext;
        public ActivoDelUserRepository(AssetDbContext dbContext)
        {
            _assetDbContext = dbContext;
        }
        public async Task Agregar(ActivoDeUser historia)
        {
            await _assetDbContext.ActivosDeUsuario.AddAsync(historia);
            await _assetDbContext.SaveChangesAsync();
        }
        public async Task<ActivoDeUser?> BuscarPorId(Guid id)=>await _assetDbContext.ActivosDeUsuario.FindAsync(id);

        public async Task<IEnumerable<ActivoDeUser>> ObtenerTodos()=>await _assetDbContext.ActivosDeUsuario.ToListAsync();
        
    }
}
