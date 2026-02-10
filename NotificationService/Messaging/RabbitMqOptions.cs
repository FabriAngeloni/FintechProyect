namespace NotificationService.Messaging
{
    public class RabbitMqOptions
    {
        public string Host { get; set; } = null!;      // Dirección del servidor RabbitMQ
        public int Port { get; set; }                   // Puerto de conexión (ej: 5672)
        public string UserName { get; set; } = null!;  // Usuario de RabbitMQ
        public string Password { get; set; } = null!;  // Contraseña del usuario
        public string Exchange { get; set; } = null!;  // Exchange donde se publican los eventos
    }
}
