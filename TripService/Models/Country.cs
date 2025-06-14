﻿

using System.ComponentModel.DataAnnotations;

namespace TripApi.Models
{
    public partial class Country
    {
        public Country()
        {
            CountryTrips = new HashSet<CountryTrip>();
        }

        public int IdCountry { get; set; }
        
        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        public virtual ICollection<CountryTrip> CountryTrips { get; set; }
    }
}