using AssetService.DTOs;
using System.Globalization;

namespace AssetService.Models.Factory_Creator.Modifier
{
    public class BonoModifier : IActivoModifier
    {
        public TipoActivo Tipo => TipoActivo.Bono;

        public void Modificar(Activo activo, ModificarActivoDto dto)
        {
            var aux = activo as Bono ?? 
                throw new InvalidOperationException("El activo a modificar no es un Bono.");
            if (!DateTime.TryParseExact(dto.Vencimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
                throw new Exception("Formato de la fecha invalido.");
            if (dto.TasaInteres == null) 
                throw new ArgumentNullException("La tasa de interes del bono no puede ser nula.");
            aux.TasaInteres = dto.TasaInteres.Value;
            aux.FechaVencimiento = fecha;
        }
    }
}
