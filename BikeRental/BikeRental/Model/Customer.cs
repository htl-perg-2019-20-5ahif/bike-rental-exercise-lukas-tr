using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public CustomerGender Gender { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(75)]
        public string LastName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(75)]
        public string Street { get; set; }
        [MaxLength(10)]
        public string HouseNumber { get; set; }
        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(75)]
        public string Town { get; set; }
        public List<Rental> Rentals { get; set; }
    }

    public enum CustomerGender
    {
        Male,
        Female,
        Unknown
    }
}
