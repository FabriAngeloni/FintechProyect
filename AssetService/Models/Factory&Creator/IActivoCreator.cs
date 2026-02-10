using AssetService.DTOs;

namespace AssetService.Models
{
    public interface IActivoCreator
    {
        public TipoActivo Tipo { get; }
        Activo crear(ActivoDtoRequest dto);
    }
}
