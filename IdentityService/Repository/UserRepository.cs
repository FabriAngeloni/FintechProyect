using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace IdentityService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;
        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ActualizarUsuario(User user)
        {
            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (usuario == null)
                throw new Exception($"Usuario ID:{user.Id} no existe.");
            usuario.PasswordHash = user.PasswordHash;
            usuario.NombreUsuario = user.NombreUsuario;
            usuario.Email = user.Email;
            usuario.Rol = user.Rol;
            await _dbContext.SaveChangesAsync();
        }

        public async Task BorrarUsuario(User user)
        {
            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (usuario == null)
                throw new Exception($"No se encontro un usuario con el ID:{user.Id}");
            _dbContext.Usuarios.Remove(usuario);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> BuscarPorEmail(string email)=> await _dbContext.Usuarios.FirstOrDefaultAsync(e => e.Email == email);
        

        public async Task<User?> BuscarPorId(Guid id)=> await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        public async Task CrearUsuario(User user)
        {
            if (user == null)
                throw new Exception("El usuario llego nulo.");
            await _dbContext.Usuarios.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>?> RetornarUsuarios() => await _dbContext.Usuarios.ToListAsync();
        
    }
}
