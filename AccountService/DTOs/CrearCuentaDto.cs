namespace AccountService.DTOs
{
    public class CrearCuentaDto
    {
        public string NombreUsuario { get; set; }
        public decimal Balance{ get; set; }

        public CrearCuentaDto(string usuario, decimal balance)
        {
            NombreUsuario = usuario;
            Balance = balance;
        }
        public CrearCuentaDto()
        {
            
        }
    }
}
