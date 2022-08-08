using System.Net;
using BusRouteApi.Helpers;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteApi.ControllerLayer
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserBody body)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _userService.CreateUser(body);

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

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authentication([FromBody] AuthenticateRequest body)
        {
            try
            {
                AuthenticateResponse authenticateResponse;
                Exception exception;

                (authenticateResponse, exception) = await _userService.Authenticate(body);

                if (exception != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = exception.Message });
                }

                return StatusCode((int)HttpStatusCode.Created, authenticateResponse);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                Queue<UserBody> userBodies = new Queue<UserBody>();
                await foreach (UserBody userBody in _userService.GetUsers())
                {
                    userBodies.Enqueue(userBody);
                }
                return StatusCode((int)HttpStatusCode.OK, userBodies.ToArray());
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQuery query)
        {
            try
            {
                Queue<UserBody> userBodies = new Queue<UserBody>();

                await foreach (UserBody userBody in _userService.GetUsers(query.Term))
                {
                    userBodies.Enqueue(userBody);
                }
                return StatusCode((int)HttpStatusCode.OK, userBodies.ToArray());
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {

                UserBody userBody;
                Exception e;

                (userBody, e) = await _userService.GetUser(id);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new { message = e.Message });
                }
                return StatusCode((int)HttpStatusCode.OK, userBody);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserBody userBody, int id)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _userService.UpdateUser(userBody);

                if (e != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { message = e.Message });
                }

                return StatusCode((int)HttpStatusCode.OK, id);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool result;
                Exception e;

                (result, e) = await _userService.DeleteUser(id);

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