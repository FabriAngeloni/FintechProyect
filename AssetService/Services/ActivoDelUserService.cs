using AssetService.DTOs;
using AssetService.Models;
using AssetService.Repositories;

namespace AssetService.Services
{
    public class ActivoDelUserService : IActivoDelUserService
    {
        private readonly IActivoDelUserRepository _ActivoDeUserRepository;
        private readonly IActivoRepository _ActivoRepository;

        public ActivoDelUserService(IActivoDelUserRepository activoDelUserRepository, IActivoRepository activoRepository)
        {
            _ActivoDeUserRepository = activoDelUserRepository;
            _ActivoRepository = activoRepository;
        }
        public async Task<ActivoDeUserDtoResponse> Agregar(ActivoDeUserDtoRequest dto)
        {
            if (dto == null)
                throw new Exception("No se puede agregar un historial vacio.");
            var activo = await _ActivoRepository.BuscarPorId(dto.ActivoId);
            if (activo == null)
                throw new Exception("El activo no existe.");

            var historia = new ActivoDeUser
                (
                Guid.NewGuid(),
                dto.UserId, 
                dto.ActivoId,
                activo, 
                dto.Cantidad, 
                dto.PrecioDeCompra
                );
            await _ActivoDeUserRepository.Agregar(historia);
            return new ActivoDeUserDtoResponse(historia);
        }

        public async Task<ActivoDeUserDtoResponse?> BuscarPorId(Guid id)
        {
            var aux = await _ActivoDeUserRepository.BuscarPorId(id);
            if (aux == null)
                throw new Exception($"No se ha encontrado ningun historial para {id}");
            return new ActivoDeUserDtoResponse(aux);
        }


        public async Task<IEnumerable<ActivoDeUserDtoResponse>> ObtenerTodos()
        {
            var aux = await _ActivoDeUserRepository.ObtenerTodos();
            return aux.Select(x=>new ActivoDeUserDtoResponse(x));
        }
    }
}
