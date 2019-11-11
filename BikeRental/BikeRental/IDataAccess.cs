using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeRental.Model;

namespace BikeRental
{
    public interface IDataAccess
    {
        Task<IEnumerable<Bike>> getCurrentlyAvailableBikes(BikeSort sort);
        Task<IEnumerable<Customer>> getCustomers(string lastNameFilter = "");
        Task<IEnumerable<Rental>> getRentals();

        Task<Bike> getBike(int id);
        Task<Customer> getCustomer(int id);
        Task<Rental> getRental(int id);

        Task<Bike> createBike(Bike bike);
        Task<Customer> createCustomer(Customer customer);

        Task<Bike> deleteBike(int id);
        Task<Customer> deleteCustomer(int id);

        Task<Bike> updateBike(Bike bike);
        Task<Customer> updateCustomer(Customer customer);

        Task<IEnumerable<UnpaidRental>> getUnpaidEndedRentals();
        Task<Rental> markEndedRentalAsPaid(int id);
        Task<Rental> endExistingActiveRental(int id);
        Task<Rental> startRental(Rental rental);
    }
}
