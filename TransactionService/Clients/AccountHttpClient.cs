
namespace TransactionService.Clients
{
    public class AccountHttpClient : IAccountClient
    {
        private readonly HttpClient _httpClient;
        public AccountHttpClient(HttpClient http)
        {
            _httpClient = http;
        }
        public async Task Acreditar(Guid accountId, decimal monto)
        {
            await _httpClient.PostAsJsonAsync("http://account-service:5000/cuenta/acreditar", new { accountId, monto });
        }

        public async Task Debitar(Guid accountId, decimal monto)
        {
            await _httpClient.PostAsJsonAsync("http://account-service:5000/cuenta/debitar", new { accountId, monto });
        }
    }
}
