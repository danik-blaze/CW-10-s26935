using Microsoft.EntityFrameworkCore;
using TripApi.Data;
using TripApi.Models;
using TripApi.Models.DTOs;

namespace TripApi.Services
{
    public class TripService : ITripService
    {
        private readonly TripContext _context;

        public TripService(TripContext context)
        {
            _context = context;
        }

        public async Task<TripListResponse> GetTripsAsync(int page, int pageSize)
        {
            var totalTrips = await _context.Trips.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

            var trips = await _context.Trips
                .Include(t => t.CountryTrips)
                .ThenInclude(ct => ct.IdCountryNavigation)
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TripDto
                {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = t.CountryTrips.Select(ct => new CountryDto
                    {
                        Name = ct.IdCountryNavigation.Name
                    }).ToList(),
                    Clients = t.ClientTrips.Select(ct => new ClientDto
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    }).ToList()
                })
                .ToListAsync();

            return new TripListResponse
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = totalPages,
                Trips = trips
            };
        }

        public async Task<ApiResponse<string>> AddClientToTripAsync(int tripId, AddClientToTripRequest request)
        {
            try
            {
       
                var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == request.Pesel);
                if (existingClient != null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Client with this PESEL already exists",
                        Data = null
                    };
                }

          
                var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == tripId);
                if (trip == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Trip not found",
                        Data = null
                    };
                }

                if (trip.DateFrom <= DateTime.Now)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Cannot register for a trip that has already started or passed",
                        Data = null
                    };
                }

     
                var newClient = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Telephone = request.Telephone,
                    Pesel = request.Pesel
                };

                _context.Clients.Add(newClient);
                await _context.SaveChangesAsync();


                var existingRegistration = await _context.ClientTrips
                    .FirstOrDefaultAsync(ct => ct.IdClient == newClient.IdClient && ct.IdTrip == tripId);

                if (existingRegistration != null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Client is already registered for this trip",
                        Data = null
                    };
                }

 
                var clientTrip = new ClientTrip
                {
                    IdClient = newClient.IdClient,
                    IdTrip = tripId,
                    RegisteredAt = DateTime.Now,
                    PaymentDate = request.PaymentDate
                };

                _context.ClientTrips.Add(clientTrip);
                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Client successfully registered for the trip",
                    Data = $"Client {request.FirstName} {request.LastName} registered for trip {request.TripName}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}