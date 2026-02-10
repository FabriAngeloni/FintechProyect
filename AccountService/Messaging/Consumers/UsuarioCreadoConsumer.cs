
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using AccountService.Services;
using AccountService.Messaging.RabbitMQ;
using AccountService.Events;
using AccountService.DTOs;

namespace AccountService.Messaging.Consumers
{
    public class UsuarioCreadoConsumer : BackgroundService
    {
        private readonly RabbitMqConnection _connection;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<UsuarioCreadoConsumer> _logger;
        public UsuarioCreadoConsumer(RabbitMqConnection connection, IServiceScopeFactory scopeFactory, ILogger<UsuarioCreadoConsumer> logger)
        {
            _connection = connection;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("UsuarioCreadoConsumer: realizando conexion.");
            var connection = await _connection.ConnectAsync();
            _logger.LogDebug("UsuarioCreadoConsumer: realizando canal.");
            var channel = await connection.CreateChannelAsync();

            //Declaro el exchange donde llegaran los mensajes
            await channel.ExchangeDeclareAsync(
                exchange: "identity.events",
                type: ExchangeType.Fanout,
                durable: true
                );

            //Declarar la cola propia del servicio
            await channel.QueueDeclareAsync(
                queue: "account.usuario-creado",
                durable: true, //Los mensajes sobreviven reinicios
                exclusive: false, //No es solo para esta conexion
                autoDelete: false //No se borra solo si nadie escucha
                );

            //Bind(unir) cola con el exchange
            //Aca lo que hace es decir que todo mensaje que llegue a el exchange "identity.events" se enviara a la cola "account.usuario-creado"
            await channel.QueueBindAsync(
                queue: "account.usuario-creado",
                exchange: "identity.events",
                routingKey: ""   //Con el routingKey decidimos a que colas va el mensaje (no necesario cuando declaramos el exchange fanout(se envia a todas la colas))
                );

            _logger.LogInformation("UsuarioCreadoConsumer: creando consumer.");
            //Se crea el consumer (quien escucha constantemente esperando un mensaje)
            var consumer = new AsyncEventingBasicConsumer(channel);

            //Lo que pasa cuando llega un mensaje
            consumer.ReceivedAsync += async (sender, args) => //Este metodo anonimo se ejecuta cada vez que llega un mensaje
            {
                try
                {
                    _logger.LogInformation("UsuarioCreadoConsumer: mensaje recibido. Exchange:{Exchange}, DeliveryTag: {Tag}",args.Exchange,args.DeliveryTag);
                    using var scope = _scopeFactory.CreateScope();
                    var cuentaService = scope.ServiceProvider.GetRequiredService<ICuentaService>();
                    var json = Encoding.UTF8.GetString(args.Body.ToArray()); //Transformamos los bytes enviados por rabbitmq a un json
                    var evento = JsonSerializer.Deserialize<UsuarioCreadoEvent>(json); //transforma el json en un objeto UsuarioCreadoEvent

                    _logger.LogInformation("UsuarioCreadoConsumer: se recepciono un user con los datos {Nombre}, {Email}, {Id}", evento.NombreUsuario, evento.Email, evento.UserId);
                    var usuario = await cuentaService.BuscarPorUserIdAsync(evento.UserId);
                    if (usuario.Any())
                    {
                        _logger.LogInformation("UsuarioCreadoConsumer: la cuenta ya existe para userId {UserId}", evento.UserId);
                        await channel.BasicAckAsync(args.DeliveryTag, false);
                        return;
                    }
                    _logger.LogInformation("UsuarioCreadoConsumer: se creara una cuenta para userId: {UserId}", evento.UserId);
                    var dtoRequest = new CrearCuentaParaUsuarioDto(evento.NombreUsuario,0);
                    await cuentaService.CrearCuentaAsync(dtoRequest ); //se le crea una cuenta al nuevo usuario a partir del desarmado del json
                   
                    _logger.LogInformation("UsuarioCreadoConsumer: proceso correctamente el evento. Exchange: {Exchange}, DeliveryTag: {Tag}",args.Exchange,args.DeliveryTag);
                    await channel.BasicAckAsync(args.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UsuarioCreadoConsumer: error en la recepcion del mensaje");
                    await channel.BasicNackAsync(args.DeliveryTag, false, requeue: false);
                }
            };

            await channel.BasicConsumeAsync(
                queue: "account.usuario-creado",
                autoAck: false,
                consumer: consumer
                );
        }
    }
}
