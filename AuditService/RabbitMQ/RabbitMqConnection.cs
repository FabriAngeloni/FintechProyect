using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace AuditService.RabbitMQ
{
    public class RabbitMqConnection : IAsyncDisposable
    {
        public IConnection? _connection { get; private set; }
        private readonly RabbitMqOptions _options;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public RabbitMqConnection(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IConnection> ConnectAsync()
        {
            if (_connection is { IsOpen: true })
                return _connection;

            await _lock.WaitAsync();
            try
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
            finally
            {
                _lock.Release();
            }

        }
        public async ValueTask DisposeAsync()
        {
            if (_connection is { IsOpen: true })
                await _connection.CloseAsync();
        }
    }
}
