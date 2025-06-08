

using System.ComponentModel.DataAnnotations;

namespace TripApi.Models
{
    public partial class Trip
    {
        public Trip()
        {
            ClientTrips = new HashSet<ClientTrip>();
            CountryTrips = new HashSet<CountryTrip>();
        }

        public int IdTrip { get; set; }
        
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(220)]
        public string Description { get; set; }
        
        [Required]
        public DateTime DateFrom { get; set; }
        
        [Required]
        public DateTime DateTo { get; set; }
        
        [Required]
        public int MaxPeople { get; set; }

        public virtual ICollection<ClientTrip> ClientTrips { get; set; }
        public virtual ICollection<CountryTrip> CountryTrips { get; set; }
    }
}