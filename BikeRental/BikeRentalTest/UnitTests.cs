using BikeRental;
using System;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace BikeRentalTest
{
    public class UnitTests
    {
        //[Fact]
        //public async Task Test1()
        //{
        //    var access = new DataAccess();
        //    var bike = await access.createBike(new BikeRental.Model.Bike {
        //    Brand = "my brand"});
            
        //}

        [Fact]
        public void TestCalculateRentalPrice()
        {
            ICostCalculator calculator = new CostCalculator();

            var price = calculator.
                calculateRentalPrice(new DateTime(2018, 2, 14, 8, 15, 0), new DateTime(2018, 2, 14, 10, 30, 0), 3, 5);

            Assert.Equal(13, price);

            price = calculator.
                calculateRentalPrice(new DateTime(2018, 2, 14, 8, 15, 0), new DateTime(2018, 2, 14, 8, 45, 0), 3, 100);

            Assert.Equal(3, price);

            price = calculator.
                calculateRentalPrice(new DateTime(2018, 2, 14, 8, 15, 0), new DateTime(2018, 2, 14, 8, 25, 0), 20, 100);

            Assert.Equal(0, price);


            Assert.Throws<InvalidRentalDurationException>(() => calculator.calculateRentalPrice(new DateTime(2019, 2, 14, 8, 15, 0), new DateTime(2018, 2, 14, 8, 25, 0), 20, 100));


        }
    }
}
