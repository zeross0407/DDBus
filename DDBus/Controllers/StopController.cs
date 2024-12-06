﻿using Microsoft.AspNetCore.Mvc;
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
                return NotFound(new { Message = "Stop not found" });
            }

            return Ok(stop);
        }


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


        [HttpPost("addstops")]
        public async Task<IActionResult> addroutes([FromBody] List<Stops>Stops)
        {
            if (Stops == null || Stops.Count <= 0 )
            {
                return BadRequest("Invalid stop data");
            }
            try
            {
                foreach ( Stops st in Stops)
                {
                    await _stopsService.AddAsync(st);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            return Ok();
        }
    }
}