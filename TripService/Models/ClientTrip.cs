using System.ComponentModel.DataAnnotations;

namespace TripApi.Models
{
    public partial class ClientTrip
    {
        public int IdClient { get; set; }
        public int IdTrip { get; set; }
        
        [Required]
        public DateTime RegisteredAt { get; set; }
        
        public DateTime? PaymentDate { get; set; }

        public virtual Client IdClientNavigation { get; set; }
        public virtual Trip IdTripNavigation { get; set; }
    }
}