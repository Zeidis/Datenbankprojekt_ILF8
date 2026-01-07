using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RocketMoonApp.Server.Data;
using RocketMoonApp.Server.Models;

namespace RocketMoonApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly RocketMoonDbContext _db;

        public LocationController(RocketMoonDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAll()
        {
            var list = await _db.Locations.Include(l => l.Launches).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> Get(int id)
        {
            var loc = await _db.Locations.Include(l => l.Launches).FirstOrDefaultAsync(l => l.LocationId == id);
            if (loc == null) return NotFound();
            return Ok(loc);
        }

        [HttpPost]
        public async Task<ActionResult<Location>> Create(Location location)
        {
            location.CreatedAt = DateTime.UtcNow;
            location.UpdatedAt = DateTime.UtcNow;
            _db.Locations.Add(location);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = location.LocationId }, location);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Location updated)
        {
            if (id != updated.LocationId) return BadRequest();
            var existing = await _db.Locations.FindAsync(id);
            if (existing == null) return NotFound();
            existing.CountryName = updated.CountryName;
            existing.Latitude = updated.Latitude;
            existing.Longitude = updated.Longitude;
            existing.UpdatedAt = DateTime.UtcNow;
            _db.Entry(existing).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Locations.FindAsync(id);
            if (existing == null) return NotFound();
            _db.Locations.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
