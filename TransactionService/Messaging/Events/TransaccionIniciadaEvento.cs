namespace TransactionService.Messaging.Events
{
    public record TransaccionIniciadaEvento
    {
        Guid TransactionId { get; set; }
        Guid DesdeCuentaId { get; set; }
        decimal Monto { get; set; }
        DateTime Creado_El { get; set; }

        public TransaccionIniciadaEvento(Guid transaccionId, Guid desdeCuentaId, decimal monto, DateTime creadoEl)
        {
            TransactionId = transaccionId;
            DesdeCuentaId = desdeCuentaId;
            Monto = monto;
            Creado_El = creadoEl;
        }
    }
}
