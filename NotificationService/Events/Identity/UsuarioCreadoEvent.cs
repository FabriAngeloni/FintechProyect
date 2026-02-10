namespace NotificationService.Events.Identity;

// Evento que representa que una cuenta fue creada
// Se usa para enviar información entre servicios (mensajería)
// record: tipo inmutable, pensado para transportar datos y eventos
public record UsuarioCreadoEvent
    (
    Guid UserId,
    string NombreUsuario,
    string Email
    );
