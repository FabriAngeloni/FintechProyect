using AccountService.DTOs;
using AccountService.Models;
using AccountService.Repositories;

namespace AccountService.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _repository;
        private readonly ILogger<CuentaService> _logger;

        public CuentaService(ICuentaRepository repository, ILogger<CuentaService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CuentaDtoResponse?> BuscarPorIdAsync(Guid accountId)
        {
            _logger.LogInformation("Service: realizando la busqueda por ID de cuenta:{Id}", accountId);
            try
            {
                var cuenta = await _repository.BuscarPorIdDeCuenta(accountId);
                if (cuenta == null)
                {
                    _logger.LogWarning("Service: no se encontro cuenta a traves del accountId: {AccountId}", accountId);
                    return null;
                }
                _logger.LogInformation("Service: cuenta encontrada correctamente con ID de cuenta:{Id}", accountId);
                return new CuentaDtoResponse(cuenta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error en la obtencion de la cuenta: {Id}", accountId);
                throw;
            }
        }

        public async Task<IEnumerable<CuentaDtoResponse>> BuscarPorUserIdAsync(Guid userId)
        {
            _logger.LogInformation("Service: buscando cuenta a traves del userId: {userId}", userId);
            try
            {
                var cuentas = await _repository.BuscarCuentasPorUserId(userId);
                if (!cuentas.Any())
                {
                    _logger.LogWarning("Service: no se encontraron cuentas a traves del userId: {userId}", userId);
                    return Enumerable.Empty<CuentaDtoResponse>();
                }
                
                _logger.LogInformation("Service: se encontraron cuentas a traves del userId {userId}.", userId);
                return cuentas.Select(c => new CuentaDtoResponse(c)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error al recibir la cuenta a traves del userId: {userId}", userId);
                throw;
            }
        }

        public async Task<CuentaDtoResponse> CrearCuentaAsync(CrearCuentaParaUsuarioDto dtoRequest)
        {
            _logger.LogInformation("Service: creando cuenta con userId, nombre y balance: \n    {userId}\n{nombre}\n{balance}", dtoRequest.UserId, dtoRequest.NombreUsuario, dtoRequest.Balance);
            try
            {
                if (string.IsNullOrWhiteSpace(dtoRequest.NombreUsuario))
                {
                    _logger.LogWarning("Service: el nombre de usuario es null.");
                    throw new ArgumentException("Nombre de usuario vacio o en blanco");
                }

                if (dtoRequest.Balance < 0)
                {
                    _logger.LogWarning("Service: el balance de la nueva cuenta es menor a 0.");
                    throw new ArgumentException($"Balance ingresado:{dtoRequest.Balance} es menor a 0.");
                }
                var cuenta = new Cuenta
                {
                    Id = Guid.NewGuid(),
                    UserId = dtoRequest.UserId,
                    NombreUsuario = dtoRequest.NombreUsuario,
                    Balance = dtoRequest.Balance,
                    CreadoEl = DateTime.UtcNow
                };

                await _repository.CrearCuenta(cuenta);
                _logger.LogInformation("Service: se ha creado una cuenta con exito...\n{UserId}\n{Id}\n{Nombre}\n{CreadoEl} ", cuenta.UserId ,cuenta.Id, cuenta.NombreUsuario, cuenta.CreadoEl);
                return new CuentaDtoResponse(cuenta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error en la creacion de la cuenta");
                throw;
            }
        }

        public async Task<BalanceDtoResponse> Debito(Guid accountId, decimal monto)
        {
            _logger.LogInformation("Service: comenzando debito por el monto de ${Monto} a la cuenta ID:{Cuenta}", monto, accountId);
            try 
            {
                var cuenta = await _repository.BuscarPorIdDeCuenta(accountId);
                if(cuenta == null)
                {
                    _logger.LogWarning("Service: no se encontro ninguna cuenta bajo el el numero de cuenta ID:{Id}",accountId);
                    throw new Exception("Cuenta no encontrada.");
                }
                if(cuenta.Balance < monto)
                {
                    _logger.LogWarning("Balance de la cuenta: ${Balance}\nMonto a descontar: ${Monto}\nEl monto no puede ser superior al balance de la cuenta.",cuenta.Balance,monto);
                    throw new Exception("El monto a descontar es mayor al balance de la cuenta.");
                }
                cuenta.Balance -= monto;
                await _repository.ActualizarCuenta(cuenta);
                return new BalanceDtoResponse(cuenta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error en el debito de ${M} en la cuenta ID:{C}",accountId,monto);
                throw;
            }
        }
        public async Task<BalanceDtoResponse> Acreditar(Guid accountId, decimal monto)
        {
            _logger.LogInformation("Service: comenzando a acreditar por el monto de ${Monto} a la cuenta ID:{Cuenta}", monto, accountId);
            try
            {
                var cuenta = await _repository.BuscarPorIdDeCuenta(accountId);
                if (cuenta == null)
                {
                    _logger.LogWarning("Service: no se encontro ninguna cuenta bajo el el numero de cuenta ID:{Id}", accountId);
                    throw new Exception("Cuenta no encontrada.");
                }
                cuenta.Balance += monto;
                await _repository.ActualizarCuenta(cuenta);
                return new BalanceDtoResponse(cuenta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: error en acreditar el monto de ${M} en la cuenta ID:{C}", accountId, monto);
                throw;
            }
        }
    }
}
