using TransactionService.Clients;
using TransactionService.DTOs;
using TransactionService.Models;
using TransactionService.Repositories;

namespace TransactionService.Services
{

    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly ILogger<TransaccionService> _logger;
        private readonly IAccountClient _accountClient;

        public TransaccionService(ITransaccionRepository transaccionRepository, IAccountClient accountClient, ILogger<TransaccionService> logger)
        {
            _transaccionRepository = transaccionRepository;
            _logger = logger;
            _accountClient = accountClient;
        }
        public async Task<TransaccionDtoResponse?> BuscarTransaccionPorId(Guid transaccionId)
        {
            var transaccion = await _transaccionRepository.BuscarTransaccionPorId(transaccionId)
                ?? throw new KeyNotFoundException("No se encontro ninguna transaccion con el id indicado.");
            return new TransaccionDtoResponse(transaccion);
        }

        public async Task CancelarTransaccion(Guid transaccionId)
        {
            _logger.LogInformation("Service: Iniciando cancelacion de la transaccion...{Id}", transaccionId);
            try
            {
                var transaccion = await _transaccionRepository.BuscarTransaccionPorId(transaccionId)
                ?? throw new KeyNotFoundException("No se encontro ninguna transaccion con el id indicado.");
                await _accountClient.Debitar(transaccion.ParaCuenta, transaccion.Monto);
                await _accountClient.Acreditar(transaccion.DesdeCuenta, transaccion.Monto);

                _logger.LogInformation("Service: se cancelo de manera correcta");
                await _transaccionRepository.CancelarTransaccion(transaccion.TransaccionId);           
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Service: error interno.");
                throw;
            }
        }

        public async Task<TransaccionDtoResponse> CrearTransaccion(Guid desdeCuenta, decimal monto, Guid paraCuenta)
        {
            _logger.LogInformation("Service: Iniciando creacion de una transaccion...");
            try
            {
                if (monto <= 0)
                    throw new Exception("El monto enviado debe ser mayor a 0.");
                await _accountClient.Debitar(desdeCuenta, monto);
                await _accountClient.Acreditar(paraCuenta, monto);
                var transaccion = new Transaccion(desdeCuenta, monto, paraCuenta);
                await _transaccionRepository.CrearTransaccion(transaccion);

                return new TransaccionDtoResponse(transaccion);
            }
            catch
            {
                _logger.LogError("Service: error en la creacion de la transaccion" +
                    "\nDESDE:{D}" +
                    "\nMONTO: {M}" +
                    "\nPARA: {P}",desdeCuenta,monto,paraCuenta);
                throw;
            }
            
        }

        public async Task<IEnumerable<TransaccionDtoResponse>?> RetornarTransacciones()
        {
            var aux = await _transaccionRepository.RetornarTransacciones();
            return aux.Select(x => new TransaccionDtoResponse(x)).ToList();
        }
    }

}
