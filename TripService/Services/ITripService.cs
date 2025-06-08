using TripApi.Models.DTOs;

namespace TripApi.Services
{
    public interface ITripService
    {
        Task<TripListResponse> GetTripsAsync(int page, int pageSize);
        Task<ApiResponse<string>> AddClientToTripAsync(int tripId, AddClientToTripRequest request);
    }
}