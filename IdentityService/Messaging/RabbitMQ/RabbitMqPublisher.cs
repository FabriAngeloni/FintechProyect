using AccountService.Messaging.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AccountService.Messaging.RabbitMQ
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly RabbitMqConnection _connection; //Iconnection proviene de la biblioteca de RabbiMQ
        private readonly RabbitMqOptions _options;
        private readonly ILogger<RabbitMqPublisher> _logger;

        //Inyectamos RabbitMqConnection y luego tambien options... IOptions viene de la configuracion                                                                                 
        //hecha en el program.cs donde hacemos la transformacion del json. 
        public RabbitMqPublisher(RabbitMqConnection connection, IOptions<RabbitMqOptions> options, ILogger<RabbitMqPublisher> logger)
        {
            _connection = connection;
            _options = options.Value;
            _logger = logger;
        }
        public async Task PublishAsync<T>(T mensaje)
        {
            try
            {
                _logger.LogDebug("RabbitMqPublisher: creando conexion. Exchange: {Exchange} Tipo: {Tipo}", _options.Exchange, typeof(T).Name);
                var connection = await _connection.ConnectAsync();

                _logger.LogDebug("RabbitMqPublisher: creando channel.");
                using var channel = await connection.CreateChannelAsync();

                _logger.LogInformation("RabbitMqPublisher: declara exchange: {Exchange}", _options.Exchange);
                //Declara el Exchange donde se van a publicar los eventos
                await channel.ExchangeDeclareAsync(
                    exchange: _options.Exchange, // Busca el nombre del exchange del _options (sacado del appsettings.json "Exchange":  "account.events")
                    type: ExchangeType.Fanout,  // Envía el mensaje a todas las colas conectadas
                    durable: true);            // No se borra si RabbitMQ se reinicia

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensaje)); //convierte el objeto en Json
                _logger.LogDebug("RabbitMqPublisher: json convertido: {Json}", body.ToString());

                _logger.LogInformation("RabbitMqPublisher: publica mensaje: {Mensaje} en el exchange: {Exchange}", typeof(T).Name, _options.Exchange);
                //Envia el mensaje al exchange
                await channel.BasicPublishAsync( //.BasicPublishAsync dira que envie el mensaje...
                    exchange: _options.Exchange, //a donde se manda
                    routingKey: "", //en fanout no se usa
                    body: body // Envia el objeto Mensaje convertido ahora en JSON
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMqPublisher: error en la publicacion del evento");
                throw;
            }


        }
    }
}
