namespace AccountService.Models
{
    public class Cuenta
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string NombreUsuario { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreadoEl { get; set; }
    }
}
