using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace AccountService.Messaging.RabbitMQ
{
    // Esta clase se encarga de mantener UNA sola conexión
    // abierta a RabbitMQ durante toda la vida del microservicio
    public class RabbitMqConnection : IAsyncDisposable
    {
        // Representa la conexión TCP real con RabbitMQ
        // Es pesada, thread-safe y debe ser de larga vida
        private IConnection? _connection;
        private readonly RabbitMqOptions _options;

        // Semáforo async para evitar que múltiples hilos
        // intenten crear la conexión al mismo tiempo
        // (muy común cuando varios BackgroundServices arrancan juntos)
        private readonly SemaphoreSlim _lock = new(1,1);

        public RabbitMqConnection(IOptions<RabbitMqOptions>options)
        {
            _options = options.Value;
        }

        // Método async que crea la conexión con RabbitMQ
        // Se llama UNA sola vez, al arrancar el microservicio (Program.cs)
        public async Task<IConnection> ConnectAsync()
        {
            if (_connection is { IsOpen: true })
                return _connection;

            // Espera su turno para crear la conexion
            await _lock.WaitAsync();
            try
            {
                if (_connection is { IsOpen: true })
                    return _connection;

                // ConnectionFactory es una clase de RabbitMQ.Client. Sirve para configurar CÓMO conectarse al broker
                var factory = new ConnectionFactory
                {
                    // Dirección del servidor RabbitMQ (localhost, IP o DNS)
                    HostName = _options.Host,

                    // Puerto AMQP (por defecto 5672)
                    Port = _options.Port,

                    // Usuario para autenticarse en RabbitMQ
                    UserName = _options.UserName,

                    // Password del usuario
                    Password = _options.Password
                };
                // Crea la conexión TCP real de forma ASYNC
                // Esta operación es costosa y por eso se hace una sola vez
                _connection = await factory.CreateConnectionAsync();
                return _connection;
            }
            finally
            {
                _lock.Release();
            }
        }

        // Se ejecuta automáticamente cuando la aplicación se apaga o cuando el contenedor libera el recurso
        public async ValueTask DisposeAsync()
        {
            // Verifica que la conexión exista y esté abierta
            if (_connection is { IsOpen: true }) 
                await _connection.CloseAsync();// Cierra la conexión correctamente de forma async  // Evita conexiones colgadas o leaks

        }
    }
}
