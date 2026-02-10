using AssetService.DTOs;

namespace AssetService.Models.Factory_Creator.Modifier
{
    public class FondoModifier : IActivoModifier
    {
        public TipoActivo Tipo => TipoActivo.Fondo;

        public void Modificar(Activo activo, ModificarActivoDto dto)
        {
            var aux = activo as Fondo ??
                        throw new InvalidOperationException("El activo a modificar no es un Fondo.");
            if (dto.Administradora == null)
                throw new ArgumentNullException("La administradora del Fondo no puede ser nula.");

            aux.Administradora = dto.Administradora;
        }
    }
}
