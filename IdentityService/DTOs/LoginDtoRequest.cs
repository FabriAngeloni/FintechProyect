using System.ComponentModel.DataAnnotations;

namespace IdentityService.DTOs
{
    public class LoginDtoRequest
    {
        [Required(ErrorMessage = "Ingresar email.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Ingresar contraseña.")]
        public string Password { get; set; } = null!;

        public override string ToString() => $"{Email}";
    }
}
