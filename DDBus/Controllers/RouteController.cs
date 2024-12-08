using Microsoft.AspNetCore.Mvc;
using DDBus.Entity;
using DDBus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DDBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly CRUD_Service<Routes> _routesService;
        private readonly CRUD_Service<Stops> _stopsService;

        

            public RoutesController(CRUD_Service<Routes> routesService,
                CRUD_Service<Stops> stopsService)
            {
                _stopsService = stopsService;
                _routesService = routesService;
            }


        [HttpGet]
        public async Task<IActionResult> GetAllRoutes()
        {
            var routes = await _routesService.GetAllAsync();
            return Ok(routes);
        }


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




        [HttpGet("findroute")]
        public async Task<IActionResult> findroute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Dữ liệu điểm dừng không hợp lệ");
            }
            try
            {
                HashSet<Routes> rs = new HashSet<Routes>();
                List<Routes> routes = await _routesService.GetAllAsync();
                List<Stops> stops = await _stopsService.FindAllAsync(name,"name");
                List<Routes> by_name = routes.Where(route => route.name.Contains(name, StringComparison.OrdinalIgnoreCase)
                || route.price.ToString() == name).ToList();


                foreach (Routes route in by_name)
                {
                    rs.Add(route);
                }

                
                foreach (Stops stop in stops)
                {
                    foreach (Routes route in routes)
                    {
                        foreach( string s in route.inbound_stops)
                        {
                            if(s == stop.Id)
                            {
                                rs.Add(route);
                            }
                        }
                        foreach (string s in route.outbound_stops)
                        {
                            if (s == stop.Id)
                            {
                                rs.Add(route);
                            }
                        }
                    }
                }


                return Ok(rs.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }




    }
}
