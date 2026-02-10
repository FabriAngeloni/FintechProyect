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
