using AuditService.Events;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;
using MailKit.Security;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using AuditService.Services;
using AuditService.DTOs;

namespace AuditService.RabbitMQ.Consumers
{
    public class UsuarioCreadoConsumer : BackgroundService
    {
        private readonly RabbitMqConnection _connection;
        //inyecto scopeFactory para poder realizar una sola instancia por request. debido a que backgroundService es singleton y AuditService utiliza dbcontext
        //el cual es scoped...Un singleton no puede depender de un scoped...
        //Se inyecta la fabrica de scopes.
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
                queue: "audit.usuario-creado",
                durable: true, //Los mensajes sobreviven reinicios
                exclusive: false, //No es solo para esta conexion
                autoDelete: false //No se borra solo si nadie escucha
                );

            //Bind(unir) cola con el exchange
            //Aca lo que hace es decir que todo mensaje que llegue a el exchange "identity.events" se enviara a la cola "audit.usuario-creado"
            await channel.QueueBindAsync(
                queue: "audit.usuario-creado",
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
                    _logger.LogInformation("UsuarioCreadoConsumer: mensaje recibido. Exchange:{Exchange}, DeliveryTag: {Tag}", args.Exchange, args.DeliveryTag);

                    var json = Encoding.UTF8.GetString(args.Body.ToArray()); //Transformamos los bytes enviados por rabbitmq a un json

                    var evento = JsonSerializer.Deserialize<UsuarioCreadoEvent>(json); //transforma el json en un objeto UsuarioCreadoEvent

                    if (evento is null)
                    {
                        _logger.LogError("UsuarioCreadoConsumer: se deserializo un json nulo.");
                        await channel.BasicNackAsync(args.DeliveryTag, false, requeue: false);
                        return;
                    }

                    _logger.LogInformation("UsuarioCreadoConsumer: se recepciono y transformo un json con un user con los datos {Nombre}, {Email}, {Id}",
                        evento.NombreUsuario, evento.Email, evento.UserId); ;

                    var cuerpo = $"¡Hola {evento.NombreUsuario}!\n" +
                    $"Tu usuario fue creado con exito.\n" +
                    $"¡Bienvenido a Fintech!\n";

                    var audit = new AuditDtoRequest(nameof(UsuarioCreadoEvent), json);
                    //creo el scope para poder utilizar AuditService por un breve periodo de ejecucion, el cual vivira hasta el fin del hilo de ejecucion
                    using var scope = _scopeFactory.CreateScope();
                    //asigno el scope y finalmente ya puedo hacer uso de el Service el cual va a vivir hasta el cierre del bloque
                    var service = scope.ServiceProvider.GetRequiredService<IAuditServices>();

                    await service.CrearLog(audit);
                    _logger.LogInformation("UsuarioCreadoConsumer: se creo un audit para el evento.");

                    await SendEmailAsync(evento.Email, $"¡Bienvenido {evento.NombreUsuario}!.", cuerpo);
                    _logger.LogInformation("UsuarioCreadoConsumer: se envio un mail a: {Email}", evento.Email);

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
                queue: "audit.usuario-creado",
                autoAck: false,
                consumer: consumer
                );
        }
        private async Task SendEmailAsync(string para, string asunto, string cuerpo)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Fintech", "Fintech.arfa01@noreply-fintech.com"));
            message.To.Add(MailboxAddress.Parse(para));
            message.Subject = asunto;
            message.Body = new TextPart("plain") { Text = cuerpo };

            using var smtp = new SmtpClient();

            //utilizo ethereal.email para realizar las pruebas
            await smtp.ConnectAsync("smtp.ethereal.email",587,SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("erik.weissnat90@ethereal.email", "T2YJDWqVxp63fJHsM3");
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
