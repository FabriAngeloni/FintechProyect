using AccountService.DTOs;
using AccountService.Migrations;
using AccountService.Models;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaServices _accountService;

        public CuentaController(ICuentaServices accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta([FromBody] CrearCuentaDto dto)
        {
            var account = await _accountService.CrearCuentaAsync(dto.NombreUsuario, dto.Balance);
            return CreatedAtAction(nameof(BuscarCuentaPorId),new { accountId = account.Id} , new CuentaDtoResponse(account));
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> BuscarCuentaPorId(Guid accountId)
        {
            var account = await _accountService.BuscarPorIdAsync(accountId);
            if (account == null) return NotFound("Cuenta no encontrada.");
            return Ok(new CuentaDtoResponse(account));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> MostrarCuentasDelUsuario(Guid userId)
        {
            var accounts = await _accountService.BuscarPorUserIdAsync(userId);
            var accountsDtos = accounts.Select(a => new CuentaDtoResponse(a)) ?? throw new Exception("No se encontraron cuentas para ese usuario");
            return Ok(accountsDtos);
        }

    }
}
