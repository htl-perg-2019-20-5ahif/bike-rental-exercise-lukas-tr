using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental
{
    public class EntityDoesNotExistException : Exception { }
    public class InvalidRentalDurationException : Exception { }
    public class BikeHasExistingRentalsException : Exception { }
}
