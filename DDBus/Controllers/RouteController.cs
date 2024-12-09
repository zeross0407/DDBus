using Microsoft.AspNetCore.Mvc;
using DDBus.Entity;
using DDBus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Xml.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

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

        class RouteDTO
        {
            public string? Id { get; set; }
            public string? name { get; set; }
            public required string index_route { get; set; }
            public required string start_time { get; set; }
            public required string end_time { get; set; }
            public required int interval { get; set; }
            public required int price { get; set; }
            public required List<Stops> outbound_stops { get; set; }
            public required List<Stops> inbound_stops { get; set; }
            public required double route_length { get; set; }
        }


        [HttpGet("findroute")]
        public async Task<IActionResult> findroute(string? name)
        {

            try
            {

                HashSet<Routes> rs = new HashSet<Routes>();
                List<Routes> routes = await _routesService.GetAllAsync();
                List<Stops> stops = [];
                List<Routes> by_name = [];
                if( !string.IsNullOrEmpty(name))
                {
                    by_name = routes.Where(route => route.name.Contains(name, StringComparison.OrdinalIgnoreCase)|| route.price.ToString() == name).ToList();
                }
                else
                {
                    by_name = routes;
                    
                }
                stops = await _stopsService.GetAllAsync();
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

                List<RouteDTO> rp = new List<RouteDTO>();
                foreach (Routes r in rs)
                {

                    RouteDTO rr = new RouteDTO { Id = r.Id , end_time = r.end_time ,
                    index_route = r.index_route ,
                    interval = r.interval ,
                    price = r.price ,
                    name = r.name ,
                    route_length = r.route_length ,
                    start_time = r.start_time ,
                    inbound_stops = [],
                    outbound_stops = []
                    };

                    foreach(string s in r.inbound_stops)
                    {
                        Stops stops1 = stops.Where(ss => ss.Id == s).FirstOrDefault()!;
                        if (stops1 != null)
                        rr.inbound_stops.Add(stops1);
                    }
                    foreach (string s in r.outbound_stops)
                    {
                        Stops stops1 = stops.Where(ss => ss.Id == s).FirstOrDefault()!;
                        if(stops1 !=null)
                        rr.outbound_stops.Add(stops1);
                    }
                    rp.Add(rr);

                }
                return Ok(rp.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }




    }
}
