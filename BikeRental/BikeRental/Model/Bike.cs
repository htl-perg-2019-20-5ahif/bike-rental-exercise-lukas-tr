using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Model
{
    public class Bike
    {
        public int BikeId { get; set; }
        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        public DateTime? DateOfLastService { get; set; }
        [Required]
        [RegularExpression("^\\d(\\.\\d{1,2})$")]
        [Range(0, double.PositiveInfinity)]
        public decimal RentalPriceInEuroForFirstHour { get; set; }
        [Required]
        [RegularExpression("^\\d(\\.\\d{1,2})$")]
        [Range(0, double.PositiveInfinity)]
        public decimal RentalPriceInEuroForEachAdditionalHour { get; set; }
        [Required]
        public BikeCategory BikeCategory { get; set; }
        public List<Rental> Rentals { get; set; }
    }

    public enum BikeCategory
    {
        StandardBike,
        Mountainbike,
        TreckingBike,
        RacingBike
    }
        
}
