using System.Net;
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

        public PayeeController(PayeeService payeeService)
        {
            _payeeService = payeeService;
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
    }
}