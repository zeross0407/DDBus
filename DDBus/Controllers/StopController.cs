using Microsoft.AspNetCore.Mvc;
using DDBus.Entity;
using DDBus.Services;

namespace DDBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopsController : ControllerBase
    {
        private readonly CRUD_Service<Stops> _stopsService;

        public StopsController(CRUD_Service<Stops> stopsService)
        {
            _stopsService = stopsService;
        }

        // GET: api/Stops
        [HttpGet]
        public async Task<IActionResult> GetAllStops()
        {
            var stops = await _stopsService.GetAllAsync();
            return Ok(stops);
        }

        // GET: api/Stops/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetStopById(string id)
        {
            var stop = await _stopsService.GetByIdAsync(id);

            if (stop == null)
            {
                return NotFound(new { Message = "Stop not found" });
            }

            return Ok(stop);
        }

        // POST: api/Stops
        [HttpPost]
        public async Task<IActionResult> CreateStop([FromBody] Stops newStop)
        {
            if (newStop == null)
            {
                return BadRequest("Invalid stop data");
            }

            var stopId = await _stopsService.AddAsync(newStop);
            return CreatedAtAction(nameof(GetStopById), new { id = stopId }, newStop);
        }

        // PUT: api/Stops/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateStop(string id, [FromBody] Stops updatedStop)
        {
            var existingStop = await _stopsService.GetByIdAsync(id);
            if (existingStop == null)
            {
                return NotFound(new { Message = "Stop not found" });
            }

            await _stopsService.UpdateAsync(id, updatedStop);
            return NoContent();
        }

        // DELETE: api/Stops/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteStop(string id)
        {
            var existingStop = await _stopsService.GetByIdAsync(id);
            if (existingStop == null)
            {
                return NotFound(new { Message = "Stop not found" });
            }

            await _stopsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
