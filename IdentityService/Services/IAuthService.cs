using IdentityService.DTOs;
using IdentityService.Models;
using IdentityService.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static IdentityService.Services.AuthService;

namespace IdentityService.Services
{
    public interface IAuthService
    {
        public Task<IEnumerable<UserDtoResponse>> RetornarUsuarios();

        public Task<UserDtoResponse> Register(string nombreUsuario, string email, string contraseña);

        public Task<UserDtoResponse> BuscarPorId(Guid id);

        public Task<UserDtoResponse> Login(string email, string contraseña);
       
        
    }
}
