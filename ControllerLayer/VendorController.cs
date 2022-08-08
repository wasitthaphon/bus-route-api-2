using System.Net;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteApi.ControllerLayer
{
    [ApiController]
    [Route("api/vendors")]
    public class VendorController : ControllerBase
    {

        private readonly VendorService _vendorService;

        public VendorController(VendorService vendorService)
        {
            _vendorService = vendorService;
        }


        [HttpGet("default")]
        public async Task<IActionResult> GetVendorName()
        {

            try
            {
                VendorBody vendorBody;
                Exception e;

                (vendorBody, e) = await _vendorService.GetFirstVendor();

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, vendorBody);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }

        }

    }
}