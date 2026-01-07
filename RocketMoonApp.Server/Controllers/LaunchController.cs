using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RocketMoonApp.Server.Data;
using RocketMoonApp.Server.Models;

namespace RocketMoonApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LaunchController : ControllerBase
    {
        private readonly RocketMoonDbContext _db;

        public LaunchController(RocketMoonDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Launch>>> GetAll()
        {
            var launches = await _db.Launches.Include(l => l.Location).ToListAsync();
            return Ok(launches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Launch>> Get(string id)
        {
            var launch = await _db.Launches.Include(l => l.Location).FirstOrDefaultAsync(l => l.LaunchId == id);
            if (launch == null) return NotFound();
            return Ok(launch);
        }

        [HttpPost]
        public async Task<ActionResult<Launch>> Create(Launch launch)
        {
            if (launch == null) return BadRequest();

            launch.CreatedAt = DateTime.UtcNow;
            launch.UpdatedAt = DateTime.UtcNow;

            if (launch.Location != null)
            {
                // If location has no id, create it first
                if (launch.Location.LocationId == 0)
                {
                    launch.Location.CreatedAt = DateTime.UtcNow;
                    launch.Location.UpdatedAt = DateTime.UtcNow;
                    _db.Locations.Add(launch.Location);
                    await _db.SaveChangesAsync();
                }
                launch.LocationId = launch.Location.LocationId;
            }

            _db.Launches.Add(launch);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = launch.LaunchId }, launch);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Launch updated)
        {
            if (id != updated.LaunchId) return BadRequest();

            var existing = await _db.Launches.Include(l => l.Location).FirstOrDefaultAsync(l => l.LaunchId == id);
            if (existing == null) return NotFound();

            existing.RocketName = updated.RocketName;
            existing.Date = updated.Date;
            existing.Status = updated.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            if (updated.Location != null)
            {
                if (updated.Location.LocationId == 0)
                {
                    updated.Location.CreatedAt = DateTime.UtcNow;
                    updated.Location.UpdatedAt = DateTime.UtcNow;
                    _db.Locations.Add(updated.Location);
                    await _db.SaveChangesAsync();
                }
                existing.LocationId = updated.Location.LocationId;
            }

            _db.Entry(existing).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _db.Launches.FindAsync(id);
            if (existing == null) return NotFound();

            _db.Launches.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
