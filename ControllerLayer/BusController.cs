using System.Net;
using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AuthorizeAttribute = BusRouteApi.Helpers.AuthorizeAttribute;

namespace BusRouteApi.ControllerLayer
{
    [Authorize]
    [ApiController]
    [Route("api/buses")]
    public class BusController : ControllerBase
    {
        private readonly BusService _busService;
        private readonly PermissionAuthorize _permissionAuthorize;
        public BusController(BusService busService, IHttpContextAccessor httpContextAccessor)
        {
            _busService = busService;
            _permissionAuthorize = new PermissionAuthorize(httpContextAccessor);
        }

        [HttpGet("{busNumber}")]
        public async Task<IActionResult> GetBus(string busNumber)
        {
            try
            {
                BusBody bus;
                Exception exception;
                (bus, exception) = await _busService.GetBus(busNumber, _permissionAuthorize._user.VendorId);

                if (exception != null)
                {
                    return StatusCode((int)HttpStatusCode.BadGateway, new { message = exception.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, bus);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetBuses([FromQuery] BusQuery query)
        {
            try
            {
                Queue<BusBody> buses = new Queue<BusBody>();


                await foreach (BusBody bus in _busService.GetBusesByTerm(query.Term, _permissionAuthorize._user.VendorId))
                {
                    buses.Enqueue(bus);
                }

                return StatusCode((int)HttpStatusCode.OK, buses);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBuses()
        {
            try
            {
                Queue<BusBody> buses = new Queue<BusBody>();

                await foreach (BusBody bus in _busService.GetAllBuses(_permissionAuthorize._user.VendorId))
                {
                    buses.Enqueue(bus);
                }

                return StatusCode((int)HttpStatusCode.OK, buses);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBus([FromBody] BusBody busBody)
        {
            try
            {
                bool result;
                Exception exception;
                (result, exception) = await _busService.CreateBus(busBody, _permissionAuthorize._user.VendorId);

                if (exception != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = exception.Message });
                }

                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBus([FromBody] BusBody busBody)
        {
            try
            {
                bool result;
                Exception exception;

                (result, exception) = await _busService.UpdateBus(busBody, _permissionAuthorize._user.VendorId);

                if (exception != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = exception.Message });
                }

                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpDelete("{busNumber}")]
        public async Task<IActionResult> DeleteBus(string busNumber)
        {
            try
            {
                bool result;
                Exception exception;

                (result, exception) = await _busService.DeleteBus(busNumber, _permissionAuthorize._user.VendorId);

                if (exception != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = exception.Message });
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