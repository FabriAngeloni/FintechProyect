namespace AccountService.DTOs
{
    public class CrearCuentaParaUsuarioDto
    {
        public Guid UserId { get; set; }
        public string NombreUsuario { get; set; }
        public decimal Balance { get; set; }

        public CrearCuentaParaUsuarioDto(string nombreUsuario,decimal balance)
        {
            UserId = Guid.NewGuid();
            NombreUsuario = nombreUsuario;
            Balance = balance;
        }
    }
}
