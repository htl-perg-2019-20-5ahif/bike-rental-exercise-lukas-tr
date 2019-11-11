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
    public class RentalsController : ControllerBase
    {
        private IDataAccess _access;

        public RentalsController(IDataAccess access)
        {
            _access = access;
        }

        // GET: api/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            return Ok(await _access.getUnpaidEndedRentals());
        }

        // GET: api/Rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _access.getRental(id);

            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }

        //// PUT: api/Rentals/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRental(int id, Rental rental)
        //{
        //    if (id != rental.RentalId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(rental).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RentalExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Rentals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            await _access.startRental(rental);
            return CreatedAtAction("GetRental", new { id = rental.RentalId }, rental);
        }


        [HttpPost("{id}/end")]
        public async Task<ActionResult<Rental>> EndRental(int id)
        {
            return Ok(await _access.endExistingActiveRental(id));
        }

        [HttpPost("{id}/markAsPaid")]
        public async Task<ActionResult<Rental>> PayRental(int id)
        {
            return Ok(await _access.markEndedRentalAsPaid(id));
        }

    }
}
