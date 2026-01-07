using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RocketMoonApp.Server.Data;
using RocketMoonApp.Server.Models;

namespace RocketMoonApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisCacheController : ControllerBase
    {
        private readonly RocketMoonDbContext _db;

        public AnalysisCacheController(RocketMoonDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnalysisCache>>> GetAll()
        {
            return Ok(await _db.AnalysisCaches.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnalysisCache>> Get(int id)
        {
            var item = await _db.AnalysisCaches.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<AnalysisCache>> Create(AnalysisCache cache)
        {
            cache.CalculatedAt = DateTime.UtcNow;
            _db.AnalysisCaches.Add(cache);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = cache.Id }, cache);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AnalysisCache updated)
        {
            if (id != updated.Id) return BadRequest();
            var existing = await _db.AnalysisCaches.FindAsync(id);
            if (existing == null) return NotFound();
            existing.MoonPhaseCategory = updated.MoonPhaseCategory;
            existing.TotalLaunches = updated.TotalLaunches;
            existing.SuccessfulLaunches = updated.SuccessfulLaunches;
            existing.SuccessRate = updated.SuccessRate;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            existing.CalculatedAt = DateTime.UtcNow;
            _db.Entry(existing).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.AnalysisCaches.FindAsync(id);
            if (existing == null) return NotFound();
            _db.AnalysisCaches.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
