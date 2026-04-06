using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TransactionService.Messaging.Abstractions;

namespace TransactionService.Messaging.RabbitMq
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly RabbitMqConnection _connection;
        private readonly ILogger _logger;
        private readonly RabbitMqOptions _options;
        public RabbitMqPublisher(RabbitMqConnection connection, ILogger<RabbitMqPublisher> logger, IOptions<RabbitMqOptions> options)
        {
            _connection = connection;  
            _logger = logger;
            _options = options.Value;
        }
        public async Task PublishAsync<T>(T message)
        {
            try 
            {
                _logger.LogDebug("RabbitMqPublisher: creando conexion. Exchange: {Exchange} Tipo: {Tipo}", _options.Exchange, typeof(T).Name);
                var connection = await _connection.ConnectAsync();

                _logger.LogDebug("RabbitMqPublisher: creando channel.");
                using var channel = await connection.CreateChannelAsync();

                _logger.LogInformation("RabbitMqPublisher: declarando exchange...\nExchange: {E}",_options.Exchange);
                await channel.ExchangeDeclareAsync (_options.Exchange, ExchangeType.Fanout, true);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                _logger.LogDebug("RabbitMqPublisher: json convertido: {Json}", body.ToString());

                _logger.LogInformation("RabbitMqPublisher: publica mensaje: {Mensaje} en el exchange: {Exchange}", typeof(T).Name, _options.Exchange);

                await channel.BasicPublishAsync(_options.Exchange, "", body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMqPublisher: error en la publicacion del evento");
                throw;
            }
        }
    }
}
