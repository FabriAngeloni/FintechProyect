using IdentityService.DTOs;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;


namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> BuscarPorId(string userId)
        {
            _logger.LogInformation("Controller: buscando al usuario {id}", userId);
            try
            {
                if (!Guid.TryParse(userId, out var result)) 
                    return BadRequest("El ID enviado no cumple con el formato.");
                var usuario = await _authService.BuscarPorId(result);
                if (usuario == null) 
                    return NotFound($"No se encontro un usuario con el ID {userId}");
                _logger.LogInformation("Controller: se recibio un usuario... {Nombre}", usuario.NombreUsuario);
                return Ok(new UserDtoResponse 
                { 
                    Email = usuario.Email, 
                    Id = usuario.Id,
                    NombreUsuario = usuario.NombreUsuario,
                    Rol = usuario.Rol
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error en la busqueda del usuario {id}", userId);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDtoRequest request)
        {
            _logger.LogInformation("Controller: registrando al usuario {Request}",request);
            try
            {
                var user = await _authService.Register(request.NombreUsuario, request.Mail, request.Password);
                _logger.LogInformation("Controller: se ha creado con exito el usuario: {Nombre}, {Email}", user.NombreUsuario, user.Email);
                return Ok(new { user.Id, user.NombreUsuario, user.Email, user.Rol });
            }   
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error en la creacion del usuario {request}", request);
                return StatusCode(500, "Error interno del servidor");
            } 
        }
        [HttpGet("todos")]
        public async Task<IActionResult> RetornarUsuarios()
        {
            _logger.LogInformation("Controller: comenzando el retorno de los usuarios...");
            try
            {
                var usuarios = await _authService.RetornarUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error inesperado retorno de los usuarios.");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDtoRequest request)
        {
            _logger.LogInformation("Controller: logeando al usuario {Request}", request);
            try
            {
                var token = await _authService.Login(request.Email, request.Password);
                _logger.LogInformation("Controller: ingreso exitoso {Nombre}, {Email}", token.NombreUsuario,token.Email);
                return Ok(new { Token = token });
            }
            catch(UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Controller: credenciales invalidas para el mail: {Email}", request.Email);
                return Unauthorized("Credenciales invalidas.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error inesperado en el logeo del usuario:{Email}", request.Email);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
