using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Misc;
using BusRouteApi.RequestModels;

namespace BusRouteApi.Helpers
{
    public class PermissionAuthorize
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly UserBody _user;


        public PermissionAuthorize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _user = (UserBody)_httpContextAccessor.HttpContext.Items["User"];
        }

        public async Task<(bool, Exception)> IsOwner(int userId)
        {
            try
            {

                if (_user.Id != userId)
                {
                    return (false, new Exception("Unauthorized"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public async Task<(bool, Exception)> IsRolePermit(List<string> roles)
        {
            try
            {

                if (roles.Contains(_user.Role))
                {
                    return (true, null);
                }

                return (false, new Exception("Unauthorized"));

            }
            catch (Exception e)
            {
                return (false, e);
            }
        }
    }
}