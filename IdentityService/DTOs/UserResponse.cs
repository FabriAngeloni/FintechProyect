namespace IdentityService.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
