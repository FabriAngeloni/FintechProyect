
using AccountService.DTOs;
using System.Net;

namespace AccountService.Clients
{
    public class IdentityHttpClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;
        public IdentityHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserDtoResponse?> BuscarUser(Guid userId)
        {
            var response = await _httpClient.GetAsync($"/api/auth/{userId}");
            if (response.StatusCode == HttpStatusCode.NotFound || response == null) 
                return null;
            if (!response.IsSuccessStatusCode) 
                throw new ApplicationException($"Error llamando al IdentityService: {response.StatusCode}");
            return await response.Content.ReadFromJsonAsync<UserDtoResponse>();
        }


    }
}
