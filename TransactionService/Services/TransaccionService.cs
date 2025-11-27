using AccountService.Repositories;
using TransactionService.DTOs;
using TransactionService.Models;
using TransactionService.Repositories;

namespace TransactionService.Services
{

    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly ICuentaRepository _cuentaRepository;

        public TransaccionService(ITransaccionRepository transaccionRepository, ICuentaRepository accountRepository)
        {
            _transaccionRepository = transaccionRepository;
            _cuentaRepository = accountRepository;

        }
        public Task<Transaccion?> BuscarTransaccionPorId(Guid transaccionId) => _transaccionRepository.BuscarTransaccionPorId(transaccionId);

        public async Task CancelarTransaccion(Guid transaccionId)
        {
            var transaccion = await _transaccionRepository.BuscarTransaccionPorId(transaccionId)
                ?? throw new Exception("No se encontro ninguna transaccion con el id indicado.");
            var cuentaOrigen = await _cuentaRepository.BuscarPorIdDeCuenta(transaccion.DesdeCuenta)
                ?? throw new Exception("La cuenta origen no existe o no se ha encontrado.");
            var cuentaDestino = await _cuentaRepository.BuscarPorIdDeCuenta(transaccion.ParaCuenta)
                ?? throw new Exception("La cuenta receptora no existe o no se ha encontrado.");
            cuentaOrigen.Balance += transaccion.Monto;
            cuentaDestino.Balance -= transaccion.Monto;
            await _transaccionRepository.CancelarTransaccion(transaccion.TransaccionId);
            await _cuentaRepository.ActualizarCuenta(cuentaOrigen);
            await _cuentaRepository.ActualizarCuenta(cuentaDestino);
        }

        public async Task<Transaccion> CrearTransaccion(Guid desdeCuenta, decimal monto, Guid paraCuenta)
        {
            if (monto <= 0)
                throw new Exception("El monto enviado debe ser mayor a 0.");
            var cuentaOrigen = await _cuentaRepository.BuscarPorIdDeCuenta(desdeCuenta)
                ?? throw new Exception("La cuenta origen no existe o no se ha encontrado.");
            var cuentaDestino = await _cuentaRepository.BuscarPorIdDeCuenta(paraCuenta)
                ?? throw new Exception("La cuenta receptora no existe o no se ha encontrado.");
            if (cuentaOrigen.Balance < monto)
                throw new Exception($"El monto({monto}) a transferir supera el balance del usuario {cuentaOrigen.Balance}.");

            cuentaOrigen.Balance -= monto;
            cuentaDestino.Balance += monto;
            var transaccion = new Transaccion(desdeCuenta, monto, paraCuenta);
            await _transaccionRepository.CrearTransaccion(transaccion);
            await _cuentaRepository.ActualizarCuenta(cuentaOrigen);
            await _cuentaRepository.ActualizarCuenta(cuentaDestino);

            return transaccion;
        }

        public Task<IEnumerable<Transaccion>?> RetornarTransacciones() => _transaccionRepository.RetornarTransacciones();
    }
}
