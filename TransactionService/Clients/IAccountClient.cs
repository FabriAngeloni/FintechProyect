namespace TransactionService.Clients
{
    public interface IAccountClient
    {
        Task Debitar(Guid accountId, decimal monto);
        Task Acreditar(Guid accountId, decimal monto);


    }
}
