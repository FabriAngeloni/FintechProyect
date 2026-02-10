namespace TransactionService.DTOs
{
    public class TransaccionDtoRequest
    {
        public string DesdeCuenta { get; set; }
        public decimal Monto { get; set; }
        public string ParaCuenta { get; set; }
    }
}
