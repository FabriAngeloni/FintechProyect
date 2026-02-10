namespace AssetService.Models
{
    public abstract class Activo
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public TipoActivo Tipo { get; set; }
    }
}
