using AssetService.DTOs;

namespace AssetService.Models
{
    public class AccionCreator : IActivoCreator
    {
        public TipoActivo Tipo => TipoActivo.Accion;

        public Activo crear(ActivoDtoRequest dto)
        {
            if(string.IsNullOrEmpty(dto.Ticker))
                throw new ArgumentNullException("Para crear una accion se requiere el Ticker.");
            return new Accion
            {
                Id = Guid.NewGuid(),
                Nombre = dto.Nombre,
                PrecioUnitario = dto.PrecioInicial,
                Tipo = TipoActivo.Accion,
                Ticker = dto.Ticker,
            };
        }
    }
}
