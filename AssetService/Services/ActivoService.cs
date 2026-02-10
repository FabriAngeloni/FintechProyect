using AssetService.DTOs;
using AssetService.Models;
using AssetService.Models.Factory_Creator;
using AssetService.Models.Factory_Creator.Modifier;
using AssetService.Repositories;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;

namespace AssetService.Services
{
    public class ActivoService : IActivoService
    {
        private readonly IActivoRepository _repository;
        private readonly IActivoFactory _factory;
        private readonly IActivoModifierFactory _modifier;
        private readonly ILogger<ActivoService> _logger;
        public ActivoService(IActivoRepository repository, IActivoFactory activoFactory, ILogger<ActivoService> logger, IActivoModifierFactory modifier)
        {
            _repository = repository;
            _factory = activoFactory;
            _logger = logger;
            _modifier = modifier;
        }
        public async Task BorrarActivo(Guid id)
        {
            _logger.LogInformation("Service: comenzando a borrar el activo... {id}",id);
            try
            {
                var aux = await _repository.BuscarPorId(id);
                if (aux == null)
                    throw new ArgumentNullException("El activo a borrar no existe.");
                await _repository.Borrar(aux);
                _logger.LogInformation("Service: activo eliminado con exito. {id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error al intentar eliminar el activo {id}", id);
                throw;
            }

        }
        public async Task<ActivoDtoResponse?> BuscarActivoPorId(Guid id)
        {
            _logger.LogInformation("Service: comenzando busqueda de un activo a traves del ID: {id}", id);
            try
            {
                var aux = await _repository.BuscarPorId(id);
                if (aux == null)
                {
                    _logger.LogWarning("Service: no se encontro ningun activo para ID: {id}", id);
                    throw new KeyNotFoundException("No se ha encontrado ningun activo.");
                }
                _logger.LogInformation("Service: se encontro un activo con el ID: {id}", aux.Id);
                return new ActivoDtoResponse(aux);
            }
            catch
            {
                _logger.LogError("Service: falla en la busqueda del activo con el ID: {id}", id);
                throw;
            }
        }
        public async Task<ActivoDtoResponse> CrearActivo(ActivoDtoRequest dto)
        {
            _logger.LogInformation("Service: creando nuevo activo de tipo {Tipo}...", dto.Tipo);
            try
            {
                var aux = _factory.Crear(dto);
                await _repository.Agregar(aux);
                _logger.LogInformation("Service: activo creado con exito. Se creo un {Tipo}", aux.Tipo);
                return new ActivoDtoResponse(aux);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: fallo la creacion de un activo de tipo {Tipo}.", dto.Tipo);
                throw;
            }
        }
        public async Task ModificarActivo(Guid id, ModificarActivoDto dto)
        {
            _logger.LogInformation("Service: iniciando modificacion del activo {id}", id);
            try
            {
                var activo = await _repository.BuscarPorId(id);
                if (activo == null)
                    throw new Exception("No se encontro activo.");
                activo.Nombre = dto.Nombre;
                activo.PrecioUnitario = dto.PrecioInicial;
                var aux = _modifier.Obtener(activo.Tipo);
                aux.Modificar(activo, dto);
                _logger.LogInformation("Service: se realizo la modificacion de manera correcta. DTO: {dto}", dto);
                await _repository.Modificar(activo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error al modificar el activo por DTO: {dto}", dto);
                throw;
            }

        }
        public async Task<IEnumerable<ActivoDtoResponse>> ObtenerTodosLosActivos()
        {
            _logger.LogInformation("Service: obteniendo todos los activos...");
            try
            {
                var aux = await _repository.ObtenerTodos();
                _logger.LogInformation("Service: se encontraron activos con exito.");
                return aux.Select(x => new ActivoDtoResponse(x));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error en la busqueda de activos.");
                throw;
            }
        }
    }
}
