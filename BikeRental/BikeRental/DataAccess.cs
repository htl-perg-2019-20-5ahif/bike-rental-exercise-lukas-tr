using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeRental.Model;
using Microsoft.EntityFrameworkCore;

namespace BikeRental
{
    public class DataAccess : IDataAccess
    {

        private Context context;
        private ICostCalculator calculator;

        public DataAccess(ICostCalculator c)
        {
            context = new Context();
            calculator = c;
        }

        public async Task<Bike> createBike(Bike bike)
        {
            context.Bikes.Add(bike);
            await context.SaveChangesAsync();
            return bike;
        }

        public async Task<Customer> createCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        public async Task<Rental> startRental(Rental rental)
        {
            // certain fields may not be set
            rental.RentalBegin = DateTime.Now;
            rental.RentalEnd = null;
            rental.TotalCostsInEuro = null;
            rental.Paid = false;
            context.Rentals.Add(rental);
            await context.SaveChangesAsync();
            return rental;
        }

        public async Task<Bike> deleteBike(int id)
        {
            var bike = await context.Bikes.FindAsync(id);
            if (bike == null)
            {
                throw new EntityDoesNotExistException();
            }
            // only allow deleting bikes that haven't been rented before or all users belonging to rentals have been deleted
            if (bike.Rentals.Count > 0)
            {
                throw new BikeHasExistingRentalsException();
            }
            context.Bikes.Remove(bike);
            await context.SaveChangesAsync();
            return bike;
        }

        public async Task<Customer> deleteCustomer(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null)
            {
                throw new EntityDoesNotExistException();
            }
            // delete all rentals
            if (customer.Rentals.Count > 0)
            {
                context.Rentals.RemoveRange(customer.Rentals);
            }
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        public async Task<IEnumerable<Bike>> getCurrentlyAvailableBikes(BikeSort sort)
        {
            switch (sort)
            {
                case BikeSort.PriceOfAdditionalHoursAsc:
                    return await context.Bikes.OrderBy(b => b.RentalPriceInEuroForEachAdditionalHour).ToListAsync();
                case BikeSort.PriceOfFirstHourAsc:
                    return await context.Bikes.OrderBy(b => b.RentalPriceInEuroForFirstHour).ToListAsync();
                case BikeSort.PurchaseDateDesc:
                    return await context.Bikes.OrderBy(b => b.PurchaseDate).ToListAsync();
            }
            return await context.Bikes.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> getCustomers(string lastNameFilter = "")
        {
            if (string.IsNullOrEmpty(lastNameFilter))
            {
                return await context.Customers.ToListAsync();
            }
            return await context.Customers.Where(c => c.LastName.Contains(lastNameFilter)).ToListAsync();
        }

        public async Task<IEnumerable<Rental>> getRentals()
        {
            return await context.Rentals.ToListAsync();
        }

        public async Task<IEnumerable<UnpaidRental>> getUnpaidEndedRentals()
        {
            // only show not paid rentals with an end date and a price larger than 0
            return await context.Rentals.Where(r => !r.Paid && r.TotalCostsInEuro > 0 && r.RentalEnd.HasValue).Select(r => new UnpaidRental
            {
                CustomerFirstName = r.Renter.FirstName,
                CustomerId = r.Renter.CustomerId,
                CustomerLastName = r.Renter.LastName,
                EndDate = r.RentalEnd.Value,
                RentalId = r.RentalId,
                StartDate = r.RentalBegin,
                TotalPrice = r.TotalCostsInEuro.Value,
            }).ToListAsync();
        }

        public async Task<Rental> markEndedRentalAsPaid(int id)
        {
            var rental = context.Rentals.Find(id);
            if (!rental.RentalEnd.HasValue && rental.TotalCostsInEuro.Value > 0)
            {
                throw new ApplicationException();
            }
            rental.Paid = true;
            await context.SaveChangesAsync();
            return rental;
        }

        public async Task<Bike> updateBike(Bike bike)
        {
            context.Entry(bike).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return bike;
        }

        public async Task<Customer> updateCustomer(Customer customer)
        {
            context.Entry(customer).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return customer;
        }

        public async Task<Rental> endExistingActiveRental(int id)
        {
            var rental = context.Rentals.Find(id);
            rental.RentalEnd = DateTime.Now;
            rental.TotalCostsInEuro = calculator.calculateRentalPrice(rental.RentalBegin, rental.RentalEnd.Value, rental.Bike.RentalPriceInEuroForFirstHour, rental.Bike.RentalPriceInEuroForEachAdditionalHour);
            await context.SaveChangesAsync();
            return rental;
        }

        public async Task<Bike> getBike(int id)
        {
            var bike = await context.Bikes.FindAsync(id);
            if (bike == null)
            {
                throw new EntityDoesNotExistException();
            }
            return bike;
        }

        public async Task<Customer> getCustomer(int id)
        {
            return await context.Customers.FindAsync(id);
        }

        public async Task<Rental> getRental(int id)
        {
            return await context.Rentals.FindAsync(id);
        }

        private bool CustomerExists(int id)
        {
            return context.Customers.Any(e => e.CustomerId == id);
        }

        private bool BikeExists(int id)
        {
            return context.Bikes.Any(e => e.BikeId == id);
        }

        private bool RentalExists(int id)
        {
            return context.Rentals.Any(e => e.RentalId == id);
        }
    }
}
