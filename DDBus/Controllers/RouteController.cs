using Microsoft.AspNetCore.Mvc;
using DDBus.Entity;
using DDBus.Services;

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

        // GET: api/Routes
        [HttpGet]
        public async Task<IActionResult> GetAllRoutes()
        {
            var routes = await _routesService.GetAllAsync();
            return Ok(routes);
        }

        // GET: api/Routes/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetRouteById(string id)
        {
            var route = await _routesService.GetByIdAsync(id);

            if (route == null)
            {
                return NotFound(new { Message = "Route not found" });
            }

            return Ok(route);
        }

        // POST: api/Routes
        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] Routes newRoute)
        {
            if (newRoute == null)
            {
                return BadRequest("Invalid route data");
            }

            var routeId = await _routesService.AddAsync(newRoute);
            return CreatedAtAction(nameof(GetRouteById), new { id = routeId }, newRoute);
        }

        // PUT: api/Routes/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateRoute(string id, [FromBody] Routes updatedRoute)
        {
            var existingRoute = await _routesService.GetByIdAsync(id);
            if (existingRoute == null)
            {
                return NotFound(new { Message = "Route not found" });
            }

            await _routesService.UpdateAsync(id, updatedRoute);
            return NoContent();
        }

        // DELETE: api/Routes/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteRoute(string id)
        {
            var existingRoute = await _routesService.GetByIdAsync(id);
            if (existingRoute == null)
            {
                return NotFound(new { Message = "Route not found" });
            }

            await _routesService.DeleteAsync(id);
            return NoContent();
        }
    }
}
