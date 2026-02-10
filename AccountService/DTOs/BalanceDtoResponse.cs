using AccountService.Models;

namespace AccountService.DTOs
{
    public class BalanceDtoResponse
    {
        public Guid AccountId { get; set; }
        public decimal Balance { get; set; }
        public string NombreUsuario { get; set; }

        public BalanceDtoResponse(Cuenta cuenta)
        {
            AccountId = cuenta.Id;
            Balance = cuenta.Balance;
            NombreUsuario = cuenta.NombreUsuario;
        }
    }
}
