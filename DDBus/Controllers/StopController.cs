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

        [HttpGet("allstop")]
        public async Task<IActionResult> GetAllStops()
        {
            var stops = await _stopsService.GetAllAsync();
            return Ok(stops);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetStopById(string id)
        {
            var stop = await _stopsService.GetByIdAsync(id);

            if (stop == null)
            {
                return NotFound(new { Message = "Không tìm thấy điểm dừng" });
            }

            return Ok(stop);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStop([FromBody] Stops newStop)
        {
            if (newStop == null)
            {
                return BadRequest("Dữ liệu điểm dừng không hợp lệ");
            }

            var stopId = await _stopsService.AddAsync(newStop);
            return Ok();
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateStop(string id, [FromBody] Stops updatedStop)
        {
            var existingStop = await _stopsService.GetByIdAsync(id);
            if (existingStop == null)
            {
                return NotFound(new { Message = "Không tìm thấy điểm dừng" });
            }

            await _stopsService.UpdateAsync(id, updatedStop);
            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteStop(string id)
        {
            var existingStop = await _stopsService.GetByIdAsync(id);
            if (existingStop == null)
            {
                return NotFound(new { Message = "Không tìm thấy điểm dừng" });
            }

            await _stopsService.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("addstops")]
        public async Task<IActionResult> addroutes([FromBody] List<Stops> Stops)
        {
            if (Stops == null || Stops.Count <= 0)
            {
                return BadRequest("Dữ liệu điểm dừng không hợp lệ");
            }
            try
            {
                foreach (Stops st in Stops)
                {
                    await _stopsService.AddAsync(st);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
            return Ok();
        }
    }
}
