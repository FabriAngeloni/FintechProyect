using AssetService.DTOs;
using System;

namespace AssetService.Models
{
    public class ActivoDeUser
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ActivoId { get; set; }
        public Activo Activo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioDeCompra { get; set; }
        public ActivoDeUser()
        {
            
        }

        public ActivoDeUser(Guid id,Guid userId,Guid activoId,Activo activo,int cantidad, decimal precioCompra )
        {
            Id = id;
            UserId = userId;
            ActivoId = activoId;
            Activo = activo;
            Cantidad = cantidad;
            PrecioDeCompra = precioCompra;
        }

    }
}
