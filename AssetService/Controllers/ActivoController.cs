using AssetService.DTOs;
using AssetService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AssetService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivoController : ControllerBase
    {
        private readonly IActivoService _activoService;
        private readonly ILogger<ActivoController> _logger;
        public ActivoController(IActivoService activoService, ILogger<ActivoController> logger)
        {
            _activoService = activoService;
            _logger = logger;
        }

        [HttpPost("nuevo")]
        public async Task<IActionResult> CrearActivo([FromBody]ActivoDtoRequest dto)
        {
            _logger.LogInformation("Controller: iniciando la creacion de un activo...");
            try
            {
                var activo = await _activoService.CrearActivo(dto);
                _logger.LogInformation("Controller: activo creado con exito.");
                return CreatedAtAction(nameof(BuscarPorId), new { id = activo.Id }, activo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Controller: error en la creacion del activo.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId([FromRoute]string id)
        {
            _logger.LogInformation("Controler: iniciando busqueda por id: {id}", id);
            try
            {
                if(!Guid.TryParse(id,out var idActivo))
                {
                    _logger.LogWarning("Controller: el id no cumple con el formato...ID: {id}",id);
                    return BadRequest("Validar el id enviado.");
                }
                var activo = await _activoService.BuscarActivoPorId(idActivo);
                if (activo == null)
                    return NotFound($"No se encontro activo por el ID: {id}");
                return Ok(activo);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex,"Controller: no se encontro el activo con el ID: {id}", id);
                return NotFound("No se encontro un activo con el ID enviado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error interno realizando la busqueda del ID: {Id}.",id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("Activos")]
        public async Task<IActionResult> RetornarTodos()
        {
            _logger.LogInformation("Controller: buscando todos los activos listados...");
            try
            {
                var activos = await _activoService.ObtenerTodosLosActivos();
                _logger.LogInformation("Controller: se encontraron activos listados con exito.");
                return Ok(activos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Controller: error en la obtencion de activos.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarActivo(string id,[FromBody] ModificarActivoDto dto)
        {
            _logger.LogInformation("Controller: empezando la modifacion de un activo...");
            try
            {
                if (!Guid.TryParse(id, out var idActivo))
                {
                    _logger.LogWarning("Controller: el id no cumple con el formato...ID: {id}", id);
                    return BadRequest("Validar el id enviado.");
                }
                await _activoService.ModificarActivo(idActivo, dto);
                _logger.LogInformation("Controller: {dto} se modifico correctamente",dto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error en la modificacion del activo: {dto}",dto);
                return StatusCode(500, "Error interno del servidor.");
            }

        }

        [HttpDelete("borrar/{id}")]
        public async Task<IActionResult> BorrarActivo(string id)
        {
            _logger.LogInformation("Controller: comenzando el borrado del activo con id {}", id);
            try
            {
                if (!Guid.TryParse(id, out var activoId))
                {
                    _logger.LogWarning("Controller: el id no cumple con el formato...ID: {id}", id);
                    return BadRequest("Validar el id enviado.");
                }
                await _activoService.BorrarActivo(activoId);
                _logger.LogInformation("Controller: activo borrado con exito.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error al intentar borrar el activo con ID {id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
            
        }
    }
}
