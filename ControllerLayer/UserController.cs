using System.Net;
using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.Misc;
using BusRouteApi.RequestModels;
using BusRouteApi.ServiceLayer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using AuthorizeAttribute = BusRouteApi.Helpers.AuthorizeAttribute;

namespace BusRouteApi.ControllerLayer
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly PermissionAuthorize _permissionAuthorize;
        public UserController
        (
            UserService userService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _userService = userService;
            _permissionAuthorize = new PermissionAuthorize(httpContextAccessor);
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

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {

                bool isAdmin, isClerk;
                Exception e;
                (isAdmin, e) = await _permissionAuthorize.IsRolePermit(
                    new List<string> { Role.Admin.ToString() });

                (isClerk, e) = await _permissionAuthorize.IsRolePermit(
                    new List<string> { Role.Clerk.ToString() });

                if (!isAdmin && !isClerk)
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, new { message = "Unauthorized" });
                }

                Queue<UserBody> userBodies = new Queue<UserBody>();
                await foreach (UserBody userBody in _userService.GetUsers(_permissionAuthorize._user))
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQuery query)
        {
            try
            {
                Queue<UserBody> userBodies = new Queue<UserBody>();

                await foreach (UserBody userBody in _userService.GetUsers(query.Term, (UserBody)HttpContext.Items["User"]))
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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