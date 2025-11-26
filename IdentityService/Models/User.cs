namespace IdentityService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string Rol { get; set; } = "User";
    }
}
