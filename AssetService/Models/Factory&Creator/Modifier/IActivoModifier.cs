using AssetService.DTOs;

namespace AssetService.Models.Factory_Creator
{
    public interface IActivoModifier
    {
        public TipoActivo Tipo { get; }
        void Modificar(Activo activo, ModificarActivoDto dto);
    }
}
