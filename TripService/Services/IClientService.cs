using TripApi.Models.DTOs;

namespace TripApi.Services
{
    public interface IClientService
    {
        Task<ApiResponse<string>> DeleteClientAsync(int clientId);
    }
}