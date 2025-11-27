using AccountService.DTOs;
using AccountService.Models;
using AccountService.Repositories;
using System.Data;

namespace AccountService.Services
{
    public class CuentaServices : ICuentaServices
    {
        private readonly ICuentaRepository _repository;
        public CuentaServices(ICuentaRepository repository)
        {
            _repository = repository;
        }
        public Task<Cuenta?> BuscarPorIdAsync(Guid accountId) => _repository.BuscarPorIdDeCuenta(accountId);

        public Task<IEnumerable<Cuenta>> BuscarPorUserIdAsync(Guid userId) => _repository.BuscarCuentasPorUserId(userId);

        public async Task<Cuenta> CrearCuentaAsync(string nombreUsuario, decimal balance)
        {
            var cuenta = new Cuenta
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                NombreUsuario = nombreUsuario,
                Balance = balance,
                CreadoEl = DateTime.UtcNow
            };
            await _repository.CrearCuenta(cuenta);
            return cuenta;
        }

        public async Task DepositarAsync(Guid accountId, decimal monto)
        {
            var cuenta = await _repository.BuscarPorIdDeCuenta(accountId) ?? throw new Exception("Cuenta no encontrada.");
            cuenta.Balance += monto;
            await _repository.ActualizarCuenta(cuenta);
        }

        public async Task ExtraerAsync(Guid accountId, decimal monto)
        {
            var cuenta = await _repository.BuscarPorIdDeCuenta(accountId) ?? throw new Exception("Cuenta no encontrada.");
            if (cuenta.Balance < monto) throw new Exception("El monto a extraer supera el balance de la cuenta.");
            cuenta.Balance -= monto;
            await _repository.ActualizarCuenta(cuenta);
        }
    }
}
