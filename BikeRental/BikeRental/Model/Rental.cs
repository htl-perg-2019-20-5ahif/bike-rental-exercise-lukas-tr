using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Model
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int RenterId { get; set; }
        [Required]
        public Customer Renter { get; set; }
        public int BikeId { get; set; }
        [Required]
        public Bike Bike { get; set; }
        [Required]
        public DateTime RentalBegin { get; set; }
        public DateTime? RentalEnd { get; set; }
        [Range(0, double.PositiveInfinity)]
        [RegularExpression("^\\d(\\.\\d{1,2})$")]
        public decimal? TotalCostsInEuro { get; set; }
        [Required]
        public bool Paid { get; set; }
    }
}
