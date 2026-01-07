using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RocketMoonApp.Server.Data;
using RocketMoonApp.Server.Models;

namespace RocketMoonApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoonPhaseEntityController : ControllerBase
    {
        private readonly RocketMoonDbContext _db;

        public MoonPhaseEntityController(RocketMoonDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoonPhase>>> GetAll()
        {
            return Ok(await _db.MoonPhases.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MoonPhase>> Get(int id)
        {
            var item = await _db.MoonPhases.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<MoonPhase>> Create(MoonPhase phase)
        {
            phase.CreatedAt = DateTime.UtcNow;
            phase.UpdatedAt = DateTime.UtcNow;
            _db.MoonPhases.Add(phase);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = phase.MoonPhaseId }, phase);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MoonPhase updated)
        {
            if (id != updated.MoonPhaseId) return BadRequest();
            var existing = await _db.MoonPhases.FindAsync(id);
            if (existing == null) return NotFound();
            existing.PhaseName = updated.PhaseName;
            existing.PhasePercentage = updated.PhasePercentage;
            existing.Date = updated.Date;
            existing.UpdatedAt = DateTime.UtcNow;
            _db.Entry(existing).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.MoonPhases.FindAsync(id);
            if (existing == null) return NotFound();
            _db.MoonPhases.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
