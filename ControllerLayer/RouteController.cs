using System.Net;
using BusRouteApi.Helpers;
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
        private readonly RouteDistanceService _routeDistanceService;
        private readonly PermissionAuthorize _permissionAuthorize;

        public RouteController(
            RoutePriceService routePriceService,
            RouteDistanceService routeDistanceService,
            RouteService routeService,
            IHttpContextAccessor httpContextAccessor)
        {
            _routePriceService = routePriceService;
            _routeDistanceService = routeDistanceService;
            _routeService = routeService;
            _permissionAuthorize = new PermissionAuthorize(httpContextAccessor);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {

                Queue<RouteBody> routeBodies = new Queue<RouteBody>();

                await foreach (RouteBody routeBody in _routeService.GetRoutes(_permissionAuthorize._user.VendorId, "All"))
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

        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] RouteBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _routeService.CreateRoute(body);

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

        [HttpPost("{id}/route-prices")]
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

        [HttpPost("{id}/route-distances")]
        public async Task<IActionResult> CreateRouteDistances([FromBody] RouteDistanceBody body)
        {
            try
            {
                bool result;
                Exception e;
                (result, e) = await _routeDistanceService.CreateRouteDistance(body);

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

        [HttpPut("{routeId}")]
        public async Task<IActionResult> UpdateRoute([FromBody] RouteBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _routeService.UpdateRoute(body);

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

        [HttpPut("{routeId}/route-prices/{id}")]
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

        [HttpPut("{routeId}/route-distances/{id}")]
        public async Task<IActionResult> UpdateRouteDistance([FromBody] RouteDistanceBody body)
        {
            try
            {
                RouteDistanceBody routeDistanceBody;
                Exception e;

                (routeDistanceBody, e) = await _routeDistanceService.UpdateRouteDistance(body);

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
        public async Task<IActionResult> DeleteRoute(int id)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _routeService.DeleteRoute(id);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { messsage = e.Message });
                }

                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpDelete("{routeId}/route-prices/{id}")]
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

        [HttpDelete("{routeId}/route-distances/{id}")]
        public async Task<IActionResult> DeleteRouteDistance(int id)
        {
            try
            {

                bool result;
                Exception e;

                (result, e) = await _routeDistanceService.DeleteRouteDistance(id);

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

        [HttpGet("{id}/route-prices")]
        public async Task<IActionResult> GetRoutePrices(int id)
        {
            try
            {
                Exception e;

                Queue<RoutePriceBody> routePriceBodies = new Queue<RoutePriceBody>();

                await foreach (RoutePriceBody routePriceBody in _routePriceService.GetRoutePrices(id))
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

        [HttpGet("{id}/route-distances")]
        public async Task<IActionResult> GetRouteDistances(int id)
        {
            try
            {

                Exception e;
                Queue<RouteDistanceBody> routeDistanceBodies = new Queue<RouteDistanceBody>();

                await foreach (RouteDistanceBody routeDistanceBody in _routeDistanceService.GetRouteDistances(id))
                {
                    routeDistanceBodies.Enqueue(routeDistanceBody);
                }

                return StatusCode((int)HttpStatusCode.OK, routeDistanceBodies);

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
                Exception e;
                RouteBody routeBody;

                (routeBody, e) = await _routeService.GetRoute(id);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, routeBody);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("{routeId}/route-prices/{id}")]
        public async Task<IActionResult> GetRoutePrice(int id)
        {
            try
            {
                RoutePriceBody routePriceBody;
                Exception e;
                (routePriceBody, e) = await _routePriceService.GetRoutePrice(id);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, routePriceBody);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("{routeId}/route-distances/{id}")]
        public async Task<IActionResult> GetRouteDistance(int id)
        {
            try
            {
                RouteDistanceBody routeDistanceBody;
                Exception e;

                (routeDistanceBody, e) = await _routeDistanceService.GetRouteDistance(id);
                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, routeDistanceBody);

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
    }
}