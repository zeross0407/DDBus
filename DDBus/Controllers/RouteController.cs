﻿using Microsoft.AspNetCore.Mvc;
using DDBus.Entity;
using DDBus.Services;
using Microsoft.AspNetCore.Authorization;

namespace DDBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly CRUD_Service<Routes> _routesService;

        public RoutesController(CRUD_Service<Routes> routesService)
        {
            _routesService = routesService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllRoutes()
        {
            var routes = await _routesService.GetAllAsync();
            return Ok(routes);
        }

        [Authorize]
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetRouteById(string id)
        {
            var route = await _routesService.GetByIdAsync(id);

            if (route == null)
            {
                return NotFound(new { Message = "Không tìm thấy tuyến đường" });
            }

            return Ok(route);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] Routes newRoute)
        {
            if (newRoute == null)
            {
                return BadRequest("Dữ liệu tuyến đường không hợp lệ");
            }

            await _routesService.AddAsync(newRoute);
            return Ok();
        }

        [Authorize]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateRoute(string id, [FromBody] Routes updatedRoute)
        {
            var existingRoute = await _routesService.GetByIdAsync(id);
            if (existingRoute == null)
            {
                return NotFound(new { Message = "Không tìm thấy tuyến đường" });
            }

            await _routesService.UpdateAsync(id, updatedRoute);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteRoute(string id)
        {
            var existingRoute = await _routesService.GetByIdAsync(id);
            if (existingRoute == null)
            {
                return NotFound(new { Message = "Không tìm thấy tuyến đường" });
            }

            await _routesService.DeleteAsync(id);
            return Ok();
        }
    }
}
