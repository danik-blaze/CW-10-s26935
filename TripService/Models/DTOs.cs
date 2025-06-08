using System.ComponentModel.DataAnnotations;

namespace TripApi.Models.DTOs
{
    public class TripListResponse
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int AllPages { get; set; }
        public List<TripDto> Trips { get; set; } = new List<TripDto>();
    }

    public class TripDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public List<CountryDto> Countries { get; set; } = new List<CountryDto>();
        public List<ClientDto> Clients { get; set; } = new List<ClientDto>();
    }

    public class CountryDto
    {
        public string Name { get; set; }
    }

    public class ClientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AddClientToTripRequest
    {
        [Required]
        [StringLength(120)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(120)]
        public string LastName { get; set; }

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(120)]
        public string Telephone { get; set; }

        [Required]
        [StringLength(120)]
        public string Pesel { get; set; }

        [Required]
        public int IdTrip { get; set; }

        [Required]
        public string TripName { get; set; }

        public DateTime? PaymentDate { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class ErrorResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }
    }
}