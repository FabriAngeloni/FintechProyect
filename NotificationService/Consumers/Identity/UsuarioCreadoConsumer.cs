
using NotificationService.Events.Identity;
using NotificationService.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationService.Consumers.Identity
{
    public class UsuarioCreadoConsumer : BackgroundService
    {
        private readonly RabbitMqConnection _connection;
        private readonly ILogger<UsuarioCreadoConsumer> _logger;
        public UsuarioCreadoConsumer(RabbitMqConnection connection, ILogger<UsuarioCreadoConsumer> logger)
        {
            _connection = connection;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("UsuarioCreadoConsumer: realizando conexion.");
            var connection = await _connection.ConnectAsync();
            _logger.LogDebug("UsuarioCreadoConsumer: realizando canal.");
            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "identity.events",
                type: ExchangeType.Fanout,
                durable: true
                );

            await channel.QueueDeclareAsync(
                queue:"notificacion.usuario-creado",
                durable: true,
                exclusive: false,
                autoDelete: false
                );

            await channel.QueueBindAsync(
                queue:"notificacion.usuario-creado",
                exchange:"identity.events",
                routingKey: ""
                );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    _logger.LogInformation("UsuarioCreadoConsumer: mensaje recibido. Exchange {Exchange}, DeliveryTag: {Tag}", args.Exchange, args.DeliveryTag);
                    var json = Encoding.UTF8.GetString(args.Body.ToArray());
                    var evento = JsonSerializer.Deserialize<UsuarioCreadoEvent>(json);

                    _logger.LogInformation("--------|Evento|--------\n" +
                        "Usuario: {User} \n" +
                        "Email: {Email} \n" +
                        "UserId: {Id}",
                        evento.NombreUsuario,evento.Email,evento.UserId);

                    _logger.LogInformation("UsuarioCreadoConsumer: se proceso correctamente el evento. Exchange {Exchange}, DeliveryTag: {Tag}", args.Exchange, args.DeliveryTag);
                    await channel.BasicAckAsync(args.DeliveryTag,false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UsuarioCreadoConsumer: error en la recepcion del mensaje");
                    await channel.BasicNackAsync(args.DeliveryTag, false, requeue: false);
                }
            };
        }
    }
}
