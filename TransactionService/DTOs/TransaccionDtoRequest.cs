namespace TransactionService.DTOs
{
    public class TransaccionDtoRequest
    {
        public Guid DesdeCuenta { get; set; }
        public decimal Monto { get; set; }
        public Guid ParaCuenta { get; set; }
    }
}
