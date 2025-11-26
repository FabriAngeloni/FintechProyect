namespace IdentityService.DTOs
{
    public class RegisterRequest
    {
        public string NombreUsuario { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
