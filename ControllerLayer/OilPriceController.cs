using System.Net;
using BusRouteApi.Helpers;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BusRouteApi.ControllerLayer
{
    [ApiController]
    [Route("api/oil-prices")]
    public class OilPriceController : ControllerBase
    {
        private readonly OilPriceService _oilPriceService;
        private readonly PermissionAuthorize _permissionAuthorize;
        public OilPriceController(OilPriceService oilPriceService, IHttpContextAccessor httpContext)
        {
            _oilPriceService = oilPriceService;
            _permissionAuthorize = new PermissionAuthorize(httpContext);
        }


        [HttpGet]
        public async Task<IActionResult> GetOilPrice()
        {
            try
            {
                Queue<OilPriceBody> oilPrices = new Queue<OilPriceBody>();

                await foreach (OilPriceBody oilPriceBody in _oilPriceService.GetOilPrices(_permissionAuthorize._user.VendorId))
                {
                    oilPrices.Enqueue(oilPriceBody);
                }

                return StatusCode((int)HttpStatusCode.OK, oilPrices);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }


        [HttpGet("{date}")]
        public async Task<IActionResult> GetOilPrice(string date)
        {
            try
            {
                OilPriceBody oilPriceBody;
                Exception e;
                DateOnly dateOnly;

                (dateOnly, e) = DateTimeParser.ParserDateFromString(date);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }


                (oilPriceBody, e) = await _oilPriceService.GetOilPrice(date, _permissionAuthorize._user.VendorId);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, oilPriceBody);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOilPrice([FromBody] OilPriceBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _oilPriceService.CreateOilPrice(body, _permissionAuthorize._user.VendorId);

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

        [HttpPut]
        public async Task<IActionResult> UpdateOilPrice([FromBody] OilPriceBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _oilPriceService.UpdateOilPrice(body, _permissionAuthorize._user.VendorId);

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
    }
}