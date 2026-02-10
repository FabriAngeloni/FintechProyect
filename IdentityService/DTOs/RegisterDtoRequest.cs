using System.ComponentModel.DataAnnotations;

namespace IdentityService.DTOs
{
    public class RegisterDtoRequest
    {
        [Required(ErrorMessage ="Nombre de usuario obligatorio.")]
        public string NombreUsuario { get; set; } = null!;
        [Required(ErrorMessage = "Email obligatorio.")]
        public string Mail { get; set; } = null!;
        [Required(ErrorMessage = "Contraseña obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener minimo 8 caracteres.")]
        public string Password { get; set; } = null!;

        public override string ToString() => $"{NombreUsuario}, {Mail}";
    }
}
