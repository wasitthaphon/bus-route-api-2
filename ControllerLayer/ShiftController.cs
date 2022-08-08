using System.Net;
using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteApi.ControllerLayer
{
    [ApiController]
    [Route("api/shifts")]
    public class ShiftController : ControllerBase
    {
        public ShiftService _shiftService;
        public ShiftController(ShiftService shiftService)
        {
            _shiftService = shiftService;
        }


        [HttpGet("suggestions")]
        public async Task<IActionResult> GetShifts([FromQuery] ShiftQuery query)
        {
            try
            {
                Queue<ShiftBody> shifts = new Queue<ShiftBody>();

                await foreach (ShiftBody shift in _shiftService.GetShifts(query.Term))
                {
                    shifts.Enqueue(shift);
                }

                return StatusCode((int)HttpStatusCode.OK, shifts);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllShifts()
        {
            try
            {
                Queue<ShiftBody> shiftBodies = new Queue<ShiftBody>();

                await foreach (ShiftBody shiftBody in _shiftService.GetAllShifts())
                {
                    shiftBodies.Enqueue(shiftBody);
                }

                return StatusCode((int)HttpStatusCode.OK, shiftBodies);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShift(int id)
        {
            try
            {
                Exception e;
                ShiftBody shiftBody;

                (shiftBody, e) = await _shiftService.GetShift(id);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, shiftBody);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewShift([FromBody] ShiftBody body)
        {
            try
            {

                bool result;
                Exception e;

                (result, e) = await _shiftService.CreateShift(body);

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
        public async Task<IActionResult> UpdateShift([FromBody] ShiftBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _shiftService.UpdateShift(body);

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
        public async Task<IActionResult> DeleteShift(int id)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _shiftService.DeleteShift(id);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
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