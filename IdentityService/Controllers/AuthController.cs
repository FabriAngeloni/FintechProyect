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
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            var user = _authService.Register(request.NombreUsuario, request.Mail, request.Password);
            return Ok(new { user.Id, user.NombreUsuario, user.Email, user.Rol });
        }
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var token = _authService.Login(request.Email, request.Password);
            if(token == null) return Unauthorized();
            return Ok(new {Token = token});
        }
    }
}
