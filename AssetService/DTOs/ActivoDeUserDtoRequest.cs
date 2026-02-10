using AssetService.Models;

namespace AssetService.DTOs
{
    public class ActivoDeUserDtoRequest
    {
        public Guid UserId { get; set; }
        public Guid ActivoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioDeCompra { get; set; }

    }
}
