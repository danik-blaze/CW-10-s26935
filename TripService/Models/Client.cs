

using System.ComponentModel.DataAnnotations;

namespace TripApi.Models
{
    public partial class Client
    {
        public Client()
        {
            ClientTrips = new HashSet<ClientTrip>();
        }

        public int IdClient { get; set; }
        
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

        public virtual ICollection<ClientTrip> ClientTrips { get; set; }
    }
}