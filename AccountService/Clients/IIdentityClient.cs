using AccountService.DTOs;

namespace AccountService.Clients
{
    public interface IIdentityClient
    {
        Task<UserDtoResponse?> BuscarUser(Guid userId);
    }
}
