using AssetService.DTOs;

namespace AssetService.Models.Factory_Creator
{
    public class FondoCreator : IActivoCreator
    {
        public TipoActivo Tipo => TipoActivo.Fondo;

        public Activo crear(ActivoDtoRequest dto)
        {
            if (string.IsNullOrEmpty(dto.Administradora))
                throw new Exception("Se requiere una administradora para crear un Fondo.");
            return new Fondo
            {
                Id = Guid.NewGuid(),
                Nombre = dto.Nombre,
                PrecioUnitario = dto.PrecioInicial,
                Tipo = TipoActivo.Fondo,
                Administradora = dto.Administradora
            };

        }
    }
}
