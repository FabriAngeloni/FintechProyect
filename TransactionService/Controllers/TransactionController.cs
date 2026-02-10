using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using TransactionService.DTOs;
using TransactionService.Models;
using TransactionService.Services;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaccionService _transaccionService;
        private readonly ILogger<TransactionController> _logger;
        public TransactionController(ITransaccionService transaccionService, ILogger<TransactionController> logger)
        {
            _transaccionService = transaccionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CrearTransaccion([FromBody] TransaccionDtoRequest transaccionDto)
        {
            _logger.LogInformation("Controller: Creando una nueva transaccion...");
            try
            {
                if (!Guid.TryParse(transaccionDto.DesdeCuenta, out var desde))
                {
                    _logger.LogWarning("Controller: El formato de la cuenta emisora es incorrecto {Desde}",transaccionDto.DesdeCuenta);
                    return BadRequest("El formato de la cuenta emisora no es correcto");
                }
                if (!Guid.TryParse(transaccionDto.ParaCuenta, out var para))
                {
                    _logger.LogWarning("Controller: El formato de la cuenta receptora es incorrecto {Desde}", transaccionDto.DesdeCuenta);
                    return BadRequest("El formato de la cuenta receptora no es correcto");
                }
                var transaccion = await _transaccionService.CrearTransaccion(desde, transaccionDto.Monto, para);
                _logger.LogInformation
                    (
                    "Controller: Se creo la transaccion con exito...\nDesde: {Desde} \nPara: {Para} \nMonto: ${Monto} \nFecha: {RealizadaEl}",
                    transaccion.DesdeCuenta,
                    transaccion.ParaCuenta,
                    transaccion.Monto,
                    transaccion.RealizadaEl
                    );

                return CreatedAtAction(nameof(BuscarTransaccionPorId), new { transaccionId = transaccion.TransaccionId }, transaccion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error: error en la transaccion.");
                return StatusCode(500, "Error interno del servidor.");
            }
            
        }

        [HttpGet("{transaccionId}")]
        public async Task<IActionResult> BuscarTransaccionPorId(string transaccionId)
        {
            _logger.LogInformation("Buscando transaccion a traves del ID: {Id}", transaccionId);
            try
            {
                if (!Guid.TryParse(transaccionId, out var id))
                {
                    _logger.LogWarning("Controller: El formato de la cuenta es incorrecto {Desde}", transaccionId);
                    return BadRequest("El formato de la cuenta no es correcto");
                }
                var transaccion = await _transaccionService.BuscarTransaccionPorId(id);
                if (transaccion == null) return NotFound("No se encontro la transaccion solicitada.");
                _logger.LogInformation("Controller: se encontro una transaccion para el ID: {Id}", transaccionId);
                return Ok(transaccion);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex,"Controller: no se encontro una transaccion con la ID: {Id}", transaccionId);
                return NotFound("No se encontro una transaccion para la id indicada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: error en la transaccion.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetornarTransacciones()
        {
            _logger.LogInformation("Buscando todas las transacciones disponibles...");
            try
            {
                var transacciones = await _transaccionService.RetornarTransacciones();
                _logger.LogWarning("Controller: lista de transacciones enviada.");
                return Ok(transacciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: error en la transaccion.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost("Cancelacion/{transaccionId}")]
        public async Task<ActionResult> CancelarTransaccion(string transaccionId)
        {
            _logger.LogInformation("Controller: iniciando cancelacion de transaccion...");
            try
            {
                if(!Guid.TryParse(transaccionId, out var id))
                {
                    _logger.LogWarning("Controller: el formato del id no cumple con los requisitos...{Id}",transaccionId);
                    return BadRequest("Validar ID ingresado");
                }
                await _transaccionService.CancelarTransaccion(id);
                _logger.LogInformation("Controller: se cancelo la transaccion con exito..."); 
                return Ok("Transaccion cancelada con exito.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex,"Controller: no se encontro una transaccion con la ID: {Id}",transaccionId);
                return NotFound("No se encontro una transaccion para la id indicada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: error en la transaccion.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
