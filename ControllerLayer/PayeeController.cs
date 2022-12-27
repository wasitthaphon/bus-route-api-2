using System.Net;
using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Mvc;


namespace BusRouteApi.ControllerLayer
{
    [ApiController]
    [Route("api/payees")]
    public class PayeeController : ControllerBase
    {
        private readonly PayeeService _payeeService;
        private readonly PermissionAuthorize _permissionAuthorize;

        public PayeeController(PayeeService payeeService, IHttpContextAccessor httpContext)
        {
            _payeeService = payeeService;
            _permissionAuthorize = new PermissionAuthorize(httpContext);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetPayees([FromQuery] PayeeQuery query)
        {
            try
            {
                Queue<PayeeBody> payeeBodies = new Queue<PayeeBody>();
                await foreach (PayeeBody payeeBody in _payeeService.GetPayees(query.Term, _permissionAuthorize._user.VendorId))
                {
                    payeeBodies.Enqueue(payeeBody);
                }

                return StatusCode((int)HttpStatusCode.OK, payeeBodies);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPayees()
        {
            try
            {

                Queue<PayeeBody> payeeBodies = new Queue<PayeeBody>();

                await foreach (PayeeBody payeeBody in _payeeService.GetAllPayees(_permissionAuthorize._user.VendorId))
                {
                    payeeBodies.Enqueue(payeeBody);
                }

                return StatusCode((int)HttpStatusCode.OK, payeeBodies);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayee(int id)
        {
            try
            {
                PayeeBody payee;
                Exception e;

                (payee, e) = await _payeeService.GetPayee(id);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, payee);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreatePayee([FromBody] PayeeBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _payeeService.CreatePayee(body);

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
        public async Task<IActionResult> UpdatePayee([FromBody] PayeeBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _payeeService.UpdatePayee(body);

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
        public async Task<IActionResult> DeletePayee(int id)
        {
            try
            {
                bool result;
                Exception e;
                (result, e) = await _payeeService.DeletePayee(id);

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