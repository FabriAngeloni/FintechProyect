using AssetService.Models;
using System.ComponentModel.DataAnnotations;

namespace AssetService.DTOs
{
    public class ActivoDtoRequest
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio inicial es requerido.")]
        public decimal PrecioInicial { get; set; }

        [Required(ErrorMessage = "El tipo de activo es requerido.")]
        public TipoActivo Tipo { get; set; }
        
        public string? Ticker { get; set; }
        public string? Vencimiento { get; set; }
        public decimal? TasaInteres { get; set; }
        public string? Administradora { get; set; }
    }
}
