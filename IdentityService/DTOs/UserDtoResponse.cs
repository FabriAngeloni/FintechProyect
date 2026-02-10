using IdentityService.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.DTOs
{
    public class UserDtoResponse
    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; } 
        public string Rol { get; set; }
        public string Token { get; set; }

        public UserDtoResponse(User user)
        {
            Id = user.Id;
            NombreUsuario = user.NombreUsuario;
            Email = user.Email;
            Rol = user.Rol;
        }
        public UserDtoResponse()
        {
        }
    }
}
