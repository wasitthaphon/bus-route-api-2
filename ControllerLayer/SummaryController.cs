using System.Net;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteApi.ControllerLayer
{
    [ApiController]
    [Route("api/summary")]
    public class SummaryController : ControllerBase
    {

        private readonly SummaryService _summaryService;
        public SummaryController(SummaryService summaryService)
        {
            _summaryService = summaryService;
        }


        [HttpPost("by-bus")]
        public async Task<IActionResult> GetSummaryBuBus([FromBody] SummaryByBusRequest request)
        {

            try
            {
                SummaryByBusResponse summaryByBusResponse;
                Exception e;

                (summaryByBusResponse, e) = await _summaryService.GetSummaryBuBus(request);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, summaryByBusResponse);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost("by-route")]
        public async Task<IActionResult> GetSummaryByRoute([FromBody] SummaryByRouteRequest request)
        {
            try
            {
                SummaryByRouteResponse[] summaryByRouteResponses;
                Exception e;

                (summaryByRouteResponses, e) = await _summaryService.GetSummaryByRoute(request);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, summaryByRouteResponses);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost("by-payee")]
        public async Task<IActionResult> GetSummaryByPayee([FromBody] SummaryByPayeeRequest request)
        {
            try
            {
                SummaryByPayeeResponse[] summaryByPayeeResponses;
                Exception e;

                (summaryByPayeeResponses, e) = await _summaryService.GetSummaryByPayee(request);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, summaryByPayeeResponses);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }
    }
}