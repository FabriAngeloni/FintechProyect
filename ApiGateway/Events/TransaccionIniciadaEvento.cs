namespace ApiGateway.Events
{
    public record TransaccionIniciadaEvento
    {
        Guid TransactionId { get; set; }
        Guid AccountId { get; set; }
        decimal Monto { get; set; }
        DateTime Creado_El { get; set; }
    }
}
