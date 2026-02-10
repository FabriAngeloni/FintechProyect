using AuditService.Data;
using AuditService.DTOs;
using AuditService.Models;
using AuditService.Repositories;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Data;

namespace AuditService.Services
{
    public class AuditServices : IAuditServices
    {
        private readonly ILogger<AuditServices> _logger;
        private readonly IAuditRepository _repository;
        public AuditServices(IAuditRepository repository, ILogger<AuditServices> logger)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<AuditDtoResponse> BuscarPorId(Guid id)
        {
            _logger.LogInformation("Service: comenzando busqueda de log por ID: {Id}...", id);
            try
            {
                var dato = await _repository.BuscarPorId(id);
                if (dato == null)
                {
                    _logger.LogWarning("Service: no se encontro ningun log.");
                    return null;
                }
                var response = new AuditDtoResponse(dato);
                _logger.LogInformation("Service: audit encontrado correctamente." +
                    "\nID: {Id}" +
                    "\nPayload: {Pl}" +
                    "\nFecha: {Fecha}" +
                    "\nEventType: {Et}",
                    response.AuditId, response.Payload, response.Created_At, response.EventType);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error en la creacion del log.");
                throw;
            }
        }

        public async Task CrearLog(AuditDtoRequest request)
        {
            _logger.LogInformation("Service: comenzando la creacion de un log...");
            try
            {
                if (request is null)
                    throw new Exception("Service: parametro request es null.");

                var response = new Audit
                {
                    AuditId = Guid.NewGuid(),
                    Created_At = DateTime.UtcNow,
                    EventType = request.EventType,
                    Payload = request.Payload
                };

                _logger.LogInformation("Service: enviando datos al repository para la creacion del log...");
                await _repository.CrearLog(response);
                _logger.LogInformation("Service: audit creado correctamente." +
                    "\nID: {Id}" +
                    "\nPayload: {Pl}" +
                    "\nFecha: {Fecha}" +
                    "\nEventType: {Et}", response.AuditId, response.Payload, response.Created_At, response.EventType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error en la creacion del log.");
                throw;
            }
        }

        public async Task<byte[]> GenerarReporte(DateTime desde, DateTime hasta)
        {
            _logger.LogInformation("Service: realizando el reporte...");
            try
            {
                var datosA = await _repository.RetornarTodos();
                //var datosA = await _repository.RetornarParaReporte(desde, hasta);
                _logger.LogInformation("Service: datos originales tipo Audit = {C}", datosA.Count());
                var datos = datosA.Where(a=>a.Created_At.Date >= desde.Date && a.Created_At.Date <= hasta.Date).Select(a=> new AuditDtoResponse(a)).ToList();
                _logger.LogInformation("Service: datos convertidos a Dto = {C}", datos.Count);
                using var workbook = new XLWorkbook();
                var sheet = workbook.Worksheets.Add("Audits");
                sheet.Cell(1, 1).Value = "AuditId";
                sheet.Cell(1, 2).Value = "EventType";
                sheet.Cell(1, 3).Value = "Payload";
                sheet.Cell(1, 4).Value = "Creado_El";
                sheet.Row(1).Style.Font.Bold = true;
                var row = 2;
                foreach(var dato in datos)
                {
                    sheet.Cell(row, 1).Value = dato.AuditId.ToString();
                    sheet.Cell(row, 2).Value = dato.EventType;
                    sheet.Cell(row, 3).Value = dato.Payload;
                    sheet.Cell(row, 4).Value = dato.Created_At.ToString();
                    sheet.Cell(row, 4).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                    row++;
                }

                sheet.Columns().AdjustToContents();
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: fallo al generarse el reporte segun los periodos.\nDesde: {desde}\nHasta: {hasta}", desde, hasta);
                throw;
            }
        }

        public async Task<IEnumerable<AuditDtoResponse>> ListarTodos()
        {
            _logger.LogInformation("Service: comenzando a listar los datos...");
            try
            {
                var datos = await _repository.RetornarTodos();
                if (!datos.Any())
                {
                    _logger.LogWarning("Service: no hay datos cargados. Count de elementos: {Count}", datos.Count());
                    return Enumerable.Empty<AuditDtoResponse>();
                }

                var auditDtoList = datos.Select(a => new AuditDtoResponse(a)).ToList();

                return auditDtoList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: fallo al listarse los datos de la base de datos.");
                throw;
            }
        }
    }
}
