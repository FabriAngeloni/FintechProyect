using AssetService.Models;

namespace AssetService.DTOs
{
    public class ActivoDtoResponse
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioInicial { get; set; }
        public TipoActivo Tipo { get; set; }

        public string? Ticker { get; set; }
        public string? Vencimiento { get; set; }
        public decimal? TasaInteres { get; set; }
        public string? Administradora { get; set; }

        public ActivoDtoResponse(Activo activo)
        {
            Id = activo.Id;
            Nombre = activo.Nombre;
            Tipo = activo.Tipo;
            PrecioInicial = activo.PrecioUnitario;
            Tipo = activo.Tipo;

            switch (activo)
            {
                case Accion accion:
                    Ticker = accion.Ticker;
                    break;
                case Bono bono:
                    Vencimiento = bono.FechaVencimiento.ToString();
                    TasaInteres = bono.TasaInteres;
                    break;
                case Fondo fondo:
                    Administradora = fondo.Administradora;
                    break;
            }
        }
    }
}
