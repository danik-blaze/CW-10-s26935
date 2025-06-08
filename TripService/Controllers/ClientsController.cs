using Microsoft.AspNetCore.Mvc;
using TripApi.Models.DTOs;
using TripApi.Services;

namespace TripApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

  
        [HttpDelete("{idClient}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteClient(int idClient)
        {
            try
            {
                if (idClient <= 0)
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Validation Error", 
                        Message = "Invalid client ID" 
                    });
                }

                var result = await _clientService.DeleteClientAsync(idClient);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Deletion Failed", 
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