
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using RabbitMQ.Client;

namespace NotificationService.Messaging
{
    public class RabbitMqConnection : IAsyncDisposable
    {
        public IConnection? _connection { get; private set; }
        private readonly RabbitMqOptions _options;

        public RabbitMqConnection(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }
        public async Task<IConnection> ConnectAsync()
        {
            if (_connection is { IsOpen: true }) 
                return _connection;
            var factory = new ConnectionFactory
            { 
                HostName = _options.Host,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password
            };
            _connection = await factory.CreateConnectionAsync();
            return _connection;
        }
        public async ValueTask DisposeAsync()
        {
            if (_connection is { IsOpen: true }) 
                await _connection.CloseAsync();
        }
    }
}
