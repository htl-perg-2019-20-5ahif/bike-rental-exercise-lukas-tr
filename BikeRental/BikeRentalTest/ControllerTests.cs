using BikeRental;
using System;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using BikeRental.Controllers;
using Microsoft.AspNetCore.Mvc;
using BikeRental.Model;

namespace BikeRentalTest
{
    public class ControllerTests
    {
        [Fact]
        public async Task BasicRentTest()
        {
            IDataAccess da = new DataAccess(new CostCalculator());
            var bikesController = new BikesController(da);
            var customersController = new CustomersController(da);
            var rentalsController = new RentalsController(da);

            var result = await bikesController.PostBike(new BikeRental.Model.Bike
            {
                Brand = "brand",
                PurchaseDate = new DateTime(2019, 11, 11),
                RentalPriceInEuroForEachAdditionalHour = 3,
                RentalPriceInEuroForFirstHour = 5,
                BikeCategory = BikeCategory.Mountainbike,
            });

            var bikeId = ((Bike)((CreatedAtActionResult)result.Result).Value).BikeId;

            var result2 = await customersController.PostCustomer(new Customer
            {
                Gender = CustomerGender.Unknown,
                FirstName = "fname",
                LastName = "lname",
                Birthday = new DateTime(1999, 1, 1),
                Street = "street",
                ZipCode = "A-1234",
                Town = "town",
            });

            var customerId = ((Customer)((CreatedAtActionResult)result2.Result).Value).CustomerId;

            var result3 = await rentalsController.PostRental(new Rental
            {
                RenterId = customerId,
                BikeId = bikeId,
            });

            var rentalId = ((Rental)((CreatedAtActionResult)result3.Result).Value).RentalId;

            await rentalsController.EndRental(rentalId);

            await rentalsController.PayRental(rentalId);

            await customersController.DeleteCustomer(customerId);

            await bikesController.DeleteBike(bikeId);

        }
    }
}
