using System.Net;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteApi.ControllerLayer
{
    [ApiController]
    [Route("api/routes")]
    public class RouteController : ControllerBase
    {

        private readonly RouteService _routeService;
        private readonly RoutePriceService _routePriceService;

        public RouteController(RoutePriceService routePriceService, RouteService routeService)
        {
            _routePriceService = routePriceService;
            _routeService = routeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {

                Queue<RoutePriceBody> routePriceBodies = new Queue<RoutePriceBody>();

                await foreach (RoutePriceBody routePriceBody in _routePriceService.GetRoutePrices())
                {
                    routePriceBodies.Enqueue(routePriceBody);
                }

                return StatusCode((int)HttpStatusCode.OK, routePriceBodies);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoute(int id)
        {
            try
            {
                RoutePriceBody routePriceBody;
                Exception e;
                (routePriceBody, e) = await _routePriceService.GetRoutePrice(id);


                return StatusCode((int)HttpStatusCode.OK, routePriceBody);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoutes()
        {
            try
            {
                Queue<RoutePriceBody> routeBodies = new Queue<RoutePriceBody>();

                await foreach (RoutePriceBody routeBody in _routePriceService.GetRoutePrices())
                {
                    routeBodies.Enqueue(routeBody);
                }

                return StatusCode((int)HttpStatusCode.OK, routeBodies);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetRouteByTerm([FromQuery] RouteQuery query)
        {
            try
            {
                Queue<RouteBody> routeBodies = new Queue<RouteBody>();

                if (query.Type != null)
                {
                    await foreach (RouteBody routeBody in _routeService.GetRoutesByType(query.Term, query.Type))
                    {
                        routeBodies.Enqueue(routeBody);
                    }
                }
                else
                {
                    await foreach (RouteBody routeBody in _routeService.GetRoutes(query.Term))
                    {
                        routeBodies.Enqueue(routeBody);
                    }
                }


                return StatusCode((int)HttpStatusCode.OK, routeBodies);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoutePrice([FromBody] RoutePriceBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _routePriceService.CreateRoutePrice(body);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.Created);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoutePrice([FromBody] RoutePriceBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _routePriceService.UpdateRoutePrice(body);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoutePrice(int id)
        {
            try
            {

                bool result;
                Exception e;

                (result, e) = await _routePriceService.DeleteRoutePrice(id);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }


    }
}