namespace TransactionService.DTOs
{
    public class TransaccionDtoResponse
    {
        public Guid TransaccionId { get; set; }
        public Guid DesdeCuenta { get; set; }
        public decimal Monto { get; set; }
        public Guid ParaCuenta { get; set; }
        public DateTime RealizadaEl { get; set; }

        
    }
}
