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
        public IConnection? Connection { get; private set; }
        private readonly SemaphoreSlim _lock = new(1, 1);

        //Inyectamos rabbitmqoptions que proviene del json
        private readonly RabbitMqOptions _options;
        public RabbitMqConnection(IOptions<RabbitMqOptions >options)
        {
            _options = options.Value;
        }

        // Método async que crea la conexión con RabbitMQ
        // Se llama UNA sola vez, al arrancar el microservicio (Program.cs)
        public async Task<IConnection> ConnectAsync()
        {
            //Verificamos que no haya una conexion ya realizada
            if (Connection is { IsOpen: true })
                return Connection;

            await _lock.WaitAsync();
            try
            {
                if (Connection is { IsOpen: true })
                    return Connection;
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
                Connection = await factory.CreateConnectionAsync();
                return Connection;
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
            if (Connection is { IsOpen: true }) 
                await Connection.CloseAsync();// Cierra la conexión correctamente de forma async  // Evita conexiones colgadas o leaks

        }
    }
}
