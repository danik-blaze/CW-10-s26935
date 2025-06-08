using Microsoft.AspNetCore.Mvc;
using TripApi.Models.DTOs;
using TripApi.Services;

namespace TripApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }


        [HttpGet]
        public async Task<ActionResult<TripListResponse>> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var result = await _tripService.GetTripsAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse 
                { 
                    Error = "Internal Server Error", 
                    Message = ex.Message 
                });
            }
        }

 
        [HttpPost("{idTrip}/clients")]
        public async Task<ActionResult<ApiResponse<string>>> AddClientToTrip(int idTrip, [FromBody] AddClientToTripRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Validation Error", 
                        Message = "Invalid request data" 
                    });
                }


                if (request.IdTrip != idTrip)
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Validation Error", 
                        Message = "Trip ID in URL does not match Trip ID in request body" 
                    });
                }

                var result = await _tripService.AddClientToTripAsync(idTrip, request);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Registration Failed", 
                        Message = result.Message 
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse 
                { 
                    Error = "Internal Server Error", 
                    Message = ex.Message 
                });
            }
        }
    }
}