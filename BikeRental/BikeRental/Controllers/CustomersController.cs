using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeRental;
using BikeRental.Model;

namespace BikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private IDataAccess _access;

        public CustomersController(IDataAccess access)
        {
            _access = access;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers([FromQuery] string lastNameFilter)
        {
            return Ok(await _access.getCustomers(lastNameFilter));
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                return await _access.getCustomer(id);
            }
            catch (EntityDoesNotExistException)
            {
                return NotFound();
            }
        }

        // GET: api/Customers/5/Rentals
        [HttpGet("{id}/Rentals")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals(int id)
        {
            try
            {
                var customer = await _access.getCustomer(id);
                return customer.Rentals;
            }
            catch (EntityDoesNotExistException)
            {
                return NotFound();
            }
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                await _access.updateCustomer(customer);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        // POST: api/Customers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            await _access.createCustomer(customer);
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            try
            {
                return Ok(await _access.deleteCustomer(id));
            }
            catch (EntityDoesNotExistException)
            {
                return NotFound();
            }
        }

    }
}
