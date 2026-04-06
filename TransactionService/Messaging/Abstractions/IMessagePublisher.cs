namespace TransactionService.Messaging.Abstractions
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message);
    }
}
