using System.Net;
using BusRouteApi.Helpers;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteApi.ControllerLayer
{

    [ApiController]
    [Route("api/bus-routes")]
    public class BusRouteController : ControllerBase
    {
        private readonly BusRouteService _busRouteService;

        public BusRouteController(BusRouteService busRouteService)
        {
            _busRouteService = busRouteService;
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> GetBusRoute(string date)
        {
            try
            {
                BusRouteBody busRouteBody;
                Exception e;
                DateOnly dateOnly;

                (dateOnly, e) = DateTimeParser.ParserDateFromString(date);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }


                (busRouteBody, e) = await _busRouteService.GetBusRoutes(dateOnly);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, busRouteBody);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateBusRoute([FromBody] BusRouteBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _busRouteService.CreateBusRoute(body);

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

        [HttpDelete("{date}")]
        public async Task<IActionResult> DeleteBusRoute(string date)
        {
            try
            {
                bool result;
                Exception e;
                DateOnly dateOnly;

                (dateOnly, e) = DateTimeParser.ParserDateFromString(date);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                (result, e) = await _busRouteService.DeleteBusRoute(dateOnly);

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