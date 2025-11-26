namespace TransactionService.Models
{
    public class Transaccion
    {
        public Guid TransaccionId { get; set; }
        public Guid DesdeCuenta { get; set; }
        public decimal Monto { get; set; }
        public Guid ParaCuenta { get; set; }
        public DateTime RealizadaEl { get; set; }

        public Transaccion(Guid desdeCuenta,decimal monto,Guid paraCuenta)
        {
            TransaccionId = Guid.NewGuid();
            DesdeCuenta = desdeCuenta;
            Monto = monto;
            ParaCuenta = paraCuenta;
            RealizadaEl = DateTime.UtcNow;
        }
    }
}
