using AssetService.DTOs;

namespace AssetService.Models.Factory_Creator
{
    public class BonoCreator : IActivoCreator
    {
        public TipoActivo Tipo => TipoActivo.Bono;

        public Activo crear(ActivoDtoRequest dto)
        {
            if (dto.TasaInteres == null)
                throw new Exception("El bono requiere tasa de interes.");
            return new Bono
            {
                Id = Guid.NewGuid(),
                Nombre = dto.Nombre,
                PrecioUnitario = dto.PrecioInicial,
                Tipo = TipoActivo.Bono,
                FechaVencimiento = DateTime.UtcNow.AddMonths(1),
                TasaInteres = dto.TasaInteres.Value
            };
        }
    }
}
