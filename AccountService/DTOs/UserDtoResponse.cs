namespace AccountService.DTOs
{
    public class UserDtoResponse
    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }

        public UserDtoResponse(Guid id, string nombreUser, string email, string rol)
        {
            Id = id;
            NombreUsuario = nombreUser;
            Email = email;
            Rol = rol;
        }
        public UserDtoResponse()
        {
        }
    }
}
