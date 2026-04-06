namespace AccountService.DTOs
{
    public class CrearCuentaParaUsuarioDto
    {
        public Guid UserId { get; set; }    
        public string NombreUsuario { get; set; }
        public decimal Balance { get; set; }

        public CrearCuentaParaUsuarioDto(Guid id, string nombreUsuario,decimal balance)
        {
            UserId = id;
            NombreUsuario = nombreUsuario;
            Balance = balance;
        }
    }
}
