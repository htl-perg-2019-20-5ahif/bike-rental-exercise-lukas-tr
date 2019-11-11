using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRental
{
    public class Context: DbContext
    {
        public DbSet<Model.Bike> Bikes { get; set; }
        public DbSet<Model.Customer> Customers { get; set; }
        public DbSet<Model.Rental> Rentals { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=BikeRental;Integrated Security=True");
        }
    }
}
