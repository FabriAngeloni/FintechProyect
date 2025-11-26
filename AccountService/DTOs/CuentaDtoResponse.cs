using AccountService.Models;

namespace AccountService.DTOs
{
    public class CuentaDtoResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string NombreUsuario { get; set; }

        public CuentaDtoResponse()
        {
        }
        public CuentaDtoResponse(Cuenta cuenta)
        {
            Id = cuenta.Id;
            UserId = cuenta.UserId;
            NombreUsuario = cuenta.NombreUsuario;
        }
    }
}
