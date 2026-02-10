namespace AssetService.Models
{
    public class Bono : Activo
    {
        public DateTime FechaVencimiento { get; set; }
        public decimal TasaInteres { get; set; }
    }
}
