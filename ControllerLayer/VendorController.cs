using System.Net;
using BusRouteApi.Helpers;
using BusRouteApi.Misc;
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
        private readonly PermissionAuthorize _permisssionAuthorize;

        public VendorController(VendorService vendorService, IHttpContextAccessor httpContext)
        {
            _vendorService = vendorService;
            _permisssionAuthorize = new PermissionAuthorize(httpContext);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorBody body)
        {
            try
            {
                Exception e;
                bool result;

                (result, e) = await _vendorService.CreateVendor(body);

                if (result == false)
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
        public async Task<IActionResult> UpdateVendor([FromBody] VendorBody body)
        {
            try
            {
                Exception e;
                bool result;

                (result, e) = await _vendorService.UpdateVendor(body);

                if (result == false)
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllVendor()
        {
            try
            {
                List<VendorBody> vendorBodies = new List<VendorBody>();

                await foreach (VendorBody vendorBody in _vendorService.GetVendors())
                {
                    vendorBodies.Add(vendorBody);
                }

                return StatusCode((int)HttpStatusCode.OK, vendorBodies);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVendor(int id)
        {
            try
            {
                VendorBody vendorBody;
                Exception e;
                bool isOwner;
                bool isAdmin;

                (isOwner, e) = await _permisssionAuthorize.IsOwner(id);
                (isAdmin, e) = await _permisssionAuthorize.IsRolePermit(new List<string> { Role.Admin.ToString() });

                if (!isOwner && !isAdmin)
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, new { message = "No authorized." });
                }

                (vendorBody, e) = await _vendorService.GetVender(id);

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