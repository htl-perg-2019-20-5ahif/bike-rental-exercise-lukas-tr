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
    public class BikesController : ControllerBase
    {
        private IDataAccess _access;

        public BikesController(IDataAccess access)
        {
            _access = access;
        }

        // GET: api/Bikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes([FromQuery] BikeSort sort)
        {
            return Ok(await _access.getCurrentlyAvailableBikes(sort));
        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            try
            {
                return await _access.getBike(id);
            }
            catch (EntityDoesNotExistException)
            {
                return NotFound();
            }
        }

        // PUT: api/Bikes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, Bike bike)
        {
            if (id != bike.BikeId)
            {
                return BadRequest();
            }
            try
            {
                await _access.updateBike(bike);
            }
            catch (Exception)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Bikes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Bike>> PostBike(Bike bike)
        {
            await _access.createBike(bike);
            return CreatedAtAction("GetBike", new { id = bike.BikeId }, bike);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bike>> DeleteBike(int id)
        {
            try
            {
                return await _access.deleteBike(id);
            }
            catch (EntityDoesNotExistException)
            {
                return NotFound();
            }
        }
    }
}
