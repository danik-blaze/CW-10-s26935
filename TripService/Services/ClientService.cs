using Microsoft.EntityFrameworkCore;
using TripApi.Data;
using TripApi.Models.DTOs;

namespace TripApi.Services
{
    public class ClientService : IClientService
    {
        private readonly TripContext _context;

        public ClientService(TripContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> DeleteClientAsync(int clientId)
        {
            try
            {
                var client = await _context.Clients
                    .Include(c => c.ClientTrips)
                    .FirstOrDefaultAsync(c => c.IdClient == clientId);

                if (client == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Client not found",
                        Data = null
                    };
                }

                // Check if client has any trip registrations
                if (client.ClientTrips.Any())
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Cannot delete client who has registered trips. Please remove all trip registrations first.",
                        Data = null
                    };
                }

                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Client successfully deleted",
                    Data = $"Client {client.FirstName} {client.LastName} has been removed"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An error occurred while deleting client: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}