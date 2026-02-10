namespace AccountService.Messaging.Abstractions
{
    // Contrato para publicar mensajes (eventos) en un sistema de mensajería
    public interface IMessagePublisher
    {
        // Publica un mensaje de cualquier tipo de forma asíncrona
        Task PublishAsync<T>(T mensaje);
    }

}
