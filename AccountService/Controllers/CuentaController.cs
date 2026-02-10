using AccountService.DTOs;
using AccountService.Models;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Runtime.CompilerServices;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaService _accountService;
        private readonly ILogger<CuentaController> _logger;

        public CuentaController(ICuentaService accountService, ILogger<CuentaController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta([FromBody] CrearCuentaParaUsuarioDto dto)
        {
            _logger.LogInformation("Controller: Iniciando la creacion de una cuenta...");
            try
            {
                var account = await _accountService.CrearCuentaAsync(dto);
                _logger.LogInformation("Controller: Cuenta creada exitosamente. ID:{CuentaId}", account.AccountId);
                return CreatedAtAction(nameof(BuscarCuentaPorId), account.AccountId, account);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Controller: El nombre de usuario o balance fallaron en la creacion.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: Error en la creacion de la cuenta con nombre de usuario: {NombreUsuario}", dto.NombreUsuario);
                return StatusCode(500, "Error interno del servidor.");
            }
            
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> BuscarCuentaPorId(string accountId)
        {
            _logger.LogInformation("Controller: Iniciando la busqueda de una cuenta con ID: {Id}...",accountId);

            try
            {
                if (!Guid.TryParse(accountId, out var id))
                {
                    _logger.LogWarning("Controller: el ID: {A.ID} no puede ser convertido a Guid porque no cumple con los requisitos.", accountId);
                    return BadRequest("Validar ID enviado.");
                }
                var account = await _accountService.BuscarPorIdAsync(id);
                if (account == null)
                {
                    _logger.LogWarning("Controller: No se ha encontrado cuenta con el ID: {AccountId}", accountId);
                    return NotFound("Cuenta no encontrada.");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: Error en la busqueda de la cuenta con ID: {AccountId}", accountId);
                return StatusCode(500, "Error interno del servicio.");
            }
            
        }

        [HttpGet("de/{userId}")]
        public async Task<IActionResult> MostrarCuentasDelUsuario(string userId)
        {
            _logger.LogInformation("Controller: Iniciando la busqueda de las cuentas del usuario con ID: {Id}...", userId);

            try
            {
                if (!Guid.TryParse(userId, out var id))
                {
                    _logger.LogWarning("Controller: el ID: {A.ID} no puede ser convertido a Guid porque no cumple con los requisitos.", userId);
                    return BadRequest("Validar ID enviado.");
                }
                var accounts = await _accountService.BuscarPorUserIdAsync(id);
                if (accounts == null)
                    _logger.LogWarning("Controller: no se encontraron cuentas para el usuario {Id}", userId);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: Error en la obtencion de las cuentas del usuario {Id}", userId);
                return StatusCode(500, $"Error interno del servidor");
            }
        }
        [HttpPost("debitar")]
        public async Task<IActionResult> Debitar(string accountId, decimal monto)
        {
            _logger.LogInformation("Controller: Iniciando el debito del monto: ${monto} en la cuenta: {Id}...", monto, accountId);
            try
            {
                if(!Guid.TryParse(accountId,out var id))
                {
                    _logger.LogWarning("Controller: el ID: {A.ID} no puede ser convertido a Guid porque no cumple con los requisitos.", accountId);
                    return BadRequest("Validar ID enviado.");
                }
                var balanceFinal = await _accountService.Debito(id, monto);
                return Ok(balanceFinal);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Controller: Error el debito de $:{Monto} en la cuenta del usuario {Id}",monto, accountId);
                return StatusCode(500, $"Error interno del servidor");
            }
        }
        [HttpPost("acreditar")]
        public async Task<IActionResult> Acreditar(string accountId, decimal monto)
        {
            _logger.LogInformation("Controller: comenzando a acreditar el monto: ${monto} en la cuenta: {Id}...", monto, accountId);
            try
            {
                if (!Guid.TryParse(accountId, out var id))
                {
                    _logger.LogWarning("Controller: el ID: {A.ID} no puede ser convertido a Guid porque no cumple con los requisitos.", accountId);
                    return BadRequest("Validar ID enviado.");
                }
                var balanceFinal = await _accountService.Acreditar(id, monto);
                return Ok(balanceFinal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: Error en acreditar el monto de $:{Monto} en la cuenta del usuario {Id}", monto, accountId);
                return StatusCode(500, $"Error interno del servidor");
            }
        }
    }
}
