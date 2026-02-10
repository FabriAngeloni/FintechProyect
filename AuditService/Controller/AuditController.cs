using AuditService.DTOs;
using AuditService.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AuditService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly ILogger<AuditController> _logger;
        private readonly IAuditServices _service;
        public AuditController(ILogger<AuditController> logger, IAuditServices services)
        {
            _logger = logger;
            _service = services;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId([FromRoute] string id)
        {
            _logger.LogInformation("Controller: comenzando la busqueda a través del ID: {Id}", id);
            try
            {
                if (!Guid.TryParse(id, out var idGuid))
                {
                    _logger.LogWarning("Controller: el ID enviado no cumple con el formato requerido.");
                    return BadRequest("Validar ID enviado.");
                }

                var response = await _service.BuscarPorId(idGuid);

                if (response == null)
                {
                    _logger.LogWarning("Controller: no se ha encontrado un log de audit para el ID: {Id}",idGuid);
                    return NotFound("No se encontro ningun log.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error interno del servidor...");
                return StatusCode(500,"Error interno.");
            }
        }

        [HttpGet("reporte")]
        public async Task<IActionResult> Reporte([FromQuery]string desde, [FromQuery]string hasta)
        {
            _logger.LogInformation("Controller: realizando reporte...\nDesde:{desde}\nHasta:{hasta}",desde,hasta);
            try
            {
                var desdeDate = DateTime.ParseExact(desde, "dd/MM/yyyy",CultureInfo.InvariantCulture);
                var hastaDate = DateTime.ParseExact(hasta, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddTicks(-1);
                _logger.LogInformation("DESDE LOCAL= {desde}\nHASTA Local= {hasta}", desdeDate, hastaDate);

                var desdeUtc = TimeZoneInfo.ConvertTimeToUtc(desdeDate);
                var hastaUtc= TimeZoneInfo.ConvertTimeToUtc(hastaDate);
                _logger.LogInformation("DESDE UTC = {desde}\nHASTA UTC = {hasta}", desdeUtc,hastaUtc);
                var bytes = await _service.GenerarReporte(desdeUtc, hastaUtc);
                
                _logger.LogInformation("Controller: archivo creado con exito...");
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",$"reporte-audit-desde:{desde}-hasta:{hasta}.xlsx");   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Controller: error interno del servidor...");
                return StatusCode(500,"Error interno.");
            }
        }
    }
}
