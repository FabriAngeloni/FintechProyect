using AccountService.Models;

namespace AccountService.DTOs
{
    public class CuentaDtoResponse
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public string NombreUsuario { get; set; }

        public CuentaDtoResponse()
        {
        }
        public CuentaDtoResponse(Cuenta cuenta)
        {
            AccountId = cuenta.Id;
            UserId = cuenta.UserId;
            NombreUsuario = cuenta.NombreUsuario;
        }
    }
}
