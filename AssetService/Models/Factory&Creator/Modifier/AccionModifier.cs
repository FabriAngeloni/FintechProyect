using AssetService.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace AssetService.Models.Factory_Creator.Modifier
{
    public class AccionModifier : IActivoModifier
    {
        public TipoActivo Tipo => TipoActivo.Accion;

        public void Modificar(Activo activo, ModificarActivoDto dto)
        {
            var aux = activo as Accion ??
                throw new InvalidOperationException("El activo a modificar no es una Accion");
            if (string.IsNullOrEmpty(dto.Ticker)) 
                throw new ArgumentNullException("Para una accion, se requiere Ticker.");

            aux.Ticker = dto.Ticker;
        }
    }
}
