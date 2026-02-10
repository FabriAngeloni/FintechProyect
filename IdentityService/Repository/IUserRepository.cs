using IdentityService.DTOs;
using IdentityService.Models;

namespace IdentityService.Repository
{
    public interface IUserRepository
    {
        public Task<User?> BuscarPorId(Guid id);
        public Task<IEnumerable<User>?> RetornarUsuarios();
        public Task ActualizarUsuario(User user);
        public Task BorrarUsuario(User user);
        public Task CrearUsuario(User user);
        public Task<User?> BuscarPorEmail(string email);
    }
}
