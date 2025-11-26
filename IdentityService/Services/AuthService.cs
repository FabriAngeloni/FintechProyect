using IdentityService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityService.Services
{
    public class AuthService
    {
        private readonly List<User> _users = new();
        private readonly string _jwtSecret = "SoyElComeTrabasDeUruguayTrabaQueVeoTrabaQueHagoMierda";
        public User Register(string nombreUsuario, string mail, string contraseña)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                NombreUsuario = nombreUsuario,
                Email = mail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(contraseña),
                Rol = "User"
            };
            _users.Add(user);
            return user;
        }

        public string Login(string email,string contraseña)
        {
            var user = _users.FirstOrDefault(x => x.Email == email);
            if (user == null) throw new ValidarUsuarioException("Validar usuario ingresado") ;
            if (!BCrypt.Net.BCrypt.Verify(contraseña, user.PasswordHash))throw new ValidarUsuarioException("La contraseña no es correcta.");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.NombreUsuario),
                    new Claim(ClaimTypes.Role, user.Rol)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public class ValidarUsuarioException : Exception
        {
            public ValidarUsuarioException(string mensaje) : base(mensaje)
            { }
        }
    }
}
