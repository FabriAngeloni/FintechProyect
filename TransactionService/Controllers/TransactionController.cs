using Microsoft.AspNetCore.Mvc;
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
        public TransactionController(ITransaccionService transaccionService)
        {
            _transaccionService = transaccionService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearTransaccion([FromBody] TransaccionDtoRequest transaccionDto)
        {
            if (!ModelState.IsValid) return BadRequest("No cumple con el formato correcto.");
            var transaccion = await _transaccionService.CrearTransaccion(transaccionDto.DesdeCuenta, transaccionDto.Monto, transaccionDto.ParaCuenta);
            return CreatedAtAction(nameof(BuscarTransaccionPorId),new {transaccionId = transaccion.TransaccionId },new TransaccionDtoResponse(transaccion));
        }

        [HttpGet("{transaccionId}")]
        public async Task<IActionResult> BuscarTransaccionPorId(Guid transaccionId)
        {
            var transaccion = await _transaccionService.BuscarTransaccionPorId(transaccionId);
            if (transaccion == null)return NotFound("No se encontro la transaccion solicitada.");
            return Ok(new TransaccionDtoResponse(transaccion));
        }

        [HttpGet("all")]
        public async Task<ActionResult> RetornarTransacciones()
        {
            var transacciones = await _transaccionService.RetornarTransacciones();
            return Ok(transacciones.Select(t => new TransaccionDtoResponse(t)).ToList());
        }

        [HttpGet("Cancelacion/{transaccionId}")]
        public async Task<ActionResult> CancelarTransaccion(Guid transaccionId)
        {
            var transaccion = await _transaccionService.BuscarTransaccionPorId(transaccionId);
            var response = new TransaccionDtoResponse(transaccion);
            await _transaccionService.CancelarTransaccion(transaccionId);
            
            return Ok(response);
        }

    }
}
