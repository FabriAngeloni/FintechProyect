using IdentityService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdentityService.DTOs;
using IdentityService.Repository;
using RabbitMQ.Client;
using AccountService.Messaging.Abstractions;
using AccountService.Messaging.Events;

namespace IdentityService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMessagePublisher _publisher;
        private readonly string _jwtSecret = "DesarrolloDeProyectoDeTipoFintechRealizadoPorFabricioAngeloni";
        public AuthService(IUserRepository repository, ILogger<AuthService> logger, IMessagePublisher publisher)
        {
            _repository = repository;
            _logger = logger;
            _publisher = publisher;
        }
        public async Task<UserDtoResponse> Register(string nombreUsuario, string email, string contraseña)
        {
            _logger.LogInformation("Service: Registrando usuario...{Email}", email);
            try
            {
                if (string.IsNullOrWhiteSpace(nombreUsuario))
                {
                    _logger.LogWarning("Service: nombre de usuario {Nombre}", nombreUsuario);
                    throw new ArgumentException("Validar el nombre de usuario enviado, el mismo no puede encontrarse vacio.");
                }

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Service: email del usuario {Email}", email);
                    throw new ArgumentException("Validar el email del usuario enviado, el mismo no puede encontrarse vacio.");
                }

                if (contraseña.Length < 8)
                {
                    _logger.LogWarning("Service: contraseña invalida para el email: {Email} nombreUsuario:{Usuario}, no puede contener menos de 8 caracteres", email, nombreUsuario);
                    throw new ArgumentException("Validar la contraseña ingresada , la misma no puede contener menos de 8 caracteres.");
                }

                _logger.LogInformation("Service: iniciando la creacion del usuario.\n" +
                    "Nombre usuario: {User}\n" +
                    "Email: {Email}\n" +
                    "¡Password aceptada!\n",
                    nombreUsuario,email);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = nombreUsuario,
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(contraseña),
                    Rol = "User"
                };
                var userDto = new UserDtoResponse(user);
                await _publisher.PublishAsync(new UsuarioCreadoEvent(userDto.Id,userDto.NombreUsuario,userDto.Email));
                _logger.LogInformation("Service: publicando evento de creacion de usuario...{Nombre}, {Email}.", nombreUsuario, email);

                await _repository.CrearUsuario(user);
                _logger.LogInformation("Service: se ha creado un usuario con exito...{Nombre}, {Email}.", nombreUsuario, email);
                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error en el registro de un nuevo usuario nombreUsuario: {Nombre}, email: {Email}", nombreUsuario, email);
                throw;
            }
        }

        public async Task<UserDtoResponse> Login(string email, string contraseña)
        {
            _logger.LogInformation("Service: Logeando usuario...{Email}", email);
            
            var user = await _repository.BuscarPorEmail(email);
            if (user == null)
            {
                _logger.LogWarning("Service:  No se encontro un usuario vinculado al mail {Email}", email);
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
            }
            if (!BCrypt.Net.BCrypt.Verify(contraseña, user.PasswordHash))
            {
                _logger.LogWarning("Service: Contraseña incorrecta para el usuario con el email {Email}", email);
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
            }
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
            var jwt = tokenHandler.WriteToken(token);
            _logger.LogInformation("Service: logeo exitoso {Nombre}, {Email}", user.NombreUsuario, user.Email);
            return new UserDtoResponse
            {
                NombreUsuario = user.NombreUsuario,
                Email = user.Email,
                Rol = user.Rol,
                Token = jwt
            };
        }
    }
}
