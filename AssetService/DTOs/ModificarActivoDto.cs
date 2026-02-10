using AssetService.Models;

namespace AssetService.DTOs
{
    public class ModificarActivoDto
    {
        public string Nombre { get; set; }
        public decimal PrecioInicial { get; set; }

        public string? Ticker { get; set; }
        public string? Vencimiento { get; set; }
        public decimal? TasaInteres { get; set; }
        public string? Administradora { get; set; }

        public override string ToString() => $"\nNombre: {Nombre} \nPrecio inicial: {PrecioInicial} \nTicker: {Ticker} \nTasa de interes: {TasaInteres} \nAdministradora: {Administradora}";
    }
}
