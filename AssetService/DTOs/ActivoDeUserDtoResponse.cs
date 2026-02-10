using AssetService.Models;

namespace AssetService.DTOs
{
    public class ActivoDeUserDtoResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ActivoId { get; set; }
        public TipoActivo TipoActivo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioDeCompra { get; set; }

        public ActivoDeUserDtoResponse(Guid id, Guid userId, Guid activoId, TipoActivo tipoActivo,int cantidad,decimal precioCompra)
        {
            Id = id;
            UserId = userId;
            ActivoId = activoId;
            TipoActivo = tipoActivo;
            Cantidad = cantidad;
            PrecioDeCompra = precioCompra;
        }
        public ActivoDeUserDtoResponse(ActivoDeUser activoDeUser)
        {
            Id = activoDeUser.Id;
            UserId = activoDeUser.UserId;
            ActivoId = activoDeUser.ActivoId;
            TipoActivo = activoDeUser.Activo.Tipo;
            Cantidad = activoDeUser.Cantidad;
            PrecioDeCompra = activoDeUser.PrecioDeCompra;
        }
    }
}
